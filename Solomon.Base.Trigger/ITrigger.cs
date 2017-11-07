using System;
using System.Collections.Generic;
using System.Text;

namespace Solomon.Base.Trigger
{
    public interface ITriggerMessage
    {
        DateTimeOffset ReceivedTime { get; set; }
        string Body { get; set; }
        string Name { get; set; }
        string Type { get; set; }
        Guid Id { get; set; }
    }
    public class TriggerMessage: ITriggerMessage
    {
        public TriggerMessage()
        {
            ReceivedTime = DateTimeOffset.Now;
        }
        public DateTimeOffset ReceivedTime { get; set; }
        public string Body { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public Guid Id { get; set; }
    }
    public interface ITrigger
    {

    }
}
