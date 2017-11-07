using Solomon.Base;
using Solomon.Handler.Base;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Solomon.Handler.ServiceNow
{
    public class StartupHandler : ISolomonHandler
    {
       
        public string ServiceNowUrl { get; set; }
        public string AuthToken { get; set; }
        public string Query { get; set; }

        public StartupHandler()
        {

        }
        
        public IHandlerLogger Logger;

        public async Task<IJobOutputContext> RunAsync(IJobInputContext context)
        {
            try
            {


                using (WebClient wc = new WebClient())
                {
                    var url = new Uri(ServiceNowUrl + $"api/now/v1/table/incident?{Query}");
                    Logger.SystemLog(LogLevel.Info, ServiceNowUrl);
                    var jsonString = wc.DownloadStringTaskAsync(url);

                   // context.Body = jsonString;
                    return JobContext.CreateJobOutput(context.JobId,await jsonString);
                }
            }
            catch (WebException exc)
            {
                Logger.SystemLog(LogLevel.Info, "Can't connect");
                throw;
            }
        }
    }

}
