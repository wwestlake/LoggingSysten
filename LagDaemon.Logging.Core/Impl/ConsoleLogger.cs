using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LagDaemon.Logging.Core.Impl
{

    public class ConsoleLogger : ILogger
    {
        private TextWriter _writer;
        private IAuditLogger _audit;
        private IDebugLogger _debug;
        private IGeneralLogger _general;
        private IErrorLogger _error;
        private string _user;
        private string _role;
        private string _system;
        private string _component;

        public static ILogger Build(TextWriter writer)
        {
            var logger = new ConsoleLogger();
            var audit = new ConsoleAuditLogger();
            var debug = new ConsoleDebugLogger();
            var general = new ConsoleGeneralLogger();
            var error = new ConsoleErrorLogger();
            audit.Logger = debug.Logger = general.Logger = logger;
            audit.Writer = debug.Writer = general.Writer = Console.Out;

            var warning = new ConsoleWarningLogger();
            var important = new ConsoleImportantLogger();
            var critical = new ConsoleCriticalLogger();
            var exception = new ConsoleExceptionLogger();

            warning.Logger = important.Logger = critical.Logger = exception.Logger = logger;
            warning.Writer = important.Writer = critical.Writer = exception.Writer = Console.Out;

            error.Warning1 = warning;
            error.Important1 = important;
            error.Critical1 = critical;
            error.Exception1 = exception;

            logger.Audit = audit;
            logger.Debug = debug;
            logger.General = general;
            logger.Error = error;

            return logger;
        }

        public ConsoleLogger()
        {
        }

        public IAuditLogger AuditLogger => Audit;

        public IDebugLogger DebugLogger => Debug;

        public IGeneralLogger GeneralLogger => General;

        public IErrorLogger ErrorLogger => Error;

        public TextWriter Writer { get => _writer; set => _writer = value; }
        public IAuditLogger Audit { get => _audit; set => _audit = value; }
        public IDebugLogger Debug { get => _debug; set => _debug = value; }
        public IGeneralLogger General { get => _general; set => _general = value; }
        public IErrorLogger Error { get => _error; set => _error = value; }
        public string User { get => _user; set => _user = value; }
        public string Role { get => _role; set => _role = value; }
        public string System { get => _system; set => _system = value; }
        public string Component { get => _component; set => _component = value; }

        public ILogger Initialize(string user, string role, string system, string component)
        {
            AuditLogger.Initialize(user, role, system, component);
            DebugLogger.Initialize(user, role, system, component);
            GeneralLogger.Initialize(user, role, system, component);
            ErrorLogger.Initialize(user, role, system, component);
            this.User = user;
            this.Role = role;
            this.System = system;
            this.Component = component;
            return this;
        }
    }

    public class ConsoleAuditLogger : IAuditLogger
    {
        private TextWriter _writer;
        private ILogger _logger;
        private string _user;
        private string _role;
        private string _system;
        private string _component;

        public ConsoleAuditLogger()
        {
        }

        public TextWriter Writer { get => _writer; set => _writer = value; }
        public ILogger Logger { get => _logger; set => _logger = value; }
        public string User { get => _user; set => _user = value; }
        public string Role { get => _role; set => _role = value; }
        public string System { get => _system; set => _system = value; }
        public string Component { get => _component; set => _component = value; }

        public IAuditLogger Initialize(string user, string role, string system, string component)
        {
            this.User = user;
            this.Role = role;
            this.System = system;
            this.Component = component;
            return this;
        }

        public ILogger Write(string message)
        {
            Write("{0}:AUDIT:{1}:{2}:{3}:{4}:{5}", DateTime.Now, User, Role, System, Component, message);
            return Logger;
        }

        public ILogger Write(string format, params object[] args)
        {
            Writer.WriteLine(format, args);
            return Logger;
        }
    }

    public class ConsoleDebugLogger : IDebugLogger
    {
        private TextWriter _writer;
        private ILogger _logger;
        private string _user;
        private string _role;
        private string _system;
        private string _component;

        public ConsoleDebugLogger()
        {

        }

        public TextWriter Writer { get => _writer; set => _writer = value; }
        public ILogger Logger { get => _logger; set => _logger = value; }
        public string User { get => _user; set => _user = value; }
        public string Role { get => _role; set => _role = value; }
        public string System { get => _system; set => _system = value; }
        public string Component { get => _component; set => _component = value; }

        public IDebugLogger Initialize(string user, string role, string system, string component)
        {
            this.User = user;
            this.Role = role;
            this.System = system;
            this.Component = component;
            return this;
        }

        public ILogger Write(string message)
        {
            Write("{0}:DEBUG:{1}:{2}:{3}:{4}:{5}", DateTime.Now, User, Role, System, Component, message);
            return Logger;
        }

        public ILogger Write(string format, params object[] args)
        {
            Writer.WriteLine(format, args);
            return Logger;
        }
    }

    public class ConsoleGeneralLogger : IGeneralLogger
    {
        private TextWriter _writer;
        private ILogger _logger;
        private string _user;
        private string _role;
        private string _system;
        private string _component;

        public ConsoleGeneralLogger()
        {
        }

        public TextWriter Writer { get => _writer; set => _writer = value; }
        public ILogger Logger { get => _logger; set => _logger = value; }
        public string User { get => _user; set => _user = value; }
        public string Role { get => _role; set => _role = value; }
        public string System { get => _system; set => _system = value; }
        public string Component { get => _component; set => _component = value; }

        public IGeneralLogger Initialize(string user, string role, string system, string component)
        {
            this.User = user;
            this.Role = role;
            this.System = system;
            this.Component = component;
            return this;
        }

        public ILogger Write(string message)
        {
            Write("{0}:GENERAL:{1}:{2}:{3}:{4}:{5}", DateTime.Now, User, Role, System, Component, message);
            return Logger;
        }

        public ILogger Write(string format, params object[] args)
        {
            Writer.WriteLine(format, args);
            return Logger;
        }
    }

    public class ConsoleErrorLogger : IErrorLogger
    {
        private IWarningLogger _warning;
        private IImportantLogger _important;
        private ICriticalLogger _critical;
        private IExceptionLogger _exception;
        private string _user;
        private string _role;
        private string _system;
        private string _component;

        public ConsoleErrorLogger()
        {
        }

        public IWarningLogger Warning => Warning1;

        public IImportantLogger Important => Important1;

        public ICriticalLogger Critical => Critical1;

        public IExceptionLogger Exception => Exception1;

        public IWarningLogger Warning1 { get => _warning; set => _warning = value; }
        public IImportantLogger Important1 { get => _important; set => _important = value; }
        public ICriticalLogger Critical1 { get => _critical; set => _critical = value; }
        public IExceptionLogger Exception1 { get => _exception; set => _exception = value; }
        public string User { get => _user; set => _user = value; }
        public string Role { get => _role; set => _role = value; }
        public string System { get => _system; set => _system = value; }
        public string Component { get => _component; set => _component = value; }

        public IErrorLogger Initialize(string user, string role, string system, string component)
        {
            Warning.Initialize(user, role, system, component);
            Important.Initialize(user, role, system, component);
            Critical.Initialize(user, role, system, component);
            Exception.Initialize(user, role, system, component);
            this.User = user;
            this.Role = role;
            this.System = system;
            this.Component = component;
            return this;
        }
    }

    public class ConsoleWarningLogger : IWarningLogger
    {

        private TextWriter _writer;
        private ILogger _logger;
        private string _user;
        private string _role;
        private string _system;
        private string _component;

        public ConsoleWarningLogger()
        {
        }

        public TextWriter Writer { get => _writer; set => _writer = value; }
        public ILogger Logger { get => _logger; set => _logger = value; }
        public string User { get => _user; set => _user = value; }
        public string Role { get => _role; set => _role = value; }
        public string System { get => _system; set => _system = value; }
        public string Component { get => _component; set => _component = value; }

        public IWarningLogger Initialize(string user, string role, string system, string component)
        {
            this.User = user;
            this.Role = role;
            this.System = system;
            this.Component = component;
            return this;
        }

        public ILogger Write(string message)
        {
            Write("{0}:WARNING:{1}:{2}:{3}:{4}:{5}", DateTime.Now, User, Role, System, Component, message);
            return Logger;
        }

        public ILogger Write(string format, params object[] args)
        {
            Writer.WriteLine(format, args);
            return Logger;
        }

    }

    public class ConsoleImportantLogger : IImportantLogger
    {
        private TextWriter _writer;
        private ILogger _logger;
        private string _user;
        private string _role;
        private string _system;
        private string _component;

        public ConsoleImportantLogger()
        {
        }

        public TextWriter Writer { get => _writer; set => _writer = value; }
        public ILogger Logger { get => _logger; set => _logger = value; }
        public string User { get => _user; set => _user = value; }
        public string Role { get => _role; set => _role = value; }
        public string System { get => _system; set => _system = value; }
        public string Component { get => _component; set => _component = value; }

        public IImportantLogger Initialize(string user, string role, string system, string component)
        {
            this.User = user;
            this.Role = role;
            this.System = system;
            this.Component = component;
            return this;
        }

        public ILogger Write(string message)
        {
            Write("{0}:IMPORTANT:{1}:{2}:{3}:{4}:{5}", DateTime.Now, User, Role, System, Component, message);
            return Logger;
        }

        public ILogger Write(string format, params object[] args)
        {
            Writer.WriteLine(format, args);
            return Logger;
        }

    }

    public class ConsoleCriticalLogger : ICriticalLogger
    {
        private TextWriter _writer;
        private ILogger _logger;
        private string _user;
        private string _role;
        private string _system;
        private string _component;

        public ConsoleCriticalLogger()
        {
        }

        public TextWriter Writer { get => _writer; set => _writer = value; }
        public ILogger Logger { get => _logger; set => _logger = value; }
        public string User { get => _user; set => _user = value; }
        public string Role { get => _role; set => _role = value; }
        public string System { get => _system; set => _system = value; }
        public string Component { get => _component; set => _component = value; }

        public ICriticalLogger Initialize(string user, string role, string system, string component)
        {
            this.User = user;
            this.Role = role;
            this.System = system;
            this.Component = component;
            return this;
        }

        public ILogger Write(string message)
        {
            Write("{0}:CRITICAL:{1}:{2}:{3}:{4}:{5}", DateTime.Now, User, Role, System, Component, message);
            return Logger;
        }

        public ILogger Write(string format, params object[] args)
        {
            Writer.WriteLine(format, args);
            return Logger;
        }
    }

    public class ConsoleExceptionLogger : IExceptionLogger
    {
        private TextWriter _writer;
        private ILogger _logger;
        private string _user;
        private string _role;
        private string _system;
        private string _component;

        public ConsoleExceptionLogger()
        {
        }

        public TextWriter Writer { get => _writer; set => _writer = value; }
        public ILogger Logger { get => _logger; set => _logger = value; }
        public string User { get => _user; set => _user = value; }
        public string Role { get => _role; set => _role = value; }
        public string System { get => _system; set => _system = value; }
        public string Component { get => _component; set => _component = value; }

        public IExceptionLogger Initialize(string user, string role, string system, string component)
        {
            this.User = user;
            this.Role = role;
            this.System = system;
            this.Component = component;
            return this;
        }

        public ILogger Write(string message)
        {
            Write("{0}:EXCEPTION:{1}:{2}:{3}:{4}:{5}", DateTime.Now, User, Role, System, Component, message);
            return Logger;
        }

        public ILogger Write(string format, params object[] args)
        {
            Writer.WriteLine(format, args);
            return Logger;
        }

    }


}
