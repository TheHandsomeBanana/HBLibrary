using HBLibrary.NetFramework.Services.Logging.Configuration;
using HBLibrary.NetFramework.Services.Logging.Statements;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.NetFramework.Services.Logging.Targets {
    public class ConsoleTarget : ILogTarget {
        public const string Logo =
              @"    __  ____       __                      _                                |" + "\r\n" +
              @"   / / / / /_     / /   ____  ____ _____ _(_)___  ____ _                    |" + "\r\n" +
              @"  / /_/ / __ \   / /   / __ \/ __ `/ __ `/ / __ \/ __ `/                    |" + "\r\n" +
              @" / __  / /_/ /  / /___/ /_/ / /_/ / /_/ / / / / / /_/ /                     |" + "\r\n" +
              @"/_/ /_/_.___/  /_____/\____/\__, /\__, /_/_/ /_/\__, /                      |" + "\r\n" +
              @"                           /____//____/        /____/                       |" + "\r\n" +
              @"                                                                            |" + "\r\n" +
              @"                                                                            |";

        public const string TargetName =
              @"                                                                            |" + "\r\n" +
              @"   ______                       __        ______                      __    |" + "\r\n" +
              @"  / ____/___  ____  _________  / /__     /_  __/___ __________ ____  / /_   |" + "\r\n" +
              @" / /   / __ \/ __ \/ ___/ __ \/ / _ \     / / / __ `/ ___/ __ `/ _ \/ __/   |" + "\r\n" +
              @"/ /___/ /_/ / / / (__  ) /_/ / /  __/    / / / /_/ / /  / /_/ /  __/ /_     |" + "\r\n" +
              @"\____/\____/_/ /_/____/\____/_/\___/    /_/  \__,_/_/   \__, /\___/\__/     |" + "\r\n" +
              @"                                                       /____/               |" + "\r\n" +
              @"____________________________________________________________________________|" + "\r\n\r\n";

        public LogLevel LevelThreshold { get; set; }

        public ConsoleTarget() {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(Logo);
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(TargetName);
            Console.ForegroundColor = ConsoleColor.White;
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
}
