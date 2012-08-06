using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using log4net;
using Microsoft.TeamFoundation.Client;
using TFSWIWatcher.BL.Providers;

namespace TFSWIWatcher.BL.Configuration
{
    public class ConfigSettingsConfigurationSection : ConfigurationSection
    {
        #region Non Public Members

        private static readonly ILog _log = LogManager.GetLogger(typeof(ConfigSettingsConfigurationSection));

        #endregion

        #region Config Properties

        [ConfigurationProperty("NotifyProviders", IsRequired = true)]
        [ConfigurationCollection(typeof(ProviderConfigSettingsCollection), AddItemName = "Provider")]
        protected ProviderConfigSettingsCollection AllNotifyProvidersSetting
        {
            get { return (ProviderConfigSettingsCollection)base["NotifyProviders"]; }
        }

        [ConfigurationProperty("ObserverAccountProviders", IsRequired = true)]
        [ConfigurationCollection(typeof(ProviderConfigSettingsCollection), AddItemName = "Provider")]
        protected ProviderConfigSettingsCollection AllObserverAccountProvidersSetting
        {
            get { return (ProviderConfigSettingsCollection)base["ObserverAccountProviders"]; }
        }

        [ConfigurationProperty("CredentialsProvider")]
        protected ProviderConfigSettings CredentialsProviderSetting
        {
            get { return (ProviderConfigSettings)base["CredentialsProvider"]; }
        }

        [ConfigurationProperty("isDev", IsRequired = false, DefaultValue = false)]
        public bool IsDev
        {
            get { return (bool) base["isDev"]; }
        }

        [ConfigurationProperty("teamServer", IsRequired = true)]
        public string TeamServer
        {
            get { return (string) base["teamServer"]; }
        }

        [ConfigurationProperty("localServicePort", IsRequired = false, DefaultValue = 50782)]
        public int LocalServicePort
        {
            get { return (int) base["localServicePort"]; }
        }

        #endregion

        #region Public Methods

        public List<INotifyProvider> GetNotifyProviders()
        {
            return GetNotifyProviders(true);
        }

        protected List<INotifyProvider> GetNotifyProviders(bool initialize)
        {
            if (!initialize)
                _log.Debug("Instanciating INotifyProviders");

            List<INotifyProvider> result = new List<INotifyProvider>();

            foreach (ProviderConfigSettings providerConfigSettings in AllNotifyProvidersSetting.OfType<ProviderConfigSettings>())
            {
                if (providerConfigSettings.Enabled)
                {
                    if (!initialize)
                        _log.Debug(string.Format("Instantiating INotifyProvider {0}", providerConfigSettings.ProviderClass));

                    INotifyProvider notifyProvider = Instancer.GetInstanceOfInterface<INotifyProvider>(providerConfigSettings.AssemblyName, providerConfigSettings.ProviderClass);

                    if (initialize)
                    {
                        _log.Debug(string.Format("Initializing INotifyProvider {0}", providerConfigSettings.ProviderClass));
                        notifyProvider.Initialize(providerConfigSettings.Parameters);
                    }

                    result.Add(notifyProvider);
                }   
            }

            return result;
        }

        public List<IObserverAccountProvider> GetObserverAccountProviders()
        {
            return GetObserverAccountProviders(true);
        }

        protected List<IObserverAccountProvider> GetObserverAccountProviders(bool initialize)
        {
            if (!initialize)
                _log.Debug("Instanciating IObserverAccountProvider");

            List<IObserverAccountProvider> result = new List<IObserverAccountProvider>();

            foreach (ProviderConfigSettings providerConfigSettings in AllObserverAccountProvidersSetting.OfType<ProviderConfigSettings>())
            {
                if (providerConfigSettings.Enabled)
                {
                    if (!initialize)
                        _log.Debug(string.Format("Instantiating IObserverAccountProvider {0}", providerConfigSettings.ProviderClass));

                    IObserverAccountProvider observerAccountProvider = Instancer.GetInstanceOfInterface<IObserverAccountProvider>(providerConfigSettings.AssemblyName, providerConfigSettings.ProviderClass);

                    if (initialize)
                    {
                        _log.Debug(string.Format("Initializing IObserverAccountProvider {0}", providerConfigSettings.ProviderClass));
                        observerAccountProvider.Initialize();
                    }

                    result.Add(observerAccountProvider);
                }
            }

            return result;
        }

        public ICredentialsProvider GetCredentialsProvider()
        {
            _log.Debug("Instanciating ICredentialsProvider");
            return Instancer.GetInstanceOfInterface<ICredentialsProvider>(CredentialsProviderSetting.AssemblyName, CredentialsProviderSetting.ProviderClass);
        }

        public static ConfigSettingsConfigurationSection GetFromConfig(string sectionName)
        {
            try
            {
                ConfigSettingsConfigurationSection config = (ConfigSettingsConfigurationSection)ConfigurationManager.GetSection(sectionName);

                if (config == null)
                {
                    _log.Error("Could not find ConfigSettingsSection.");
                    throw new ConfigurationErrorsException("Could not find ConfigSettingsSection.");
                }

                return config;
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Error while reading ConfigSettings from config file: {0}", sectionName, ex);
                throw;
            }
        }

        protected override void PostDeserialize()
        {
            if (!CredentialsProviderSetting.Deserialized)
                throw new ConfigurationErrorsException("Missing CredentialsProvider Section in config.");

            if (!AllObserverAccountProvidersSetting.Deserialized)
                throw new ConfigurationErrorsException("Missing ObserverAccountProviders Section in config.");

            if (GetObserverAccountProviders(false).Count == 0)
                throw new ConfigurationErrorsException("Please provide at least one enabled ObserverAccountProvider in ObserverAccountProviders Section in config.");

            if (!AllNotifyProvidersSetting.Deserialized)
                throw new ConfigurationErrorsException("Missing NotifyProviders Section in config.");

            if (GetNotifyProviders(false).Count == 0)
                throw new ConfigurationErrorsException("Please provide at least one enabled NotifyProvider in NotifyProviders Section in config.");
        }

        #endregion
    }
}