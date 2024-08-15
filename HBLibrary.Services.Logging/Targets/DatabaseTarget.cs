using HBLibrary.Services.Logging.Configuration;
using HBLibrary.Services.Logging.Statements;
using HBLibrary.Services.Logging.Targets.SqlHelper;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.Logging.Targets;
internal class DatabaseTarget : ILogTarget, IAsyncLogTarget {
    private readonly DbProviderFactory dbProviderFactory;
    private readonly string providerName;
    private readonly string connectionString;
    public string TableName { get; }

    public LogLevel? LevelThreshold { get; }

    public DatabaseTarget(string providerName, string connectionString, LogLevel? minLevel = null, string tableName = "Logs") {
        this.providerName = providerName;
        this.dbProviderFactory = DbProviderFactories.GetFactory(providerName);
        this.connectionString = connectionString;
        this.TableName = tableName;
        this.LevelThreshold = minLevel;

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

    public void WriteLog(LogStatement log, LogDisplayFormat displayFormat = LogDisplayFormat.Full) {
        using DbConnection connection = dbProviderFactory.CreateConnection()
            ?? throw new InvalidOperationException("DbProvider not registered.");

        connection.ConnectionString = connectionString;
        connection.Open();

        using DbCommand command = connection.CreateLogTableInsertCommand(providerName, TableName);
        command.AddLogParameters(providerName, log);
        command.ExecuteNonQuery();
    }

    public async Task WriteLogAsync(LogStatement log, LogDisplayFormat displayFormat = LogDisplayFormat.Full) {
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
