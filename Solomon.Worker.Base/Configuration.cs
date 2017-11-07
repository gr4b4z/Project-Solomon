using System;
using System.IO;

namespace Solomon.Worker
{
    public class Configuration
    {
        public static string HandlersPath
        {get
            {
             return AppDomain.CurrentDomain.BaseDirectory;
             
            }
        }

        public static string DataRepositoryUri { get; internal set; }
        public static string JobDataUri { get; internal set; }

        public static string GetAssemblyPath(string assemblyName)
        {
            return Path.Combine(HandlersPath, assemblyName + ".dll");
        }
    }
}
