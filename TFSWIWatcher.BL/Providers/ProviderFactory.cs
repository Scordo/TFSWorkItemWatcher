using System;
using System.Reflection;
using System.Configuration;
using log4net;
using Microsoft.TeamFoundation.Client;

namespace TFSWIWatcher.BL.Providers
{
    public static class ProviderFactory
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(ProviderFactory));

        #region Public Methods

        public static IObserverAccountProvider GetObserverAccountProvider()
        {
            _log.Debug("Determining IObserverAccountProvider");
            return GetConfiguredProviderInstance<IObserverAccountProvider>("ObserverAccountProvider");
        }

        public static INotifyProvider GetNotifyProvider()
        {
            _log.Debug("Determining INotifyProvider");
            return GetConfiguredProviderInstance<INotifyProvider>("NotifyProvider");
        }

        public static ICredentialsProvider GetCredentialsProvider()
        {
            _log.Debug("Determining ICredentialsProvider");
            return GetConfiguredProviderInstance<ICredentialsProvider>("CredentialsProvider");
        }

        #endregion

        #region Non Public Methods

        private static T GetConfiguredProviderInstance<T>(string sectionName)
        {
            if (!typeof(T).IsInterface)
                throw new ArgumentException("Generic type parameter <T> is not an interface.");

            ProviderConfig configSection = (ProviderConfig)ConfigurationManager.GetSection(sectionName);

            if (configSection == null)
                throw new ConfigurationErrorsException(string.Format("Could not find ProviderSection '{0}'.", sectionName));

            Assembly assembly;

            try
            {
                assembly = Assembly.Load(new AssemblyName(configSection.AssemblyName));
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Could not load Assembly " + configSection.AssemblyName, ex);
            }

            object providerInstance;

            try
            {
                providerInstance = assembly.CreateInstance(configSection.ProviderClass);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Could not create instance of " + configSection.ProviderClass, ex);
            }

            if (providerInstance == null)
                throw new ArgumentException("Could not create instance of " + configSection.ProviderClass);

            if (!(providerInstance is T))
                throw new ArgumentException(string.Format("Class '{0}' does not implement {1}.", providerInstance.GetType().FullName, typeof(T).FullName));

            return (T) providerInstance;
        }

        #endregion
    }
}