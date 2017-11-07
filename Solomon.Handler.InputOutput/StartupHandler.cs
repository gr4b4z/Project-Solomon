using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Solomon.Base;
using Solomon.Handler.Base;
using System;
using System.Threading.Tasks;

namespace Solomon.Handler.InputOutput
{

    public class StartupHandler : ISolomonHandler
    {

        public string Transformation { get; set; }
        private string[] BaseImports = { "Newtonsoft.Json", "System.Collections.Generic", "System.Linq" };

        public StartupHandler()
        {

        }

        public IHandlerLogger Logger;

        public async Task<IJobOutputContext> RunAsync(IJobInputContext context)
        {
            try
            {
                if (Transformation == null)
                    throw new ArgumentException("Transformation is null");
               
                if (Transformation.Contains("INPUT_STRING"))
                    throw new ArgumentException("NO INPUT_STRING");
                var preprocessing = Transformation.Replace("INPUT_STRING",context.InputBody);

                var returnData =  
                    await CSharpScript.EvaluateAsync(
                        Transformation,
                        ScriptOptions.Default.WithImports(BaseImports)
                        ) as string;

                return JobContext.CreateJobOutput(context.JobId, returnData);

            }
            catch (CompilationErrorException e)
            {
                Logger.SystemLog(LogLevel.Error, e.Diagnostics.ToString());
                throw;
            }

        }
    }
}
