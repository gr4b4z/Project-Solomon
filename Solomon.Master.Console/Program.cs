using Solomon.Base.Trigger;
using Solomon.Infrastructure;
using Solomon.Master.RabbitMq;
using Solomon.Queue.RabbitMq;
using System;
using System.Collections.Generic;

namespace Solomon.Master
{
   public class Program
    {

        static  void Main(string[] args)
        {
            using (var receiver = new MasterRabbitMQReciver())
            {

                var logger = new DefaultLoggerFactory(receiver);
                var triggerType = "master";
                var triggerName = "master01";
                var log = logger.CreateLogLogger(triggerName, triggerType);
                var messageStorage = new CouchDbMessageStore();

                var engine = new MessageEngine(messageStorage, receiver, log);



                receiver.OnMessageFromTrigger = (msg) =>
                {
                   engine.ProcessMessageFromTriggerAsync(msg);

                };
                receiver.OnJobResults = (jobResults) =>
                {
                    engine.ProcesJobResultsAsync(jobResults);
                };
                receiver.Start();
                
                

                Console.ReadKey();
                //receiver.OnNewMessageWasTriggered

            }

        }
    }
  
}
