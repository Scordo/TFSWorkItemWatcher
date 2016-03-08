using System.Collections.ObjectModel;
using System.Xml.Linq;

namespace TFSWIWatcher.BL.Configuration
{
    public class SubscriberConfig
    {
        public XElement XmlRootElement { get; internal set; }
        public bool IsDevelopment { get; internal set; }
        public ReadOnlyCollection<ProviderConfigSettings> ObserverAccountProviders { get; internal set; }
        public ReadOnlyCollection<ProviderConfigSettings> NotifyProviders { get; internal set; }
    }
}
