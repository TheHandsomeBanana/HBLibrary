using BenchmarkDotNet.Attributes;
using HBLibrary.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerformanceTests;
[MemoryDiagnoser]
public class ExceptionVersusResult {

    [Benchmark]
    public void TestExceptionSpeed() {
        try {
            TestExceptionSpeedInternal();
        }
        catch(Exception ex) {
            string exString = ex.ToString();
        }
    } 

    private void TestExceptionSpeedInternal() {
        throw new NotImplementedException();
    }

    [Benchmark]
    public void TestResultTypeSpeed() {
        Result res = TestResultTypeSpeedInternal();
        string exString;
        res.TapError(e => exString = e.ToString());
    }

    private Result TestResultTypeSpeedInternal() {
        return Result.Fail(new NotImplementedException());
    }
}
