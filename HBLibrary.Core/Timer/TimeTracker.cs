using System.Diagnostics;

namespace HBLibrary.Core.Timer;
public static class TimeTracker {
    public static TimeTrackerResult<TResult> ExecuteTimed<TResult>(Func<TResult> func) {
        if (func is null) {
            throw new ArgumentNullException(nameof(func));
        }

        Stopwatch sw = Stopwatch.StartNew();
        TResult result = func();
        sw.Stop();

        return new TimeTrackerResult<TResult>(result, sw.Elapsed);
    }

    public static TimeTrackerResult<TResult> ExecuteTimed<T1, TResult>(Func<T1, TResult> func, T1 arg1) {
        if (func is null) {
            throw new ArgumentNullException(nameof(func));
        }

        Stopwatch sw = Stopwatch.StartNew();
        TResult result = func(arg1);
        sw.Stop();

        return new TimeTrackerResult<TResult>(result, sw.Elapsed);
    }

    public static TimeTrackerResult<TResult> ExecuteTimed<T1, T2, TResult>(Func<T1, T2, TResult> func, T1 arg1, T2 arg2) {
        if (func is null) {
            throw new ArgumentNullException(nameof(func));
        }

        Stopwatch sw = Stopwatch.StartNew();
        TResult result = func(arg1, arg2);
        sw.Stop();

        return new TimeTrackerResult<TResult>(result, sw.Elapsed);
    }

    public static TimeTrackerResult<TResult> ExecuteTimed<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> func, T1 arg1, T2 arg2, T3 arg3) {
        if (func is null) {
            throw new ArgumentNullException(nameof(func));
        }

        Stopwatch sw = Stopwatch.StartNew();
        TResult result = func(arg1, arg2, arg3);
        sw.Stop();

        return new TimeTrackerResult<TResult>(result, sw.Elapsed);
    }

    public static TimeTrackerResult<TResult> ExecuteTimed<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> func, T1 arg1, T2 arg2, T3 arg3, T4 arg4) {
        if (func is null) {
            throw new ArgumentNullException(nameof(func));
        }

        Stopwatch sw = Stopwatch.StartNew();
        TResult result = func(arg1, arg2, arg3, arg4);
        sw.Stop();

        return new TimeTrackerResult<TResult>(result, sw.Elapsed);
    }

    public static TimeTrackerResult<TResult> ExecuteTimed<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, TResult> func, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) {
        if (func is null) {
            throw new ArgumentNullException(nameof(func));
        }

        Stopwatch sw = Stopwatch.StartNew();
        TResult result = func(arg1, arg2, arg3, arg4, arg5);
        sw.Stop();

        return new TimeTrackerResult<TResult>(result, sw.Elapsed);
    }

    public static TimeTrackerResult<TResult> ExecuteTimed<T1, T2, T3, T4, T5, T6, TResult>(Func<T1, T2, T3, T4, T5, T6, TResult> func, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) {
        if (func is null) {
            throw new ArgumentNullException(nameof(func));
        }

        Stopwatch sw = Stopwatch.StartNew();
        TResult result = func(arg1, arg2, arg3, arg4, arg5, arg6);
        sw.Stop();

        return new TimeTrackerResult<TResult>(result, sw.Elapsed);
    }

    public static TimeTrackerResult<TResult> ExecuteTimed<T1, T2, T3, T4, T5, T6, T7, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, TResult> func, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) {
        if (func is null) {
            throw new ArgumentNullException(nameof(func));
        }

        Stopwatch sw = Stopwatch.StartNew();
        TResult result = func(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        sw.Stop();

        return new TimeTrackerResult<TResult>(result, sw.Elapsed);
    }

    public static TimeTrackerResult<TResult> ExecuteTimed<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> func, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8) {
        if (func is null) {
            throw new ArgumentNullException(nameof(func));
        }

        Stopwatch sw = Stopwatch.StartNew();
        TResult result = func(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        sw.Stop();

        return new TimeTrackerResult<TResult>(result, sw.Elapsed);
    }

    public static TimeTrackerResult ExecuteTimed(Action action) {
        if (action is null) {
            throw new ArgumentNullException(nameof(action));
        }

        Stopwatch sw = Stopwatch.StartNew();
        action();
        sw.Stop();

        return new TimeTrackerResult(sw.Elapsed);
    }

    public static TimeTrackerResult ExecuteTimed<T1>(Action<T1> action, T1 arg1) {
        if (action == null) {
            throw new ArgumentNullException(nameof(action));
        }

        Stopwatch sw = Stopwatch.StartNew();
        action(arg1);
        sw.Stop();

        return new TimeTrackerResult(sw.Elapsed);
    }

    public static TimeTrackerResult ExecuteTimed<T1, T2>(Action<T1, T2> action, T1 arg1, T2 arg2) {
        if (action == null) {
            throw new ArgumentNullException(nameof(action));
        }

        Stopwatch sw = Stopwatch.StartNew();
        action(arg1, arg2);
        sw.Stop();

        return new TimeTrackerResult(sw.Elapsed);
    }

    public static TimeTrackerResult ExecuteTimed<T1, T2, T3>(Action<T1, T2, T3> action, T1 arg1, T2 arg2, T3 arg3) {
        if (action == null) {
            throw new ArgumentNullException(nameof(action));
        }

        Stopwatch sw = Stopwatch.StartNew();
        action(arg1, arg2, arg3);
        sw.Stop();

        return new TimeTrackerResult(sw.Elapsed);
    }
    public static TimeTrackerResult ExecuteTimed<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4) {
        if (action == null) {
            throw new ArgumentNullException(nameof(action));
        }

        Stopwatch sw = Stopwatch.StartNew();
        action(arg1, arg2, arg3, arg4);
        sw.Stop();

        return new TimeTrackerResult(sw.Elapsed);
    }

    public static TimeTrackerResult ExecuteTimed<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) {
        if (action == null) {
            throw new ArgumentNullException(nameof(action));
        }

        Stopwatch sw = Stopwatch.StartNew();
        action(arg1, arg2, arg3, arg4, arg5);
        sw.Stop();

        return new TimeTrackerResult(sw.Elapsed);
    }

    public static TimeTrackerResult ExecuteTimed<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) {
        if (action == null) {
            throw new ArgumentNullException(nameof(action));
        }

        Stopwatch sw = Stopwatch.StartNew();
        action(arg1, arg2, arg3, arg4, arg5, arg6);
        sw.Stop();

        return new TimeTrackerResult(sw.Elapsed);
    }

    public static TimeTrackerResult ExecuteTimed<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) {
        if (action == null) {
            throw new ArgumentNullException(nameof(action));
        }

        Stopwatch sw = Stopwatch.StartNew();
        action(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        sw.Stop();

        return new TimeTrackerResult(sw.Elapsed);
    }

    public static TimeTrackerResult ExecuteTimed<T1, T2, T3, T4, T5, T6, T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8) {
        if (action == null) {
            throw new ArgumentNullException(nameof(action));
        }

        Stopwatch sw = Stopwatch.StartNew();
        action(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        sw.Stop();

        return new TimeTrackerResult(sw.Elapsed);
    }
}
