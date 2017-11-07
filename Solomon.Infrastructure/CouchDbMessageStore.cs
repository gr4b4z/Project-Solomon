using Solomon.Master;
using System;
using Solomon.Base.Trigger;
using Solomon.Master.Model;
using System.Collections.Generic;
using Solomon.Base;
using System.Net;
using Newtonsoft.Json;
using System.Linq;
 using MyCouch;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace Solomon.Infrastructure
{
  
    public class UnderscoreIdContractResolver: CamelCasePropertyNamesContractResolver
    {
        public static readonly UnderscoreIdContractResolver Instance = new UnderscoreIdContractResolver();

        protected override string ResolvePropertyName(string propertyName)
        {
            if (propertyName == "_id") propertyName = "id";
            return base.ResolvePropertyName(propertyName);
        }
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
                if (property.PropertyName.Equals("Id", StringComparison.OrdinalIgnoreCase))
                {
                    property.PropertyName = "_id";
                }

            return property;
        }
         
    }
    public class CouchDbMessageStore : IMessageStore// : IMessageStore
    {
         private string url = "http://localhost:5984/";
         public CouchDbMessageStore()
        {
         
        }
        public async System.Threading.Tasks.Task<RunTree> GetRun(Guid jobId)
        {
            using (var client = new MyCouchClient(url, "runs"))
            {
                var response =  await client.Documents.GetAsync(jobId.ToString());

                return ToModel<RunTree>(response.Content);
            }
        }

        public async System.Threading.Tasks.Task<IEnumerable<Tree>> GetTreesByTriggerAsync(string name)
        {
            using (var client = new MyCouchClient(url, "trees"))
            {
                var response = await client.Documents.GetAsync("123");
                return JsonConvert.DeserializeObject<Tree[]>(response.Content);
            }
        }

        public object GetWorkflowsMessageToTigger()
        {
            throw new NotImplementedException();
        }
        //"Id":
        private string ToEntity<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj, new JsonSerializerSettings { ContractResolver = UnderscoreIdContractResolver.Instance });
        }
        private T ToModel<T>(string msg)
        {
            return JsonConvert.DeserializeObject<T>(msg, new JsonSerializerSettings { ContractResolver = UnderscoreIdContractResolver.Instance });
        }
        public async System.Threading.Tasks.Task SaveMessageAsync(ITriggerMessage message)
        {
            using (var client = new MyCouchClient(url, "trigger_events"))
            {

                var msg = ToEntity(message);
                var results =  await client.Documents.PostAsync(msg);
                var r = results.IsSuccess;
            }
        }

        public async System.Threading.Tasks.Task SaveRunAsync(RunTree newRun)
        {
            using (var client = new MyCouchClient(url, "runs"))
            {
               var results =  await client.Documents.PutAsync(newRun.Id.ToString(),ToEntity(newRun));
            }
            
        }
    }
}
