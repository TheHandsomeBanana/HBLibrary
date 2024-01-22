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
    public class Logger : ILogger {
        public ILoggerRegistry Registry { get; set; }
        public bool IsEnabled => Registry?.IsEnabled ?? true;
        public string Name { get; protected set; }
        public ILogConfiguration Configuration { get; set; } = LogConfiguration.Default;

        protected Logger() { }
        internal Logger(string name) {
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
                    if ((levelThreshold.HasValue && levelThreshold > level) 
                        || (target.LevelThreshold.HasValue && target.LevelThreshold > level))
                        continue;

                    LogStatement log = new LogStatement(message, Name, level, DateTime.Now);
                    target.WriteLog(log, Configuration.DisplayFormat);
                }
            }
        }

        public void Dispose() {
            Configuration.Dispose();
            Configuration = null;
        }
    }

    public class Logger<T> : Logger, ILogger<T> where T : class {
        internal Logger() {
            Name = typeof(T).Name;
        }
    }
}
