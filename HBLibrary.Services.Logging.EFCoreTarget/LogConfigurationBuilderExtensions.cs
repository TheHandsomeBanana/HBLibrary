using HBLibrary.Services.Logging.Configuration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.Logging.EFCoreTarget;
public static class LogConfigurationBuilderExtensions {
    public static ILogConfigurationBuilder AddEFCoreDatabaseTarget(this ILogConfigurationBuilder builder, bool useAsync, DbContextOptions<LoggingContext> dbContextOptions, string tableName = "Logs") {
        if(useAsync) {
            builder.AddAsyncTarget(new EFCoreDatabaseTarget(dbContextOptions, tableName));
        }
        else {
            builder.AddTarget(new EFCoreDatabaseTarget(dbContextOptions, tableName));
        }
        
        return builder;
    }
}
