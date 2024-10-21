using System.Data.Common;

namespace HBLibrary.Logging.Targets.SqlHelper;
public static class DbConnectionExtensions {
    public static DbCommand CreateLogTableInsertCommand(this DbConnection connection, string providerName, string tableName) {
        DbCommand command = connection.CreateCommand();

        switch (providerName) {
            case Providers.SQLiteProvider:
                command.CommandText = SqliteLoggingCommands.GetCreateTableCommand(tableName);
                break;
            case Providers.SQLServerProvider:
                command.CommandText = SqlServerLoggingCommands.GetCreateTableCommand(tableName);
                break;
            case Providers.PostgreSQLprovider:
                command.CommandText = PostgresLoggingCommands.GetCreateTableCommand(tableName);
                break;
            case Providers.MariaDBProvider:
                command.CommandText = MariaDBLoggingCommands.GetCreateTableCommand(tableName);
                break;
            case Providers.OracleProvider:
                command.CommandText = OracleLoggingCommands.GetCreateTableCommand(tableName);
                break;
        }


        return command;
    }

    public static DbCommand CreateLogTableCreateCommand(this DbConnection connection, string providerName, string tableName) {
        DbCommand command = connection.CreateCommand();

        switch (providerName) {
            case Providers.SQLiteProvider:
                command.CommandText = SqliteLoggingCommands.GetInsertLogCommand(tableName);
                break;
            case Providers.SQLServerProvider:
                command.CommandText = SqlServerLoggingCommands.GetInsertLogCommand(tableName);
                break;
            case Providers.PostgreSQLprovider:
                command.CommandText = PostgresLoggingCommands.GetInsertLogCommand(tableName);
                break;
            case Providers.MariaDBProvider:
                command.CommandText = MariaDBLoggingCommands.GetInsertLogCommand(tableName);
                break;
            case Providers.OracleProvider:
                command.CommandText = OracleLoggingCommands.GetInsertLogCommand(tableName);
                break;
        }

        return command;
    }
}
