using HBLibrary.Interface.Logging;
using HBLibrary.Interface.Logging.Configuration;
using HBLibrary.Interface.Logging.Formatting;
using HBLibrary.Interface.Logging.Targets;
using HBLibrary.Logging.Formatter;
using HBLibrary.Logging.Targets;

namespace HBLibrary.Logging.Configuration;
internal class LogConfiguration : ILogConfiguration {
    public IReadOnlyList<ILogTarget> Targets { get; set; } = [];
    public IReadOnlyList<IAsyncLogTarget> AsyncTargets { get; set; } = [];
    public ILogFormatter? Formatter { get; }
    public LogLevel? LevelThreshold { get; set; } = null;
    public static LogConfiguration Default => new();

    private LogConfiguration() { }
    public LogConfiguration(List<ILogTarget> targets, List<IAsyncLogTarget> asyncTargets, ILogFormatter? formatter, LogLevel? levelThreshold = null) {
        Targets = targets.ToList();
        AsyncTargets = asyncTargets.ToList();
        Formatter = formatter;
        LevelThreshold = levelThreshold;
    }

    public LogConfiguration(ILogConfiguration configuration) {
        foreach (ILogTarget target in configuration.Targets)
            target.Dispose();

        foreach (IAsyncLogTarget asyncTarget in AsyncTargets)
            asyncTarget.Dispose();

        Targets = configuration.Targets;
        AsyncTargets = configuration.AsyncTargets;
        Formatter = configuration.Formatter;
        LevelThreshold = configuration.LevelThreshold;
    }

    public void Dispose() {
        foreach (ILogTarget target in Targets)
            target.Dispose();

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        Targets = null;
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

        foreach (IAsyncLogTarget asyncTarget in AsyncTargets)
            asyncTarget.Dispose();

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        AsyncTargets = null;
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
    }
}
