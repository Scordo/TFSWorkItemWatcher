using Microsoft.TeamFoundation.Client;
using TFSWIWatcher.BL.Configuration;

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

        public TfsTeamProjectCollection TeamProjectCollection
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

        public WorkItemChangedContext(string notifyXML, WorkItemChangeInfo changeInfo, TfsTeamProjectCollection teamProjectCollection, int workItemID, int workItemRevision, ConfigSettingsConfigurationSection configSettings)
        {
            NotifyXML = notifyXML;
            WorkItemChangeInfo = changeInfo;
            TeamProjectCollection = teamProjectCollection;
            WorkItemID = workItemID;
            WorkItemRevision = workItemRevision;
            ConfigSettings = configSettings;
        }

        #endregion
    }
}