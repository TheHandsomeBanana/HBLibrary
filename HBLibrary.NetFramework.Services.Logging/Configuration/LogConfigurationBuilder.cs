using HBLibrary.NetFramework.Services.Logging.Exceptions;
using HBLibrary.NetFramework.Services.Logging.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.NetFramework.Services.Logging.Configuration {
    internal class LogConfigurationBuilder : ILogConfigurationBuilder {
        private readonly List<ILogTarget> targets = new List<ILogTarget>();
        private readonly List<IAsyncLogTarget> asyncTargets = new List<IAsyncLogTarget>();
        private LogDisplayFormat displayFormat = LogDisplayFormat.Full;
        private LogLevel? levelThreshold = null;
        private bool overrideConfig = false;
        public ILogConfigurationBuilder AddTarget(ILogTarget target) {
            if (target is ConsoleTarget && targets.Any(e => e is ConsoleTarget))
                throw new LoggingException("Console can only be targeted once.");

            else if(target is DebugTarget && targets.Any(e => e is DebugTarget))
                throw new LoggingException("Debug output can only be targeted once.");

            targets.Add(target);
            return this;
        }

        public ILogConfigurationBuilder AddAsyncTarget(IAsyncLogTarget target) {
            asyncTargets.Add(target);
            return this;
        }

        public ILogConfigurationBuilder AddFileTarget(string fileName, bool useAsync, LogLevel? levelThreshold = null) {
            if (useAsync)
                asyncTargets.Add(new FileTarget(fileName, levelThreshold, useAsync));
            else
                targets.Add(new FileTarget(fileName, levelThreshold, useAsync));

            return this;
        }

        public ILogConfigurationBuilder AddMethodTarget(LogStatementDelegate method, LogLevel? levelThreshold = null) {
            targets.Add(new MethodTarget(method, levelThreshold));
            return this;
        }

        public ILogConfigurationBuilder AddAsyncMethodTarget(AsyncLogStatementDelegate method, LogLevel? levelThreshold = null) {
            asyncTargets.Add(new AsyncMethodTarget(method, levelThreshold));
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

        public ILogConfigurationBuilder WithLevelThreshold(LogLevel level) {
            this.levelThreshold = level;
            return this;
        }

        public ILogConfiguration Build() {
            if (overrideConfig) {
                Reset();
                overrideConfig = false;
                return new LogConfiguration(logConfiguration);
            }

            LogConfiguration result = new LogConfiguration(targets, asyncTargets, displayFormat, levelThreshold);
            Reset();
            return result;
        }

        private void Reset() {
            targets.Clear();
            asyncTargets.Clear();
            displayFormat = LogDisplayFormat.Full;
            levelThreshold = LogLevel.Debug;
        }
    }
}
