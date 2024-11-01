using HBLibrary.Interface.Logging.Configuration;
using HBLibrary.Interface.Logging.Statements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Interface.Logging.Formatting;
public interface ILogFormatter {
    string Format(LogStatement logStatement, LogDisplayFormat format);

}
