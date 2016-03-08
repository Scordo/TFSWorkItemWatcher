using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Framework.Server;
using Microsoft.TeamFoundation.WorkItemTracking.Server;
using TFSWIWatcher.BL.Configuration;

namespace TFSWIWatcher.BL
{
    public class WorkItemChangedContext
    {
        #region Properties

        public WorkItemChangedEvent WorkItemChangedEvent
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

        public SubscriberConfig ConfigSettings
        {
            get;
            internal set;
        }

        public IVssRequestContext RequestContext { get; internal set; }

        #endregion

        #region Constructors

        public WorkItemChangedContext(WorkItemChangeInfo changeInfo, TfsTeamProjectCollection teamProjectCollection, int workItemID, int workItemRevision, SubscriberConfig configSettings, WorkItemChangedEvent workItemChangedEvent, IVssRequestContext requestContext)
        {
            WorkItemChangeInfo = changeInfo;
            TeamProjectCollection = teamProjectCollection;
            WorkItemID = workItemID;
            WorkItemRevision = workItemRevision;
            ConfigSettings = configSettings;
            WorkItemChangedEvent = workItemChangedEvent;
            RequestContext = requestContext;
        }

        #endregion
    }
}