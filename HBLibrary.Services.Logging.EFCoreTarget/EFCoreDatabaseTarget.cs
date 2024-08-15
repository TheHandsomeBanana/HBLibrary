using HBLibrary.Services.Logging.Configuration;
using HBLibrary.Services.Logging.Statements;
using HBLibrary.Services.Logging.Targets;
using Microsoft.EntityFrameworkCore;

namespace HBLibrary.Services.Logging.EFCoreTarget;

public class EFCoreDatabaseTarget : ILogTarget, IAsyncLogTarget {
    private readonly DbContextOptions<LoggingContext> dbContextOptions;
    public LogLevel? LevelThreshold { get; }

    public EFCoreDatabaseTarget(DbContextOptions<LoggingContext> dbContextOptions, string tableName = "Logs") {
        this.dbContextOptions = dbContextOptions;
        using LoggingContext context = new LoggingContext(dbContextOptions, tableName);
        context.Database.EnsureCreated();
    }


    public void WriteLog(LogStatement log, LogDisplayFormat displayFormat = LogDisplayFormat.Full) {
        using LoggingContext context = new LoggingContext(dbContextOptions);
        LogEntry logEntry = new LogEntry {
            Date = log.CreatedOn,
            Category = log.Name,
            Level = log.Level.ToString(),
            Message = log.Message,
        };

        context.LogEntries.Add(logEntry);
        context.SaveChanges();
    }

    public async Task WriteLogAsync(LogStatement log, LogDisplayFormat displayFormat = LogDisplayFormat.Full) {
        await using LoggingContext context = new LoggingContext(dbContextOptions);
        LogEntry logEntry = new LogEntry {
            Date = log.CreatedOn,
            Category = log.Name,
            Level = log.Level.ToString(),
            Message = log.Message,
        };

        context.LogEntries.Add(logEntry);
        await context.SaveChangesAsync();
    }

    public void Dispose() {
        // Nothing to dispose
    }
}
