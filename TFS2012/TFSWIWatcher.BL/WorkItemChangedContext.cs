using Microsoft.TeamFoundation.Client;
using TFSWIWatcher.BL.Configuration;
using TFSWIWatcher.BL.WorkItemRelated;

namespace TFSWIWatcher.BL
{
    public class WorkItemChangedContext
    {
        #region Properties

        public string NotifyXML
        {
            get; internal set;
        }

        public WorkItemChangeInfo WorkItemChangeInfo
        {
            get; internal set;
        }

        public ServerInfo ServerInfo
        {
            get; internal set;
        }

        public TeamFoundationServer TeamServer
        {
            get; internal set;
        }

        public int WorkItemID
        {
            get; internal set;
        }

        public int WorkItemRevision
        {
            get; internal set;
        }

        public ConfigSettingsConfigurationSection ConfigSettings
        {
            get;
            internal set;
        }

        #endregion

        #region Constructors

        public WorkItemChangedContext(string notifyXML, WorkItemChangeInfo changeInfo, ServerInfo serverInfo, TeamFoundationServer teamserver, int workItemID, int workItemRevision, ConfigSettingsConfigurationSection configSettings)
        {
            NotifyXML = notifyXML;
            WorkItemChangeInfo = changeInfo;
            ServerInfo = serverInfo;
            TeamServer = teamserver;
            WorkItemID = workItemID;
            WorkItemRevision = workItemRevision;
            ConfigSettings = configSettings;
        }

        #endregion
    }
}