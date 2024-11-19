using HBLibrary.Interface.Logging;
using HBLibrary.Interface.Logging.Statements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Wpf.Logging.Statements;
public class LogBlockStatement : ILogStatement {
    public string Message { get; set; }
    public string? Name { get => null; set => throw new InvalidOperationException("Block statements dont have a name"); }
    public LogLevel? Level { get => null; set => throw new InvalidOperationException("Block statements dont have a level"); }
    public DateTime CreatedOn { get; set; }


    public LogBlockStatement(string block) {
        this.Message = block;
        CreatedOn = DateTime.UtcNow;
    }

    public string ToDefaultString() {
        return Message;
    }

    public string ToFullString() {
        return Message;
    }

    public static LogBlockStatement CreateSeperationBlock(int size = 64, char character = '=') {
        return new LogBlockStatement(new string(character, size));
    }

    public static LogBlockStatement CreateEmptyBlock() {
        return new LogBlockStatement("");
    }
}
