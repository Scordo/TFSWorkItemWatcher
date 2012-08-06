using System;
using System.Configuration;
using System.Net;
using log4net;
using Microsoft.TeamFoundation.Client;
using TFSWIWatcher.BL.Configuration;

namespace TFSWIWatcher.BL.Providers
{
    public class ConfigCredentialsProvider : ICredentialsProvider
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(ConfigCredentialsProvider));

        #region ICredentialsProvider Members

        public ICredentials GetCredentials(Uri uri, ICredentials failedCredentials)
        {
            NetworkCredential configCredentials = ConfigCredentialsConfigurationSection.GetFromConfig().Credentials;

            if (CredentialsAreEqual(configCredentials, (NetworkCredential) failedCredentials))
            {
                _log.Error("Credentials from config are wrong, can not connect to team foundation server.");
                throw new ConfigurationErrorsException("The credentials for connecting to team foundation server are either wrong or do not have sufficient rights. Please specify other credentials in ConfigCredentialsConfigSection.");
            }

            return configCredentials;
        }

        public void NotifyCredentialsAuthenticated(Uri uri)
        {
            // not used by TFSWIWatcher
        }

        #endregion

        private static bool CredentialsAreEqual(NetworkCredential a, NetworkCredential b)
        {
            return a.Domain == b.Domain && a.Password == b.Password && a.Domain == b.Domain;
        }
    }
}