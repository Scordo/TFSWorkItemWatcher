using System.Collections.Generic;
using System.Xml.Linq;

namespace TFSWIWatcher.BL.Providers
{
    public interface IObserverAccountProvider
    {
        void Initialize(XElement configRootElement);
        List<string> GetObservers(WorkItemChangedContext context);
    }
}
