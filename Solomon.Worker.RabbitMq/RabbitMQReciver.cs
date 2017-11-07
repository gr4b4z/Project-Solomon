using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Solomon.Base;
using Solomon.Handler.Base;
using Solomon.Worker.Base;
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Solomon.Worker.RabbitMq
{
    public class RabbitMQReciver : BaseMQReciver, IDisposable, IJobQueue, IHandlerService, ILogQueue
    {

        public Action<IJobInputContext> NewJobHasArrived { get; set; }
        private IConnection connection;
        protected IModel channel;
        private readonly string handlersRepositoryUri;
        private readonly string jobRepository;

        public RabbitMQReciver(string handlersRepositoryUri, string jobRepository)
        {
            this.handlersRepositoryUri = handlersRepositoryUri;
            this.jobRepository = jobRepository;
        }

        public virtual void Start()
        {
            base.Start();

            channel.QueueDeclare(queue: MqContants.JobQueue,
                                         durable: true,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

            channel.ExchangeDeclare(exchange: MqContants.ExchangeTrigger, type: "direct");
            channel.ExchangeDeclare(MqContants.LogsExchange, "fanout");



            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body);
                var jobContext = JsonConvert.DeserializeObject<IJobInputContext>(message);
                NewJobHasArrived(jobContext);
                channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);

            };
            //channel.BasicConsume(queue: "hello",
            //                     autoAck: true,
            //                     consumer: consumer);


        }



        public Task<byte[]> GetHandlerAsync(object handler)
        {
            using (var wc = new WebClient())
            {
                return wc.DownloadDataTaskAsync(String.Format(handlersRepositoryUri, handler));
            }
        }


        public Task<T> GetJobOutputAsync<T>(string taskName) where T : class
        {
            //move to queue
            throw new NotImplementedException();
        }

        public Task<T> GetRunOutputAsync<T>(Guid runId) where T : class
        {
            throw new NotImplementedException();
        }

        public Task SetJobResults(IJobOutputContext context)
        {
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(context));

            channel.BasicPublish(exchange: "",
                                 routingKey: MqContants.JobQueue,
                                 basicProperties: null,
                                 body: body);
            return Task.FromResult(true);
        }

        public void AddLog(ISolomonLog message)
        {
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

            channel.BasicPublish(exchange: "logs",
                                 routingKey: message.Level.ToString(),
                                 basicProperties: null,
                                 body: body);

        }
    }
}
