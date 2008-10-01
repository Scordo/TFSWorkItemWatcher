using System.ServiceModel;
using Microsoft.TeamFoundation.Server;

namespace TFSWIWatcher.BL
{
    /// <summary>
    /// Service interface for TFS notifications.
    /// </summary>
    [ServiceContract(Namespace = "http://schemas.microsoft.com/TeamFoundation/2005/06/Services/Notification/03")]
    public interface ITFSNotification
    {
        /// <summary>
        /// The notification method.
        /// </summary>
        /// <param name="eventXml">The event as xml string.</param>
        /// <param name="tfsIdentityXml">An identity xml string.</param>
        /// <param name="objSubscriptionInfo">An additional subscription info.</param>
        [OperationContract(Action = "http://schemas.microsoft.com/TeamFoundation/2005/06/Services/Notification/03/Notify", ReplyAction = "*")]
        [XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void Notify(string eventXml, string tfsIdentityXml, SubscriptionInfo objSubscriptionInfo);
    }
}