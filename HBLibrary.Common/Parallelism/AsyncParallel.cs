using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common.Parallelism;
public static class AsyncParallel {
    // example usage: await ParallelForEachAsync(source, async func => list.Add(await func())
    public static Task ForEachAsync<T>(this IEnumerable<T> source, Func<T, Task> func, int degreeOfParallelism = -1) {
        async Task AwaitPartition(IEnumerator<T> partition) {
            using (partition) {
                while(partition.MoveNext())
                    await func(partition.Current);
            }
        }

        return Task.WhenAll(Partitioner.Create(source)
            .GetPartitions(degreeOfParallelism)
            .AsParallel()
            .Select(AwaitPartition));
    }

    public static AsyncParallelQuery<T> AsAsyncParallel<T>(this IEnumerable<T> source, int degreeOfParallelism = -1) {
        return new AsyncParallelQuery<T>(source, degreeOfParallelism);
    }
}
