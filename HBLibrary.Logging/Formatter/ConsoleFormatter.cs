using HBLibrary.Interface.Logging;
using HBLibrary.Interface.Logging.Formatting;
using HBLibrary.Interface.Logging.Statements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Logging.Formatter;
public class ConsoleFormatter : ILogFormatter {
    public object Format(ILogStatement logStatement) {
        switch (logStatement.Level) {
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

        return logStatement.ToDefaultString();
    }
}
