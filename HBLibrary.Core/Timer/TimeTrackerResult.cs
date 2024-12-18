﻿namespace HBLibrary.Core.Timer;
public readonly ref struct TimeTrackerResult<T> {
    public T Result { get; }
    public TimeSpan ElapsedTime { get; }

    public TimeTrackerResult(T result, TimeSpan elapsedTime) {
        Result = result;
        ElapsedTime = elapsedTime;
    }
}

public readonly ref struct TimeTrackerResult {
    public TimeSpan ElapsedTime { get; }

    public TimeTrackerResult(TimeSpan elapsedTime) {
        ElapsedTime = elapsedTime;
    }
}
