using HBLibrary.NetFramework.Services.Logging.Targets;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.NetFramework.Services.Logging.Configuration {
    internal class LogConfiguration : ILogConfiguration {
        public ImmutableArray<LogTarget> Targets { get; } = ImmutableArray<LogTarget>.Empty;
        public LogDisplayFormat DisplayFormat { get; } = LogDisplayFormat.Full;
        public static LogConfiguration Default => new LogConfiguration();
        private LogConfiguration() { }
        public LogConfiguration(IEnumerable<LogTarget> targets, LogDisplayFormat displayFormat) {
            this.Targets = targets.ToImmutableArray();
            DisplayFormat = displayFormat;
        }

        public LogConfiguration(ILogConfiguration configuration) {
            Targets = configuration.Targets;
            DisplayFormat = configuration.DisplayFormat;
        }
    }
}
