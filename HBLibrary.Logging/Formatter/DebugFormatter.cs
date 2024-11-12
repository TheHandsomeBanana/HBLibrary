using HBLibrary.Interface.Logging.Formatting;
using HBLibrary.Interface.Logging.Statements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Logging.Formatter;
public class DebugFormatter : ILogFormatter {


    public string FormatString(LogStatement logStatement) {
        return logStatement.ToDefaultString();
    }

    public object Format(LogStatement logStatement) {
        return FormatString(logStatement);
    }
}
