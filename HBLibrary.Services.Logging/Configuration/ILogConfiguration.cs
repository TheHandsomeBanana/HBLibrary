using HBLibrary.Services.Logging.Targets;

namespace HBLibrary.Services.Logging.Configuration;
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
    LogDisplayFormat DisplayFormat { get; }
    /// <summary>
    /// Logging level threshold on the logger or global layer.<br/>
    /// </summary>
    LogLevel? LevelThreshold { get; }
}

public enum LogDisplayFormat {
    MessageOnly,
    Minimal,
    Full
}
