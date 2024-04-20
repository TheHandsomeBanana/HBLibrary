using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using HBLibrary.Code.Analysis.Utilities;
using HBLibrary.Common.Parallelism;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using System.Collections.Concurrent;
using System.Threading;

namespace HBLibrary.Code.Analysis.Tests.Performance;


//| Method                               | Mean       | Error    | StdDev   | StdErr  | Min        | Median     | Max        | Allocated |
//|------------------------------------- |-----------:|---------:|---------:|--------:|-----------:|-----------:|-----------:|----------:|
//| LoadSemanticModelCustomParallelAsync |   293.2 us |  5.78 us |  6.19 us | 1.46 us |   282.7 us |   292.8 us |   306.3 us | 417.94 KB |
//| LoadSemanticModel                    |   207.6 us |  3.13 us |  2.93 us | 0.76 us |   203.4 us |   208.4 us |   214.3 us | 187.88 KB |
//| LoadSemanticModelAsyncNew            |   216.4 us |  1.07 us |  0.83 us | 0.24 us |   215.6 us |   216.3 us |   218.6 us | 187.88 KB |
//| LoadSemanticModelParallel            |   307.7 us |  6.15 us | 10.93 us | 1.73 us |   282.0 us |   307.0 us |   328.2 us | 349.82 KB |
//| LoadSemanticModelParallelAsync       | 2,577.8 us | 22.84 us | 21.37 us | 5.52 us | 2,549.9 us | 2,568.4 us | 2,614.9 us | 524.14 KB |
//| LoadSemanticModelAsParallel          |   264.3 us |  3.79 us |  3.55 us | 0.92 us |   258.7 us |   264.9 us |   270.8 us | 363.85 KB |
//| LoadSemanticModelAsyncOld            |   311.2 us |  3.49 us |  3.27 us | 0.84 us |   307.3 us |   309.9 us |   318.9 us | 308.36 KB |
//| LoadSemanticModelAsyncSemaphore      |   378.5 us |  5.51 us |  5.15 us | 1.33 us |   372.8 us |   377.3 us |   388.5 us | 261.03 KB |






[MemoryDiagnoser]
public class PerformanceTests {
    private MSBuildWorkspace workspace;
    private List<Document> documents;
    public static void Main(string[] args) {
        BenchmarkRunner.Run<PerformanceTests>(new PerformanceTestConfig());
    }

    [GlobalSetup]
    public async Task GlobalSetup() {
        workspace = MSBuildWorkspaceUtility.OpenWorkspace();
        Solution solution = await workspace.OpenSolutionAsync(@"P:\01_THIRD_PARTY_Projects\vs-editor-api\VSEditorCore.sln");
        documents = solution.Projects.SelectMany(e => e.Documents).ToList();
    }

    [GlobalCleanup]
    public void GlobalCleanup() {
        workspace?.Dispose();
    }

    [Benchmark]
    public async Task LoadSemanticModelCustomParallelAsync() {
        ConcurrentDictionary<string, SemanticModel> modelCollection = [];

        await documents.AsAsyncParallel().ForEachAsync(async e => {
            if (e.FilePath == null || !e.SupportsSemanticModel || modelCollection.ContainsKey(e.FilePath))
                return;

            SemanticModel loadedModel = (await e.GetSemanticModelAsync())!;

            modelCollection.TryAdd(e.FilePath, loadedModel);
        });
    }

    [Benchmark]
    public void LoadSemanticModel() {
        Dictionary<string, SemanticModel> modelCollection = [];

        foreach(var document in documents) {
            if (document.FilePath == null || !document.SupportsSemanticModel || modelCollection.ContainsKey(document.FilePath))
                continue;

            SemanticModel loadedModel = document.GetSemanticModelAsync().GetAwaiter().GetResult()!;
            modelCollection.Add(document.FilePath, loadedModel);
        }
    }

    [Benchmark]
    public async Task LoadSemanticModelAsyncNew() {
        Dictionary<string, SemanticModel> modelCollection = [];

        foreach (var document in documents) {
            if (document.FilePath == null || !document.SupportsSemanticModel || modelCollection.ContainsKey(document.FilePath))
                continue;

            SemanticModel loadedModel = (await document.GetSemanticModelAsync())!;
            modelCollection.Add(document.FilePath, loadedModel);
        }
    }

    [Benchmark]
    public void LoadSemanticModelParallel() {
        ConcurrentDictionary<string, SemanticModel> modelCollection = [];

        Parallel.ForEach(documents, e => {
            if (e.FilePath == null || !e.SupportsSemanticModel || modelCollection.ContainsKey(e.FilePath))
                return;

            SemanticModel loadedModel = e.GetSemanticModelAsync().GetAwaiter().GetResult()!;
            modelCollection.TryAdd(e.FilePath, loadedModel);
        });
    }

    [Benchmark]
    public async Task LoadSemanticModelParallelAsync() {
        ConcurrentDictionary<string, SemanticModel> modelCollection = [];

        await Parallel.ForEachAsync(documents, async (e, t) => {
            if (e.FilePath == null || !e.SupportsSemanticModel || modelCollection.ContainsKey(e.FilePath))
                return;

            SemanticModel loadedModel = (await e.GetSemanticModelAsync(t))!;
            modelCollection.TryAdd(e.FilePath, loadedModel);
        });
    }

    [Benchmark]
    public void LoadSemanticModelAsParallel() {
        ConcurrentDictionary<string, SemanticModel> modelCollection = [];

        documents.AsParallel().ForAll(e => {
            if (e.FilePath == null || !e.SupportsSemanticModel || modelCollection.ContainsKey(e.FilePath))
                return;

            SemanticModel loadedModel = e.GetSemanticModelAsync().GetAwaiter().GetResult()!;
            modelCollection.TryAdd(e.FilePath, loadedModel);
        });
    }

    [Benchmark]
    public async Task LoadSemanticModelAsyncOld() {
        Dictionary<string, Task<SemanticModel>> taskMapping = new Dictionary<string, Task<SemanticModel>>();
        foreach (Document document in documents) {
            if (document.FilePath == null || !document.SupportsSemanticModel || taskMapping.ContainsKey(document.FilePath))
                continue;

            taskMapping.Add(document.FilePath, document.GetSemanticModelAsync());
        }

        SemanticModel[] semanticModels = await Task.WhenAll(taskMapping.Values);

        Dictionary<string, Task<SemanticModel>>.Enumerator enumerator = taskMapping.GetEnumerator();
        int counter = 0;

        Dictionary<string, SemanticModel> modelCollection = [];

        while (enumerator.MoveNext()) {
            if (enumerator.Current.Key == null)
                continue;

            modelCollection.Add(enumerator.Current.Key, semanticModels[counter]);
            counter++;
        }

        enumerator.Dispose();
    }

    [Benchmark]
    public async Task LoadSemanticModelAsyncSemaphore() {
        ConcurrentDictionary<string, SemanticModel> modelCollection = [];

        static async Task ForEachAsync<T>(IEnumerable<T> source, Func<T, Task> func, int degreeOfParallelism = 0) {
            if (degreeOfParallelism == 0)
                degreeOfParallelism = Environment.ProcessorCount; // Default to number of processors

            var throttler = new SemaphoreSlim(degreeOfParallelism);

            var tasks = source.Select(async item =>
            {
                await throttler.WaitAsync();
                try {
                    await func(item);
                }
                finally {
                    throttler.Release();
                }
            });

            await Task.WhenAll(tasks);
        }

        await ForEachAsync(documents, async e => {
            if (e.FilePath == null || !e.SupportsSemanticModel || modelCollection.ContainsKey(e.FilePath))
                return;

            SemanticModel loadedModel = (await e.GetSemanticModelAsync())!;
            modelCollection.TryAdd(e.FilePath, loadedModel);

        });
    }
}

public class PerformanceTestConfig : ManualConfig {
    public PerformanceTestConfig() {
        AddLogger(ConsoleLogger.Default); // Ensures console output
        AddColumnProvider(DefaultColumnProviders.Instance); // Adds default columns
        AddColumn(StatisticColumn.AllStatistics);
        AddDiagnoser(MemoryDiagnoser.Default); // Adds memory diagnoser
    }
}