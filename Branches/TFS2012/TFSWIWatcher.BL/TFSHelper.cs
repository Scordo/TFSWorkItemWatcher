using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Framework.Client;
using Microsoft.TeamFoundation.Server;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace TFSWIWatcher.BL
{
    public class TFSHelper
    {
        /// <summary>
        /// Gets the workitem.
        /// </summary>
        /// <param name="teamProjectCollection">The team project collection.</param>
        /// <param name="workItemId">The work item ID.</param>
        /// <returns></returns>
        public static WorkItem GetWorkitem(TfsTeamProjectCollection teamProjectCollection, int workItemId)
        {
            WorkItemStore workitemStore = new WorkItemStore(teamProjectCollection);
            return workitemStore.GetWorkItem(workItemId);
        }

        /// <summary>
        /// Gets the E mail of user.
        /// </summary>
        /// <param name="projectCollection">The project collection.</param>
        /// <param name="domainAndUsername">The domain and username.</param>
        /// <returns></returns>
        public static string GetEMailOfUser(TfsTeamProjectCollection projectCollection, string domainAndUsername)
        {
			if (domainAndUsername != null && domainAndUsername.Contains("@"))
			{
				// username is an email address --> return the email
				return domainAndUsername.Trim();
			}

            IGroupSecurityService groupSecurityService = (IGroupSecurityService) projectCollection.GetService(typeof(IIdentityManagementService));
            Identity identity = groupSecurityService.ReadIdentity(SearchFactor.AccountName, domainAndUsername, QueryMembership.None);

            return (identity != null) ? identity.MailAddress : null;
        }
    }
}