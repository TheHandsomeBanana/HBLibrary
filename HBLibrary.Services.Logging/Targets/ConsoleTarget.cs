using HBLibrary.Services.Logging.Configuration;
using HBLibrary.Services.Logging.Statements;

namespace HBLibrary.Services.Logging.Targets;
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

    public LogLevel? LevelThreshold { get; } = null;

    public ConsoleTarget() {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write(Logo);
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine(TargetName);
        Console.ForegroundColor = ConsoleColor.White;
    }

    public ConsoleTarget(LogLevel threshold) : this() {
        this.LevelThreshold = threshold;
    }

    public void WriteLog(LogStatement log, LogDisplayFormat format = LogDisplayFormat.Full) {
        switch (log.Level) {
            case LogLevel.Debug:
                Console.ForegroundColor = ConsoleColor.White;
                break;
            case LogLevel.Info:
                Console.ForegroundColor = ConsoleColor.Green;
                break;
            case LogLevel.Warning:
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                break;
            case LogLevel.Error:
                Console.ForegroundColor = ConsoleColor.Red;
                break;
            case LogLevel.Fatal:
                Console.ForegroundColor = ConsoleColor.DarkRed;
                break;
        }

        Console.WriteLine(log.Format(format) + "\n");
        Console.ForegroundColor = ConsoleColor.White;
    }

    public void Dispose() { }
}
