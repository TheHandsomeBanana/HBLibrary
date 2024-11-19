using HBLibrary.Interface.Logging;
using HBLibrary.Interface.Logging.Configuration;
using HBLibrary.Interface.Logging.Formatting;
using HBLibrary.Interface.Logging.Statements;
using HBLibrary.Interface.Logging.Targets;
using HBLibrary.Logging.Configuration;
using HBLibrary.Logging.Formatter;
using System.Runtime.Serialization;

namespace HBLibrary.Logging.Targets;
public sealed class ConsoleTarget : TargetWithHeader, ILogTarget {
    public const string TargetName =
          @"                                                                            |" + "\n" +
          @"   ______                       __        ______                      __    |" + "\n" +
          @"  / ____/___  ____  _________  / /__     /_  __/___ __________ ____  / /_   |" + "\n" +
          @" / /   / __ \/ __ \/ ___/ __ \/ / _ \     / / / __ `/ ___/ __ `/ _ \/ __/   |" + "\n" +
          @"/ /___/ /_/ / / / (__  ) /_/ / /  __/    / / / /_/ / /  / /_/ /  __/ /_     |" + "\n" +
          @"\____/\____/_/ /_/____/\____/_/\___/    /_/  \__,_/_/   \__, /\___/\__/     |" + "\n" +
          @"                                                       /____/               |" + "\n" +
          @"____________________________________________________________________________|" + "\n\n";

    public LogLevel? LevelThreshold { get; }
    public ILogFormatter? Formatter { get; }

    public ConsoleTarget(ILogFormatter? formatter = null) {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write(Logo);
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine(TargetName);
        Console.ForegroundColor = ConsoleColor.White;
    }

    public ConsoleTarget(LogLevel threshold, ILogFormatter? formatter = null) : this(formatter) {
        LevelThreshold = threshold;
    }

    public void WriteLog(ILogStatement log, ILogFormatter? formatter = null) {
        formatter ??= LogFormatters.DefaultConsole;

        Console.WriteLine((string)formatter.Format(log) + "\n");
        Console.ForegroundColor = ConsoleColor.White;
    }

    public void Dispose() { }
}
