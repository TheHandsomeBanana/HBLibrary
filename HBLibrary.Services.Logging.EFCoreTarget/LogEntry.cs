using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.Logging.EFCoreTarget;
public class LogEntry {
    public int Id { get; set; }
    public required DateTime Date { get; set; }
    public required string Category { get; set; }
    public required string Level { get; set; }
    public required string Message { get; set; }
}
