using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace TFSWIWatcher.BL.Configuration
{
    public class TFSObserverAccountConfigSettingsDeserializer
    {
        private readonly static Dictionary<string, RegexOptions> _options;

        static TFSObserverAccountConfigSettingsDeserializer()
        {
            _options = new Dictionary<string, RegexOptions>();

            foreach (RegexOptions option in Enum.GetValues(typeof(RegexOptions)))
            {
                if (option != RegexOptions.None)
                    _options.Add(option.ToString().ToUpper(), option);
            }
        }

        public static TFSObserverAccountConfigSettings LoadFromXElement(XElement element)
        {
            TFSObserverAccountConfigSettings settings = new TFSObserverAccountConfigSettings();

            XElement regexPatternElement = element.Element("RegexPattern");
            if (regexPatternElement == null)
                throw new ConfigurationErrorsException("Could not find RegexPattern-Node in TFSObserverAccountConfig-Node.");

            settings.RegexPattern = regexPatternElement.Value.Trim();

            if (settings.RegexPattern.Length == 0)
                throw new ConfigurationErrorsException("RegexPattern-Node is empty in TFSObserverAccountConfig-Node.");

            XElement observerFieldNameElement = element.Element("ObserversFieldName");
            if (observerFieldNameElement == null)
                throw new ConfigurationErrorsException("Could not find ObserversFieldName-Node in TFSObserverAccountConfig-Node.");

            settings.ObserversFieldName = observerFieldNameElement.Value.Trim();

            if (settings.ObserversFieldName.Length == 0)
                throw new ConfigurationErrorsException("ObserversFieldName-Node is empty in TFSObserverAccountConfig-Node.");

            XElement regexOptionsElement = element.Element("RegexOptions");
            if (regexOptionsElement == null)
                throw new ConfigurationErrorsException("Could not find RegexOptions-Node in TFSObserverAccountConfig-Node.");

            settings.RegexOptions = ParseRegexOptions(regexOptionsElement.Value);

            settings.Projects = new TfsProjectConfigurationList(element.Element("Projects"));

            return settings;
        }

        private static RegexOptions ParseRegexOptions(string optionString)
        {
            RegexOptions result = RegexOptions.None;

            string[] options = optionString.ToUpper().Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string option in options)
            {
                string trimmedOption = option.Trim();

                if (_options.ContainsKey(trimmedOption))
                    result |= _options[trimmedOption];
            }

            return result;
        }
    }
}
