using HBLibrary.Interface.Logging;
using HBLibrary.Interface.Logging.Configuration;
using HBLibrary.Interface.Logging.Formatting;
using HBLibrary.Interface.Logging.Statements;
using HBLibrary.Interface.Logging.Targets;
using Microsoft.EntityFrameworkCore;

namespace HBLibrary.Services.Logging.EFCoreTarget;

public class EFCoreDatabaseTarget : ILogTarget, IAsyncLogTarget {
    private readonly DbContextOptions<LoggingContext> dbContextOptions;
    public LogLevel? LevelThreshold { get; }

    public ILogFormatter? Formatter => throw new NotImplementedException();

    public EFCoreDatabaseTarget(DbContextOptions<LoggingContext> dbContextOptions, string tableName = "Logs") {
        this.dbContextOptions = dbContextOptions;
        using LoggingContext context = new LoggingContext(dbContextOptions, tableName);
        context.Database.EnsureCreated();
    }


    public void WriteLog(ILogStatement log, ILogFormatter? formatter = null) {
        using LoggingContext context = new LoggingContext(dbContextOptions);
        LogEntry logEntry = new LogEntry {
            Date = log.CreatedOn,
            Category = log.Name!,
            Level = log.Level!.ToString()!,
            Message = log.Message,
        };

        context.LogEntries.Add(logEntry);
        context.SaveChanges();
    }

    public async Task WriteLogAsync(ILogStatement log, ILogFormatter? formatter = null) {
        await using LoggingContext context = new LoggingContext(dbContextOptions);
        LogEntry logEntry = new LogEntry {
            Date = log.CreatedOn,
            Category = log.Name!,
            Level = log.Level!.ToString()!,
            Message = log.Message,
        };

        context.LogEntries.Add(logEntry);
        await context.SaveChangesAsync();
    }

    public void Dispose() {
        // Nothing to dispose
    }
}
