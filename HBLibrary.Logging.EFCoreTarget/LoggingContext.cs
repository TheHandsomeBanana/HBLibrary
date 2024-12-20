using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.Logging.EFCoreTarget;
public class LoggingContext : DbContext {
    private readonly string tableName;
    public DbSet<LogEntry>? LogEntries { get; }

    public LoggingContext(DbContextOptions<LoggingContext> options, string tableName = "Logs") : base(options) { 
        this.tableName = tableName;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<LogEntry>().ToTable(tableName);
        modelBuilder.Entity<LogEntry>().HasKey(e => e.Id);
    }
}
