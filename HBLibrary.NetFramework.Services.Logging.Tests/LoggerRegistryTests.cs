using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.NetFramework.Services.Logging.Tests {
    [TestClass]
    public class LoggerRegistryTests {
        [TestMethod]
        public void GlobalConfiguration_ThresholdOverride_Valid() {
            ILoggerRegistry registry = LoggerRegistry.FromConfiguration(e => e.WithLevelThreshold(LogLevel.Error).Build());
            ILoggerFactory factory = new LoggerFactory(registry);

            ILogger logger = factory.CreateStandardLogger("Logger 1");
            registry.RegisterLogger(logger);
            registry.ConfigureLogger(logger, e => e.WithLevelThreshold(LogLevel.Debug).Build());

            Assert.AreEqual(registry.GlobalConfiguration.LevelThreshold, logger.Configuration.LevelThreshold);
            registry.Dispose();
        }
    }
}
