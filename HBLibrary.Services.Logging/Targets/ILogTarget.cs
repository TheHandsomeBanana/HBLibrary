using HBLibrary.Services.Logging.Configuration;
using HBLibrary.Services.Logging.Statements;

namespace HBLibrary.Services.Logging.Targets;
public interface ILogTarget : IDisposable {
    /// <summary>
    /// Logging level threshold on the target layer.
    /// </summary>
    LogLevel? LevelThreshold { get; }
    /// <summary>
    /// Writes the provided <paramref name="log"/> to this target
    /// </summary>
    /// <param name="log"></param>
    /// <param name="displayFormat"></param>
    void WriteLog(LogStatement log, LogDisplayFormat displayFormat = LogDisplayFormat.Full);
}

public interface IAsyncLogTarget : IDisposable {
    /// <summary>
    /// Logging level threshold on the target layer.
    /// </summary>
    LogLevel? LevelThreshold { get; }
    /// <summary>
    /// Writes the provided <paramref name="log"/> to this target
    /// </summary>
    /// <param name="log"></param>
    /// <param name="displayFormat"></param>
    Task WriteLogAsync(LogStatement log, LogDisplayFormat displayFormat = LogDisplayFormat.Full);
}
