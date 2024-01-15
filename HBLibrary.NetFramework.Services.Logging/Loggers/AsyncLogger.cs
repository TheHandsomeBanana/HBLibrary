using HBLibrary.NetFramework.Services.Logging.Configuration;
using HBLibrary.NetFramework.Services.Logging.Statements;
using HBLibrary.NetFramework.Services.Logging.Target;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HBLibrary.NetFramework.Services.Logging.Loggers {
    public class AsyncLogger : LoggerBase, IAsyncLogger {

        protected AsyncLogger() { }
        internal AsyncLogger(string category) {
            Category = category;
        }

        public void Configure(LogConfigurationDelegate configMethod) {
            LogConfigurationBuilder builder = new LogConfigurationBuilder();
            foreach (LogTarget target in Configuration.Targets)
                builder.AddTarget(target);

            Configuration = configMethod.Invoke(builder);
        }

        public Task Debug(string message) => LogInternal(message, LogLevel.Debug);
        public Task Error(string message) => LogInternal(message, LogLevel.Error);
        public Task Error(Exception exception) => LogInternal(exception.ToString(), LogLevel.Error);
        public Task Fatal(string message) => LogInternal(message, LogLevel.Fatal);
        public Task Info(string message) => LogInternal(message, LogLevel.Info);
        public Task Warn(string message) => LogInternal(message, LogLevel.Warning);

        public static SemaphoreSlim SemaphoreSlim { get; } = new SemaphoreSlim(1);
        protected virtual async Task LogInternal(string message, LogLevel level) {
            await SemaphoreSlim.WaitAsync();
            try {
                foreach (LogTarget target in Configuration.Targets) {
                    if (target.Level > level)
                        continue;

                    LogStatement log = new LogStatement(message, Category, level, DateTime.Now);
                    switch (target.Value) {
                        case LogStatementDelegate statementMethod:
                            statementMethod.Invoke(log);
                            break;
                        case AsyncLogStatementDelegate statementAsyncMethod:
                            await statementAsyncMethod.Invoke(log);
                            break;
                        case string filePath:
                            using (StreamWriter sw = new StreamWriter(filePath, true))
                                await sw.WriteLineAsync(GetFormattedString(log));
                            break;
                    }
                }
            }
            finally {
                SemaphoreSlim.Release();
            }
        }
    }

    public class AsyncLogger<T> : AsyncLogger, IAsyncLogger<T> where T : class {
        internal AsyncLogger() {
            Category = typeof(T).Name;
        }
    }
}
