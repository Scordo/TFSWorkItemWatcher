using System.Collections.Generic;
using System.Xml.Linq;

namespace TFSWIWatcher.BL.Providers
{
    public interface INotifyProvider
    {
        void Initialize(XElement configRootElement);
        void Notify(List<string> observerAccounts, WorkItemChangedContext context);
    }
}