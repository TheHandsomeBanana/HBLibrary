using HBLibrary.NetFramework.Services.Logging.Configuration;
using HBLibrary.NetFramework.Services.Logging.Statements;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.NetFramework.Services.Logging.Tests {
    [TestClass]
    public class AsyncLoggerTests {
        private const string LogFile = "../../assets/asyncLogFile";
        private readonly static ILoggerFactory factory = new LoggerFactory();

        [TestMethod]
        public async Task AsyncLogger_LogToFile_Valid() {
            IAsyncLogger<AsyncLoggerTests> logger = factory.GetOrCreateAsyncLogger<AsyncLoggerTests>();

            factory.ConfigureLogger(logger, e => e
                .AddTarget(LogFile, LogLevel.Debug)
                .WithDisplayFormat(LogDisplayFormat.Minimal)
                .Build());

            string testString = "Testinfo";
            await logger.Info(testString);
            string content = File.ReadAllText(LogFile);
            Assert.AreNotEqual("", content);

            File.WriteAllText(LogFile, "");
        }

        [TestMethod]
        public async Task AsyncLogger_LogToMethod_Valid() {
            IAsyncLogger logger = factory.GetOrCreateAsyncLogger("TestCategory");
            logger.Configure(e => e
            .AddTarget(f => Console.WriteLine(f.ToString()), LogLevel.Debug)
            .WithDisplayFormat(LogDisplayFormat.Full)
            .Build());

            using (StringWriter sw = new StringWriter()) {
                Console.SetOut(sw);
                await logger.Info("Testinfo");
                Assert.IsTrue(sw.ToString().EndsWith("Log Level: Info\nMessage: Testinfo\r\n"));
            }
        }

        [TestMethod]
        public async Task AsyncLogger_LogToFile_CheckThreadSafety_Valid() {
            factory.ConfigureFactory(e => e.AddTarget(LogFile, LogLevel.Debug).Build());
            IAsyncLogger logger1 = factory.GetOrCreateAsyncLogger("Logger1");
            IAsyncLogger logger2 = factory.GetOrCreateAsyncLogger("Logger2");

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

        private async Task WriteLog(IAsyncLogger logger, int iterations, string message) {
            Random rnd = new Random();
            for (int i = 0; i < iterations; i++) {
                await Task.Delay(rnd.Next(1, 5)); // Random delay between 1 and 5 milliseconds
                await logger.Info(message + "[" + i + "]");
            }
        }
    }
}
