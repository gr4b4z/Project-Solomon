//using RabbitMQ.Client;
//using RabbitMQ.Client.Events;
//using System;
//using System.Text;
//using Solomon.Handler.Base;

//namespace Solomon.Worker
//{
//    public class RabbitMQLogsReceiver: RabbitMQReciver
//    {
//        private readonly RabbitMQReciver rabbitMQReciver;
//        public Action<IJobInputContext> NewLogHasArrived { get; set; }

//        public RabbitMQLogsReceiver(RabbitMQReciver rabbitMQReciver):base("","")
//        {
//            this.rabbitMQReciver = rabbitMQReciver;
//        }
//        public override void Start()
//        {
//            base.Start();

//            channel.ExchangeDeclare("logs", "fanout");

//            var queueName = channel.QueueDeclare().QueueName;
//            channel.QueueBind(queue: queueName,exchange: "logs",routingKey: "");

//            var consumer = new EventingBasicConsumer(channel);
//            consumer.Received += (model, ea) =>
//            {
//                var body = ea.Body;
//                var message = Encoding.UTF8.GetString(body);
//                Console.WriteLine(" [x] {0}", message);
//            };
//            channel.BasicConsume(queue: queueName,
//                                 autoAck: true,
//                                 consumer: consumer);



//        }


//    }

//}
