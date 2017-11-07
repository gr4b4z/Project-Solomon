using Solomon.Base;
using System.Threading.Tasks;

namespace Solomon.Handler.Base
{
    public interface ISolomonHandler
    {
        Task<IJobOutputContext> RunAsync(IJobInputContext context);
     }
}
