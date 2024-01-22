using HBLibrary.Services.Logging.Targets;

namespace HBLibrary.Services.Logging.Configuration;
internal class LogConfiguration : ILogConfiguration {
    public IReadOnlyList<ILogTarget> Targets { get; set; } = [];
    public IReadOnlyList<IAsyncLogTarget> AsyncTargets { get; set; } = [];
    public LogDisplayFormat DisplayFormat { get; } = LogDisplayFormat.Full;
    public LogLevel? LevelThreshold { get; set; } = null;
    public static LogConfiguration Default => new();

    private LogConfiguration() { }
    public LogConfiguration(List<ILogTarget> targets, List<IAsyncLogTarget> asyncTargets, LogDisplayFormat displayFormat, LogLevel? levelThreshold = null) {
        Targets = targets.ToList();
        AsyncTargets = asyncTargets.ToList();
        DisplayFormat = displayFormat;
        LevelThreshold = levelThreshold;
    }

    public LogConfiguration(ILogConfiguration configuration) {
        foreach (ILogTarget target in configuration.Targets)
            target.Dispose();

        foreach (IAsyncLogTarget asyncTarget in AsyncTargets)
            asyncTarget.Dispose();

        Targets = configuration.Targets;
        AsyncTargets = configuration.AsyncTargets;
        DisplayFormat = configuration.DisplayFormat;
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
