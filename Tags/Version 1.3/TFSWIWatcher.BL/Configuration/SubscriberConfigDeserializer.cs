using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Linq;

namespace TFSWIWatcher.BL.Configuration
{
    public class SubscriberConfigDeserializer
    {
        public static SubscriberConfig LoadFromFile(string filename)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException(string.Format("Could not find config file '{0}'.", filename), filename);

            XDocument doc = XDocument.Load(filename);

            return LoadFromXElement(doc.Root);
        }

        public static SubscriberConfig LoadFromXElement(XElement configElement)
        {
            SubscriberConfig config = new SubscriberConfig();
            config.XmlRootElement = configElement;
            config.ObserverAccountProviders = new ReadOnlyCollection<ProviderConfigSettings>(configElement.XPathSelectElements("ObserverAccountProviders/Provider").Select(ProviderConfigSettingsDeserializer.LoadFromXElement).Where(p => p.Enabled).ToList());
            config.NotifyProviders = new ReadOnlyCollection<ProviderConfigSettings>(configElement.XPathSelectElements("NotifyProviders/Provider").Select(ProviderConfigSettingsDeserializer.LoadFromXElement).Where(p => p.Enabled).ToList());

            XAttribute isDevAttribute = configElement.Attribute("isDev");
            if (isDevAttribute != null && "true".Equals(isDevAttribute.Value, StringComparison.InvariantCultureIgnoreCase))
                config.IsDevelopment = true;


            return config;
        }
    }
}