using System;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Server;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace TFSWIWatcher.BL
{
    public class TFSHelper
    {
        /// <summary>
        /// Create a local WS host which the TFS can connect (soap).
        /// </summary>
        /// <param name="port">The port number.</param>
        /// <param name="notificationInstance">The type of the class which implements the <see cref="ITFSNotification"/> interface for notifications.</param>
        /// <param name="method">The method.</param>
        /// <returns>
        /// The <see cref="ServiceHost"/> which has to be started by <see cref="CommunicationObject.Open()"/>
        /// </returns>
        public static ServiceHost CreateServiceHost(int port, ITFSNotification notificationInstance, string method)
        {
            ServiceHost serviceHost = new ServiceHost(notificationInstance);
            WSHttpBinding webserviceBinding = new WSHttpBinding(SecurityMode.None);

            BindingElementCollection bindingElementCollection = webserviceBinding.CreateBindingElements();
            TextMessageEncodingBindingElement textMessageEncodingBindingElement = bindingElementCollection.Find<TextMessageEncodingBindingElement>();
            textMessageEncodingBindingElement.MessageVersion = MessageVersion.Soap11;

            CustomBinding binding = new CustomBinding(bindingElementCollection);
            string serviceEndPointURL = string.Format("http://{0}:{1}/{2}", Environment.MachineName, port, method);
            serviceHost.AddServiceEndpoint(typeof(ITFSNotification), binding, serviceEndPointURL);

            return serviceHost;
        }

        /// <summary>
        /// Method for registering a WS at the TFS server.
        /// </summary>
        /// <param name="server">The <see cref="TeamFoundationServer"/> to register.</param>
        /// <param name="eventType">The event type.</param>
        /// <param name="filter">XPath filter.</param>
        /// <returns>The registration id.</returns>
        public static int RegisterWithTFS(TeamFoundationServer server, string eventType, string filter, int port)
        {
            return RegisterWithTFS(server, eventType, filter, port, eventType);
        }

        /// <summary>
        /// Method for registering a WS at the TFS server.
        /// </summary>
        /// <param name="server">The <see cref="TeamFoundationServer"/> to register.</param>
        /// <param name="eventType">The event type.</param>
        /// <param name="filter">XPath filter.</param>
        /// <param name="port">The port.</param>
        /// <param name="receiveMethod">The receive method.</param>
        /// <returns>The registration id.</returns>
        public static int RegisterWithTFS(TeamFoundationServer server, string eventType, string filter, int port, string receiveMethod)
        {
            string serviceEndPointURL = string.Format("http://{0}:{1}/{2}", Environment.MachineName, port, receiveMethod);
            DeliveryPreference preferences = new DeliveryPreference { Schedule = DeliverySchedule.Immediate, Type = DeliveryType.Soap, Address = serviceEndPointURL };

            IEventService eventService = (IEventService)server.GetService(typeof(IEventService));
            return eventService.SubscribeEvent(server.AuthenticatedUserName, eventType, filter, preferences);
        }


        /// <summary>
        /// Un subscribe event.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="eventID">The event ID.</param>
        public static void UnSubscribeEvent(TeamFoundationServer server, int eventID)
        {
            IEventService objEventService = (IEventService) server.GetService(typeof(IEventService));
            objEventService.UnsubscribeEvent(eventID);
        }

        /// <summary>
        /// Creates the instance of a typed represented by the provided xml.
        /// </summary>
        /// <typeparam name="T">The type to instanciate</typeparam>
        /// <param name="xmlOfSerializedType">Type xml of the serialized instance.</param>
        /// <returns>An instance of the type represented by the provided xml</returns>
        public static T CreateInstance<T>(string xmlOfSerializedType) where T : new()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            using (XmlReader xmlReader = new XmlTextReader(new StringReader(xmlOfSerializedType)))
            {
                return (T)serializer.Deserialize(xmlReader);
            }
        }

        /// <summary>
        /// Gets the workitem.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="workItemID">The work item ID.</param>
        /// <returns></returns>
        public static WorkItem GetWorkitem(TeamFoundationServer server, int workItemID)
        {
            WorkItemStore workitemStore = new WorkItemStore(server);
            return workitemStore.GetWorkItem(workItemID);
        }

        /// <summary>
        /// Gets the E mail of user.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="domainAndUsername">The domain and username.</param>
        /// <returns></returns>
        public static string GetEMailOfUser(TeamFoundationServer server, string domainAndUsername)
        {
            IGroupSecurityService groupSecurityService = (IGroupSecurityService) server.GetService(typeof (IGroupSecurityService));
            Identity identity = groupSecurityService.ReadIdentity(SearchFactor.AccountName, domainAndUsername, QueryMembership.None);

            return (identity != null) ? identity.MailAddress : null;
        }
    }
}
