using System.Collections.Generic;

namespace Solomon.Master.Model
{
    public class Tree
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string[] TriggerNames { get; set; }
        public string TriggerName { get; set; }
        public Dictionary<string,string> GlobalParameters { get; set; }
        public List<Task> Tasks { get; set; }
    }
}
