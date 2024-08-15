using System.Collections.Concurrent;

namespace HBLibrary.Common.Parallelism;
public class AsyncParallelQuery<T> {
    private readonly IEnumerable<T> source;
    private readonly int degreeOfParallelism;

    public AsyncParallelQuery(IEnumerable<T> source, int degreeOfParallelism = 0) {
        if (degreeOfParallelism < 0)
            throw new ArgumentOutOfRangeException(nameof(degreeOfParallelism));

        if (degreeOfParallelism == 0)
            degreeOfParallelism = Environment.ProcessorCount;

        this.source = source;
        this.degreeOfParallelism = degreeOfParallelism;
    }

    public Task ForEachAsync(Func<T, Task> func) {
        async Task AwaitPartition(IEnumerator<T> partition) {
            using (partition) {
                while (partition.MoveNext())
                    await func(partition.Current);
            }
        }

        return Task.WhenAll(Partitioner.Create(source)
            .GetPartitions(degreeOfParallelism)
            .AsParallel()
            .Select(AwaitPartition));
    }

    public async Task<T?> FirstOrDefaultAsync(Func<T, Task<bool>> predicate) {
        foreach (T item in source) {
            if (await predicate(item))
                return item;
        }

        return default;
    }

    public async Task<ConcurrentBag<T>> WhereAsync(Func<T, Task<bool>> predicate) {
        ConcurrentBag<T> bag = [];
        await ForEachAsync(async item => {
            if (await predicate(item))
                bag.Add(item);
        });

        return bag;
    }

    public async Task<ConcurrentBag<TResult>> SelectAsync<TResult>(Func<T, Task<TResult>> selector) {
        ConcurrentBag<TResult> bag = [];
        await ForEachAsync(async item => {
            bag.Add(await selector(item));
        });

        return bag;
    }

    public async Task<int> CountAsync(Func<T, Task<bool>> predicate) {
        int count = 0;
        await ForEachAsync(async item => {
            if (await predicate(item))
                Interlocked.Increment(ref count);
        });
        return count;
    }

}
