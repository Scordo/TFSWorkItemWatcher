using System;
using System.IO;

namespace TFSWIWatcher.BL
{
    public class Util
    {
        public static string GetPluginAssemblyFolderPath()
        {
            return Path.GetDirectoryName(GetPluginAssemblyFilePath());
        }

        public static string GetPluginAssemblyFilePath()
        {
            return new Uri(typeof(Util).Assembly.CodeBase).LocalPath;
        }
    }
}
