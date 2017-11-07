using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Solomon.Base;
using Solomon.Base.Contant;
using Solomon.Base.Trigger;
using Solomon.Master.Model;
using Solomon.Worker;
using System;
using System.Text;

namespace Solomon.Master.RabbitMq
{
    public class MasterRabbitMQReciver : BaseMQReciver, INewJobQueue
    {

       public Action<ITriggerMessage> OnMessageFromTrigger;
       public Action<IJobOutputContext> OnJobResults;


        public void PublishJob(JobInputContext newJobContext)
        {
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(newJobContext));
            channel.BasicPublish(MqContants.ExchangeJobs, null, null, body);

        }

        public override void Start()
        {
            base.Start();

            channel.ExchangeDeclare(exchange: MqContants.ExchangeTrigger, type: "direct");
            channel.ExchangeDeclare(exchange: MqContants.ExchangeJobs, type: "direct");

            channel.QueueDeclare(queue: MqContants.TriggerQueue,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            channel.QueueDeclare(queue: MqContants.JobResultsQueue,
                             durable: true,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);



            channel.QueueBind(MqContants.JobResultsQueue, MqContants.ExchangeJobs, "");
            channel.QueueBind(MqContants.TriggerQueue, MqContants.ExchangeTrigger, "");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                if (ea.Exchange == "trigger")
                {
                    var message = Encoding.UTF8.GetString(ea.Body);
                    var jobContext = JsonConvert.DeserializeObject<TriggerMessage>(message);
                    OnMessageFromTrigger(jobContext);

                    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                }
                else
                {
                    Console.WriteLine("Not known exchange");
                }


            };
            channel.BasicConsume(queue: MqContants.TriggerQueue,autoAck: false, consumer: consumer);


        }
    }

}
