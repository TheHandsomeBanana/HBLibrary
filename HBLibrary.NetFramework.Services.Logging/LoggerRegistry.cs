using HBLibrary.NetFramework.Services.Logging.Configuration;
using HBLibrary.NetFramework.Services.Logging.Exceptions;
using HBLibrary.NetFramework.Services.Logging.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HBLibrary.NetFramework.Services.Logging {
    public class LoggerRegistry : ILoggerRegistry {
        private readonly Dictionary<string, ILogger> registeredLoggers = new Dictionary<string, ILogger>();
        private readonly Dictionary<string, ILogConfiguration> loggerConfigurations = new Dictionary<string, ILogConfiguration>();
        public ILogConfiguration GlobalConfiguration { get; private set; } = LogConfiguration.Default;
        public IReadOnlyDictionary<string, ILogger> RegisteredLoggers => registeredLoggers;
        public IReadOnlyDictionary<string, ILogConfiguration> LoggerConfigurations => loggerConfigurations;

        public void ConfigureLogger(ILogger logger, LogConfigurationDelegate configMethod) {
            LogConfigurationBuilder builder = new LogConfigurationBuilder();
            foreach (LogTarget target in GlobalConfiguration.Targets)
                builder.AddTarget(target);

            ILogConfiguration configuration = configMethod.Invoke(builder);
            loggerConfigurations[logger.Name] = configuration;
            logger.Configuration = configuration;
        }

        public ILoggerRegistry ConfigureRegistry(LogConfigurationDelegate configMethod) {
            GlobalConfiguration = configMethod.Invoke(new LogConfigurationBuilder());

            LogConfigurationBuilder builder = new LogConfigurationBuilder();

            foreach (string name in loggerConfigurations.Keys.ToList()) {
                ILogConfiguration config = builder.OverrideConfig(GlobalConfiguration).Build();
                loggerConfigurations[name] = config;
                registeredLoggers[name].Configuration = config;
            }

            return this;
        }

        public static LoggerRegistry FromConfiguration(LogConfigurationDelegate configMethod) {
            LoggerRegistry registry = new LoggerRegistry();
            registry.GlobalConfiguration = configMethod.Invoke(new LogConfigurationBuilder());
            return registry;
        }

        public bool ContainsLogger(string name) => RegisteredLoggers.ContainsKey(name);
        public bool ContainsLogger<T>() where T : class => RegisteredLoggers.ContainsKey(typeof(T).Name);

        public ILogger GetLogger(string name) {
            if (!registeredLoggers.ContainsKey(name))
                LoggerException.ThrowLoggerNotRegistered(name);

            return registeredLoggers[name];
        }

        public ILogger<T> GetLogger<T>() where T : class {
            string typeName = typeof(T).Name;
            if (!registeredLoggers.ContainsKey(typeName))
                LoggerException.ThrowLoggerNotRegistered(typeName);

            return (ILogger<T>)registeredLoggers[typeName];
        }

        public IAsyncLogger GetAsyncLogger(string name) {
            if (!registeredLoggers.ContainsKey(name))
                LoggerException.ThrowLoggerNotRegistered(name);

            try {
                return (IAsyncLogger)registeredLoggers[name];
            }
            catch (InvalidCastException) {
                throw LoggerException.LoggerNotAsync(name);
            }
        }

        public IAsyncLogger<T> GetAsyncLogger<T>() where T : class {
            string typeName = typeof(T).Name;
            if (!registeredLoggers.ContainsKey(typeName))
                LoggerException.ThrowLoggerNotRegistered(typeName);

            try {
                return (IAsyncLogger<T>)registeredLoggers[typeName];
            }
            catch (InvalidCastException) {
                throw LoggerException.LoggerNotAsync(typeName);
            }
        }

        public void RegisterLogger(ILogger logger) {
            if (registeredLoggers.ContainsKey(logger.Name))
                LoggerException.ThrowLoggerRegistered(logger.Name);

            ConfigureLogger(logger, e => e.OverrideConfig(GlobalConfiguration).Build());
            registeredLoggers[logger.Name] = logger;
        }

        public ILogConfiguration GetConfiguration(string name) {
            if (loggerConfigurations.ContainsKey(name))
                return loggerConfigurations[name];

            throw LoggerException.ConfigurationNotFound(name);
        }
    }
}
