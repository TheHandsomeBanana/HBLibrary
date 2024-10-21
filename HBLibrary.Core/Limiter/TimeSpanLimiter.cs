namespace HBLibrary.Core.Limiter;
public static class TimeSpanLimiter {
    public static void LimitToRangeRef(this ref TimeSpan value, TimeSpan min, TimeSpan max) {
        if (min > max)
            throw new ArgumentException($"Min {min} is greater than {max}");

        if (value < min) { value = min; }
        if (value > max) { value = max; }
    }

    public static TimeSpan LimitToRange(this TimeSpan value, TimeSpan min, TimeSpan max) {
        if (min > max)
            throw new ArgumentException($"Min {min} is greater than {max}");

        if (value < min) { return min; }
        if (value > max) { return max; }

        return value;
    }
}
