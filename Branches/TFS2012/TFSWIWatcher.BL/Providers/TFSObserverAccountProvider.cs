using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using log4net;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TFSWIWatcher.BL.Configuration;
using System.Linq;

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
            _log.DebugFormat("Start: Getting WorkItem with ID {0} and Revision {1}.", context.WorkItemID, context.WorkItemRevision);

            WorkItem workItem;

            try
            {
                workItem = TFSHelper.GetWorkitem(context.TeamProjectCollection, context.WorkItemID);
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Error while trying to get WorkItem with ID {0}: {1}", context.WorkItemID, ex);
                throw;
            }

            _log.DebugFormat("Finish: Getting WorkItem with ID {0} and Revision {1}.", context.WorkItemID, context.WorkItemRevision);
            _log.DebugFormat("Getting field containing observers from workitem. Fieldname is: {0}", _config.ObserversFieldName);

			int intCurrentRevisionIndex = context.WorkItemRevision - 1;
			HashSet<string> uniqueObservers = new HashSet<string>();

			if (workItem.Revisions.Count > intCurrentRevisionIndex)
			{
				Revision objCurrentRevision = workItem.Revisions[intCurrentRevisionIndex];

				if (objCurrentRevision.Fields.Contains(_config.ObserversFieldName))
				{
					Field currentObserverField = objCurrentRevision.Fields[_config.ObserversFieldName];
					uniqueObservers = new HashSet<string>(GetObserversFromText(Convert.ToString(currentObserverField.Value)));
					int intPreviousRevisionIndex = intCurrentRevisionIndex - 1;

					if (workItem.Revisions.Count > intPreviousRevisionIndex && intPreviousRevisionIndex >= 0)
					{
						Field previousObserverField = workItem.Revisions[intPreviousRevisionIndex].Fields[_config.ObserversFieldName];
						uniqueObservers.UnionWith(GetObserversFromText(Convert.ToString(previousObserverField.Value)));
					}
					else
						_log.Info("No previous workitem revision found.");	
				}
				else
					_log.WarnFormat("Could not find field containing observers. Fieldname is: {0}", _config.ObserversFieldName);
			}
			else
				_log.Warn("Could not find current revision data.");

            _log.Debug("Finish: Getting List of Observers.");

			return uniqueObservers.ToList();
        }

        private List<string> GetObserversFromText(string observerString)
        {
            List<string> observers = new List<string>();
            _log.DebugFormat("Found field containing observers. Fieldname is: {0}, Observers are: {1}", _config.ObserversFieldName, observerString);

            if (observerString != null && observerString.Trim().Length > 0)
            {
                _log.DebugFormat("Extracting Observers using RegexPattern: {0}, RegexOptions: {1}, Observers: {2}", _config.RegexPattern, _config.RegexOptions, observerString);

                MatchCollection matches = Regex.Matches(observerString, _config.RegexPattern, _config.RegexOptions);

                try
                {
                    foreach (Match match in matches)
                    {
                        string user = match.Groups["user"].Value.Trim();

                        if (user.Length > 0)
                        {
                            _log.DebugFormat("Found Observer: {0}.", user);
                            observers.Add(user.Trim());
                        }
                    }
                }
                catch (Exception ex)
                {
                    _log.ErrorFormat("Error while trying to extract observers: {0}", ex);
                    throw;
                }
            }

            return observers;
        }

        #endregion
    }
}
