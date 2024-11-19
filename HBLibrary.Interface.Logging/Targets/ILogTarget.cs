using HBLibrary.Interface.Logging.Configuration;
using HBLibrary.Interface.Logging.Formatting;
using HBLibrary.Interface.Logging.Statements;

namespace HBLibrary.Interface.Logging.Targets;
public interface ILogTarget : IDisposable {
    /// <summary>
    /// Logging level threshold on the target layer.
    /// </summary>
    LogLevel? LevelThreshold { get; }
    /// <summary>
    /// Target specific formatter that overrides the logger specific formatter
    /// </summary>
    ILogFormatter? Formatter { get; }
    /// <summary>
    /// Writes the provided <paramref name="log"/> to this target, formatted using <paramref name="formatter"/>.
    /// If no specific formatter is passed, the default formatter is used.
    /// </summary>
    /// <param name="log"></param>
    /// <param name="formatter"></param>
    void WriteLog(ILogStatement log, ILogFormatter? formatter = null);
}

