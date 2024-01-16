using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.NetFramework.Services.Logging.Targets {
    public readonly struct LogTarget {
        public object Value { get; }
        public LogLevel Level { get; }

        public LogTarget(object value, LogLevel level) {
            Value = value;
            Level = level;
        }
    }
}
