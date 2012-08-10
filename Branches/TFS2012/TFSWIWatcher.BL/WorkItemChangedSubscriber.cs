using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Common;
using Microsoft.TeamFoundation.Framework.Server;
using Microsoft.TeamFoundation.WorkItemTracking.Server;
using TFSWIWatcher.BL.Configuration;
using TFSWIWatcher.BL.Providers;
using log4net;
using System.Linq;
using log4net.Config;

namespace TFSWIWatcher.BL
{
    public class WorkItemChangedSubscriber : ISubscriber
    {
        static WorkItemChangedSubscriber()
        {
            XmlConfigurator.Configure();
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string currentAssemblyPath = Path.GetDirectoryName(typeof(WorkItemChangedSubscriber).Assembly.Location);
            string assemblyPath = Path.Combine(currentAssemblyPath, new AssemblyName(args.Name).Name + ".dll");

            if (File.Exists(assemblyPath) == false) 
                return null;

            return Assembly.LoadFrom(assemblyPath);
        }

        #region ISubscriber Implementation

        string ISubscriber.Name
        {
            get { return "TFS Workitem Watcher"; }
        }

        SubscriberPriority ISubscriber.Priority
        {
            get { return SubscriberPriority.Normal; }
        }

        Type[] ISubscriber.SubscribedTypes()
        {
            return new[] {typeof (WorkItemChangedEvent)};
        }

        EventNotificationStatus ISubscriber.ProcessEvent(TeamFoundationRequestContext requestContext, NotificationType notificationType, object notificationEventArgs, out int statusCode, out string statusMessage, out ExceptionPropertyCollection properties)
        {
            statusCode = 0;
            properties = null;
            statusMessage = String.Empty;

            try
            {
                if (notificationType == NotificationType.Notification && notificationEventArgs is WorkItemChangedEvent)
                    OnWorkitemChanged(requestContext, notificationEventArgs as WorkItemChangedEvent);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex);
            }

            return EventNotificationStatus.ActionPermitted;
        }

        #endregion

        #region Workitem Changed Notification Implementation

        private static readonly ILog Log = LogManager.GetLogger(typeof (WorkItemChangedSubscriber));
        private static readonly Lazy<SubscriberConfig> _configSettings = new Lazy<SubscriberConfig>(GetSettingsFromConfig);
        private static readonly Lazy<List<IObserverAccountProvider>> _observerAccountProviders = new Lazy<List<IObserverAccountProvider>>(GetObserverAccountProviders);
        private static readonly Lazy<List<INotifyProvider>> _notifyProviders = new Lazy<List<INotifyProvider>>(GetNotifyProviders);

        private static SubscriberConfig ConfigSettings
        {
            get { return _configSettings.Value; }
        }

        private static IEnumerable<IObserverAccountProvider> ObserverAccountProviders
        {
            get { return _observerAccountProviders.Value; }
        }

        private static IEnumerable<INotifyProvider> NotifyProviders
        {
            get { return _notifyProviders.Value; }
        }

        private void OnWorkitemChanged(TeamFoundationRequestContext requestContext, WorkItemChangedEvent workItemChangedEvent)
        {
            Log.Debug("Start: Creating context");
            WorkItemChangedContext context = GetWorkItemChangedContext(requestContext, workItemChangedEvent);
            Log.Debug("Finish: Creating context");

            Log.Debug("Start: Getting observers");
            List<string> observerAccounts = new List<string>();

            foreach (IObserverAccountProvider observerAccountProvider in ObserverAccountProviders)
            {
                observerAccounts.AddRange(observerAccountProvider.GetObservers(context));
            }

            Log.Debug("Finish: Getting observers");

            Log.Debug("Start: Notifying");
            if (observerAccounts.Count != 0)
            {
                foreach (INotifyProvider provider in NotifyProviders)
                {
                    provider.Notify(observerAccounts, context);
                }
            }
            else
            {
                Log.Debug("No observers found to notify!");   
            }
            Log.Debug("Finish: Notifying");
        }

        private WorkItemChangedContext GetWorkItemChangedContext(TeamFoundationRequestContext requestContext, WorkItemChangedEvent workItemChangedEvent)
        {
            Log.DebugFormat("Start: Creating WorkItemChangedContext");

            try
            {
                int workitemId = workItemChangedEvent.CoreFields.IntegerFields.First(x => x.ReferenceName == "System.Id").NewValue;
                int workitemRevision = workItemChangedEvent.CoreFields.IntegerFields.First(x => x.ReferenceName == "System.Rev").NewValue;

                TfsTeamProjectCollection teamProjectCollection = GetTeamProjectCollection(requestContext);
                //WorkItemStore workItemStore = teamProjectCollection.GetService<WorkItemStore>();

                WorkItemChangeInfo workItemChangeInfo = new WorkItemChangeInfo
                                                            {
                                                                PortfolioProject = workItemChangedEvent.PortfolioProject,
                                                                WorkItemTitle = workItemChangedEvent.WorkItemTitle,
                                                                WorkitemType = workItemChangedEvent.CoreFields.StringFields.First(w => w.ReferenceName == "System.WorkItemType").NewValue
                                                            };

                return new WorkItemChangedContext(workItemChangeInfo, teamProjectCollection, workitemId, workitemRevision, ConfigSettings, workItemChangedEvent);
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("Error while creating WorkItemChangedContext: {0}", ex);
                throw;
            }
        }

        private static SubscriberConfig GetSettingsFromConfig()
        {
            return SubscriberConfigDeserializer.LoadFromFile(new Uri(typeof(WorkItemChangedSubscriber).Assembly.CodeBase).LocalPath + ".config");
        }

        private static List<IObserverAccountProvider> GetObserverAccountProviders()
        {
            List<IObserverAccountProvider> result = new List<IObserverAccountProvider>();

            foreach (ProviderConfigSettings providerConfigSettings in ConfigSettings.ObserverAccountProviders)
            {
                Log.Debug(string.Format("Instantiating IObserverAccountProvider {0}", providerConfigSettings.ProviderClass));
                IObserverAccountProvider observerAccountProvider = Instancer.GetInstanceOfInterface<IObserverAccountProvider>(providerConfigSettings.AssemblyName, providerConfigSettings.ProviderClass);

                Log.Debug(string.Format("Initializing IObserverAccountProvider {0}", providerConfigSettings.ProviderClass));
                observerAccountProvider.Initialize(ConfigSettings.XmlRootElement);

                result.Add(observerAccountProvider);
            }

            return result;
        }

        private static List<INotifyProvider> GetNotifyProviders()
        {
            List<INotifyProvider> result = new List<INotifyProvider>();

            foreach (ProviderConfigSettings providerConfigSettings in ConfigSettings.NotifyProviders)
            {
                Log.Debug(string.Format("Instantiating INotifyProvider {0}", providerConfigSettings.ProviderClass));
                INotifyProvider notifyProvider = Instancer.GetInstanceOfInterface<INotifyProvider>(providerConfigSettings.AssemblyName, providerConfigSettings.ProviderClass);

                Log.Debug(string.Format("Initializing INotifyProvider {0}", providerConfigSettings.ProviderClass));
                notifyProvider.Initialize(ConfigSettings.XmlRootElement);

                result.Add(notifyProvider);
            }

            return result;
        }

        private TfsTeamProjectCollection GetTeamProjectCollection(TeamFoundationRequestContext requestContext)
        {
            TeamFoundationLocationService locationService = requestContext.GetService<TeamFoundationLocationService>();
            Uri uri = new Uri(string.Format("{0}/{1}", locationService.GetServerAccessMapping(requestContext).AccessPoint, requestContext.ServiceHost.Name));

            return TfsTeamProjectCollectionFactory.GetTeamProjectCollection(uri);
        }

        #endregion
    }
}