using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using Solomon.Handler.Base;
 
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using Solomon.Base;

namespace Solomon.Worker
{
    public class MqContants
    {
        public static string ExchangeTrigger = "trigger";

        public static string ExchangeJobs = "jobs";

        public static string JobQueue = "jobQueue";
        public static string LogsExchange = "logs";


        public static string JobResultsQueue = "jobResults";

        public static string TriggerQueue = "triggerQueue";
    }
    public class BaseMQReciver : IDisposable, ILogQueue
    {
        public Action<IJobInputContext> NewJobHasArrived { get; set; }
        private IConnection connection;
        protected IModel channel;
        private readonly string handlersRepositoryUri;
        private readonly string jobRepository;

        
        public void Dispose()
        {

            if (channel != null) channel.Dispose();
            if (connection != null) connection.Dispose();
        }


        public virtual void Start()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            Console.WriteLine(connection.ToString());


            channel.ExchangeDeclare("logs", "fanout");
            //var body = Encoding.UTF8.GetBytes("");
            //channel.BasicPublish(exchange: "logs",
            //                     routingKey: "",
            //                     basicProperties: null,
            //                     body: body);

        }



        public void AddLog(ISolomonLog message)
        {
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

            channel.BasicPublish(exchange: "logs",
                                 routingKey: message.Level.ToString(),
                                 basicProperties: null,
                                 body: body);
            Console.WriteLine($"[DEBUG] {message.Timestamp}  {message.Name}  {message.Type}  {message.Level}   {message.Body} ");


        }
    }

}
