using HBLibrary.Interface.Logging.Formatting;
using HBLibrary.Interface.Logging.Targets;

namespace HBLibrary.Interface.Logging.Configuration;
/// <summary>
/// </summary>
/// <param name="builder"></param>
/// <returns><see cref="ILogConfiguration"/> created by <see cref="ILogConfigurationBuilder"/></returns>
public delegate ILogConfiguration LogConfigurationDelegate(ILogConfigurationBuilder builder);
/// <summary>
/// <see cref="Targets"/> are accessed by sync and async loggers.<br/>
/// <see cref="AsyncTargets"/> are only accessed by async loggers.
/// </summary>
public interface ILogConfiguration : IDisposable {
    /// <summary>
    /// Only accessed by <see cref="ILogger"/> and <see cref="ILogger{T}"/>.
    /// </summary>
    IReadOnlyList<ILogTarget> Targets { get; }
    /// <summary>
    /// Only accessed by <see cref="IAsyncLogger"/> and <see cref="IAsyncLogger{T}"/>.
    /// </summary>
    IReadOnlyList<IAsyncLogTarget> AsyncTargets { get; }
    /// <summary>
    /// Display format for the log to write.
    /// </summary>
    ILogFormatter? Formatter { get; }
    /// <summary>
    /// Logging level threshold on the logger or global layer.<br/>
    /// </summary>
    LogLevel? LevelThreshold { get; }
}