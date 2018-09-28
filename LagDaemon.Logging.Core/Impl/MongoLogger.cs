using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LagDaemon.Logging.Core.Impl
{

    public class MongoLogger : ILogger
    {
        private IAuditLogger _audit;
        private IDebugLogger _debug;
        private IGeneralLogger _general;
        private IErrorLogger _error;
        private string _user;
        private string _role;
        private string _system;
        private string _component;

        public static ILogger Build()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            var configuration = builder.Build();
            var connectionString = configuration.GetConnectionString("MongoDbLogger");
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("SystemLog");
            

            var logger = new MongoLogger();
            var audit = new MongoAuditLogger();
            var debug = new MongoDebugLogger();
            var general = new MongoGeneralLogger();
            var error = new MongoErrorLogger();
            audit.Logger = debug.Logger = general.Logger = logger;
            audit.Db = debug.Db = general.Db = database;
            var warning = new MongoWarningLogger();
            var important = new MongoImportantLogger();
            var critical = new MongoCriticalLogger();
            var exception = new MongoExceptionLogger();

            warning.Logger = important.Logger = critical.Logger = exception.Logger = logger;
            warning.Db = important.Db = critical.Db = exception.Db = database;
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

        public MongoLogger()
        {
        }

        public IAuditLogger AuditLogger => Audit;

        public IDebugLogger DebugLogger => Debug;

        public IGeneralLogger GeneralLogger => General;

        public IErrorLogger ErrorLogger => Error;

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

    public class MongoAuditLogger : IAuditLogger
    {
        private IMongoDatabase _db;
        private ILogger _logger;
        private string _user;
        private string _role;
        private string _system;
        private string _component;

        public MongoAuditLogger()
        {
        }

        public ILogger Logger { get => _logger; set => _logger = value; }
        public string User { get => _user; set => _user = value; }
        public string Role { get => _role; set => _role = value; }
        public string System { get => _system; set => _system = value; }
        public string Component { get => _component; set => _component = value; }
        public IMongoDatabase Db { get => _db; set => _db = value; }

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
            Write("", DateTime.Now, User, Role, System, Component, message);
            return Logger;
        }

        public ILogger Write(string format, params object[] args)
        {
            var document = new BsonDocument
            {
                {"Date", BsonValue.Create(args[0]) },
                {"Type", BsonValue.Create("AUDIT") },
                {"User", BsonValue.Create(args[1]) },
                {"Role", BsonValue.Create(args[2]) },
                {"System", BsonValue.Create(args[3]) },
                {"Component", BsonValue.Create(args[4]) },
                {"Message", BsonValue.Create(args[5]) }
            };
            Write(document).Wait();
            return Logger;
        }

        private async Task Write(BsonDocument document)
        {
            var collection = Db.GetCollection<BsonDocument>("Audit");
            await collection.InsertOneAsync(document);
        }

    }

    public class MongoDebugLogger : IDebugLogger
    {
        private IMongoDatabase _db;
        private ILogger _logger;
        private string _user;
        private string _role;
        private string _system;
        private string _component;

        public MongoDebugLogger()
        {

        }

        public ILogger Logger { get => _logger; set => _logger = value; }
        public string User { get => _user; set => _user = value; }
        public string Role { get => _role; set => _role = value; }
        public string System { get => _system; set => _system = value; }
        public string Component { get => _component; set => _component = value; }
        public IMongoDatabase Db { get => _db; set => _db = value; }

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
            Write("", DateTime.Now, User, Role, System, Component, message);
            return Logger;
        }

        public ILogger Write(string format, params object[] args)
        {
            var document = new BsonDocument
            {
                {"Date", BsonValue.Create(args[0]) },
                {"Type", BsonValue.Create("DEBUG") },
                {"User", BsonValue.Create(args[1]) },
                {"Role", BsonValue.Create(args[2]) },
                {"System", BsonValue.Create(args[3]) },
                {"Component", BsonValue.Create(args[4]) },
                {"Message", BsonValue.Create(args[5]) }
            };
            Write(document).Wait();
            return Logger;
        }

        private async Task Write(BsonDocument document)
        {
            var collection = Db.GetCollection<BsonDocument>("Audit");
            await collection.InsertOneAsync(document);
        }
    }

    public class MongoGeneralLogger : IGeneralLogger
    {
        private IMongoDatabase _db;
        private ILogger _logger;
        private string _user;
        private string _role;
        private string _system;
        private string _component;

        public MongoGeneralLogger()
        {
        }

        public ILogger Logger { get => _logger; set => _logger = value; }
        public string User { get => _user; set => _user = value; }
        public string Role { get => _role; set => _role = value; }
        public string System { get => _system; set => _system = value; }
        public string Component { get => _component; set => _component = value; }
        public IMongoDatabase Db { get => _db; set => _db = value; }

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
            Write("", DateTime.Now, User, Role, System, Component, message);
            return Logger;
        }

        public ILogger Write(string format, params object[] args)
        {
            var document = new BsonDocument
            {
                {"Date", BsonValue.Create(args[0]) },
                {"Type", BsonValue.Create("GENERAL") },
                {"User", BsonValue.Create(args[1]) },
                {"Role", BsonValue.Create(args[2]) },
                {"System", BsonValue.Create(args[3]) },
                {"Component", BsonValue.Create(args[4]) },
                {"Message", BsonValue.Create(args[5]) }
            };
            Write(document);
            return Logger;
        }

        private async Task Write(BsonDocument document)
        {
            var collection = Db.GetCollection<BsonDocument>("Audit");
            await collection.InsertOneAsync(document);
            return;
        }
    }

    public class MongoErrorLogger : IErrorLogger
    {
        private IWarningLogger _warning;
        private IImportantLogger _important;
        private ICriticalLogger _critical;
        private IExceptionLogger _exception;
        private string _user;
        private string _role;
        private string _system;
        private string _component;

        public MongoErrorLogger()
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

    public class MongoWarningLogger : IWarningLogger
    {
        private IMongoDatabase _db;
        private ILogger _logger;
        private string _user;
        private string _role;
        private string _system;
        private string _component;

        public MongoWarningLogger()
        {
        }

        public ILogger Logger { get => _logger; set => _logger = value; }
        public string User { get => _user; set => _user = value; }
        public string Role { get => _role; set => _role = value; }
        public string System { get => _system; set => _system = value; }
        public string Component { get => _component; set => _component = value; }
        public IMongoDatabase Db { get => _db; set => _db = value; }

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
            Write("", DateTime.Now, User, Role, System, Component, message);
            return Logger;
        }

        public ILogger Write(string format, params object[] args)
        {
            var document = new BsonDocument
            {
                {"Date", BsonValue.Create(args[0]) },
                {"Type", BsonValue.Create("WARNING") },
                {"User", BsonValue.Create(args[1]) },
                {"Role", BsonValue.Create(args[2]) },
                {"System", BsonValue.Create(args[3]) },
                {"Component", BsonValue.Create(args[4]) },
                {"Message", BsonValue.Create(args[5]) }
            };
            Write(document).Wait();
            return Logger;
        }

        private async Task Write(BsonDocument document)
        {
            var collection = Db.GetCollection<BsonDocument>("Audit");
            await collection.InsertOneAsync(document);
        }

    }

    public class MongoImportantLogger : IImportantLogger
    {
        private IMongoDatabase _db;
        private ILogger _logger;
        private string _user;
        private string _role;
        private string _system;
        private string _component;

        public MongoImportantLogger()
        {
        }

        public ILogger Logger { get => _logger; set => _logger = value; }
        public string User { get => _user; set => _user = value; }
        public string Role { get => _role; set => _role = value; }
        public string System { get => _system; set => _system = value; }
        public string Component { get => _component; set => _component = value; }
        public IMongoDatabase Db { get => _db; set => _db = value; }

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
            Write("", DateTime.Now, User, Role, System, Component, message);
            return Logger;
        }

        public ILogger Write(string format, params object[] args)
        {
            var document = new BsonDocument
            {
                {"Date", BsonValue.Create(args[0]) },
                {"Type", BsonValue.Create("IMPORTANT") },
                {"User", BsonValue.Create(args[1]) },
                {"Role", BsonValue.Create(args[2]) },
                {"System", BsonValue.Create(args[3]) },
                {"Component", BsonValue.Create(args[4]) },
                {"Message", BsonValue.Create(args[5]) }
            };
            Write(document).Wait();
            return Logger;
        }

        private async Task Write(BsonDocument document)
        {
            var collection = Db.GetCollection<BsonDocument>("Audit");
            await collection.InsertOneAsync(document);
        }

    }

    public class MongoCriticalLogger : ICriticalLogger
    {
        private IMongoDatabase _db;
        private ILogger _logger;
        private string _user;
        private string _role;
        private string _system;
        private string _component;

        public MongoCriticalLogger()
        {
        }

        public ILogger Logger { get => _logger; set => _logger = value; }
        public string User { get => _user; set => _user = value; }
        public string Role { get => _role; set => _role = value; }
        public string System { get => _system; set => _system = value; }
        public string Component { get => _component; set => _component = value; }
        public IMongoDatabase Db { get => _db; set => _db = value; }

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
            Write("", DateTime.Now, User, Role, System, Component, message);
            return Logger;
        }

        public ILogger Write(string format, params object[] args)
        {
            var document = new BsonDocument
            {
                {"Date", BsonValue.Create(args[0]) },
                {"Type", BsonValue.Create("CRITICAL") },
                {"User", BsonValue.Create(args[1]) },
                {"Role", BsonValue.Create(args[2]) },
                {"System", BsonValue.Create(args[3]) },
                {"Component", BsonValue.Create(args[4]) },
                {"Message", BsonValue.Create(args[5]) }
            };
            Write(document).Wait();
            return Logger;
        }

        private async Task Write(BsonDocument document)
        {
            var collection = Db.GetCollection<BsonDocument>("Audit");
            await collection.InsertOneAsync(document);
        }
    }

    public class MongoExceptionLogger : IExceptionLogger
    {
        private IMongoDatabase _db;
        private ILogger _logger;
        private string _user;
        private string _role;
        private string _system;
        private string _component;

        public MongoExceptionLogger()
        {
        }

        public ILogger Logger { get => _logger; set => _logger = value; }
        public string User { get => _user; set => _user = value; }
        public string Role { get => _role; set => _role = value; }
        public string System { get => _system; set => _system = value; }
        public string Component { get => _component; set => _component = value; }
        public IMongoDatabase Db { get => _db; set => _db = value; }

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
            Write("", DateTime.Now, User, Role, System, Component, message);
            return Logger;
        }

        public ILogger Write(string format, params object[] args)
        {
            var document = new BsonDocument
            {
                {"Date", BsonValue.Create(args[0]) },
                {"Type", BsonValue.Create("EXCEPTION") },
                {"User", BsonValue.Create(args[1]) },
                {"Role", BsonValue.Create(args[2]) },
                {"System", BsonValue.Create(args[3]) },
                {"Component", BsonValue.Create(args[4]) },
                {"Message", BsonValue.Create(args[5]) }
            };
            Write(document).Wait();
            return Logger;
        }

        private async Task Write(BsonDocument document)
        {
            var collection = Db.GetCollection<BsonDocument>("Audit");
            await collection.InsertOneAsync(document);
        }

    }


}
