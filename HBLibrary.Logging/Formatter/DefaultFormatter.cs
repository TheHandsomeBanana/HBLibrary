using HBLibrary.Interface.Logging.Formatting;
using HBLibrary.Interface.Logging.Statements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Logging.Formatter;
public class DefaultFormatter : ILogFormatter {
    public object Format(ILogStatement logStatement) {
        return logStatement.ToDefaultString();
    }

}
