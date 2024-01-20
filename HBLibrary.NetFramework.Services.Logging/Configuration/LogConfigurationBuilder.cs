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

            targets.Add(target);
            return this;
        }

        public ILogConfigurationBuilder AddAsyncTarget(IAsyncLogTarget target) {
            asyncTargets.Add(target);
            return this;
        }

        public ILogConfigurationBuilder AddFileTarget(string filePath, bool useAsync) {
            if (!levelThreshold.HasValue)
                throw LoggingException.LevelThresholdNotSet();

            return AddFileTarget(filePath, levelThreshold.Value, useAsync);
        }

        public ILogConfigurationBuilder AddFileTarget(string fileName, LogLevel minLevel, bool useAsync) {
            if (useAsync)
                asyncTargets.Add(new FileTarget(fileName, minLevel, useAsync));
            else
                targets.Add(new FileTarget(fileName, minLevel, useAsync));

            return this;
        }

        public ILogConfigurationBuilder AddMethodTarget(LogStatementDelegate method) {
            if (!levelThreshold.HasValue)
                throw LoggingException.LevelThresholdNotSet();

            return AddMethodTarget(method, levelThreshold.Value);
        }

        public ILogConfigurationBuilder AddMethodTarget(LogStatementDelegate method, LogLevel minLevel) {
            targets.Add(new MethodTarget(method, minLevel));
            return this;
        }

        public ILogConfigurationBuilder AddAsyncMethodTarget(AsyncLogStatementDelegate method) {
            if (!levelThreshold.HasValue)
                throw LoggingException.LevelThresholdNotSet();

            return AddAsyncMethodTarget(method, levelThreshold.Value);
        }

        public ILogConfigurationBuilder AddAsyncMethodTarget(AsyncLogStatementDelegate method, LogLevel minLevel) {
            asyncTargets.Add(new AsyncMethodTarget(method, minLevel));
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

            if(levelThreshold.HasValue) {
                foreach(ILogTarget target in targets) {
                    if (target.LevelThreshold != levelThreshold.Value)
                        target.LevelThreshold = levelThreshold.Value;
                }

                foreach(IAsyncLogTarget target in asyncTargets) {
                    if (target.LevelThreshold != levelThreshold.Value)
                        target.LevelThreshold = levelThreshold.Value;
                }
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
