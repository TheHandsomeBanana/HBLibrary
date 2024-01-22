using HBLibrary.NetFramework.Services.Logging.Configuration;
using HBLibrary.NetFramework.Services.Logging.Statements;
using HBLibrary.NetFramework.Services.Logging.Targets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HBLibrary.NetFramework.Services.Logging.Loggers {
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
            await SemaphoreSlim.WaitAsync();
            try {
                foreach (ILogTarget target in Configuration.Targets) {
                    if (target.LevelThreshold > level)
                        continue;

                    LogStatement log = new LogStatement(message, Name, level, DateTime.Now);
                    target.WriteLog(log, Configuration.DisplayFormat);
                }

                foreach(IAsyncLogTarget target in Configuration.AsyncTargets) {
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
}
