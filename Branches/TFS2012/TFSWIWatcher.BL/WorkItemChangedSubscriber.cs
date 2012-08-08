using System;
using System.Collections.Generic;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Common;
using Microsoft.TeamFoundation.Framework.Server;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
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
        private static readonly Lazy<ConfigSettingsConfigurationSection> _configSettings = new Lazy<ConfigSettingsConfigurationSection>(GetSettingsFromConfig);
        private static readonly Lazy<List<IObserverAccountProvider>> _observerAccountProviders = new Lazy<List<IObserverAccountProvider>>(GetObserverAccountProviders);
        private static readonly Lazy<List<INotifyProvider>> _notifyProviders = new Lazy<List<INotifyProvider>>(GetNotifyProviders);

        private static ConfigSettingsConfigurationSection ConfigSettings
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
            foreach (INotifyProvider provider in NotifyProviders)
            {
                provider.Notify(observerAccounts, context);
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

        private static ConfigSettingsConfigurationSection GetSettingsFromConfig()
        {
            return ConfigSettingsConfigurationSection.GetFromConfig("ConfigSettings");
        }

        private static List<IObserverAccountProvider> GetObserverAccountProviders()
        {
            return ConfigSettings.GetObserverAccountProviders();
        }

        private static List<INotifyProvider> GetNotifyProviders()
        {
            return ConfigSettings.GetNotifyProviders();
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