using System.Collections.Generic;

namespace TFSWIWatcher.BL.Providers
{
    public interface INotifyProvider
    {
        void Initialize();
        void Notify(List<string> observerAccounts, WorkItemChangedContext context);
    }
}