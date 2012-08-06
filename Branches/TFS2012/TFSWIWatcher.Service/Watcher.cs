using System;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceModel;
using log4net;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Server;
using TFSWIWatcher.BL;
using TFSWIWatcher.BL.Configuration;
using TFSWIWatcher.BL.Providers;
using TFSWIWatcher.BL.WorkItemRelated;

namespace TFSWIWatcher.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class Watcher : ITFSNotification
    {
        #region Non Public Members

        private static readonly ILog _log = LogManager.GetLogger(typeof(Watcher));
        private List<IObserverAccountProvider> _observerAccountProviders;
        private List<INotifyProvider> _notifyProviders;
        private ServiceHost _serviceHost;
        private TeamFoundationServer _tfsServer;
        private int _eventID;
        private ConfigSettingsConfigurationSection _configSettings;

        #endregion

        #region Properties

        private ConfigSettingsConfigurationSection ConfigSettings
        {
            get
            {
                if (_configSettings == null)
                    _configSettings = ConfigSettingsConfigurationSection.GetFromConfig("ConfigSettings");

                return _configSettings;
            }
        }

        #endregion


        #region Public Methods

        public void Start()
        {
            try
            {
                string server = ConfigSettings.TeamServer;

                if (server.Trim().Length == 0)
                    throw new ConfigurationErrorsException("Please provide a non empty TeamServer in AppSettings.");

                _observerAccountProviders = ConfigSettings.GetObserverAccountProviders();
                _notifyProviders = ConfigSettings.GetNotifyProviders();

                TeamFoundationServer tfs = new TeamFoundationServer(server, ConfigSettings.GetCredentialsProvider());
                _log.DebugFormat("Authenticating against teamserver: {0}", server);
                tfs.Authenticate();
                _tfsServer = tfs;

                _log.Debug("Creating wcf service host");
                _serviceHost = TFSHelper.CreateServiceHost(ConfigSettings.LocalServicePort, this, "Notify");
                _log.Debug("Starting wcf service host");
                _serviceHost.Open();

                _log.Debug("Registering WorkItemChangedEvent with teamserver");
                _eventID = TFSHelper.RegisterWithTFS(_tfsServer, "WorkItemChangedEvent", string.Empty, ConfigSettings.LocalServicePort, "Notify");
                _log.DebugFormat("Registered WorkItemChangedEvent with teamserver, eventID is: {0}", _eventID);
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Error during startup: {0}", ex);
                throw;
            }
           
        }

        public void Stop()
        {
            _log.Debug("Start: Stopping watcher");

            if (_tfsServer != null)
            {
                _log.DebugFormat("Start: Unsubscribe event with ID; {0}", _eventID);
                
                try
                {
                    TFSHelper.UnSubscribeEvent(_tfsServer, _eventID);
                }
                catch (Exception ex)
                {
                    _log.ErrorFormat("Error while unsubscribing event with id {0}: {1}", _eventID, ex);
                    throw;
                }
                
                _log.DebugFormat("Finish: Unsubscribe event with ID; {0}", _eventID);
            }

            if (_serviceHost != null && _serviceHost.State == CommunicationState.Opened)
            {
                _log.Debug("Closing WCF service host");    
                _serviceHost.Close();
            }

            _log.Debug("Finish: Stopping watcher");
        }

        #endregion

        #region ITFSNotification Members

        void ITFSNotification.Notify(string eventXml, string tfsIdentityXml)
        {
            _log.Debug("Start: Creating context");
            WorkItemChangedContext context = GetContext(eventXml, tfsIdentityXml);
            _log.Debug("Finish: Creating context");

            _log.Debug("Start: Getting observers");
            List<string> observerAccounts = new List<string>();

            foreach (IObserverAccountProvider observerAccountProvider in _observerAccountProviders)
            {
                observerAccounts.AddRange(observerAccountProvider.GetObservers(context));
            }

            _log.Debug("Finish: Getting observers");

            _log.Debug("Start: Notifying");
            foreach (INotifyProvider provider in _notifyProviders)
            {
                provider.Notify(observerAccounts, context);    
            }
            _log.Debug("Finish: Notifying");
        }

        #endregion

        #region Non Public Methods

        private WorkItemChangedContext GetContext(string eventXml, string tfsIdentityXml)
        {
            _log.DebugFormat("Start: Creating WorkItemChangedContext for eventXML: {0}, tfsIdentityXml: {1}", eventXml, tfsIdentityXml);

            try
            {
                WorkItemChangeInfo workItemChangeInfo = TFSHelper.CreateInstance<WorkItemChangeInfo>(eventXml);
                ServerInfo serverInfo = TFSHelper.CreateInstance<ServerInfo>(tfsIdentityXml);

                WorkItemIntegerField identityField = workItemChangeInfo.CoreFields.IntegerFields.Find(x => x.ReferenceName == "System.Id");
                WorkItemIntegerField revisionField = workItemChangeInfo.CoreFields.IntegerFields.Find(x => x.ReferenceName == "System.Rev");

                return new WorkItemChangedContext(eventXml, workItemChangeInfo, serverInfo, _tfsServer, identityField.NewValue, revisionField.NewValue, ConfigSettings);
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Error while creating WorkItemChangedContext: {0}", ex);
                throw;
            }
            
        }

        #endregion
    }
}