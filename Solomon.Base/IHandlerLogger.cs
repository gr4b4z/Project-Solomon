using System;

namespace Solomon.Handler.Base
{
    public interface IHandlerLogger
    {
        void SystemLog(LogLevel level, string url);
        void Log(LogLevel error, Exception somethingWasWrong);
    }
}
