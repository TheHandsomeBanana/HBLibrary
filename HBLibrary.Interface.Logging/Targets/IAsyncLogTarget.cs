using HBLibrary.Interface.Logging.Configuration;
using HBLibrary.Interface.Logging.Formatting;
using HBLibrary.Interface.Logging.Statements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Interface.Logging.Targets;
public interface IAsyncLogTarget : IDisposable {
    /// <summary>
    /// Logging level threshold on the target layer.
    /// </summary>
    LogLevel? LevelThreshold { get; }
    /// <summary>
    /// Writes the provided <paramref name="log"/> to this target, formatted using <paramref name="formatter"/>
    /// </summary>
    /// <param name="log"></param>
    /// <param name="formatter"></param>
    Task WriteLogAsync(ILogStatement log, ILogFormatter? formatter = null);
}
