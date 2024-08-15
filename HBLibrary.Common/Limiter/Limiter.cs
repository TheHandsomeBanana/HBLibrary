using System.Globalization;

namespace HBLibrary.Common.Limiter;
public static class Limiter {
    public static T LimitToRange<T>(this T value, T min, T max) where T : notnull, IComparable<T> {
        if (min.CompareTo(max) > 0) {
            var minString = Convert.ToString(min, CultureInfo.InvariantCulture);
            var maxString = Convert.ToString(max, CultureInfo.InvariantCulture);
            throw new ArgumentOutOfRangeException(nameof(min), $"The argument {nameof(min)} ({minString}) must not be greater than the argument {nameof(max)} ({maxString}).");
        }

        return value.CompareTo(min) < 0 ? min : value.CompareTo(max) > 0 ? max : value;
    }

    public static void LimitToRangeRef<T>(this ref T value, T min, T max) where T : struct, IComparable<T> {
        if (min.CompareTo(max) > 0) {
            var minString = Convert.ToString(min, CultureInfo.InvariantCulture);
            var maxString = Convert.ToString(max, CultureInfo.InvariantCulture);
            throw new ArgumentOutOfRangeException(nameof(min), $"The argument {nameof(min)} ({minString}) must not be greater than the argument {nameof(max)} ({maxString}).");
        }

        value = value.CompareTo(min) < 0 ? min : value.CompareTo(max) > 0 ? max : value;
    }


    public static async Task ForEachAsync<T>(IEnumerable<T> source, Func<T, Task> func, int parallelLimit = 0) {
        if (parallelLimit == 0)
            parallelLimit = Environment.ProcessorCount; // Default to number of processors

        SemaphoreSlim throttler = new SemaphoreSlim(parallelLimit);

        IEnumerable<Task> tasks = source.Select(async item => {
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
}
