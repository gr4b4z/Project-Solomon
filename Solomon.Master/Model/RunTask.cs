using System;
using System.Collections.Generic;

namespace Solomon.Master.Model
{
    public class RunTask
    {
        public RunTask(Task task)
        {
            Name = task.Name;
            HandlerName = task.HandlerName;
            InputParameters = task.InputParameters;
            Order = task.Order;
            Completed = false;
        }
        public RunTask()
        {

        }
        public string Name { get; set; }
        public string HandlerName { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset EndTime { get; set; }
        public int Order { get; set; }
        public bool Completed { get; set; }
        public Dictionary<string, string> InputParameters { get; set; }
        public string Results{ get; set; }
        public Guid Id { get; internal set; }
    }
}
