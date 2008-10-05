using System;
using System.IO;
using System.Xml;
using System.Configuration;
using log4net;

namespace TFSWIWatcher.BL.Providers
{
    public class MailNotifyConfigSection
    {
        #region Non Public Members

        private static readonly ILog _log = LogManager.GetLogger(typeof(MailNotifyConfigSection));

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

        internal MailNotifyConfigSection(XmlNode section)
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

        public static MailNotifyConfigSection GetFromConfig()
        {
            try
            {
                MailNotifyConfigSection config = (MailNotifyConfigSection)ConfigurationManager.GetSection("MailNotifyConfig");

                if (config == null)
                {
                    _log.Error("Could not find MailNotifyConfigSection.");
                    throw new ConfigurationErrorsException("Could not find MailNotifyConfigSection.");
                }

                return config;
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Error while reading MailNotifyConfigSection from config file: {0}", ex);
                throw;
            }
        }

        #endregion
    }

    public class MailNotifyConfigSectionHandler : IConfigurationSectionHandler
    {
        #region IConfigurationSectionHandler Members

        public object Create(object parent, object configContext, XmlNode section)
        {
            return new MailNotifyConfigSection(section);
        }

        #endregion
    }
}