﻿using HBLibrary.NetFramework.Services.Logging.Configuration;
using HBLibrary.NetFramework.Services.Logging.Exceptions;
using HBLibrary.NetFramework.Services.Logging.Targets;
using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HBLibrary.NetFramework.Services.Logging {
    public class LoggerRegistry : ILoggerRegistry {
        private readonly Dictionary<string, ILogger> registeredLoggers = new Dictionary<string, ILogger>();
        public ILogConfiguration GlobalConfiguration { get; private set; } = LogConfiguration.Default;
        public IReadOnlyDictionary<string, ILogger> RegisteredLoggers => registeredLoggers;
        public bool IsConfigured { get; private set; } = false;

        public LoggerRegistry() {
            AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
        }

        ~LoggerRegistry() {
            AppDomain.CurrentDomain.ProcessExit -= OnProcessExit;
        }

        public void ConfigureLogger(ILogger logger, LogConfigurationDelegate configMethod) {
            LogConfigurationBuilder builder = new LogConfigurationBuilder();
            foreach (ILogTarget target in GlobalConfiguration.Targets)
                builder.AddTarget(target);

            if (logger is IAsyncLogger) {
                foreach (IAsyncLogTarget target in GlobalConfiguration.AsyncTargets)
                    builder.AddAsyncTarget(target);
            }

            ILogConfiguration configuration = configMethod?.Invoke(builder) ?? builder.Build();
            if (GlobalConfiguration.LevelThreshold.HasValue)
                configuration.LevelThreshold = GlobalConfiguration.LevelThreshold;

            if (!(logger is IAsyncLogger) && configuration.AsyncTargets.Count > 0)
                LoggingException.ThrowAsyncTargetsNotAllowed(logger.GetType().Name);

            logger.Configuration = configuration;
        }

        public ILoggerRegistry ConfigureRegistry(LogConfigurationDelegate configMethod) {
            if (IsConfigured)
                LoggingException.ThrowRegistryConfigured();

            IsConfigured = true;
            GlobalConfiguration = configMethod.Invoke(new LogConfigurationBuilder());

            LogConfigurationBuilder builder = new LogConfigurationBuilder();
            foreach (ILogger logger in registeredLoggers.Values)
                logger.Configuration = builder.OverrideConfig(GlobalConfiguration).Build();

            return this;
        }

        public static LoggerRegistry FromConfiguration(LogConfigurationDelegate configMethod) {
            LoggerRegistry registry = new LoggerRegistry();
            registry.ConfigureRegistry(configMethod);
            return registry;
        }

        public bool ContainsLogger(string name) => registeredLoggers.ContainsKey(name);
        public bool ContainsLogger<T>() where T : class => registeredLoggers.ContainsKey(typeof(T).Name);

        public ILogger GetLogger(string name) {
            if (!registeredLoggers.ContainsKey(name))
                LoggingException.ThrowLoggerNotRegistered(name);

            return registeredLoggers[name];
        }

        public ILogger<T> GetLogger<T>() where T : class {
            string typeName = typeof(T).Name;
            if (!registeredLoggers.ContainsKey(typeName))
                LoggingException.ThrowLoggerNotRegistered(typeName);

            return (ILogger<T>)registeredLoggers[typeName];
        }

        public IAsyncLogger GetAsyncLogger(string name) {
            if (!registeredLoggers.ContainsKey(name))
                LoggingException.ThrowLoggerNotRegistered(name);

            try {
                return (IAsyncLogger)registeredLoggers[name];
            }
            catch (InvalidCastException) {
                throw LoggingException.LoggerNotAsync(name);
            }
        }

        public IAsyncLogger<T> GetAsyncLogger<T>() where T : class {
            string typeName = typeof(T).Name;
            if (!registeredLoggers.ContainsKey(typeName))
                LoggingException.ThrowLoggerNotRegistered(typeName);

            try {
                return (IAsyncLogger<T>)registeredLoggers[typeName];
            }
            catch (InvalidCastException) {
                throw LoggingException.LoggerNotAsync(typeName);
            }
        }

        public void RegisterLogger(ILogger logger) {
            if (registeredLoggers.ContainsKey(logger.Name))
                LoggingException.ThrowLoggerRegistered(logger.Name);

            ConfigureLogger(logger, null);
            registeredLoggers[logger.Name] = logger;
        }

        private bool disposed = false;
        public void Dispose() {
            if (disposed)
                return;

            foreach (ILogger logger in registeredLoggers.Values)
                logger.Dispose();

            registeredLoggers.Clear();
            disposed = true;
        }

        private void OnProcessExit(object sender, EventArgs e) {
            Dispose();
        }
    }
}
