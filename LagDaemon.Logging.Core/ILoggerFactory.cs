using System;
using System.Collections.Generic;
using System.Text;

namespace LagDaemon.Logging.Core
{
    public interface ILoggerFactory
    {
        ILogger CreateLogger { get; }
    }
}
