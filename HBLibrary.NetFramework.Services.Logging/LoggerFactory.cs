using HBLibrary.NetFramework.Services.Logging.Configuration;
using HBLibrary.NetFramework.Services.Logging.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HBLibrary.NetFramework.Services.Logging {
    public class LoggerFactory : ILoggerFactory {
        public ILoggerRegistry Registry { get; }
        public ILogger CreateStandardLogger(string name) => new StandardLogger(name);
        public ILogger<T> CreateStandardLogger<T>() where T : class => new StandardLogger<T>();
        public ILogger CreateThreadSafeLogger(string name) => new ThreadSafeLogger(name);
        public ILogger<T> CreateThreadSafeLogger<T>() where T : class => new ThreadSafeLogger<T>();
        public IAsyncLogger CreateAsyncLogger(string name) => new AsyncLogger(name);
        public IAsyncLogger<T> CreateAsyncLogger<T>() where T : class => new AsyncLogger<T>();

        public LoggerFactory(ILoggerRegistry registry) {
            this.Registry = registry;
        }

        public ILogger GetOrCreateStandardLogger(string name) {
            if (Registry.ContainsLogger(name))
                return Registry.GetLogger(name);

            StandardLogger logger = new StandardLogger(name);
            Registry.RegisterLogger(logger);
            return logger;
        }

        public ILogger<T> GetOrCreateStandardLogger<T>() where T : class {
            if (Registry.ContainsLogger<T>())
                return Registry.GetLogger<T>();

            StandardLogger<T> logger = new StandardLogger<T>();
            Registry.RegisterLogger(logger);
            return logger;
        }

        public ILogger GetOrCreateThreadSafeLogger(string name) {
            if (Registry.ContainsLogger(name))
                return Registry.GetLogger(name);

            ThreadSafeLogger logger = new ThreadSafeLogger(name);
            Registry.RegisterLogger(logger);
            return logger;
        }

        public ILogger<T> GetOrCreateThreadSafeLogger<T>() where T : class {
            if (Registry.ContainsLogger<T>())
                return Registry.GetLogger<T>();

            ThreadSafeLogger<T> logger = new ThreadSafeLogger<T>();
            Registry.RegisterLogger(logger);
            return logger;
        }

        public IAsyncLogger GetOrCreateAsyncLogger(string name) {
            if (Registry.ContainsLogger(name))
                return Registry.GetAsyncLogger(name);

            AsyncLogger logger = new AsyncLogger(name);
            Registry.RegisterLogger(logger);
            return logger;
        }

        public IAsyncLogger<T> GetOrCreateAsyncLogger<T>() where T : class {
            if (Registry.ContainsLogger<T>())
                return Registry.GetAsyncLogger<T>();

            AsyncLogger<T> logger = new AsyncLogger<T>();
            Registry.RegisterLogger(logger);
            return logger;
        }
    }
}
