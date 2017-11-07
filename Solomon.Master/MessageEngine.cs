using Solomon.Base;
using Solomon.Base.Trigger;
using Solomon.Handler.Base;
using Solomon.Master.Model;
using System.Linq;
using System.Text;

namespace Solomon.Master
{

    public class MessageEngine
    {
        private readonly IMessageStore messageStore;
        private readonly INewJobQueue newJobQueue;
        private readonly IHandlerLogger log;

        public MessageEngine(IMessageStore messageStore, INewJobQueue newJobQueue, IHandlerLogger log)
        {
            this.messageStore = messageStore;
            this.newJobQueue = newJobQueue;
            this.log = log;
        }


        private void TriggerNewJobs(ITriggerMessage message, Tree[] workflowsToTrigger)
        {
            foreach (var workflow in workflowsToTrigger)
            {
                var newRun = new RunTree(workflow)
                {
                    TriggerName = message.Name,
                    TriggerId = message.Id
                };

                var task = newRun.GetNext();
                var newJobContext = new JobInputContext(newRun, task, message.Body);
                newJobQueue.PublishJob(newJobContext);
                
                messageStore.SaveRunAsync(newRun);
            }
        }

        public async void ProcessMessageFromTriggerAsync(ITriggerMessage message)
        {
            log.SystemLog(LogLevel.Info, $"New message {message.Id} from trigger {message.Type}");

            await messageStore.SaveMessageAsync(message);
            var workflowsToTrigger = await messageStore.GetTreesByTriggerAsync(message.Id.ToString());
            TriggerNewJobs(message, workflowsToTrigger.ToArray());
        }

        public async void ProcesJobResultsAsync(IJobOutputContext jobOutput)
        {
            log.SystemLog(LogLevel.Info, $"Received job results {jobOutput.RunId} from  {jobOutput.JobId}");

            var run = await messageStore.GetRun(jobOutput.JobId);
            run.SetResults(jobOutput);
            var task =  run.GetNext();

            var newJobContext = new JobInputContext(run, task, jobOutput.Output);

            newJobQueue.PublishJob(newJobContext);
            log.SystemLog(LogLevel.Info, $"Next task has been published");
            await messageStore.SaveRunAsync(run);
        }


    }
}
