using Solomon.Base;
using Solomon.Handler.Base;
using Solomon.Worker.Base;

namespace Solomon.Worker
{
    public class LoggerFactory : ILoggerFactory
    {
        public LoggerFactory(ILogQueue logQueue)
        {
            LogQueue = logQueue;
        }

        public ILogQueue LogQueue { get; }

        public IHandlerLogger CreateLogLogger(string name, string type)
        {
            throw new System.NotImplementedException();
        }

        public IHandlerLogger CreateRunLogger(IJobInputContext runContext)
        {
            return new HandlerLogger(LogQueue, runContext.RunId, runContext.JobId);
        }
    }
}
