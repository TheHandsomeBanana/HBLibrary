using HBLibrary.Services.Logging.Configuration;
using HBLibrary.Services.Logging.Statements;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.Logging.Tests {
    [TestClass]
    public class AsyncLoggerTests {
        private const string LogFile = "../../assets/asyncLogFile";

        [TestMethod]
        public async Task AsyncLogger_LogToFile_Valid() {
            LoggerRegistry registry = new LoggerRegistry();
            LoggerFactory factory = new LoggerFactory(registry);
            IAsyncLogger<AsyncLoggerTests> logger = factory.GetOrCreateAsyncLogger<AsyncLoggerTests>();

            registry.ConfigureLogger(logger, e => e
                .AddFileTarget(LogFile, true)
                .WithDisplayFormat(LogDisplayFormat.Minimal)
                .Build());

            string testString = "Testinfo";
            await logger.InfoAsync(testString);
            logger.Dispose();

            string content = File.ReadAllText(LogFile);
            Assert.AreNotEqual("", content);
            File.WriteAllText(LogFile, "");
        }

        [TestMethod]
        public async Task AsyncLogger_LogToMethod_Valid() {
            LoggerRegistry registry = new LoggerRegistry();
            LoggerFactory factory = new LoggerFactory(registry);

            IAsyncLogger logger = factory.GetOrCreateAsyncLogger("TestCategory");
            registry.ConfigureLogger(logger, e => e
            .AddMethodTarget((f, _) => Console.WriteLine(f.ToFullString()))
            .Build());

            using (StringWriter sw = new StringWriter()) {
                Console.SetOut(sw);
                await logger.InfoAsync("Testinfo");
                Assert.IsTrue(sw.ToString().EndsWith("Log Level: Info\nMessage: Testinfo\r\n"));
            }

            logger.Dispose();
        }

        [TestMethod]
        public async Task AsyncLogger_LogToFile_CheckThreadSafety_Valid() {
            LoggerRegistry registry = new LoggerRegistry();
            LoggerFactory factory = new LoggerFactory(registry);

            registry.ConfigureRegistry(e => e.AddFileTarget(LogFile, false).Build());
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
                registry.Dispose();
                File.WriteAllText(LogFile, "");
            }
        }

        private async Task WriteLog(IAsyncLogger logger, int iterations, string message) {
            Random rnd = new Random();
            for (int i = 0; i < iterations; i++) {
                await Task.Delay(rnd.Next(1, 5)); // Random delay between 1 and 5 milliseconds
                await logger.InfoAsync(message + "[" + i + "]");
            }
        }
    }
}
