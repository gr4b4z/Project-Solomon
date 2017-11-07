using Solomon.Base;
using Solomon.Handler.Base;
using System;
using System.Collections.Generic;

namespace Solomon.Master.Model
{
    public class JobInputContext : IJobInputContext
    {
        public JobInputContext(RunTree tree, RunTask task, string body)
        {
            RunId = tree.Id;
            JobId = task.Id;
            JobName = task.Name;
            InputParameters = task.InputParameters;
            GlobalVariables = tree.GlobalParameters;
            StartTime = task.StartTime;
            HandlerName = task.HandlerName;
            InputBody = body;
        }

        public Guid RunId { get; set; }

        public Guid JobId { get; set; }

        public string JobName { get; set; }

        public IReadOnlyDictionary<string, string> InputParameters { get; set; }

        public IDictionary<string, string> GlobalVariables { get; set; }

        public DateTimeOffset StartTime { get; set; }

        public string HandlerName { get; set; }

        public string InputBody { get; set; }
    }
}
