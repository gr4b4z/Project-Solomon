using Solomon.Base;
using Solomon.Handler.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Solomon.Master.Model
{
    public class RunTree
    {
        public RunTree(Tree tree)
        {
            TreeId = tree.Id;
            Id = Guid.NewGuid();
            GlobalParameters = tree.GlobalParameters;
            Tasks = tree.Tasks.Select(t => new RunTask(t)).ToList();
        }
        public RunTree()
        {

        }
        public RunTask GetNext()
        {
            var run =  Tasks.Where(e => e.Completed == false).OrderBy(e => e.Order).FirstOrDefault();
            if (run == null) return null;
            run.StartTime = DateTimeOffset.UtcNow;
            return run;
        }
        public Guid Id { get; set; }
        public string TriggerName { get; set; }
        public string TreeId { get; set; }
        public IDictionary<string, string> GlobalParameters { get; set; }
        public List<RunTask> Tasks { get; set; }
        public Guid TriggerId { get; set; }

        internal void SetResults(IJobOutputContext jobOutput)
        {
            GlobalParameters = jobOutput.GlobalVariables;

            var task = Tasks.First(a => a.Id == jobOutput.JobId);
            task.EndTime = jobOutput.EndTime;
            task.Completed = true;
            task.Results = jobOutput.Output;
        }
    }
}
