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
    public class DebugTarget : TargetWithHeader, ILogTarget {
        public const string TargetName =
              @"                                                                            |" + "\r\n" +
              @"    ____       __                   ______                      __          |" + "\r\n" +
              @"   / __ \___  / /_  __  ______ _   /_  __/___ __________ ____  / /_         |" + "\r\n" +
              @"  / / / / _ \/ __ \/ / / / __ `/    / / / __ `/ ___/ __ `/ _ \/ __/         |" + "\r\n" +
              @" / /_/ /  __/ /_/ / /_/ / /_/ /    / / / /_/ / /  / /_/ /  __/ /_           |" + "\r\n" +
              @"/_____/\___/_.___/\__,_/\__, /    /_/  \__,_/_/   \__, /\___/\__/           |" + "\r\n" +
              @"                       /____/                    /____/                     |" + "\r\n" +
              @"____________________________________________________________________________|" + "\r\n\r\n";

        public LogLevel LevelThreshold { get; set; }
        public bool Enabled { get; }
        public DebugTarget() { }
        public DebugTarget(LogLevel threshold) {
            LevelThreshold = threshold;
            Enabled = Debugger.IsAttached && Debugger.IsLogging();
            if (Enabled) {
                Debug.WriteLine(Logo);
                Debug.WriteLine(TargetName);
            }
        }

        public void WriteLog(LogStatement log, LogDisplayFormat displayFormat = LogDisplayFormat.Full) {
            Debug.WriteLine(log.Format(displayFormat));
        }
        public void Dispose() { }
    }
}
