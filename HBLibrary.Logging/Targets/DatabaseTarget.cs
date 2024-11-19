using HBLibrary.Interface.Logging;
using HBLibrary.Interface.Logging.Configuration;
using HBLibrary.Interface.Logging.Formatting;
using HBLibrary.Interface.Logging.Statements;
using HBLibrary.Interface.Logging.Targets;
using HBLibrary.Logging.Configuration;
using HBLibrary.Logging.Targets.SqlHelper;
using System.Data.Common;

namespace HBLibrary.Logging.Targets;
internal class DatabaseTarget : ILogTarget, IAsyncLogTarget {
    private readonly DbProviderFactory dbProviderFactory;
    private readonly string providerName;
    private readonly string connectionString;
    public string TableName { get; }

    public LogLevel? LevelThreshold { get; }

    public ILogFormatter? Formatter { get; }

    public DatabaseTarget(string providerName, string connectionString, LogLevel? minLevel = null, ILogFormatter? formatter = null, string tableName = "Logs") {
        this.providerName = providerName;
        dbProviderFactory = DbProviderFactories.GetFactory(providerName);
        this.connectionString = connectionString;
        TableName = tableName;
        LevelThreshold = minLevel;
        Formatter = formatter;

        InitDatabase();
    }

    private void InitDatabase() {
        using DbConnection connection = dbProviderFactory.CreateConnection()
            ?? throw new InvalidOperationException("DbProvider not registered.");

        connection.ConnectionString = connectionString;
        connection.Open();

        using DbCommand command = connection.CreateLogTableCreateCommand(providerName, TableName);
        command.ExecuteNonQuery();
    }

    public void WriteLog(ILogStatement log, ILogFormatter? formatter = null) {
        using DbConnection connection = dbProviderFactory.CreateConnection()
            ?? throw new InvalidOperationException("DbProvider not registered.");

        connection.ConnectionString = connectionString;
        connection.Open();

        using DbCommand command = connection.CreateLogTableInsertCommand(providerName, TableName);
        command.AddLogParameters(providerName, log);
        command.ExecuteNonQuery();
    }

    public async Task WriteLogAsync(ILogStatement log, ILogFormatter? formatter = null) {
        using DbConnection connection = dbProviderFactory.CreateConnection()
                    ?? throw new InvalidOperationException("DbProvider not registered.");

        connection.ConnectionString = connectionString;
        await connection.OpenAsync();

        using DbCommand command = connection.CreateLogTableInsertCommand(providerName, TableName);
        command.AddLogParameters(providerName, log);
        command.ExecuteNonQuery();
    }

    public void Dispose() {
        // Nothing to dispose
    }
}
