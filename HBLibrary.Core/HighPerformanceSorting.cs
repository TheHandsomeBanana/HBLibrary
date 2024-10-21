namespace HBLibrary.Core;
public static class HighPerformanceSorting {
    /// <summary>
    /// Monkey
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    public static void BogoSort<T>(this IList<T> list) where T : IComparable<T> {
        while (!list.IsSorted())
            list.BogoShuffle();
    }

    public static void BogoShuffle<T>(this IList<T> list) {
        for (int i = list.Count; i > 1; i--) {
            int k = new Random().Next(i + 1);
            (list[i], list[k]) = (list[k], list[i]);
        }
    }

    /// <summary>
    /// Two monkeys are better than one
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static void BogoBogoSort<T>(this IList<T> list) where T : IComparable<T> {
        BogoBogoSortInternal(list, 1);
    }

    private static void BogoBogoSortInternal<T>(IList<T> list, int size) where T : IComparable<T> {
        if (size > list.Count)
            return;

        bool isSorted = false;
        while (!isSorted) {
            list.BogoBogoShuffle(size);
            if (size == 1)
                isSorted = true;
            else {
                BogoBogoSortInternal(list, size - 1);
                isSorted = list.IsSorted();
            }
        }

        BogoBogoSortInternal(list, size + 1);
    }

    public static void BogoBogoShuffle<T>(this IList<T> list, int count) {
        for (int i = 0; i < count; i++) {
            int swapIndex = new Random().Next(i, count);
            (list[swapIndex], list[i]) = (list[i], list[swapIndex]);
        }
    }

    /// <summary>
    /// Multiply and surrender
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    public static void SlowSort<T>(IList<T> list) where T : IComparable<T> {
        SlowSortInternal(list, 0, list.Count);
    }

    private static void SlowSortInternal<T>(IList<T> list, int start, int end) where T : IComparable<T> {
        if (start >= end)
            return;

        int mid = (start + end) / 2;

        SlowSortInternal(list, start, mid);
        SlowSortInternal(list, mid + 1, end);

        if (list[end].CompareTo(list[mid]) < 0)
            (list[mid], list[end]) = (list[end], list[mid]);

        SlowSortInternal(list, start, end - 1);
    }

    /// <summary>
    /// Multithreading is the solution to everything
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    public static void SleepSort<T>(this IList<T> list) where T : IConvertible {
        T[] temp = new T[list.Count];
        list.CopyTo(temp, 0);
        list.Clear();
        object locker = new object();

        List<Task> tasks = [];
        foreach (T item in temp) {
            Task task = Task.Run(async () => {
                double delay = Convert.ToDouble(item);
                await Task.Delay((int)delay * 10);

                lock (locker)
                    list.Add(item);
            });
        }
    }

    /// <summary>
    /// Sheer determination
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    public static void StupidSort<T>(this IList<T> list) where T : IComparable<T> {
        int i = 0;
        int n = list.Count;
        while (i < n) {
            if (i == 0 || list[i - 1].CompareTo(list[i]) <= 0)
                i++;
            else {
                T temp = list[i];
                list[i] = list[i - 1];
                list[i - 1] = temp;
                i--;
            }
        }
    }

    public static void IntelligentDesignSort<T>(this IList<T> list) {
        Console.WriteLine("List already sorted.");
    }

    public static bool IsSorted<T>(this IList<T> list) where T : IComparable<T> {
        for (int i = 1; i < list.Count; i++) {
            if (list[i - 1].CompareTo(list[i]) > 0)
                return false;
        }

        return true;
    }
}
