using System;

namespace LagDaemon.Logging.Core
{
    public interface ILogger
    {
        ILogger Initialize(string user, string role, string system, string component);
        IAuditLogger AuditLogger { get; }
        IDebugLogger DebugLogger { get; }
        IGeneralLogger GeneralLogger { get; }
        IErrorLogger ErrorLogger { get; }
    }

    public interface IAuditLogger
    {
        IAuditLogger Initialize(string user, string role, string system, string component);
        ILogger Write(string message);
        ILogger Write(string format, params object[] args);
    }

    public interface IDebugLogger
    {
        IDebugLogger Initialize(string user, string role, string system, string component);
        ILogger Write(string message);
        ILogger Write(string format, params object[] args);

    }

    public interface IGeneralLogger
    {
        IGeneralLogger Initialize(string user, string role, string system, string component);
        ILogger Write(string message);
        ILogger Write(string format, params object[] args);
    }

    public interface IErrorLogger
    {
        IErrorLogger Initialize(string user, string role, string system, string component);
        IWarningLogger Warning { get; }
        IImportantLogger Important { get; }
        ICriticalLogger Critical { get; }
        IExceptionLogger Exception { get; }
    }

    public interface IWarningLogger
    {
        IWarningLogger Initialize(string user, string role, string system, string component);
        ILogger Write(string message);
        ILogger Write(string format, params object[] args);
    }

    public interface IImportantLogger
    {
        IImportantLogger Initialize(string user, string role, string system, string component);
        ILogger Write(string message);
        ILogger Write(string format, params object[] args);
    }

    public interface ICriticalLogger
    {
        ICriticalLogger Initialize(string user, string role, string system, string component);
        ILogger Write(string message);
        ILogger Write(string format, params object[] args);

    }

    public interface IExceptionLogger
    {
        IExceptionLogger Initialize(string user, string role, string system, string component);
        ILogger Write(string message);
        ILogger Write(string format, params object[] args);
    }

}
