using HBLibrary.NetFramework.Services.Logging.Configuration;
using HBLibrary.NetFramework.Services.Logging.Loggers;
using HBLibrary.NetFramework.Services.Logging.Statements;
using HBLibrary.NetFramework.Services.Logging.Target;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.NetFramework.Services.Logging {
    public class StandardLogger : LoggerBase, ILogger {
        protected StandardLogger() {
            Configuration = Factory.Configuration;
        }
        internal StandardLogger(string category) : this() {
            this.Category = category;
        }

        public void Configure(LogConfigurationDelegate configMethod) {
            LogConfigurationBuilder builder = new LogConfigurationBuilder();
            foreach (LogTarget target in Configuration.Targets)
                builder.AddTarget(target);

            Configuration = configMethod.Invoke(builder);
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

        protected virtual void LogInternal(string message, LogLevel level) {
            foreach (LogTarget target in Configuration.Targets) {
                if (target.Level > level)
                    continue;

                LogStatement log = new LogStatement(message, Category, level, DateTime.Now);
                switch (target.Value) {
                    case LogStatementDelegate statementMethod:
                        statementMethod.Invoke(log);
                        break;
                    case LogStringDelegate stringMethod:
                        stringMethod.Invoke(GetFormattedString(log));
                        break;
                    case AsyncLogStatementDelegate statementAsyncMethod:
                        statementAsyncMethod.Invoke(log).Wait();
                        break;
                    case AsyncLogStringDelegate stringAsyncMethod:
                        stringAsyncMethod.Invoke(GetFormattedString(log)).Wait();
                        break;
                    case string filePath:
                        using (StreamWriter sw = new StreamWriter(filePath, true))
                            sw.WriteLine(GetFormattedString(log));
                        break;
                }
            }
        }
    }

    public class StandardLogger<T> : StandardLogger, ILogger<T> where T : class {
        internal StandardLogger() : base() {
            Category = typeof(T).Name;
        }
    }
}
