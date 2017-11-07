using Solomon.Worker.RabbitMq;
using System.IO;
using System.IO.Compression;

namespace Solomon.Worker
{
    public class HandlerPackageManager : IHandlerPackageManager
    {
        private RabbitMQReciver receiver;

        public HandlerPackageManager(RabbitMQReciver receiver)
        {
            this.receiver = receiver;
        }

        public string GetHandlerAssembly(string handlerName)
        {
            return Configuration.GetAssemblyPath(handlerName);
        }
        public async System.Threading.Tasks.Task UpdateHandlerAsync(string handlerName)
        {
          var handlerPackage = await  receiver.GetHandlerAsync(handlerName);
            SaveHandler(handlerName, handlerPackage);
        }
        public async void SaveHandler(string handlerName, byte[] handler)
        {
            var zippedHandlerPath = Path.Combine(Configuration.HandlersPath, handlerName);
            await File.WriteAllBytesAsync(zippedHandlerPath+".zip", handler);
            ZipFile.ExtractToDirectory(zippedHandlerPath + ".zip", zippedHandlerPath);
            File.Delete(zippedHandlerPath + ".zip");
        }
    }
}
