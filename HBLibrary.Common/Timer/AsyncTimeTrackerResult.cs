using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common.Timer;
public readonly struct AsyncTimeTrackerResult<T> {
    public T Result { get; }
    public TimeSpan ElapsedTime { get; }

    public AsyncTimeTrackerResult(T result, TimeSpan elapsedTime) {
        Result = result;
        ElapsedTime = elapsedTime;
    }
}
