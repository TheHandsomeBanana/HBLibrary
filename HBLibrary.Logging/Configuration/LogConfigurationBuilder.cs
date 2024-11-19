using HBLibrary.Interface.Logging;
using HBLibrary.Interface.Logging.Configuration;
using HBLibrary.Interface.Logging.Exceptions;
using HBLibrary.Interface.Logging.Formatting;
using HBLibrary.Interface.Logging.Targets;
using HBLibrary.Logging.Targets;

namespace HBLibrary.Logging.Configuration;
internal class LogConfigurationBuilder : ILogConfigurationBuilder {
    private readonly List<ILogTarget> targets = [];
    private readonly List<IAsyncLogTarget> asyncTargets = [];
    private ILogFormatter? formatter;
    private LogLevel? levelThreshold = null;
    private bool overrideConfig = false;
    public ILogConfigurationBuilder AddTarget(ILogTarget target) {
        if (target is ConsoleTarget && targets.Any(e => e is ConsoleTarget))
            throw new LoggingException("Console can only be targeted once.");

        else if (target is DebugTarget && targets.Any(e => e is DebugTarget))
            throw new LoggingException("Debug output can only be targeted once.");

        targets.Add(target);
        return this;
    }

    public ILogConfigurationBuilder AddAsyncTarget(IAsyncLogTarget target) {
        asyncTargets.Add(target);
        return this;
    }

    public ILogConfigurationBuilder AddFileTarget(string fileName, bool useAsync, LogLevel? levelThreshold = null, ILogFormatter? formatter = null) {
        if (useAsync)
            asyncTargets.Add(new FileTarget(fileName, levelThreshold, useAsync, formatter));
        else
            targets.Add(new FileTarget(fileName, levelThreshold, useAsync, formatter));

        return this;
    }

    public ILogConfigurationBuilder AddMethodTarget(LogStatementDelegate method, LogLevel? levelThreshold = null, ILogFormatter? formatter = null) {
        targets.Add(new MethodTarget(method, levelThreshold, formatter));
        return this;
    }

    public ILogConfigurationBuilder AddAsyncMethodTarget(AsyncLogStatementDelegate method, LogLevel? levelThreshold = null, ILogFormatter? formatter = null) {
        asyncTargets.Add(new AsyncMethodTarget(method, levelThreshold, formatter));
        return this;
    }

    public ILogConfigurationBuilder AddDatabaseTarget(string providerName, string connectionString, bool useAsync, LogLevel? levelThreshold = null, ILogFormatter? formatter = null, string tableName = "Logs") {
        if (useAsync) {
            asyncTargets.Add(new DatabaseTarget(providerName, connectionString, levelThreshold, formatter, tableName));
        }
        else {
            targets.Add(new DatabaseTarget(providerName, connectionString, levelThreshold, formatter, tableName));
        }

        return this;
    }

    private ILogConfiguration? logConfiguration;
    public ILogConfigurationBuilder OverrideConfig(ILogConfiguration logConfiguration) {
        this.logConfiguration = logConfiguration;
        overrideConfig = true;
        return this;
    }

    public ILogConfigurationBuilder WithFormatter(ILogFormatter format) {
        formatter = format;
        return this;
    }

    public ILogConfigurationBuilder WithLevelThreshold(LogLevel level) {
        levelThreshold = level;
        return this;
    }



    public ILogConfiguration Build() {
        if (overrideConfig) {
            Reset();
            overrideConfig = false;
            return new LogConfiguration(logConfiguration!);
        }

        LogConfiguration result = new LogConfiguration(targets, asyncTargets, formatter, levelThreshold);
        Reset();
        return result;
    }

    private void Reset() {
        targets.Clear();
        asyncTargets.Clear();
        formatter = null;
        levelThreshold = LogLevel.Debug;
    }


}
