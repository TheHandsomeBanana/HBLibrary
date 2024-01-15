using HBLibrary.NetFramework.Services.Logging.Target;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.NetFramework.Services.Logging.Configuration {
    internal class LogConfiguration : ILogConfiguration {
        public ImmutableArray<LogTarget> Targets { get; } = ImmutableArray<LogTarget>.Empty;
        public LogDisplayFormat DisplayFormat { get; } = LogDisplayFormat.Normal;
        public static LogConfiguration Default => new LogConfiguration();
        private LogConfiguration() { }
        public LogConfiguration(IEnumerable<LogTarget> targets, LogDisplayFormat displayFormat) {
            this.Targets = targets.ToImmutableArray();
            DisplayFormat = displayFormat;
        }
    }
}
