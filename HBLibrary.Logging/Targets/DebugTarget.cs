﻿using HBLibrary.Interface.Logging;
using HBLibrary.Interface.Logging.Configuration;
using HBLibrary.Interface.Logging.Formatting;
using HBLibrary.Interface.Logging.Statements;
using HBLibrary.Interface.Logging.Targets;
using HBLibrary.Logging.Configuration;
using HBLibrary.Logging.Formatter;
using System.Diagnostics;
using System.Runtime.Serialization;

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

    public LogLevel? LevelThreshold { get; }
    public ILogFormatter? Formatter { get; }
    public bool Enabled { get; }


    public DebugTarget(ILogFormatter? formatter) {
        Formatter = formatter;
        Enabled = Debugger.IsAttached && Debugger.IsLogging();
        if (Enabled) {
            Debug.Write(Logo);
            Debug.WriteLine(TargetName);
        }
    }

    public DebugTarget(LogLevel threshold, ILogFormatter? formatter) : this(formatter) {
        LevelThreshold = threshold;
    }

    public void WriteLog(ILogStatement log, ILogFormatter? formatter = null) {
        formatter ??= LogFormatters.DefaultDebug;
        Debug.WriteLine(formatter.Format(log));
    }
    public void Dispose() { }
}
