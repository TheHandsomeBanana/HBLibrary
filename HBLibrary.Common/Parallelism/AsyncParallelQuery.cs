using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common.Parallelism;
public class AsyncParallelQuery<T> {
    private readonly IEnumerable<T> source;
    private readonly int degreeOfParallelism;

    public AsyncParallelQuery(IEnumerable<T> source, int degreeOfParallelism) {
        this.source = source;
        this.degreeOfParallelism = degreeOfParallelism;
    }

    public Task ForEachAsync(Func<T, Task> func) {
        return AsyncParallel.ForEachAsync(source, func, degreeOfParallelism);
    }
}
