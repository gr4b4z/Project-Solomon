using Newtonsoft.Json;
using RabbitMQ.Client;
using Solomon.Base.Trigger;
using Solomon.Worker;
using System.Text;

namespace Solomon.Trigger
{
    class RabbitMqTrigger : BaseMQReciver
    {

        public override void Start()
        {
            base.Start();
            channel.ExchangeDeclare(exchange: MqContants.ExchangeTrigger, type: "direct");

        }
        public void TriggerNewMessage(TriggerMessage context)
        {
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(context));

            channel.BasicPublish(exchange: MqContants.ExchangeTrigger,
                                 routingKey: "",
                                 basicProperties: null,
                                 body: body);
        
        }
    }
}
