using System;
using System.Configuration;
using System.Net;
using System.Xml;
using log4net;

namespace TFSWIWatcher.BL.Configuration
{
    public class ConfigCredentialsConfigurationSection
    {
        #region Non Public Members

        private static readonly ILog _log = LogManager.GetLogger(typeof(ConfigCredentialsConfigurationSection));

        #endregion

        #region Properties

        public string Username
        {
            get;
            private set;
        }

        public string Password
        {
            get;
            private set;
        }

        public string Domain
        {
            get;
            private set;
        }

        public NetworkCredential Credentials
        {
            get
            {
                if (Username == null || Password == null)
                    return null;

                return new NetworkCredential(Username, Password, Domain);
            }
        }

        #endregion

        #region Constructors

        internal ConfigCredentialsConfigurationSection(XmlNode section)
        {
            if (section == null)
                throw new ConfigurationErrorsException("ConfigCredentialsConfig-Node does not exist.");

            #region Read Username

            XmlNode usernameNode = section.SelectSingleNode("Username");

            if (usernameNode == null)
                throw new ConfigurationErrorsException("Could not find Username-Node in ConfigCredentialsConfig-Node.");

            if (usernameNode.InnerText.Trim().Length == 0)
                throw new ConfigurationErrorsException("Username-Node in ConfigCredentialsConfig-Node is empty.");

            Username = usernameNode.InnerText.Trim();

            #endregion

            #region Read Password

            XmlNode passwordNode = section.SelectSingleNode("Password");

            if (passwordNode == null)
                throw new ConfigurationErrorsException("Could not find Password-Node in ConfigCredentialsConfig-Node.");

            if (passwordNode.InnerText.Trim().Length == 0)
                throw new ConfigurationErrorsException("Password-Node in ConfigCredentialsConfig-Node is empty.");

            Password = passwordNode.InnerText.Trim();

            #endregion

            #region Read Domain

            XmlNode domainNode = section.SelectSingleNode("Domain");

            if (domainNode != null)
            {
                if (domainNode.InnerText.Trim().Length > 0)
                    Domain = domainNode.InnerText.Trim();
            }

            #endregion
        }

        #endregion

        #region Public Methods

        public static ConfigCredentialsConfigurationSection GetFromConfig()
        {
            try
            {
                ConfigCredentialsConfigurationSection config = (ConfigCredentialsConfigurationSection)ConfigurationManager.GetSection("ConfigCredentialsConfig");
                
                if (config == null)
                {
                    _log.Error("Could not find ConfigCredentialsConfigSection.");
                    throw new ConfigurationErrorsException("Could not find ConfigCredentialsConfigSection.");
                }

                return config;
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Error while reading ConfigCredentialsConfigSection from config file: {0}", ex);
                throw;
            }
        }

        #endregion
    }

    public class ConfigCredentialsConfigurationSectionHandler : IConfigurationSectionHandler
    {
        #region IConfigurationSectionHandler Members

        public object Create(object parent, object configContext, XmlNode section)
        {
            return new ConfigCredentialsConfigurationSection(section);
        }

        #endregion
    }
}