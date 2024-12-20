using BenchmarkDotNet.Running;

namespace PerformanceTests;

internal class Program {
    static void Main(string[] args) {
        BenchmarkRunner.Run<ExceptionVersusResult>();
    }    
}
