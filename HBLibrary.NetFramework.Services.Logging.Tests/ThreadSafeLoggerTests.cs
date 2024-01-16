﻿using HBLibrary.NetFramework.Services.Logging.Statements;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.NetFramework.Services.Logging.Tests {
    [TestClass]
    public class ThreadSafeLoggerTests {
        private const string LogFile = "../../assets/threadSafeLogFile";
        private readonly static ILoggerFactory factory = new LoggerFactory();

        [TestMethod]
        public async Task ThreadSafeLogger_LogToFile_Valid() {
            factory.ConfigureFactory(e => e.AddTarget(LogFile, LogLevel.Debug).Build());
            ILogger logger1 = factory.CreateThreadSafeLogger("Logger1");
            ILogger logger2 = factory.CreateThreadSafeLogger("Logger2");

            try {
                Task log1 = WriteLog(logger1, 10, "test");
                Task log2 = WriteLog(logger2, 10, "test2");
                await Task.WhenAll(log1, log2);
            }
            catch (Exception ex) {
                Assert.Fail(ex.ToString());
            }
            finally {
                File.WriteAllText(LogFile, "");
            }
        }


        private readonly List<string> logs = new List<string>();
        [TestMethod]
        public async Task ThreadSafeLogger_LogToMethod_Valid() {
            factory.ConfigureFactory(e => e.AddTarget(OnLog, LogLevel.Debug).Build());
            ILogger logger1 = factory.CreateThreadSafeLogger("Logger1");
            ILogger logger2 = factory.CreateThreadSafeLogger("Logger2");

            Task log1 = WriteLog(logger1, 10, "test");
            Task log2 = WriteLog(logger2, 10, "test2");
            await Task.WhenAll(log1, log2);

            Assert.AreEqual(20, logs.Count);

            logs.Clear();
        }

        private void OnLog(LogStatement logStatement) {
            logs.Add(logStatement.Message);
        }

        private async Task WriteLog(ILogger logger, int iterations, string message) {
            Random rnd = new Random();
            for (int i = 0; i < iterations; i++) {
                await Task.Delay(rnd.Next(1, 5)); // Random delay between 1 and 5 milliseconds
                logger.Info(message + "[" + i + "]");
            }
        }
        
    }
}
