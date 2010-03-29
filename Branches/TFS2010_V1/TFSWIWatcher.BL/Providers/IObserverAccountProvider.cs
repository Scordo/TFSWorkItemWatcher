using System.Collections.Generic;

namespace TFSWIWatcher.BL.Providers
{
    public interface IObserverAccountProvider
    {
        void Initialize();
        List<string> GetObservers(WorkItemChangedContext context);
    }
}
