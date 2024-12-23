﻿using HBLibrary.Interface.Logging;
using HBLibrary.Interface.Logging.Configuration;
using HBLibrary.Interface.Logging.Exceptions;
using HBLibrary.Logging.Configuration;

namespace HBLibrary.Logging;
public sealed class LoggerRegistry : ILoggerRegistry {
    private readonly Dictionary<string, ILogger> registeredLoggers = [];
    public ILogConfiguration GlobalConfiguration { get; private set; } = LogConfiguration.Default;
    public IReadOnlyDictionary<string, ILogger> RegisteredLoggers => registeredLoggers;
    public bool IsConfigured { get; private set; } = false;
    public bool IsEnabled { get; private set; } = true;

    public LoggerRegistry() {
        AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
    }

    public void ConfigureLogger(ILogger logger, LogConfigurationDelegate configMethod) {
        ILogConfiguration configuration = configMethod.Invoke(new LogConfigurationBuilder());
        if (logger is not IAsyncLogger && configuration.AsyncTargets.Count > 0)
            LoggingException.ThrowAsyncTargetsNotAllowed(logger.GetType().Name);

        logger.Configuration = configuration;
    }

    public ILoggerRegistry ConfigureRegistry(LogConfigurationDelegate configMethod) {
        if (IsConfigured)
            LoggingException.ThrowRegistryConfigured();

        IsConfigured = true;
        GlobalConfiguration = configMethod.Invoke(new LogConfigurationBuilder());

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

        logger.Registry = this;
        registeredLoggers[logger.Name] = logger;
    }

    private bool disposed = false;
    public void Dispose() {
        if (disposed)
            return;

        AppDomain.CurrentDomain.ProcessExit -= OnProcessExit;

        foreach (ILogger logger in registeredLoggers.Values)
            logger.Dispose();

        registeredLoggers.Clear();

        GlobalConfiguration.Dispose();
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        GlobalConfiguration = null;
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

        disposed = true;
    }

    private void OnProcessExit(object? sender, EventArgs e) {
        Dispose();
    }

    public void Enable() {
        IsEnabled = true;
    }

    public void Disable() {
        IsEnabled = false;
    }
}
