using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common.Limiter;
public static class Limiter {
    public static T LimitToRange<T>(this T value, T min, T max) where T : notnull, IComparable<T>  {
        if (min.CompareTo(max) > 0) {
            var minString = Convert.ToString(min, CultureInfo.InvariantCulture);
            var maxString = Convert.ToString(max, CultureInfo.InvariantCulture);
            throw new ArgumentOutOfRangeException(nameof(min), $"The argument {nameof(min)} ({minString}) must not be greater than the argument {nameof(max)} ({maxString}).");
        }

        return value.CompareTo(min) < 0 ? min : value.CompareTo(max) > 0 ? max : value;
    }
}
