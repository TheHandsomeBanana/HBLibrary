using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using HBLibrary.Code.Analysis.Tests.Performance.SemanticModelCache;
using HBLibrary.Code.Analysis.Utilities;
using HBLibrary.Common.Parallelism;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using System.Collections.Concurrent;
using System.Threading;

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