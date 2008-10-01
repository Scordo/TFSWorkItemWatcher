using System;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceModel;
using log4net;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Server;
using TFSWIWatcher.BL;
using TFSWIWatcher.BL.Providers;
using TFSWIWatcher.BL.WorkItemRelated;

namespace TFSWIWatcher.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class Watcher : ITFSNotification
    {
        #region Non Public Members

        private static readonly ILog _log = LogManager.GetLogger(typeof(Watcher));
        private IObserverAccountProvider _observerAccountProvider;
        private INotifyProvider _notifyProvider;
        private ServiceHost _serviceHost;
        private TeamFoundationServer _tfsServer;
        private int _eventID;

        #endregion

        #region Public Methods

        public void Start()
        {
            try
            {
                _log.Debug("Determining TeamServer");
                string server = ConfigurationManager.AppSettings["TeamServer"];

                if (server == null)
                    throw new ConfigurationErrorsException("Could not find TeamServer-Key in AppSettings.");

                if (server.Trim().Length == 0)
                    throw new ConfigurationErrorsException("Please provide a non empty TeamServer in AppSettings.");

                _log.Debug("Determining LocalServicePort");
                string portString = ConfigurationManager.AppSettings["LocalServicePort"];

                if (portString == null)
                    throw new ConfigurationErrorsException("Could not find LocalServicePort-Key in AppSettings.");

                int port;

                if (!int.TryParse(portString, out port))
                    throw new ConfigurationErrorsException("Please provide a valid LocalServicePort.");

                _log.Debug("Determining IObserverAccountProvider");
                _observerAccountProvider = ProviderFactory.GetObserverAccountProvider();
                _log.Debug("Initializing IObserverAccountProvider");
                _observerAccountProvider.Initialize();

                _log.Debug("Determining INotifyProvider");
                _notifyProvider = ProviderFactory.GetNotifyProvider();
                _log.Debug("Initializing INotifyProvider");
                _notifyProvider.Initialize();

                TeamFoundationServer tfs = new TeamFoundationServer(server, new UICredentialsProvider());
                _log.DebugFormat("Authenticating against teamserver: {0}", server);
                tfs.Authenticate();
                _tfsServer = tfs;

                _log.Debug("Creating wcf service host");
                _serviceHost = TFSHelper.CreateServiceHost(port, this, "Notify");
                _log.Debug("Starting wcf service host");
                _serviceHost.Open();

                _log.Debug("Registering WorkItemChangedEvent with teamserver");
                _eventID = TFSHelper.RegisterWithTFS(_tfsServer, "WorkItemChangedEvent", string.Empty, port, "Notify");
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

        void ITFSNotification.Notify(string eventXml, string tfsIdentityXml, SubscriptionInfo objSubscriptionInfo)
        {
            _log.Debug("Start: Creating context");
            WorkItemChangedContext context = GetContext(eventXml, tfsIdentityXml);
            _log.Debug("Finish: Creating context");

            _log.Debug("Start: Getting observers");
            List<string> observerAccounts = _observerAccountProvider.GetObservers(context);
            _log.Debug("Finish: Getting observers");

            _log.Debug("Start: Notifying");
            _notifyProvider.Notify(observerAccounts, context);
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

                return new WorkItemChangedContext(eventXml, workItemChangeInfo, serverInfo, _tfsServer, identityField.NewValue, revisionField.NewValue);
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