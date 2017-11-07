using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Solomon.Base;
using Solomon.Worker;
using System;
using System.Text;
using System.Threading;

namespace Solomon.Logger.RabbitMq
{
    public class LoggerRabbitMq: BaseMQReciver
    {
        public Action<ISolomonLog> logMessageReceived;
        public override void Start()
        {
            base.Start();


            var queueName = channel.QueueDeclare().QueueName;
       
       

            channel.QueueBind(queue: queueName,
                              exchange: "logs",
                              routingKey: "");

            Console.WriteLine(" [*] Waiting for logs.");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);

                logMessageReceived?.Invoke(JsonConvert.DeserializeObject<ISolomonLog>(message));
                Thread.Sleep(3000);

            };
            channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);


        }
    }
}
