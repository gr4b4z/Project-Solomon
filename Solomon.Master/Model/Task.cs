using System.Collections.Generic;

namespace Solomon.Master.Model
{
    public class Task
    {
        public string Name { get; set; }
        public string HandlerName { get; set; }
        public int Order { get; set; }
        public Dictionary<string, string> InputParameters { get; set; }
    }
}
