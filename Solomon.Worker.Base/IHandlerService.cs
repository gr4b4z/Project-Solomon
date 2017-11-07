using System.Threading.Tasks;

namespace Solomon.Worker.Base
{
    public interface IHandlerService
    {
        Task<byte[]> GetHandlerAsync(object handler);

    }
}
