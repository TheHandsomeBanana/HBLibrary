﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common.Parallelism;
public static class AsyncParallel {
    // example usage: await ParallelForEachAsync(source, async func => list.Add(await func())
    public static Task ForEachAsync<T>(IEnumerable<T> source, Func<T, Task> func, int degreeOfParallelism = 0) {
        return source.AsAsyncParallel(degreeOfParallelism).ForEachAsync(func);
    }

    public static AsyncParallelQuery<T> AsAsyncParallel<T>(this IEnumerable<T> source, int degreeOfParallelism = 0) {
        return new AsyncParallelQuery<T>(source, degreeOfParallelism);
    }
}
