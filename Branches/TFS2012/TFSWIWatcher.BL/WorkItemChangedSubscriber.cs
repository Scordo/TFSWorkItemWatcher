using System;
using Microsoft.TeamFoundation.Framework.Server;
using Microsoft.TeamFoundation.WorkItemTracking.Server;

namespace TFSWIWatcher.BL
{
    public class WorkItemChangedSubscriber : ISubscriber
    {
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

        EventNotificationStatus ISubscriber.ProcessEvent(TeamFoundationRequestContext requestContext, NotificationType notificationType, object notificationEventArgs, out int statusCode, out string statusMessage, out Microsoft.TeamFoundation.Common.ExceptionPropertyCollection properties)
        {
            throw new NotImplementedException();
        }
    }
}
