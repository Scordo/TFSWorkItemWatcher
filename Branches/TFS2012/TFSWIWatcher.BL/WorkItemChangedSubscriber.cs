using System;
using Microsoft.TeamFoundation.Common;
using Microsoft.TeamFoundation.Framework.Server;
using Microsoft.TeamFoundation.WorkItemTracking.Server;
using log4net;

namespace TFSWIWatcher.BL
{
    public class WorkItemChangedSubscriber : ISubscriber
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(WorkItemChangedSubscriber));

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
            return new[] { typeof(WorkItemChangedEvent) };
        }

        EventNotificationStatus ISubscriber.ProcessEvent(TeamFoundationRequestContext requestContext, NotificationType notificationType, object notificationEventArgs, out int statusCode, out string statusMessage, out ExceptionPropertyCollection properties)
        {
            statusCode = 0;
            properties = null;
            statusMessage = String.Empty;

            try
            {
                if (notificationType == NotificationType.Notification && notificationEventArgs is WorkItemChangedEvent)
                    OnWorkitemChanged(notificationEventArgs as WorkItemChangedEvent);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex);
            }

            return EventNotificationStatus.ActionPermitted;
        }

        private void OnWorkitemChanged(WorkItemChangedEvent workItemChangedEvent)
        {
            // TODO: Implement stuff here
        }
    }
}
