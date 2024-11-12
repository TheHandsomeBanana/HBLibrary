using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Logging.Formatter;
public static class LogFormatters {
    public static DefaultFormatter Default => new DefaultFormatter();
    public static ConsoleFormatter DefaultConsole => new ConsoleFormatter();
    public static DebugFormatter DefaultDebug => new DebugFormatter();
    public static FileFormatter DefaultFile => new FileFormatter();
}
