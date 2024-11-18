using HBLibrary.Interface.Logging;
using HBLibrary.Interface.Logging.Configuration;
using HBLibrary.Logging.Configuration;
using HBLibrary.Logging.Loggers;
using System.Xml.Linq;

namespace HBLibrary.Logging;
public sealed class LoggerFactory : ILoggerFactory {
    public ILoggerRegistry Registry { get; }
    public ILogger CreateLogger(string name, LogConfigurationDelegate configuration) {
        Logger logger = new Logger(name) {
            Configuration = configuration.Invoke(new LogConfigurationBuilder())
        };

        return logger;
    }

    public ILogger<T> CreateLogger<T>(LogConfigurationDelegate configuration) where T : class {
        Logger<T> logger = new Logger<T> {
            Configuration = configuration.Invoke(new LogConfigurationBuilder())
        };

        return logger;
    }


    public IAsyncLogger CreateAsyncLogger(string name, LogConfigurationDelegate configuration) { 
        AsyncLogger logger = new AsyncLogger(name) {
            Configuration = configuration.Invoke(new LogConfigurationBuilder())
        };

        return logger;
    }

    public IAsyncLogger<T> CreateAsyncLogger<T>(LogConfigurationDelegate configuration) where T : class {
        AsyncLogger<T> logger = new AsyncLogger<T> {
            Configuration = configuration.Invoke(new LogConfigurationBuilder())
        };

        return logger;
    }

    public LoggerFactory(ILoggerRegistry registry) {
        Registry = registry;
    }

    public ILogger GetOrCreateLogger(string name) {
        if (Registry.ContainsLogger(name))
            return Registry.GetLogger(name);

        Logger logger = new Logger(name) {
            Registry = Registry
        };
        Registry.RegisterLogger(logger);
        return logger;
    }

    public ILogger<T> GetOrCreateLogger<T>() where T : class {
        if (Registry.ContainsLogger<T>())
            return Registry.GetLogger<T>();

        Logger<T> logger = new Logger<T> {
            Registry = Registry
        };
        Registry.RegisterLogger(logger);
        return logger;
    }

    public IAsyncLogger GetOrCreateAsyncLogger(string name) {
        if (Registry.ContainsLogger(name))
            return Registry.GetAsyncLogger(name);

        AsyncLogger logger = new AsyncLogger(name) {
            Registry = Registry
        };
        Registry.RegisterLogger(logger);
        return logger;
    }

    public IAsyncLogger<T> GetOrCreateAsyncLogger<T>() where T : class {
        if (Registry.ContainsLogger<T>())
            return Registry.GetAsyncLogger<T>();

        AsyncLogger<T> logger = new AsyncLogger<T> {
            Registry = Registry
        };
        Registry.RegisterLogger(logger);
        return logger;
    }

    public ILogConfiguration CreateConfiguration(LogConfigurationDelegate configuration) {
        return configuration(new LogConfigurationBuilder());
    }
}
