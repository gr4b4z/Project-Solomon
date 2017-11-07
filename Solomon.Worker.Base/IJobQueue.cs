using Solomon.Base;
using Solomon.Handler.Base;
using System;
using System.Threading.Tasks;

namespace Solomon.Worker.Base
{
    public interface IJobQueue
    {
        Action<IJobInputContext> NewJobHasArrived { get; set; }
        Task<T> GetJobOutputAsync<T>(string taskName) where T : class;
        Task<T> GetRunOutputAsync<T>(Guid runId) where T : class;
        Task SetJobResults(IJobOutputContext context);
    }
}
