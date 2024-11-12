using HBLibrary.Interface.Logging.Formatting;
using HBLibrary.Interface.Logging.Statements;
using HBLibrary.Interface.Logging.Targets;

namespace HBLibrary.Interface.Logging.Configuration;
public delegate void LogStatementDelegate(LogStatement log, ILogFormatter? formatter);
public delegate Task AsyncLogStatementDelegate(LogStatement log, ILogFormatter? formatter);

public interface ILogConfigurationBuilder {
    ILogConfigurationBuilder AddTarget(ILogTarget target);
    ILogConfigurationBuilder AddAsyncTarget(IAsyncLogTarget target);
    ILogConfigurationBuilder AddFileTarget(string filePath, bool useAsync, LogLevel? levelThreshold = null);
    ILogConfigurationBuilder AddMethodTarget(LogStatementDelegate method, LogLevel? levelThreshold = null);
    ILogConfigurationBuilder AddAsyncMethodTarget(AsyncLogStatementDelegate method, LogLevel? levelThreshold = null);
    ILogConfigurationBuilder AddDatabaseTarget(string providerName, string connectionString, bool useAsync, LogLevel? levelThreshold = null, string tableName = "Logs");
    ILogConfigurationBuilder WithFormatter(ILogFormatter formatter);
    /// <summary>
    /// Requires to be called before adding targets to overwrite the target level threshold.
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    ILogConfigurationBuilder WithLevelThreshold(LogLevel level);
    ILogConfigurationBuilder OverrideConfig(ILogConfiguration logConfiguration);
    ILogConfiguration Build();
}


