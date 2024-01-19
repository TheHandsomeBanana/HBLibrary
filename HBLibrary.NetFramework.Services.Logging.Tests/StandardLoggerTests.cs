using HBLibrary.NetFramework.Services.Logging.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Reflection.Emit;
using System.Runtime.Remoting.Messaging;

namespace HBLibrary.NetFramework.Services.Logging.Tests {
    [TestClass]
    public class StandardLoggerTests {
        private const string LogFile = "../../assets/standardLogFile";
        private readonly static ILoggerRegistry registry = new LoggerRegistry();
        private readonly static ILoggerFactory factory = new LoggerFactory(registry);

        [TestMethod]
        public void StandardLogger_LogToFile_Valid() {
            ILogger<StandardLoggerTests> logger = factory.CreateStandardLogger<StandardLoggerTests>();

            registry.ConfigureLogger(logger, e => e
                .AddFileTarget(LogFile, LogLevel.Debug)
                .WithDisplayFormat(LogDisplayFormat.Minimal)
                .Build());

            string testString = "Testinfo";
            logger.Info(testString);
            string content = File.ReadAllText(LogFile);
            Assert.AreNotEqual("", content);

            File.WriteAllText(LogFile, "");
        }

        [TestMethod]
        public void StandardLogger_LogConfiguration_Valid() {
            registry.ConfigureRegistry(e => e
               .AddMethodTarget(f => File.WriteAllText(LogFile, f.ToMinimalString()), LogLevel.Debug)
               .WithDisplayFormat(Configuration.LogDisplayFormat.Minimal)
               .Build());

            ILogger<StandardLoggerTests> logger = factory.GetOrCreateStandardLogger<StandardLoggerTests>();
            Assert.AreEqual(1, logger.Configuration.Targets.Length);
            registry.ConfigureLogger(logger, e => e.AddMethodTarget(f => Console.WriteLine(f.ToString()), LogLevel.Debug).Build());
            Assert.AreEqual(2, logger.Configuration.Targets.Length);

            // Overrides logger configuration
            registry.ConfigureRegistry(e => e
            .AddFileTarget(LogFile, LogLevel.Debug)
            .Build());

            Assert.AreEqual(1, logger.Configuration.Targets.Length);
        }

        [TestMethod]
        public void StandardLogger_LogToMethod_Valid() {
            ILogger logger = factory.CreateStandardLogger("TestCategory");
            registry.ConfigureLogger(logger, e => e
            .AddMethodTarget(f => Console.WriteLine(f.ToFullString()), LogLevel.Debug)
            .WithDisplayFormat(LogDisplayFormat.Full)
            .Build());

            using (StringWriter sw = new StringWriter()) {
                Console.SetOut(sw);
                logger.Error("Testerror");
                Assert.IsTrue(sw.ToString().EndsWith("Log Level: Error\nMessage: Testerror\r\n"));
            }
        }

        [TestMethod]
        public void StandardLogger_NoLog_LogLevelTooHigh() {
            ILogger<StandardLoggerTests> logger = factory.CreateStandardLogger<StandardLoggerTests>();

            registry.ConfigureLogger(logger, e => e
                .AddFileTarget(LogFile, LogLevel.Error)
                .WithDisplayFormat(LogDisplayFormat.Minimal)
                .Build());

            string testString = "Testinfo";
            logger.Warn(testString);
            string content = File.ReadAllText(LogFile);
            Assert.AreEqual("", content);
        }
    }
}
