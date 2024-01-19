using HBLibrary.NetFramework.Services.Logging.Configuration;
using HBLibrary.NetFramework.Services.Logging.Statements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.NetFramework.Services.Logging.Targets {
    public class MethodTarget : ILogTarget, IEquatable<MethodTarget> {
        public LogLevel LevelThreshold { get; set; }
        public LogStatementDelegate Method { get; private set; }

        public MethodTarget(LogStatementDelegate method, LogLevel minLevel) {
            LevelThreshold = minLevel;
            Method = method;
        }

        public void WriteLog(LogStatement log, LogDisplayFormat format = LogDisplayFormat.Full) => Method?.Invoke(log, format);

        public void Dispose() {
            Method = null;
        }

        public bool Equals(MethodTarget other) => Method == other.Method;

        public override bool Equals(object obj) {
            return obj is MethodTarget mt && Equals(mt);
        }

        public override int GetHashCode() {
            return Method.GetHashCode();
        }

        public static bool operator ==(MethodTarget a, MethodTarget b) => a.Equals(b);
        public static bool operator !=(MethodTarget a, MethodTarget b) => !(a == b);
    }
}
