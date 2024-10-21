using HBLibrary.Interface.Logging;
using HBLibrary.Interface.Logging.Configuration;
using HBLibrary.Interface.Logging.Statements;
using HBLibrary.Interface.Logging.Targets;
using HBLibrary.Logging.Configuration;
using System.Diagnostics;

namespace HBLibrary.Logging.Targets;
public sealed class DebugTarget : TargetWithHeader, ILogTarget {
    public const string TargetName =
          @"                                                                            |" + "\n" +
          @"    ____       __                   ______                      __          |" + "\n" +
          @"   / __ \___  / /_  __  ______ _   /_  __/___ __________ ____  / /_         |" + "\n" +
          @"  / / / / _ \/ __ \/ / / / __ `/    / / / __ `/ ___/ __ `/ _ \/ __/         |" + "\n" +
          @" / /_/ /  __/ /_/ / /_/ / /_/ /    / / / /_/ / /  / /_/ /  __/ /_           |" + "\n" +
          @"/_____/\___/_.___/\__,_/\__, /    /_/  \__,_/_/   \__, /\___/\__/           |" + "\n" +
          @"                       /____/                    /____/                     |" + "\n" +
          @"____________________________________________________________________________|" + "\n";

    public LogLevel? LevelThreshold { get; } = null;
    public bool Enabled { get; }
    public DebugTarget() {
        Enabled = Debugger.IsAttached && Debugger.IsLogging();
        if (Enabled) {
            Debug.Write(Logo);
            Debug.WriteLine(TargetName);
        }
    }

    public DebugTarget(LogLevel threshold) : this() {
        LevelThreshold = threshold;
    }

    public void WriteLog(LogStatement log, LogDisplayFormat displayFormat = LogDisplayFormat.Full) {
        Debug.WriteLine(log.Format(displayFormat));
    }
    public void Dispose() { }
}
