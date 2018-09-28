using System;

namespace LagDaemon.Logging.Core
{
    public class DefaultLoggerFactory : ILoggerFactory
    {
        public ILogger CreateLogger {
            get
            {
                return Impl.ConsoleLogger.Build(Console.Out);
            }
        }
    }
}