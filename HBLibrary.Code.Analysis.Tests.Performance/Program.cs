using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;
using HBLibrary.Code.Analysis.Tests.Performance.SemanticModelCache;

namespace HBLibrary.Code.Analysis.Tests.Performance;



[MemoryDiagnoser]
public class PerformanceTests {
    public static void Main(string[] args) {
        BenchmarkRunner.Run<SMCPerformanceTests>(new SMCPerformanceTestConfig());
    }
}


public class StatisticLogger : ILogger {
    public void Write(LogKind logKind, string text) {
        if (logKind == LogKind.Statistic)
            Console.Write(text);
    }
    public void WriteLine() {
        Console.WriteLine();
    }

    public void WriteLine(LogKind logKind, string text) {
        if (logKind == LogKind.Statistic)
            Console.WriteLine(text);
    }

    public void Flush() { }

    public string Id => nameof(StatisticLogger);
    public int Priority => 0;
}