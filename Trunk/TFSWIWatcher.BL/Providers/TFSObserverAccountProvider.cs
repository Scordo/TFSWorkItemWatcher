using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml.Linq;

using log4net;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Server;
using TFSWIWatcher.BL.Configuration;
using System.Linq;
using WorkItem = Microsoft.TeamFoundation.WorkItemTracking.Client.WorkItem;

namespace TFSWIWatcher.BL.Providers
{
    public class TFSObserverAccountProvider : IObserverAccountProvider
    {
        #region Non Public Members

        private TFSObserverAccountConfigSettings _config;
        private static readonly ILog _log = LogManager.GetLogger(typeof(TFSObserverAccountProvider));

        #endregion

        #region IObserverAccountProvider Members

        void IObserverAccountProvider.Initialize(XElement configRootElement)
        {
            _log.Debug("Start: Initializing.");

            try
            {
                _config = TFSObserverAccountConfigSettingsDeserializer.LoadFromXElement(configRootElement.Element("TFSObserverAccountConfig"));
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Error while trying to read config: {0}", ex);
                throw;
            }
            
            _log.Debug("Finish: Initializing.");
        }

        List<string> IObserverAccountProvider.GetObservers(WorkItemChangedContext context)
        {
			_log.Debug("Start: Getting List of Observers.");
			string workitemType = context.WorkItemChangeInfo.WorkitemType;
        	string projectName = context.WorkItemChangeInfo.PortfolioProject;
            
			_log.DebugFormat("Start: Checking if workitem  of type {0} in project {1} should be observed.", workitemType, projectName);

			if (!_config.Projects.IsWorkitemTypeSupported(projectName, workitemType))
			{
				_log.DebugFormat("Finish: Workitem  of type {0} in project {1} is not configured for being observed.", workitemType, projectName);
				return new List<string>();
			}

			_log.DebugFormat("Finish: Workitem  of type {0} in project {1} is configured for being observed.", workitemType, projectName);
            
            _log.DebugFormat("Getting field containing observers from workitem. Fieldname is: {0}", _config.ObserversFieldName);

			HashSet<string> uniqueObservers = new HashSet<string>();

            StringField changedObserverField = context.WorkItemChangedEvent.ChangedFields.StringFields.FirstOrDefault(f => f.Name.Equals(_config.ObserversFieldName));
            if (changedObserverField != null)
            {
                // observers have been changed with this workitem change
                uniqueObservers = new HashSet<string>(GetObserversFromText(changedObserverField.OldValue));
                uniqueObservers.UnionWith(GetObserversFromText(changedObserverField.NewValue));
            }
            else
            {
                _log.DebugFormat("Start: Getting WorkItem with ID {0} and Revision {1}.", context.WorkItemID, context.WorkItemRevision);

                WorkItem workItem;

                try
                {
                    workItem = context.TeamProjectCollection.GetService<WorkItemStore>().GetWorkItem(context.WorkItemID, context.WorkItemRevision);
                }
                catch (Exception ex)
                {
                    _log.ErrorFormat("Error while trying to get WorkItem with ID {0}: {1}", context.WorkItemID, ex);
                    throw;
                }

                _log.DebugFormat("Finish: Getting WorkItem with ID {0} and Revision {1}.", context.WorkItemID, context.WorkItemRevision);

                Field presentObserverField = workItem.Fields.OfType<Field>().FirstOrDefault(f => f.Name.Equals(_config.ObserversFieldName)) ?? workItem.Fields.OfType<Field>().FirstOrDefault(f => f.ReferenceName.Equals(_config.ObserversFieldName));

                if (presentObserverField != null)
                    uniqueObservers = new HashSet<string>(GetObserversFromText(Convert.ToString(presentObserverField.Value)));
                else
                    _log.WarnFormat("Could not find field containing observers. Fieldname is: {0}", _config.ObserversFieldName);
            }

            _log.Debug("Finish: Getting List of Observers.");

			return uniqueObservers.ToList();
        }

        private IEnumerable<string> GetObserversFromText(string observerString)
        {
            _log.DebugFormat("Found field containing observers. Fieldname is: {0}, Observers are: {1}", _config.ObserversFieldName, observerString);

            if (observerString != null && observerString.Trim().Length > 0)
            {
                _log.DebugFormat("Extracting Observers using RegexPattern: {0}, RegexOptions: {1}, Observers: {2}", _config.RegexPattern, _config.RegexOptions, observerString);

                MatchCollection matches = Regex.Matches(observerString, _config.RegexPattern, _config.RegexOptions);

                foreach (Match match in matches)
                {
                    string user = match.Groups["user"].Value.Trim();

                    if (user.Length > 0)
                    {
                        _log.DebugFormat("Found Observer: {0}.", user);
                        yield return user.Trim();
                    }
                }
            }
        }

        #endregion
    }
}
