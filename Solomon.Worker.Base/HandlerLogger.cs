using Solomon.Base;
using Solomon.Handler.Base;
using Solomon.Worker.Base;
using System;

namespace Solomon.Worker
{
    class HandlerLogger : IHandlerLogger
    {
        private readonly ILogQueue logQueue;
        private readonly string runId;
        private readonly string jobId;

        public HandlerLogger(ILogQueue logQueue,Guid runId, Guid jobId)
        {
            this.logQueue = logQueue;
            this.runId = runId.ToString();
            this.jobId = jobId.ToString();
        }
        public void SystemLog(LogLevel level, string url)
        {
            logQueue.AddLog(new Solomon.Base.ISolomonLog());
                }

        public void Log(LogLevel error, Exception somethingWasWrong)
        {
            logQueue.AddLog(new Solomon.Base.ISolomonLog());
        }
    }
}
