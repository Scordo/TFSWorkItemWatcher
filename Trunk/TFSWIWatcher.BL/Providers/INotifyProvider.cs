using System.Collections.Generic;

namespace TFSWIWatcher.BL.Providers
{
    public interface INotifyProvider
    {
        void Initialize(string parameters);
        void Notify(List<string> observerAccounts, WorkItemChangedContext context);
    }
}