using HBLibrary.Interface.Logging;
using HBLibrary.Interface.Logging.Configuration;
using HBLibrary.Interface.Logging.Statements;
using HBLibrary.Interface.Logging.Targets;
using HBLibrary.Logging.Configuration;
using HBLibrary.Logging.Targets;

namespace HBLibrary.Logging.Loggers;
public class Logger : ILogger {
    public ILoggerRegistry? Registry { get; set; }
    public bool IsEnabled => Registry?.IsEnabled ?? true;
    public string Name { get; protected set; }
    public ILogConfiguration Configuration { get; set; } = LogConfiguration.Default;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    protected Logger() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    internal Logger(string name) {
        Name = name;
    }

    public void Debug(string message) {
        LogInternal(message, LogLevel.Debug);
    }

    public void Error(string message) {
        LogInternal(message, LogLevel.Error);
    }

    public void Error(Exception exception) {
        LogInternal(exception.ToString(), LogLevel.Debug);
    }

    public void Fatal(string message) {
        LogInternal(message, LogLevel.Fatal);
    }

    public void Info(string message) {
        LogInternal(message, LogLevel.Info);
    }

    public void Warn(string message) {
        LogInternal(message, LogLevel.Warning);
    }

    private static readonly object lockObj = new();
    protected virtual void LogInternal(string message, LogLevel level) {
        if (!IsEnabled)
            return;

        lock (lockObj) {
            // set right threshold --> Global layer > logger layer > target layer
            LogLevel? levelThreshold = Registry?.GlobalConfiguration.LevelThreshold ?? Configuration.LevelThreshold;

            // Concat global targets if registry contains logger
            IEnumerable<ILogTarget> allTargets = Registry != null
                ? Configuration.Targets.Concat(Registry.GlobalConfiguration.Targets)
                : Configuration.Targets;

            foreach (ILogTarget target in allTargets) {
                // Check for threshold global or per target, no threshold = always log
                if (levelThreshold.HasValue && levelThreshold > level
                    || target.LevelThreshold.HasValue && target.LevelThreshold > level)
                    continue;

                LogStatement log = new LogStatement(message, Name, level, DateTime.Now);
                target.WriteLog(log, Configuration.Formatter);
            }
        }
    }

    public void Dispose() {
        GC.SuppressFinalize(this);
        Configuration.Dispose();
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        Configuration = null;
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
    }
}

public class Logger<T> : Logger, ILogger<T> where T : class {
    internal Logger() {
        Name = typeof(T).Name;
    }
}
