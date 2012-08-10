using System;
using System.Configuration;
using System.Xml.Linq;

namespace TFSWIWatcher.BL.Configuration
{
    public class ProviderConfigSettingsDeserializer
    {
        public static ProviderConfigSettings LoadFromXElement(XElement element)
        {
            ProviderConfigSettings settings = new ProviderConfigSettings();

            XAttribute assemblyNameAttribute = element.Attribute("assemblyName");
            if (assemblyNameAttribute == null)
                throw new ConfigurationErrorsException("Could not find assemblyName attribute");

            settings.AssemblyName = assemblyNameAttribute.Value;


            XAttribute providerClassAttribute = element.Attribute("providerClass");
            if (providerClassAttribute == null)
                throw new ConfigurationErrorsException("Could not find providerClass attribute");

            settings.ProviderClass = providerClassAttribute.Value;

            XAttribute parametersAttribute = element.Attribute("parameters");
            if (parametersAttribute != null)
                settings.Parameters = parametersAttribute.Value;

            XAttribute enabledAttribute = element.Attribute("enabled");
            if (enabledAttribute != null && "true".Equals(enabledAttribute.Value, StringComparison.InvariantCultureIgnoreCase))
                settings.Enabled = true;
            
            return settings;
        }
    }
}