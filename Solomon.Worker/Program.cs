using Solomon.Base;
using Solomon.Worker.RabbitMq;

namespace Solomon.Worker
{
    class Program
    {
        static void Main(string[] args)
        {
            var master = "http://localhost:7878/";
            var receiver = new RabbitMQReciver(master+"/handlers/{0}", master + "/rundata/{0}");
            

            var worker = new WorkerStartup(receiver, new HandlerPackageManager(receiver),new LoggerFactory(receiver));
            worker.StartWorker();
            receiver.Start();


        }
    }
}
