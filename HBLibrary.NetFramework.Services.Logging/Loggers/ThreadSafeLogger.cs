using HBLibrary.NetFramework.Services.Logging.Target;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.NetFramework.Services.Logging.Loggers {
    public sealed class ThreadSafeLogger : StandardLogger, ILogger {
        public ThreadSafeLogger(string category) : base(category) { }

        private static readonly object lockObj = new object();
        protected override void LogInternal(string message, LogLevel level) {
            lock(lockObj) {
                base.LogInternal(message, level);
            }
        }
    }

    public sealed class ThreadSafeLogger<T> : StandardLogger<T>, ILogger<T> where T : class {
        private static readonly object lockObj = new object();
        protected override void LogInternal(string message, LogLevel level) {
            lock (lockObj) {
                base.LogInternal(message, level);
            }
        }
    }

}
