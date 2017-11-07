using Newtonsoft.Json;
using Solomon.Base;
using Solomon.Logger.RabbitMq;
using Solomon.Worker;
using System;
using System.Net;

namespace Solomon.Logger
{
    class CouchDbLogStore
    {
       public void SaveLog(ISolomonLog log)
        {
            var url = $"http://localhost:5984/logs01";
            var wc = new WebClient();
            wc.Headers.Add("Content-Type", "application/json");

            var z =   wc.UploadStringTaskAsync(url, JsonConvert.SerializeObject(log)).Result;
       
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var s = new CouchDbLogStore();
            using (var receiver = new LoggerRabbitMq())
            {
                
                receiver.Start();

                receiver.logMessageReceived = (message) =>
                {
                    Console.WriteLine($"{message.Timestamp}  {message.Name}  {message.Type}  {message.Level}   {message.Body} ");
                    s.SaveLog(message);
                    
                };

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}
