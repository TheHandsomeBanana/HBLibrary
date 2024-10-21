namespace HBLibrary.Core.Limiter;
public static class Int32Limiter {
    public static void LimitToRangeRef(this ref int value, int min, int max) {
        if (min > max)
            throw new ArgumentException($"Min {min} is greater than {max}");

        if (value < min) { value = min; }
        if (value > max) { value = max; }
    }

    public static int LimitToRange(this int value, int min, int max) {
        if (min > max)
            throw new ArgumentException($"Min {min} is greater than {max}");

        if (value < min) { return min; }
        if (value > max) { return max; }
        return value;
    }
}
