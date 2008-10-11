using System;
using System.IO;
using System.Xml;
using System.Configuration;
using log4net;

namespace TFSWIWatcher.BL.Configuration
{
    public class MailNotifyConfigurationSection
    {
        #region Non Public Members

        private static readonly ILog _log = LogManager.GetLogger(typeof(MailNotifyConfigurationSection));

        #endregion

        #region Properties

        public string MailTransformationFile
        {
            get;
            private set;
        }

        public string DevMail
        {
            get;
            private set;
        }

        #endregion

        #region Constructors

        internal MailNotifyConfigurationSection(XmlNode section)
        {
            if (section == null)
                throw new ConfigurationErrorsException("MailNotifyConfig-Node does not exist.");

            XmlNode mailTransformationFileNode = section.SelectSingleNode("MailTransformationFile");

            if (mailTransformationFileNode == null)
                throw new ConfigurationErrorsException("Could not find MailTransformationFile-Node in MailNotifyConfig-Node.");

            MailTransformationFile = mailTransformationFileNode.InnerText.Trim();

            if (MailTransformationFile.Length == 0)
                throw new ConfigurationErrorsException("MailTransformationFile-Node is empty in MailNotifyConfig-Node.");

            if (!File.Exists(MailTransformationFile))
                throw new ConfigurationErrorsException(string.Format("MailTransformationFile-Node has an invalid path in it: {0}", MailTransformationFile));

            XmlNode devMailNode = section.SelectSingleNode("DevMail");

            if (devMailNode != null)
                DevMail = devMailNode.InnerText.Trim();
        }

        #endregion

        #region Public Methods

        public static MailNotifyConfigurationSection GetFromConfig(string configSectionName)
        {
            if (configSectionName == null || configSectionName.Trim().Length == 0)
                configSectionName = "MailNotifyConfig";

            try
            {
                MailNotifyConfigurationSection config = (MailNotifyConfigurationSection)ConfigurationManager.GetSection(configSectionName);

                if (config == null)
                {
                    _log.ErrorFormat("Could not find MailNotifyConfigSection: {0}.", configSectionName);
                    throw new ConfigurationErrorsException(string.Format("Could not find MailNotifyConfigSection: {0}.", configSectionName));
                }

                return config;
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Error while reading MailNotifyConfigSection '{0}' from config file: {1}", configSectionName, ex);
                throw;
            }
        }

        #endregion
    }

    public class MailNotifyConfigurationSectionHandler : IConfigurationSectionHandler
    {
        #region IConfigurationSectionHandler Members

        public object Create(object parent, object configContext, XmlNode section)
        {
            return new MailNotifyConfigurationSection(section);
        }

        #endregion
    }
}