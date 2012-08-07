﻿using System;
using System.Collections.Generic;
using System.Xml;
using System.Configuration;
using System.Text.RegularExpressions;
using log4net;

namespace TFSWIWatcher.BL.Providers
{
    public class TFSObserverAccountConfigSection
    {
        #region Non Public Members

        private readonly static Dictionary<string, RegexOptions> _options;
        private static readonly ILog _log = LogManager.GetLogger(typeof(TFSObserverAccountConfigSection));

        #endregion

        #region Properties

        public string RegexPattern
        {
            get;
            private set;
        }

        public RegexOptions RegexOptions
        {
            get;
            private set;
        }

        public string ObserversFieldName
        {
            get;
            private set;
        }

        #endregion

        #region Constructor

        internal TFSObserverAccountConfigSection(XmlNode section)
        {
            if (section == null)
                throw new ConfigurationErrorsException("TFSObserverAccountConfig-Node does not exist.");

            XmlNode regexPatternNode = section.SelectSingleNode("RegexPattern");

            if (regexPatternNode == null)
                throw new ConfigurationErrorsException("Could not find RegexPattern-Node in TFSObserverAccountConfig-Node.");

            RegexPattern = regexPatternNode.InnerText.Trim();

            if (RegexPattern.Length == 0)
                throw new ConfigurationErrorsException("RegexPattern-Node is empty in TFSObserverAccountConfig-Node.");

            XmlNode observersFieldNameNode = section.SelectSingleNode("ObserversFieldName");

            if (observersFieldNameNode == null)
                throw new ConfigurationErrorsException("Could not find ObserversFieldName-Node in TFSObserverAccountConfig-Node.");

            ObserversFieldName = observersFieldNameNode.InnerText.Trim();

            if (ObserversFieldName.Length == 0)
                throw new ConfigurationErrorsException("ObserversFieldName-Node is empty in TFSObserverAccountConfig-Node.");

            XmlNode regexOptionsNode = section.SelectSingleNode("RegexOptions");

            if (regexOptionsNode == null)
                throw new ConfigurationErrorsException("Could not find RegexOptions-Node in TFSObserverAccountConfig-Node.");

            RegexOptions = ParseRegexOptions(regexOptionsNode.InnerText);
        }

        static TFSObserverAccountConfigSection()
        {
            _options = new Dictionary<string, RegexOptions>();

            foreach (RegexOptions option in Enum.GetValues(typeof(RegexOptions)))
            {
                if (option != RegexOptions.None)
                    _options.Add(option.ToString().ToUpper(), option);
            }
        }

        #endregion

        #region Public Methods

        public static TFSObserverAccountConfigSection GetFromConfig()
        {
            try
            {
                return (TFSObserverAccountConfigSection)ConfigurationManager.GetSection("TFSObserverAccountConfig");
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Error while reading TFSObserverAccountConfigSection from config file: {0}", ex);
                throw;
            }
        }

        #endregion

        #region Non Public Methods

        private static RegexOptions ParseRegexOptions(string optionString)
        {
            RegexOptions result = System.Text.RegularExpressions.RegexOptions.None;

            string[] options = optionString.ToUpper().Split(new[] {'|'}, StringSplitOptions.RemoveEmptyEntries);

            foreach (string option in options)
            {
                string trimmedOption = option.Trim();

                if (_options.ContainsKey(trimmedOption))
                    result |= _options[trimmedOption];
            }

            return result;
        }

        #endregion
    }

    public class TFSObserverAccountConfigSectionHandler : IConfigurationSectionHandler
    {
        #region IConfigurationSectionHandler Members

        public object Create(object parent, object configContext, XmlNode section)
        {
            return new TFSObserverAccountConfigSection(section);
        }

        #endregion
    }
}