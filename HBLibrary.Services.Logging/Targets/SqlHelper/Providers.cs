using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.Logging.Targets.SqlHelper;
public class Providers {
    public const string SQLiteProvider = "Microsoft.Data.Sqlite";
    public const string SQLServerProvider = "Microsoft.Data.SqlClient";
    public const string PostgreSQLprovider = "Npgsql";
    public const string MariaDBProvider = "MySqlConnector";
    public const string OracleProvider = "Oracle.ManagedDataAccess.Client";
}
