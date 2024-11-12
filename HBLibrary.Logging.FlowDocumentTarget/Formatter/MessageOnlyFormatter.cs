using HBLibrary.Interface.Logging.Formatting;
using HBLibrary.Interface.Logging.Statements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Logging.FlowDocumentTarget.Formatter;
internal class MessageOnlyFormatter : ILogFormatter {
    public object Format(LogStatement logStatement) {
        return logStatement.Message;
    }
}
