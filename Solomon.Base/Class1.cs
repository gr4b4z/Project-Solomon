using Solomon.Handler.Base;
using System;

namespace Solomon.Base.Contant
{
}

namespace Solomon.Base
{



    public class ISolomonLog
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public LogLevel Level { get; set; }
        public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;
        public string Body { get; set; }

    }
}
