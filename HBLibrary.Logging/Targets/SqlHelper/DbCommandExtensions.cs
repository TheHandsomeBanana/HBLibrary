using HBLibrary.Interface.Logging.Statements;
using System.Data.Common;

namespace HBLibrary.Logging.Targets.SqlHelper;
public static class DbCommandExtensions {
    public static void AddLogParameters(this DbCommand command, string providerName, ILogStatement logStatement) {
        char prefix = GetParameterPrefix(providerName);

        DbParameter dateParameter = command.CreateParameter();
        dateParameter.ParameterName = $"{prefix}Date";

        // Handle Date as string for SQLite
        dateParameter.Value = providerName == Providers.SQLiteProvider
            ? logStatement.CreatedOn.ToString("o")
            : logStatement.CreatedOn;

        command.Parameters.Add(dateParameter);

        DbParameter loggerNameParameter = command.CreateParameter();
        loggerNameParameter.ParameterName = $"{prefix}Category";
        loggerNameParameter.Value = logStatement.Name;
        command.Parameters.Add(loggerNameParameter);

        DbParameter levelParameter = command.CreateParameter();
        levelParameter.ParameterName = $"{prefix}Level";
        levelParameter.Value = logStatement.Level.ToString();
        command.Parameters.Add(levelParameter);

        DbParameter messageParameter = command.CreateParameter();
        messageParameter.ParameterName = $"{prefix}Message";
        messageParameter.Value = logStatement.Message;
        command.Parameters.Add(messageParameter);
    }

    public static char GetParameterPrefix(string providerName) {
        return providerName == Providers.OracleProvider
            ? ':'
            : '@';
    }
}