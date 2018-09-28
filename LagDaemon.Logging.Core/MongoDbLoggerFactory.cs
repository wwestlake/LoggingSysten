using System;
using System.Collections.Generic;
using System.Text;

namespace LagDaemon.Logging.Core
{
    public class MongoDbLoggerFactory : ILoggerFactory
    {
        public ILogger CreateLogger
        {
            get
            {
                return Impl.MongoLogger.Build();
            }
        }
    }
}
