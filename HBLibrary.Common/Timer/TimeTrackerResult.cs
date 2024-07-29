using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common.Timer;
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
