using Solomon.Handler.Base;

namespace Solomon.Base
{
    public interface ILoggerFactory
    {
        IHandlerLogger CreateLogLogger(string name, string type);
    }
}
