using HBLibrary.NetFramework.Services.Logging.Configuration;
using HBLibrary.NetFramework.Services.Logging.Loggers;
using HBLibrary.NetFramework.Services.Logging.Statements;
using HBLibrary.NetFramework.Services.Logging.Targets;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.NetFramework.Services.Logging {
    public class StandardLogger : ILogger {
        public string Name { get; protected set; }
        public ILogConfiguration Configuration { get; set; } = LogConfiguration.Default;

        protected StandardLogger() { }
        internal StandardLogger(string name) {
            this.Name = name;
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

        private static readonly object lockObj = new object();
        protected virtual void LogInternal(string message, LogLevel level) {
            lock (lockObj) {
                foreach (ILogTarget target in Configuration.Targets) {
                    if (target.LevelThreshold > level)
                        continue;

                    LogStatement log = new LogStatement(message, Name, level, DateTime.Now);
                    target.WriteLog(log, Configuration.DisplayFormat);
                }
            }
        }

        protected string GetFormattedString(LogStatement log) {
            switch (Configuration.DisplayFormat) {
                case LogDisplayFormat.MessageOnly:
                    return log.ToString();
                case LogDisplayFormat.Minimal:
                    return log.ToMinimalString();
                case LogDisplayFormat.Full:
                    return log.ToFullString();
                default:
                    throw new NotSupportedException(Configuration.DisplayFormat.ToString());
            }
        }

        public void Dispose() {
            Configuration.Dispose();
            Configuration = null;
        }
    }

    public class StandardLogger<T> : StandardLogger, ILogger<T> where T : class {
        internal StandardLogger() {
            Name = typeof(T).Name;
        }
    }
}
