using HBLibrary.Services.Logging.Configuration;
using HBLibrary.Services.Logging.Statements;
using HBLibrary.Services.Logging.Targets;

namespace HBLibrary.Services.Logging.Tests;
[TestClass]
public class StandardLoggerTests {
    private const string LogFile = "../../../assets/standardLogFile";

    [TestMethod]
    public void StandardLogger_LogToFile() {
        LoggerRegistry registry = new LoggerRegistry();
        LoggerFactory factory = new LoggerFactory(registry);

        ILogger<StandardLoggerTests> logger = factory.CreateLogger<StandardLoggerTests>();

        registry.ConfigureLogger(logger, e => e
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
    public void StandardLogger_LogConfiguration() {
        LoggerRegistry registry = new LoggerRegistry();
        LoggerFactory factory = new LoggerFactory(registry);

        registry.ConfigureRegistry(e => e
            .AddMethodTarget((f, _) => File.WriteAllText(LogFile, f.ToMinimalString()))
            .WithDisplayFormat(LogDisplayFormat.Minimal)
            .Build());

        Assert.AreEqual(1, registry.GlobalConfiguration.Targets.Count);

        ILogger<StandardLoggerTests> logger = factory.GetOrCreateLogger<StandardLoggerTests>();
        Assert.AreEqual(0, logger.Configuration.Targets.Count);
        registry.ConfigureLogger(logger, e => e.AddMethodTarget((f, _) => Console.WriteLine(f.ToString())).Build());
        Assert.AreEqual(1, logger.Configuration.Targets.Count);

        registry.Dispose();
        Assert.AreEqual(null, logger.Configuration);
    }

    [TestMethod]
    public void StandardLogger_LogToMethod() {
        LoggerRegistry registry = new LoggerRegistry();
        LoggerFactory factory = new LoggerFactory(registry);

        ILogger logger = factory.CreateLogger("TestCategory");
        registry.ConfigureLogger(logger, e => e
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
        LoggerRegistry registry = new LoggerRegistry();
        LoggerFactory factory = new LoggerFactory(registry);

        ILogger<StandardLoggerTests> logger = factory.CreateLogger<StandardLoggerTests>();

        registry.ConfigureLogger(logger, e => e
            .WithLevelThreshold(LogLevel.Error)
            .AddFileTarget(LogFile, false)
            .WithDisplayFormat(LogDisplayFormat.Minimal)
            .Build());

        string testString = "Testinfo";
        logger.Warn(testString);
        logger.Dispose();
        string content = File.ReadAllText(LogFile);
        try {
            Assert.AreEqual(FileTarget.Logo + FileTarget.TargetName + "\r\n", content);
        }
        catch (Exception e) {
            Assert.Fail(e.ToString());
        }
        finally {
            File.WriteAllText(LogFile, "");
        }
    }

    [TestMethod]
    public async Task StandardLogger_ThreadSafety_LogToFile() {
        LoggerRegistry registry = new LoggerRegistry();
        LoggerFactory factory = new LoggerFactory(registry);

        registry.ConfigureRegistry(e => e.WithLevelThreshold(LogLevel.Debug).AddFileTarget(LogFile, false).Build());
        ILogger logger1 = factory.GetOrCreateLogger("Logger1");
        ILogger logger2 = factory.GetOrCreateLogger("Logger2");

        try {
            Task log1 = WriteLog(logger1, 10, "test");
            Task log2 = WriteLog(logger2, 10, "test2");
            await Task.WhenAll(log1, log2);

            registry.Dispose();
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
    public async Task StandardLogger_ThreadSafety_LogToMethod() {
        LoggerRegistry registry = new LoggerRegistry();
        LoggerFactory factory = new LoggerFactory(registry);

        registry.ConfigureRegistry(e => e.AddMethodTarget(OnLog, LogLevel.Debug).Build());
        ILogger logger1 = factory.GetOrCreateLogger("Logger1");
        ILogger logger2 = factory.GetOrCreateLogger("Logger2");

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
    public void StandardLogger_ConsoleTarget() {
        LoggerRegistry registry = new LoggerRegistry();
        LoggerFactory factory = new LoggerFactory(registry);

        registry.ConfigureRegistry(e => e
        .AddTarget(new ConsoleTarget())
        .Build());

        ILogger logger = factory.GetOrCreateLogger<StandardLoggerTests>();
        using (StringWriter sw = new StringWriter()) {
            Console.SetOut(sw);
            logger.Error("Testerror");
            Assert.IsTrue(sw.ToString().EndsWith("Log Level: Error\nMessage: Testerror\n\r\n"));
        }
    }

    [TestMethod]
    public void StandardLogger_TargetLevelThreshold_GlobalApplied() {
        LoggerRegistry registry = LoggerRegistry.FromConfiguration(e => e.WithLevelThreshold(LogLevel.Error).Build());
        LoggerFactory factory = new LoggerFactory(registry);
        StringWriter sw = new StringWriter();
        Console.SetOut(sw);


        ILogger logger = factory.GetOrCreateLogger<StandardLoggerTests>();
        registry.ConfigureLogger(logger, e => e
        .WithLevelThreshold(LogLevel.Warning)
        .AddMethodTarget((f, _) => Console.WriteLine(f.ToFullString()), LogLevel.Info)
        .Build());

        logger.Warn("Testwarning");

        Assert.AreEqual("", sw.ToString());
        sw.Dispose();
        registry.Dispose();
    }

    [TestMethod]
    public void StandardLogger_TargetLevelThreshold_LoggerApplied() {
        LoggerRegistry registry = new LoggerRegistry();
        LoggerFactory factory = new LoggerFactory(registry);
        StringWriter sw = new StringWriter();
        Console.SetOut(sw);

        ILogger logger = factory.GetOrCreateLogger<StandardLoggerTests>();
        registry.ConfigureLogger(logger, e => e
        .WithLevelThreshold(LogLevel.Error)
        .AddMethodTarget((f, _) => Console.WriteLine(f.ToFullString()), LogLevel.Warning)
        .Build());

        logger.Warn("Testwarning");

        Assert.AreEqual("", sw.ToString());
        sw.Dispose();
        registry.Dispose();
    }

    [TestMethod]
    public void StandardLogger_TargetLevelThreshold_TargetApplied() {
        LoggerRegistry registry = new LoggerRegistry();
        LoggerFactory factory = new LoggerFactory(registry);

        StringWriter sw = new StringWriter();
        Console.SetOut(sw);

        ILogger logger = factory.GetOrCreateLogger<StandardLoggerTests>();
        registry.ConfigureLogger(logger, e => e
        .AddMethodTarget((f, _) => Console.WriteLine(f.ToFullString()), LogLevel.Error)
        .Build());

        logger.Warn("Testwarning");

        Assert.AreEqual("", sw.ToString());
        sw.Dispose();
        registry.Dispose();
    }

    [TestMethod]
    public void StandardLogger_DebugTarget() {
        LoggerRegistry registry = new LoggerRegistry();
        LoggerFactory factory = new LoggerFactory(registry);

        ILogger<StandardLoggerTests> logger = factory.CreateLogger<StandardLoggerTests>();

        registry.ConfigureLogger(logger, e => e
            .AddTarget(new DebugTarget())
            .WithDisplayFormat(LogDisplayFormat.Minimal)
            .Build());

        try {
            logger.Info("Testinformation");
        }
        catch (Exception e) {
            Assert.Fail(e.ToString());
        }

        logger.Dispose();
        registry.Dispose();
        // Debug.Listeners is not available on cross platform .net

        //TextWriter writer = Debug.Listeners[1].GetType().GetProperty("Writer").GetValue(Debug.Listeners[1]) as TextWriter;
        //Assert.IsNotNull(writer);
        //string value = writer.ToString();
        //Assert.IsTrue(value.EndsWith("[Info]: Testinformation\r\n"));
    }
}
