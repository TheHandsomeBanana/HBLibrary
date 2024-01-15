using HBLibrary.NetFramework.Services.Logging.Configuration;
using HBLibrary.NetFramework.Services.Logging.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.NetFramework.Services.Logging {
    public class LoggerFactory : ILoggerFactory {
        private readonly Dictionary<string, IAsyncLogger> asyncLoggers = new Dictionary<string, IAsyncLogger>();
        private readonly Dictionary<string, ILogger> loggers = new Dictionary<string, ILogger>();
        public ILogConfiguration Configuration { get; private set; } = LogConfiguration.Default;
        public IDictionary<string, ILogger> RegisteredLoggers => loggers;
        public IDictionary<string, IAsyncLogger> RegisteredAsyncLoggers => asyncLoggers;

        public ILogger GetOrCreateStandardLogger(string category) {
            if(loggers.TryGetValue(category, out ILogger logger)) 
                return logger;

            logger = new StandardLogger(category);
            logger.Configuration = Configuration;
            return logger;
        }

        public ILogger<T> GetOrCreateStandardLogger<T>() where T : class {
            if (loggers.TryGetValue(typeof(T).Name, out ILogger logger))
                return (ILogger<T>)logger;

            logger = new StandardLogger<T>();
            logger.Configuration = Configuration;
            return (ILogger<T>)logger;
        }

        public ILogger GetOrCreateThreadSafeLogger(string category) {
            if (loggers.TryGetValue(category, out ILogger logger))
                return logger;

            logger = new ThreadSafeLogger(category);
            logger.Configuration = Configuration;
            return logger;
        }

        public ILogger<T> GetOrCreateThreadSafeLogger<T>() where T : class {
            if (loggers.TryGetValue(typeof(T).Name, out ILogger logger))
                return (ILogger<T>)logger;

            logger = new ThreadSafeLogger<T>();
            logger.Configuration = Configuration;
            return (ILogger<T>)logger;
        }

        public IAsyncLogger GetOrCreateAsyncLogger(string category) {
            if (asyncLoggers.TryGetValue(category, out IAsyncLogger logger))
                return logger;

            logger = new AsyncLogger(category);
            logger.Configuration = Configuration;
            return logger;
        }

        public IAsyncLogger<T> GetOrCreateAsyncLogger<T>() where T : class {
            if (asyncLoggers.TryGetValue(typeof(T).Name, out IAsyncLogger logger))
                return (IAsyncLogger<T>)logger;

            logger = new AsyncLogger<T>();
            logger.Configuration = Configuration;
            return (IAsyncLogger<T>)logger;
        }

        public ILoggerFactory ConfigureFactory(LogConfigurationDelegate configMethod) {
            Configuration = configMethod.Invoke(new LogConfigurationBuilder());
            return this;
        }

        public static LoggerFactory FromConfiguration(LogConfigurationDelegate configMethod) {
            LoggerFactory factory = new LoggerFactory();
            factory.Configuration = configMethod.Invoke(new LogConfigurationBuilder());
            return factory;
        }

        public void ConfigureLogger(ILogger logger, LogConfigurationDelegate configMethod) 
            => logger.Configure(configMethod);
        
        public void ConfigureLogger(IAsyncLogger logger, LogConfigurationDelegate configMethod) 
            => logger.Configure(configMethod);

        
    }
}
