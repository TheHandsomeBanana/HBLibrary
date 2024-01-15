using HBLibrary.NetFramework.Services.Logging.Target;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.NetFramework.Services.Logging.Configuration {
    internal class LogConfigurationBuilder : ILogConfigurationBuilder {
        private readonly List<LogTarget> targets = new List<LogTarget>();
        private LogDisplayFormat displayFormat;
        private bool overrideConfig = false;
        public ILogConfigurationBuilder AddTarget(string filePath, LogLevel minLevel) {
            targets.Add(new LogTarget(filePath, minLevel));
            return this;
        }

        public ILogConfigurationBuilder AddTarget(LogStatementDelegate method, LogLevel minLevel) {
            AddInternal(method, minLevel);
            return this;
        }

        public ILogConfigurationBuilder AddTarget(AsyncLogStatementDelegate method, LogLevel minLevel) {
            AddInternal(method, minLevel);
            return this;
        }

        internal LogConfigurationBuilder AddTarget(LogTarget target) {
            targets.Add(target);
            return this;
        }

        private ILogConfiguration logConfiguration;
        public ILogConfigurationBuilder OverrideConfig(ILogConfiguration logConfiguration) {
            this.logConfiguration = logConfiguration;
            this.overrideConfig = true;
            return this;
        }

        public ILogConfigurationBuilder WithDisplayFormat(LogDisplayFormat format) {
            displayFormat = format;
            return this;
        }

        public ILogConfiguration Build() {
            if (overrideConfig) {
                overrideConfig = false;
                return new LogConfiguration(logConfiguration);
            }

            LogConfiguration result = new LogConfiguration(targets, displayFormat);
            targets.Clear();
            displayFormat = LogDisplayFormat.Normal;
            return result;
        }

        private void AddInternal(object value, LogLevel minLevel)
            => targets.Add(new LogTarget(value, minLevel));

        
    }
}
