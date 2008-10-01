using System;
using System.Configuration;
using log4net;

namespace TFSWIWatcher.BL.Providers
{
    public class ProviderConfig : ConfigurationSection
    {
        #region Non Public Members

        private static readonly ILog _log = LogManager.GetLogger(typeof(ProviderConfig));

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the name of the assembly.
        /// </summary>
        /// <value>The name of the assembly.</value>
        [ConfigurationProperty("assemblyName", IsRequired = true)]
        public string AssemblyName
        {
            get { return (string)base["assemblyName"]; }
            set { base["assemblyName"] = value; }
        }

        /// <summary>
        /// Gets or sets the adapter provider class.
        /// </summary>
        /// <value>The adapter provider class.</value>
        [ConfigurationProperty("providerClass", IsRequired = true)]
        public string ProviderClass
        {
            get { return (string)base["providerClass"]; }
            set { base["providerClass"] = value; }
        }

        #endregion

        #region Public Methods

        public static ProviderConfig GetFromConfig(string sectionName)
        {
            try
            {
                return (ProviderConfig)ConfigurationManager.GetSection(sectionName);
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Error while reading ProviderConfig from config file: {0}", ex);
                throw;
            }
        }

        #endregion
    }
}