using System;
using LagDaemon.Logging.Core;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            LogManager mgr = new LogManager();
            mgr.Factory.CreateLogger.Initialize("wwestlake", "admin", "ConsoleTest", "Program").GeneralLogger.Write("This is test");


            Console.ReadKey();
        }
    }
}
