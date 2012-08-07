using System;
using System.ServiceProcess;
using log4net;
using log4net.Config;

namespace TFSWIWatcher.Service
{
    static class Program
    {
        #region Non Public Members

        private static readonly ILog _log = LogManager.GetLogger(typeof(Program));
        private static Watcher _watcher;

        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            XmlConfigurator.Configure();

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            if (Environment.UserInteractive)
            {
                _log.Debug("Running in console mode.");

                _watcher = new Watcher();
                _watcher.Start();
                Console.WriteLine("Press Enter to stop the service...");
                Console.ReadLine();
                _watcher.Stop();

                _log.Debug("Finished in console mode.");
            }
            else
            {
                _log.Debug("Running in service mode.");
                ServiceBase.Run(new[] { new MainService() });
            }
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            _log.ErrorFormat("Unhandled error: {0}", e.ExceptionObject);
        }
    }
}