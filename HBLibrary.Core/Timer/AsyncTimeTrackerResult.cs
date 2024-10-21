namespace HBLibrary.Core.Timer;
public readonly struct AsyncTimeTrackerResult<T> {
    public T Result { get; }
    public TimeSpan ElapsedTime { get; }

    public AsyncTimeTrackerResult(T result, TimeSpan elapsedTime) {
        Result = result;
        ElapsedTime = elapsedTime;
    }
}
