using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common.Tests;
[TestClass]
[MemoryDiagnoser]
public class PropertyMapperTests {
    [TestMethod]
    public void AutoMapperTest1() {
        BenchmarkRunner.Run<PropertyMapperTests>();
    }

    private TestModel model;

    [GlobalSetup]
    public void GlobalSetup() {
        CancellationTokenSource cts = new CancellationTokenSource();
        model = new TestModel {
            Test1 = "Test",
            Test2 = 0,
            Test3 = DateTime.MinValue,
            Test4 = true,
            Test5 = cts.Token,
            Test6 = cts,
            Test7 = new Memory<string>(["abc", "def", "ghi"]),
            Test8 = "Test",
            Test9 = new Stack<int>([1, 2, 3, 4, 5]),
            Test10 = 0.0
        };
    }

    [Benchmark]
    public void AutoMapper() {
        TestCoreModel model = this.model.Map<TestModel, TestCoreModel>();

        Assert.AreEqual(this.model.Test1, model.Test1);
        Assert.AreEqual(this.model.Test2, model.Test2);
        Assert.AreEqual(this.model.Test3, model.Test3);
        Assert.AreEqual(this.model.Test4, model.Test4);
        Assert.AreEqual(this.model.Test5, model.Test5);
        Assert.AreEqual(this.model.Test6, model.Test6);
        Assert.AreEqual(this.model.Test7, model.Test7);
        Assert.AreEqual(this.model.Test8, model.Test8);
        Assert.AreEqual(this.model.Test9, model.Test9);
        Assert.AreEqual(this.model.Test10, model.Test10);
        Assert.AreEqual(this.model.test22, model.test22);
        Assert.AreEqual(this.model.webclient, model.webclient);
    }

    [Benchmark]
    public void GenericUnsafeMapper() {
        TestCoreModel model = this.model.MapUnsafe<TestModel, TestCoreModel>();

        Assert.AreEqual(this.model.Test1, model.Test1);
        Assert.AreEqual(this.model.Test2, model.Test2);
        Assert.AreEqual(this.model.Test3, model.Test3);
        Assert.AreEqual(this.model.Test4, model.Test4);
        Assert.AreEqual(this.model.Test5, model.Test5);
        Assert.AreEqual(this.model.Test6, model.Test6);
        Assert.AreEqual(this.model.Test7, model.Test7);
        Assert.AreEqual(this.model.Test8, model.Test8);
        Assert.AreEqual(this.model.Test9, model.Test9);
        Assert.AreEqual(this.model.Test10, model.Test10);
        Assert.AreEqual(this.model.test22, model.test22);
        Assert.AreEqual(this.model.webclient, model.webclient);
    }

    [Benchmark]
    public void UnsafeMapper() {
        TestCoreModel model = (TestCoreModel)this.model.MapUnsafe(typeof(TestModel), typeof(TestCoreModel));

        Assert.AreEqual(this.model.Test1, model.Test1);
        Assert.AreEqual(this.model.Test2, model.Test2);
        Assert.AreEqual(this.model.Test3, model.Test3);
        Assert.AreEqual(this.model.Test4, model.Test4);
        Assert.AreEqual(this.model.Test5, model.Test5);
        Assert.AreEqual(this.model.Test6, model.Test6);
        Assert.AreEqual(this.model.Test7, model.Test7);
        Assert.AreEqual(this.model.Test8, model.Test8);
        Assert.AreEqual(this.model.Test9, model.Test9);
        Assert.AreEqual(this.model.Test10, model.Test10);
        Assert.AreEqual(this.model.test22, model.test22);
        Assert.AreEqual(this.model.webclient, model.webclient);
    }
}

public class TestModel {
    public string test22 = "test22";
    public WebClient webclient = new WebClient();
    public string Test1 { get; set; }
    public int Test2 { get; set; }
    public DateTime Test3 { get; set; }
    public bool Test4 { get; set; }
    public CancellationToken Test5 { get; set; }
    public CancellationTokenSource Test6 { get; set; }
    public Memory<string> Test7 { get; set; }
    public string Test8 { get; set; }
    public Stack<int> Test9 { get; set; }
    public double Test10 { get; set; }
}

public class TestCoreModel {
    public string test22;
    public WebClient webclient;
    public string Test1 { get; set; }
    public int Test2 { get; set; }
    public DateTime Test3 { get; set; }
    public bool Test4 { get; set; }
    public CancellationToken Test5 { get; set; }
    public CancellationTokenSource Test6 { get; set; }
    public Memory<string> Test7 { get; set; }
    public string Test8 { get; set; }
    public Stack<int> Test9 { get; set; }
    public double Test10 { get; set; }
}


