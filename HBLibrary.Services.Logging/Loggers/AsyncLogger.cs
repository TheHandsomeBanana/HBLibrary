using HBLibrary.Services.Logging.Statements;
using HBLibrary.Services.Logging.Targets;

namespace HBLibrary.Services.Logging.Loggers;
public class AsyncLogger : Logger, IAsyncLogger {
    protected AsyncLogger() { }
    internal AsyncLogger(string name) {
        Name = name;
    }

    public Task DebugAsync(string message) => LogInternalAsync(message, LogLevel.Debug);
    public Task ErrorAsync(string message) => LogInternalAsync(message, LogLevel.Error);
    public Task ErrorAsync(Exception exception) => LogInternalAsync(exception.ToString(), LogLevel.Error);
    public Task FatalAsync(string message) => LogInternalAsync(message, LogLevel.Fatal);
    public Task InfoAsync(string message) => LogInternalAsync(message, LogLevel.Info);
    public Task WarnAsync(string message) => LogInternalAsync(message, LogLevel.Warning);

    public static SemaphoreSlim SemaphoreSlim { get; } = new SemaphoreSlim(1);
    protected virtual async Task LogInternalAsync(string message, LogLevel level) {
        if (!IsEnabled)
            return;

        await SemaphoreSlim.WaitAsync();
        try {
            foreach (ILogTarget target in Configuration.Targets) {
                if (target.LevelThreshold > level)
                    continue;

                LogStatement log = new LogStatement(message, Name, level, DateTime.Now);
                target.WriteLog(log, Configuration.DisplayFormat);
            }

            foreach (IAsyncLogTarget target in Configuration.AsyncTargets) {
                if (target.LevelThreshold > level)
                    continue;

                LogStatement log = new LogStatement(message, Name, level, DateTime.Now);
                await target.WriteLogAsync(log, Configuration.DisplayFormat);
            }
        }
        finally {
            SemaphoreSlim.Release();
        }
    }
}

public class AsyncLogger<T> : AsyncLogger, IAsyncLogger<T> where T : class {
    internal AsyncLogger() {
        Name = typeof(T).Name;
    }
}
