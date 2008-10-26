using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using log4net;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TFSWIWatcher.BL.Configuration;

namespace TFSWIWatcher.BL.Providers
{
    public class TFSObserverAccountProvider : IObserverAccountProvider
    {
        #region Non Public Members

        private TFSObserverAccountConfigurationSection _config;
        private static readonly ILog _log = LogManager.GetLogger(typeof(TFSObserverAccountProvider));

        #endregion

        #region IObserverAccountProvider Members

        void IObserverAccountProvider.Initialize()
        {
            _log.Debug("Start: Initializing.");
           
            try
            {
                _config = TFSObserverAccountConfigurationSection.GetFromConfig();
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
            _log.DebugFormat("Start: Getting WorkItem with ID {0}.", context.WorkItemID);

            WorkItem workItem;

            try
            {
                workItem = TFSHelper.GetWorkitem(context.TeamServer, context.WorkItemID);
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Error while trying to get WorkItem with ID {0}: {1}", context.WorkItemID, ex);
                throw;
            }

            _log.DebugFormat("Finish: Getting WorkItem with ID {0}.", context.WorkItemID);


            _log.DebugFormat("Getting field containing observers from workitem. Fieldname is: {0}", _config.ObserversFieldName);
            Field currentObserverField = workItem.Revisions[context.WorkItemRevision - 1].Fields[_config.ObserversFieldName];

            Field previousObserverField = null;

            if (workItem.Revisions.Count > 1)
                previousObserverField = workItem.Revisions[context.WorkItemRevision - 2].Fields[_config.ObserversFieldName];

            HashSet<string> uniqueObservers = null;

            if (currentObserverField != null)
            {
                uniqueObservers = new HashSet<string>(GetObserversFromText(Convert.ToString(currentObserverField.Value)));

                if (previousObserverField != null)
                    uniqueObservers.UnionWith(GetObserversFromText(Convert.ToString(previousObserverField.Value)));
            }
            else
                _log.WarnFormat("Could not find field containing observers. Fieldname is: {0}", _config.ObserversFieldName);

            _log.Debug("Finish: Getting List of Observers.");

            List<string> result = new List<string>();

            if (uniqueObservers != null)
            {
                foreach (string observer in uniqueObservers)
                {
                    result.Add(observer);
                }
            }

            return result;
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
