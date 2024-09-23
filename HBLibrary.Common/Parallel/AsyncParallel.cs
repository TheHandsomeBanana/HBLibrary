namespace HBLibrary.Common.Parallel;
public static class AsyncParallel {
    // example usage: await ParallelForEachAsync(source, async func => list.Add(await func())
    public static Task ForEachAsync<T>(IEnumerable<T> source, Func<T, Task> func, int degreeOfParallelism = 0) {
        return source.AsAsyncParallel(degreeOfParallelism).ForEachAsync(func);
    }

    public static AsyncParallelQuery<T> AsAsyncParallel<T>(this IEnumerable<T> source, int degreeOfParallelism = 0) {
        return new AsyncParallelQuery<T>(source, degreeOfParallelism);
    }
}
