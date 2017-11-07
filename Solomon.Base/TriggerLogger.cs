using Solomon.Base;
using Solomon.Handler.Base;
 using System;

namespace Solomon.Queue.RabbitMq
{
    public interface ILogStore
    {
        void SaveLog(ISolomonLog log);
    }

    public interface IJobStore
    {
        void SaveTrigger();
        void SaveLog(ISolomonLog log);
    }

    
    public class DefaultLoggerFactory : Base.ILoggerFactory
    {
        private readonly ILogQueue queue;

        public DefaultLoggerFactory(ILogQueue queue)
        {
            this.queue = queue;
        }
        public IHandlerLogger CreateLogLogger(string name, string type)
        {
            return new TriggerLogger(name, type, queue);

        }
    }

    public class TriggerLogger : IHandlerLogger
    {
        private readonly string name;
        private readonly string type;
        private readonly ILogQueue queue;

        public TriggerLogger(string name, string type, ILogQueue queue)
        {
            this.name = name;
            this.type = type;
            this.queue = queue;
        }
        public void SystemLog(LogLevel level, string Body)
        {
            
           queue.AddLog(new ISolomonLog
           {
               Type = type,
               Name = name,
               Level = level,
               Body = Body
           });
        }

        public void Log(LogLevel error, Exception somethingWasWrong)
        {
            SystemLog(error, somethingWasWrong.ToString());
        }
    }
}


