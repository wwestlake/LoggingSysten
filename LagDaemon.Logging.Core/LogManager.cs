using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace LagDaemon.Logging.Core
{
    public class LogManager
    {
        public LogManager()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            var configuration = builder.Build();

            string factory = configuration.GetSection("LoggingSystem")["Factory"];
            if (string.IsNullOrEmpty(factory))
            {
                Factory = new DefaultLoggerFactory();
            } else
            {
                Factory = Assembly.GetExecutingAssembly().CreateInstance(factory) as ILoggerFactory;
            }
        }

        public LogManager(ILoggerFactory customFactory)
        {
            Factory = customFactory;
        }

        public ILoggerFactory Factory
        {
            get;
            private set;
        }

    }
}
