using System;
using System.Configuration;
using log4net;

namespace TFSWIWatcher.BL.Configuration
{
    //public class XMLObserverAccountProviderConfigurationSection : ConfigurationSection
    //{
    //    #region Non Public Members

    //    private static readonly ILog _log = LogManager.GetLogger(typeof(XMLObserverAccountProviderConfigurationSection));

    //    #endregion

    //    #region Properties

    //    [ConfigurationProperty("file", IsRequired = true)]
    //    public string ConfigurationFile
    //    {
    //        get { return (string) base["file"];}
    //    }

    //    #endregion

    //    #region Public Methods

    //    public static XMLObserverAccountProviderConfigurationSection GetFromConfig()
    //    {
    //        return GetFromConfig(null);
    //    }

    //    public static XMLObserverAccountProviderConfigurationSection GetFromConfig(string configSectionName)
    //    {
    //        if (configSectionName == null || configSectionName.Trim().Length == 0)
    //            configSectionName = "XMLObserverAccountConfig";

    //        try
    //        {
    //            XMLObserverAccountProviderConfigurationSection config = (XMLObserverAccountProviderConfigurationSection)ConfigHelper.GetSection(configSectionName);

    //            if (config == null)
    //            {
    //                _log.ErrorFormat("Could not find XMLObserverAccountProviderConfigurationSection: {0}.", configSectionName);
    //                throw new ConfigurationErrorsException(string.Format("Could not find XMLObserverAccountProviderConfigurationSection: {0}.", configSectionName));
    //            }

    //            return config;
    //        }
    //        catch (Exception ex)
    //        {
    //            _log.ErrorFormat("Error while reading XMLObserverAccountProviderConfigurationSection '{0}' from config file: {1}", configSectionName, ex);
    //            throw;
    //        }
    //    }

    //    #endregion
    //}
}