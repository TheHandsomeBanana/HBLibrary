using HBLibrary.NetFramework.Services.Logging.Configuration;
using HBLibrary.NetFramework.Services.Logging.Statements;
using HBLibrary.NetFramework.Services.Logging.Targets;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Emit;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;

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
                .WithLevelThreshold(LogLevel.Debug)
                .AddFileTarget(LogFile, false)
                .WithDisplayFormat(LogDisplayFormat.Minimal)
                .Build());

            string testString = "Testinfo";
            logger.Info(testString);
            logger.Dispose();
            string content = File.ReadAllText(LogFile);
            Assert.AreNotEqual("", content);

            File.WriteAllText(LogFile, "");
        }

        [TestMethod]
        public void StandardLogger_LogConfiguration_Valid() {
            registry.ConfigureRegistry(e => e
                .WithLevelThreshold(LogLevel.Debug)
                .AddMethodTarget((f, _) => File.WriteAllText(LogFile, f.ToMinimalString()), LogLevel.Debug)
                .WithDisplayFormat(LogDisplayFormat.Minimal)
                .Build());

            ILogger<StandardLoggerTests> logger = factory.GetOrCreateStandardLogger<StandardLoggerTests>();
            Assert.AreEqual(1, logger.Configuration.Targets.Count);
            registry.ConfigureLogger(logger, e => e.AddMethodTarget((f, _) => Console.WriteLine(f.ToString())).Build());
            Assert.AreEqual(2, logger.Configuration.Targets.Count);

            registry.Dispose();
            Assert.AreEqual(null, logger.Configuration);
        }

        [TestMethod]
        public void StandardLogger_LogToMethod_Valid() {
            ILogger logger = factory.CreateStandardLogger("TestCategory");
            registry.ConfigureLogger(logger, e => e
            .WithLevelThreshold(LogLevel.Debug)
            .AddMethodTarget((f, _) => Console.WriteLine(f.ToFullString()))
            .WithDisplayFormat(LogDisplayFormat.Full)
            .Build());

            using (StringWriter sw = new StringWriter()) {
                Console.SetOut(sw);
                logger.Error("Testerror");
                Assert.IsTrue(sw.ToString().EndsWith("Log Level: Error\nMessage: Testerror\r\n"));
            }

            logger.Dispose();
        }

        [TestMethod]
        public void StandardLogger_NoLog_LogLevelTooHigh() {
            ILogger<StandardLoggerTests> logger = factory.CreateStandardLogger<StandardLoggerTests>();

            registry.ConfigureLogger(logger, e => e
                .WithLevelThreshold(LogLevel.Error)
                .AddFileTarget(LogFile, false)
                .WithDisplayFormat(LogDisplayFormat.Minimal)
                .Build());

            string testString = "Testinfo";
            logger.Warn(testString);
            logger.Dispose();
            string content = File.ReadAllText(LogFile);
            Assert.AreEqual("", content);
        }

        [TestMethod]
        public async Task StandardLogger_ThreadSafety_LogToFile_Valid() {
            registry.ConfigureRegistry(e => e.WithLevelThreshold(LogLevel.Debug).AddFileTarget(LogFile, false).Build());
            ILogger logger1 = factory.GetOrCreateStandardLogger("Logger1");
            ILogger logger2 = factory.GetOrCreateStandardLogger("Logger2");

            try {
                Task log1 = WriteLog(logger1, 10, "test");
                Task log2 = WriteLog(logger2, 10, "test2");
                await Task.WhenAll(log1, log2);
            }
            catch (Exception ex) {
                Assert.Fail(ex.ToString());
            }
            finally {
            }

            logger1.Dispose();
            logger2.Dispose();

            File.WriteAllText(LogFile, "");
        }


        private readonly List<string> logs = new List<string>();
        [TestMethod]
        public async Task StandardLogger_ThreadSafety_LogToMethod_Valid() {
            registry.ConfigureRegistry(e => e.AddMethodTarget(OnLog, LogLevel.Debug).Build());
            ILogger logger1 = factory.GetOrCreateStandardLogger("Logger1");
            ILogger logger2 = factory.GetOrCreateStandardLogger("Logger2");

            Task log1 = WriteLog(logger1, 10, "test");
            Task log2 = WriteLog(logger2, 10, "test2");
            await Task.WhenAll(log1, log2);

            Assert.AreEqual(20, logs.Count);

            logs.Clear();
        }

        private void OnLog(LogStatement logStatement, LogDisplayFormat format) {
            logs.Add(logStatement.Message);
        }

        private async Task WriteLog(ILogger logger, int iterations, string message) {
            Random rnd = new Random();
            for (int i = 0; i < iterations; i++) {
                await Task.Delay(rnd.Next(1, 5)); // Random delay between 1 and 5 milliseconds
                logger.Info(message + "[" + i + "]");
            }
        }

        [TestMethod]
        public void StandardLogger_ConsoleTarget_Valid() {
            registry.ConfigureRegistry(e => e
            .WithLevelThreshold(LogLevel.Debug)
            .AddTarget(new ConsoleTarget())
            .Build());

            ILogger logger = factory.GetOrCreateStandardLogger<StandardLoggerTests>();
            using (StringWriter sw = new StringWriter()) {
                Console.SetOut(sw);
                logger.Error("Testerror");
                Assert.IsTrue(sw.ToString().EndsWith("Log Level: Error\nMessage: Testerror\n\r\n"));
            }
        }

        [TestMethod]
        public void StandardLogger_TargetLevelThresholdOverride_Valid() {
            ILogger logger = factory.GetOrCreateStandardLogger<StandardLoggerTests>();
            registry.ConfigureLogger(logger, e => e
            .WithLevelThreshold(LogLevel.Error)
            .AddMethodTarget((f, _) => Console.WriteLine(f.ToFullString()), LogLevel.Warning)
            .Build());

            Assert.AreEqual(logger.Configuration.LevelThreshold, logger.Configuration.Targets[0].LevelThreshold);
        }
    }
}
