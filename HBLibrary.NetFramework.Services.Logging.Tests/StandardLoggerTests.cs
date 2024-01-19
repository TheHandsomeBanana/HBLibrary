﻿using HBLibrary.NetFramework.Services.Logging.Configuration;
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
                .AddFileTarget(LogFile, false)
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
               .AddMethodTarget((f, _) => File.WriteAllText(LogFile, f.ToMinimalString()), LogLevel.Debug)
               .WithDisplayFormat(Configuration.LogDisplayFormat.Minimal)
               .Build());

            ILogger<StandardLoggerTests> logger = factory.GetOrCreateStandardLogger<StandardLoggerTests>();
            Assert.AreEqual(1, logger.Configuration.Targets.Count);
            registry.ConfigureLogger(logger, e => e.AddMethodTarget((f, _) => Console.WriteLine(f.ToString())).Build());
            Assert.AreEqual(2, logger.Configuration.Targets.Count);

            // Overrides logger configuration
            registry.ConfigureRegistry(e => e
            .AddFileTarget(LogFile, false)
            .Build());

            Assert.AreEqual(1, logger.Configuration.Targets.Count);
        }

        [TestMethod]
        public void StandardLogger_LogToMethod_Valid() {
            ILogger logger = factory.CreateStandardLogger("TestCategory");
            registry.ConfigureLogger(logger, e => e
            .AddMethodTarget((f, _) => Console.WriteLine(f.ToFullString()))
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
                .AddFileTarget(LogFile, false)
                .WithDisplayFormat(LogDisplayFormat.Minimal)
                .Build());

            string testString = "Testinfo";
            logger.Warn(testString);
            string content = File.ReadAllText(LogFile);
            Assert.AreEqual("", content);
        }
    }
}
