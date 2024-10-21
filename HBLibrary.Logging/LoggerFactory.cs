using HBLibrary.Interface.Logging;
using HBLibrary.Logging.Loggers;

namespace HBLibrary.Logging;
public sealed class LoggerFactory : ILoggerFactory {
    public ILoggerRegistry Registry { get; }
    public ILogger CreateLogger(string name) => new Logger(name);
    public ILogger<T> CreateLogger<T>() where T : class => new Logger<T>();
    public IAsyncLogger CreateAsyncLogger(string name) => new AsyncLogger(name);
    public IAsyncLogger<T> CreateAsyncLogger<T>() where T : class => new AsyncLogger<T>();

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
}
