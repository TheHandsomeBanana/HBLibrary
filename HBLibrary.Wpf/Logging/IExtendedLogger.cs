using HBLibrary.Interface.Logging;
using HBLibrary.Interface.Logging.Formatting;
using HBLibrary.Interface.Logging.Targets;
using HBLibrary.Wpf.Logging.Statements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Wpf.Logging;
public interface IExtendedLogger : ILogger {
    public void AddBlock(string block);
    public void AddBlock(LogBlockStatement block);

    public int IndexedDebug(string message);
    public int IndexedInfo(string message);
    public int IndexedWarn(string message);
    public int IndexedError(string message);
    public int IndexedError(Exception exception);
    public int IndexedFatal(string message);

    public void RewriteIndexed(int index, string message);
}        
