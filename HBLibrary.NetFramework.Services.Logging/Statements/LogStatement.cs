using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.NetFramework.Services.Logging.Statements {
    public readonly struct LogStatement {
        public string Message { get; }
        public string Category { get; }
        public LogLevel Level { get; }
        public DateTime CreatedOn { get; }

        public LogStatement(string message, string category, LogLevel level, DateTime createdOn) {
            Message = message;
            Category = category;
            Level = level;
            CreatedOn = createdOn;
        }

        public override string ToString() => $"[{Level}]: {Message}";

        public string ToFullString()
            => $"Category: {Category}\nCreated On: {CreatedOn:dd.MM.yyyy hh:MM:ss}\nLog Level: {Level}\nMessage: {Message}";
        public string ToMinimalString() => $"[{CreatedOn:hh:MM:ss}] [{Level}]: {Message}";
    }
}
