using System;
using System.Collections.Generic;

namespace Solomon.Base
{   public interface IJobInputContext
    {
        Guid RunId { get; }
        Guid JobId { get; }
        string JobName { get; }
        IReadOnlyDictionary<string, string> InputParameters { get; }
        IDictionary<string, string> GlobalVariables { get; }
        DateTimeOffset StartTime { get; set; }
        string HandlerName { get; }
        string InputBody { get; }
    }
    public interface IJobOutputContext
    {
        Guid RunId { get; }
        Guid JobId { get; }
        IDictionary<string, string> GlobalVariables { get; }
        DateTimeOffset EndTime { get; set; }
        string Output { get; }


    }
    public interface IJobContext : IJobInputContext, IJobOutputContext
    {
      
    }
   
    public class JobContext : IJobContext
    {
        
        public static IJobOutputContext CreateJobOutput(Guid jobId, string body)
        {
            return new JobContext
            {
                JobId = jobId,
                Output = body
            };
        }
        public Guid RunId  {get;set;}

        public Guid JobId { get; set; }

        public string JobName { get; set; }

        public IReadOnlyDictionary<string, string> InputParameters { get; set; }

        public IDictionary<string, string> GlobalVariables { get; set; }

        public DateTimeOffset StartTime { get; set; }

        public string HandlerName { get; set; }

        public string InputBody { get; set; }
        public DateTimeOffset EndTime { get; set; }

        public string Output { get; set; }
     
    }
}
