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
        public ILogConfigurationBuilder AddTarget(string filePath, LogLevel minLevel) {
            targets.Add(new LogTarget(filePath, minLevel));
            return this;
        }

        public ILogConfigurationBuilder AddTarget(LogStringDelegate method, LogLevel minLevel) {
            AddInternal(method, minLevel);
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

        public ILogConfigurationBuilder AddTarget(AsyncLogStringDelegate method, LogLevel minLevel) {
            AddInternal(method, minLevel);
            return this;
        }

        internal LogConfigurationBuilder AddTarget(LogTarget target) {
            targets.Add(target);
            return this;
        }

        public ILogConfiguration Build() {
            LogConfiguration result = new LogConfiguration(targets, displayFormat);
            targets.Clear();
            displayFormat = LogDisplayFormat.Normal;
            return result;
        }

        public ILogConfigurationBuilder WithDisplayFormat(LogDisplayFormat format) {
            displayFormat = format;
            return this;
        }

        private void AddInternal(object value, LogLevel minLevel)
            => targets.Add(new LogTarget(value, minLevel));
    }
}
