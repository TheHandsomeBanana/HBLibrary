using HBLibrary.NetFramework.Services.Logging.Configuration;
using HBLibrary.NetFramework.Services.Logging.Statements;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.NetFramework.Services.Logging.Targets {
    public class DebugTarget : ILogTarget {
        public LogLevel LevelThreshold { get; set; }

        public DebugTarget() { }
        public DebugTarget(LogLevel threshold) {
            LevelThreshold = threshold;
        }

        public void WriteLog(LogStatement log, LogDisplayFormat displayFormat = LogDisplayFormat.Full) {
            if(Debugger.IsLogging()) {
                Debug.WriteLine(log.Format(displayFormat));
            }
        }
        public void Dispose() { }
    }
}
