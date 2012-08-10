using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using log4net;
using TFSWIWatcher.BL.Configuration;

namespace TFSWIWatcher.BL.Providers
{
    //public class XMLObserverAccountProvider : IObserverAccountProvider
    //{
    //    #region Non Public Members

    //    private static readonly ILog _log = LogManager.GetLogger(typeof(XMLObserverAccountProvider));
    //    private XMLObserverAccountProviderConfigurationSection _config;
    //    private FileSystemWatcher _watcher;
    //    private Dictionary<int, List<string>> _workItemIDAccountsDictionary;
    //    private List<string> _catchAllAccountList;

    //    #endregion

    //    #region IObserverAccountProvider Members

    //    public void Initialize(XElement configRootElement)
    //    {
    //        _config = XMLObserverAccountProviderConfigurationSection.GetFromConfig();
    //        _watcher = new FileSystemWatcher(Path.GetDirectoryName(_config.ConfigurationFile), Path.GetFileName(_config.ConfigurationFile))
    //        {
    //            EnableRaisingEvents = true,
    //            NotifyFilter = NotifyFilters.LastWrite
    //        };
    //        _watcher.Changed += ConfigurationFile_Changed;

    //        ReadConfigFile(_config.ConfigurationFile);
    //    }

    //    public List<string> GetObservers(WorkItemChangedContext context)
    //    {
    //        List<string> result = new List<string>(_catchAllAccountList);

    //        if (_workItemIDAccountsDictionary.ContainsKey(context.WorkItemID))
    //            result.AddRange(_workItemIDAccountsDictionary[context.WorkItemID]);

    //        return result;
    //    }

    //    #endregion

    //    #region Non Public Methods

    //    private void ConfigurationFile_Changed(object sender, FileSystemEventArgs e)
    //    {
    //        if (e.ChangeType == WatcherChangeTypes.Changed)
    //        {
    //            _log.DebugFormat("Configuration file '{0}' for XMLObserverAccountProvider has changed. Rereading config file.", _config.ConfigurationFile);
    //            ReadConfigFile(_config.ConfigurationFile);
    //        }
    //    }

    //    private void ReadConfigFile(string filePath)
    //    {
    //        if (!File.Exists(filePath))
    //        {
    //            _log.ErrorFormat("Could not find configuration file '{0}' for XMLObserverAccountProvider.", filePath);
    //            throw new FileNotFoundException(string.Format("Could not find XMLObserverAccountProvider configuration file: {0}", filePath), filePath);
    //        }

    //        _workItemIDAccountsDictionary = new Dictionary<int, List<string>>();
    //        _catchAllAccountList = new List<string>();

    //        _log.DebugFormat("Start: Reading configuration file '{0}' for XMLObserverAccountProvider", filePath);
    //        using (StreamReader streamReader = new StreamReader(filePath))
    //        {
    //            using (XmlReader xmlReader = XmlReader.Create(streamReader, new XmlReaderSettings { IgnoreWhitespace = true }))
    //            {
    //                xmlReader.ReadStartElement("Observers");

    //                long elementStartPosition = streamReader.BaseStream.Position;
    //                while (xmlReader.Name == "Observer")
    //                {
    //                    XElement observerElement = (XElement)XNode.ReadFrom(xmlReader);
                        
    //                    XAttribute nameAttribute = observerElement.Attribute(XName.Get("name"));
    //                    if (nameAttribute == null)
    //                        throw new XmlException(string.Format("Missing 'name' attribute for Observer-Node. Element-Position: {0}", elementStartPosition));

    //                    string accountName = nameAttribute.Value.Trim();

    //                    if (accountName.Trim().Length == 0)
    //                        throw new XmlException(string.Format("'name' attribute for Observer-Node is empty. Element-Position: {0}", elementStartPosition));

    //                    XAttribute idsElement = observerElement.Attribute(XName.Get("workItemIDs"));
    //                    if (idsElement == null)
    //                        throw new XmlException(string.Format("Missing 'workItemIDs' attribute for Observer-Node. Element-Position: {0}", elementStartPosition));

    //                    List<int?> ids = GetIDs(idsElement.Value);

    //                    if (!ids.Contains(null))
    //                    {
    //                        foreach (int? id in ids)
    //                        {
    //                            if (_workItemIDAccountsDictionary.ContainsKey(id.Value))
    //                                _workItemIDAccountsDictionary[id.Value].Add(accountName);
    //                            else
    //                                _workItemIDAccountsDictionary[id.Value] = new List<string>(new[] { accountName });
    //                        }
    //                    }
    //                    else
    //                        _catchAllAccountList.Add(accountName);
    //                }

    //                xmlReader.ReadEndElement();
    //            }
    //        }
    //        _log.DebugFormat("Finish: Reading configuration file '{0}' for XMLObserverAccountProvider", filePath);
    //    }

    //    private static List<int?> GetIDs(string separatedIDsString)
    //    {
    //        List<int?> result = new List<int?>();

    //        string[] ids = separatedIDsString.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

    //        foreach (string idString in ids)
    //        {
    //            List<int?> localIDs = GetIDsFromIDString(idString);

    //            if (localIDs.Contains(null))
    //                return new List<int?>(new int?[] {null});
                
    //            result.AddRange(localIDs);
    //        }

    //        return result;
    //    }

    //    private static List<int?> GetIDsFromIDString(string idString)
    //    {
    //        List<int?> result = new List<int?>();

    //        if (idString != null)
    //        {
    //            int workItemID;

    //            if (idString.Trim() == "*")
    //                result.Add(null);
    //            else if (int.TryParse(idString, out workItemID))
    //                result.Add(workItemID);
    //            else if (idString.Contains("-"))
    //            {
    //                string[] parts = idString.Split('-');
    //                int startWorkItemID, endWorkItemID;

    //                if (parts.Length == 2 && int.TryParse(parts[0], out startWorkItemID) && int.TryParse(parts[1], out endWorkItemID))
    //                {
    //                    int tempWorkItemID = Math.Min(startWorkItemID, endWorkItemID);
    //                    endWorkItemID = Math.Max(startWorkItemID, endWorkItemID);
    //                    startWorkItemID = tempWorkItemID;

    //                    for (int i = startWorkItemID; i <= endWorkItemID; i++)
    //                    {
    //                        result.Add(i);
    //                    }
    //                }
    //            }
    //        }

    //        return result;
    //    }

    //    #endregion
    //}
}