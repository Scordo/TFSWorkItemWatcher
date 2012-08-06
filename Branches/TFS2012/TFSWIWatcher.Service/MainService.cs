using System.ServiceProcess;

namespace TFSWIWatcher.Service
{
    public partial class MainService : ServiceBase
    {
        private Watcher _watcher;

        public MainService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _watcher = new Watcher();
            _watcher.Start();
        }

        protected override void OnStop()
        {
            _watcher.Stop();
        }
    }
}