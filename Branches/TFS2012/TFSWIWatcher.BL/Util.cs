using System;
using System.IO;
using System.Reflection;

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

        public static T GetInstanceOfInterface<T>(string assemblyFullName, string typeNameToInstantiate)
        {
            if (!typeof(T).IsInterface)
                throw new ArgumentException("Generic type parameter <T> is not an interface.");

            Assembly assembly;

            try
            {
                assembly = Assembly.Load(new AssemblyName(assemblyFullName));
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Could not load Assembly " + assemblyFullName, ex);
            }

            object providerInstance;

            try
            {
                providerInstance = assembly.CreateInstance(typeNameToInstantiate);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Could not create instance of " + typeNameToInstantiate, ex);
            }

            if (providerInstance == null)
                throw new ArgumentException("Could not create instance of " + typeNameToInstantiate);

            if (!(providerInstance is T))
                throw new ArgumentException(string.Format("Class '{0}' does not implement {1}.", providerInstance.GetType().FullName, typeof(T).FullName));

            return (T)providerInstance;
        }
    }
}
