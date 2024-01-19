using HBLibrary.NetFramework.Services.Logging.Configuration;
using HBLibrary.NetFramework.Services.Logging.Statements;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.NetFramework.Services.Logging.Targets {
    public class AsyncMethodTarget : IAsyncLogTarget, IEquatable<AsyncMethodTarget> {
        public AsyncLogStatementDelegate Method { get; private set; }
        public LogLevel LevelThreshold { get; set; }

        public AsyncMethodTarget(AsyncLogStatementDelegate method, LogLevel minLevel) {
            Method = method;
            LevelThreshold = minLevel;
        }

        public Task WriteLogAsync(LogStatement log, LogDisplayFormat format = LogDisplayFormat.Full) => Method.Invoke(log, format);

        public void Dispose() {
            Method = null;
        }

        public bool Equals(AsyncMethodTarget other) => Method == other.Method;
        public override bool Equals(object obj) {
            return obj is AsyncMethodTarget amt && Equals(amt);
        }

        public override int GetHashCode() {
            return Method.GetHashCode();
        }


        public static bool operator ==(AsyncMethodTarget a, AsyncMethodTarget b) => a.Equals(b); 
        public static bool operator !=(AsyncMethodTarget a, AsyncMethodTarget b) => !(a == b);
    }
}
