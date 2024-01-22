using HBLibrary.Services.Logging.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.Logging.Tests {
    [TestClass]
    public class LoggerRegistryTests {
        [TestMethod]
        public void LoggerRegistry_RegisterLogger_Valid() {
            ILoggerRegistry registry = LoggerRegistry.FromConfiguration(e => e.WithLevelThreshold(LogLevel.Error).Build());
            ILoggerFactory factory = new LoggerFactory(registry);

            ILogger logger = factory.CreateLogger("Logger 1");
            registry.RegisterLogger(logger);
            Assert.IsNotNull(logger.Registry);
            registry.Dispose();
        }

        [TestMethod]
        public void LoggerRegistry_RegisterLogger_ThrowsException() {
            ILoggerRegistry registry = LoggerRegistry.FromConfiguration(e => e.WithLevelThreshold(LogLevel.Error).Build());
            ILoggerFactory factory = new LoggerFactory(registry);

            ILogger logger = factory.GetOrCreateLogger("Logger 1");
            Assert.ThrowsException<LoggingException>(() => registry.RegisterLogger(logger));

            registry.Dispose();
        }

        [TestMethod]
        public void LoggerRegistry_GetLogger_Valid() {
            ILoggerRegistry registry = LoggerRegistry.FromConfiguration(e => e.WithLevelThreshold(LogLevel.Error).Build());
            ILoggerFactory factory = new LoggerFactory(registry);

            ILogger logger = factory.GetOrCreateLogger("Logger 1");
            try {
                logger = registry.GetLogger(logger.Name);
            }
            catch (Exception e) {
                Assert.Fail(e.ToString());
            }
            finally {
                registry.Dispose();
            }
        }

        [TestMethod]
        public void LoggerRegistry_GetLogger_ThrowsException() {
            ILoggerRegistry registry = LoggerRegistry.FromConfiguration(e => e.WithLevelThreshold(LogLevel.Error).Build());
            ILoggerFactory factory = new LoggerFactory(registry);

            ILogger logger = factory.CreateLogger("Logger 1");
            Assert.ThrowsException<LoggingException>(() => registry.GetLogger(logger.Name));

            registry.Dispose();
        }
    }
}
