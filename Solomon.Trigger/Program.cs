using Newtonsoft.Json;
using Solomon.Base.Trigger;
using Solomon.Queue.RabbitMq;
using System;
using System.Text;
using System.Threading;

namespace Solomon.Trigger
{
    class Program
    {
        
        static void Main(string[] args)
        {
            var receiver = new RabbitMqTrigger();
            receiver.Start();


            var logger = new DefaultLoggerFactory(receiver);
 

            var triggerType = "trigger:mail";
            var triggerName = "my_small_maill";

            var log = logger.CreateLogLogger(triggerName, triggerType);

            log.SystemLog(Handler.Base.LogLevel.Info, "Started");
            log.SystemLog(Handler.Base.LogLevel.Info, "Queue has been declared");




            var z = 0;
            while (true)
            {
                Thread.Sleep(1000);
                log.SystemLog(Handler.Base.LogLevel.Info, "New message has been received");

                var  msg = new TriggerMessage
                {
                    Id = Guid.NewGuid(),
                    Name = triggerName,
                    Type = triggerType,
                    Body = $"Wiadomość email {z++}"
                };
                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(msg));

                receiver.TriggerNewMessage(msg);

            

               log.SystemLog(Handler.Base.LogLevel.Info, $"New message {msg.Id} has been published");



            }




        }
    }
}
