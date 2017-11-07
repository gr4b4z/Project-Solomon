using System;
using System.Collections.Generic;
using System.Text;
using Solomon.Base.Trigger;
using Solomon.Handler.Base;
using Solomon.Master.Model;
using Solomon.Base;
using System.Threading.Tasks;

namespace Solomon.Master
{
    public interface IMessageStore
    {
        Task<RunTree> GetRun(Guid jobId);
        Task<IEnumerable<Tree>> GetTreesByTriggerAsync(string name);
        object GetWorkflowsMessageToTigger();
        System.Threading.Tasks.Task SaveMessageAsync(ITriggerMessage message);
        System.Threading.Tasks.Task SaveRunAsync(RunTree newRun);
    }
}
