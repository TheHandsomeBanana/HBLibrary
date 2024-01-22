using HBLibrary.Services.Logging.Configuration;

namespace HBLibrary.Services.Logging;
public interface ILogger : IDisposable {
    ILoggerRegistry? Registry { get; internal set; }
    ILogConfiguration Configuration { get; internal set; }
    string Name { get; }
    void Debug(string message);
    void Info(string message);
    void Warn(string message);
    void Error(string message);
    void Error(Exception exception);
    void Fatal(string message);
}

public interface ILogger<T> : ILogger where T : class {
}

public enum LogLevel {
    Debug,
    Info,
    Warning,
    Error,
    Fatal
}
