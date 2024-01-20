using HBLibrary.NetFramework.Services.Logging.Targets;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.NetFramework.Services.Logging.Configuration {
    internal class LogConfiguration : ILogConfiguration {
        public IReadOnlyList<ILogTarget> Targets { get; set; } = Array.Empty<ILogTarget>();
        public IReadOnlyList<IAsyncLogTarget> AsyncTargets { get; set; } = Array.Empty<IAsyncLogTarget>();
        public LogDisplayFormat DisplayFormat { get; } = LogDisplayFormat.Full;
        public LogLevel? LevelThreshold { get; } = null;
        public static LogConfiguration Default => new LogConfiguration();

        private LogConfiguration() { }
        public LogConfiguration(List<ILogTarget> targets, List<IAsyncLogTarget> asyncTargets, LogDisplayFormat displayFormat, LogLevel? levelThreshold) {
            Targets = targets.ToList();
            AsyncTargets = asyncTargets.ToList();
            DisplayFormat = displayFormat;
            LevelThreshold = levelThreshold;
        }

        public LogConfiguration(ILogConfiguration configuration) {
            foreach(ILogTarget target in configuration.Targets)
                target.Dispose();

            foreach(IAsyncLogTarget asyncTarget in AsyncTargets) 
                asyncTarget.Dispose();
            
            Targets = configuration.Targets;
            AsyncTargets = configuration.AsyncTargets;
            DisplayFormat = configuration.DisplayFormat;
            LevelThreshold = configuration.LevelThreshold;
        }

        public void Dispose() {
            foreach(ILogTarget target in Targets)
                target.Dispose();

            Targets = null;

            foreach(IAsyncLogTarget asyncTarget in AsyncTargets)
                asyncTarget.Dispose();

            AsyncTargets = null;
        }
    }
}
