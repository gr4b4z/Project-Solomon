using Solomon.Base;
using Solomon.Handler.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Loader;

namespace Solomon.Worker
{
    public class InstanceMaker
    {
        public InstanceMaker(IJobInputContext jobContext, 
            IHandlerLogger handlerLogger, 
            IHandlerPackageManager packageManager)
        {
            JobContext = jobContext;
            Logger = handlerLogger;
            PackageManager = packageManager;
        }

        public IJobInputContext JobContext { get; }
        public IHandlerLogger Logger { get; }
        public IHandlerPackageManager PackageManager { get; }
 

        private Type GetAssemblyType(string handlerName)
        {
            var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(PackageManager.GetHandlerAssembly(handlerName));
            return assembly.GetTypes().FirstOrDefault(t => t.Name.Contains("StartupHandler"));
        }

        private ISolomonHandler CreateInstance(Type type)
        {
            var instance = Activator.CreateInstance(type) as ISolomonHandler;
            return instance;
        }

        private void SetProperties(Type type, ISolomonHandler instance, IReadOnlyDictionary<string, string> properties)
        {
            var existingProperties = type.GetProperties()
                .Where(prop => properties
                .Any(ip => ip.Key.Equals(prop.Name,StringComparison.OrdinalIgnoreCase)));
            foreach (var item in type.GetProperties())
            {
                var prop = properties.FirstOrDefault(p => p.Key.Equals(item.Name, StringComparison.OrdinalIgnoreCase));
                if (prop.Key!=null)
                {
                    item.SetValue(instance, prop.Value);
                }
                
            }
        }
       
        internal ISolomonHandler Build()
        {
            
            EnsureHandlerExists();
            var assemblyType = GetAssemblyType(JobContext.HandlerName);
            var instance = CreateInstance(assemblyType);
            SetProperties(assemblyType, instance, JobContext.InputParameters);
            return instance;

        }

        private async void EnsureHandlerExists()
        {
            var handler = JobContext.HandlerName;
            Logger.SystemLog(LogLevel.Info,$"Checking if handler  {handler} exists");
            
            if (!File.Exists(PackageManager.GetHandlerAssembly(handler)))
            {
                Logger.SystemLog(LogLevel.Info, $"Handler {JobContext.HandlerName}  doesn't  exists");
                await PackageManager.UpdateHandlerAsync(handler);
            }
            else
            {
                Logger.SystemLog(LogLevel.Info,$"Handler {JobContext.HandlerName}  exists");
            }
        }
    }
}
