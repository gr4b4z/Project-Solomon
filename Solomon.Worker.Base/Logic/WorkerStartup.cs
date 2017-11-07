using Solomon.Base;
using Solomon.Handler.Base;
using Solomon.Worker.Base;
using System;

namespace Solomon.Worker
{


    public class WorkerStartup:IDisposable
    {
        private readonly IJobQueue jobQueue;
        private readonly IHandlerPackageManager handlerPackageManager;
        private readonly ILoggerFactory loggerFactory;

        public WorkerStartup(IJobQueue jobQueue,IHandlerPackageManager handlerPackageManager, ILoggerFactory loggerFactory )
        {
            this.jobQueue = jobQueue;
            this.handlerPackageManager = handlerPackageManager;
            this.loggerFactory = loggerFactory;
        }

        public void Dispose()
        {
            if (jobQueue != null)
            { }
        }
        public void StartWorker()
        {
            jobQueue.NewJobHasArrived = HandleJob;

        }
        private async void HandleJob(IJobInputContext context)
        {

            IHandlerLogger handlerLogger = loggerFactory.CreateLogLogger(null, null);

            try
            {
                
                handlerLogger.SystemLog(LogLevel.Info,"Start Creating instance");

                var instanceMaker = new InstanceMaker(context, handlerLogger, handlerPackageManager);
                var handler = instanceMaker.Build();

                handlerLogger.SystemLog(LogLevel.Info,"Preparing Run");

                context.StartTime = DateTime.UtcNow;

                var results = await handler.RunAsync(context);

                results.EndTime = DateTime.UtcNow;

                await jobQueue.SetJobResults(results);


            }
            catch (Exception somethingWasWrong)
            {
                handlerLogger.Log(LogLevel.Error,somethingWasWrong);
            }

        }
    }
}
