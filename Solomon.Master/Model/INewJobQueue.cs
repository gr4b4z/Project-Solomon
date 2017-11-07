using Solomon.Base.Trigger;
using System;

namespace Solomon.Master.Model
{
    public interface INewJobQueue
    {
        void PublishJob(JobInputContext newJobContext);
    }
}
