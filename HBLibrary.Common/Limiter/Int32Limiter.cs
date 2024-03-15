using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common.Limiter;
public static class Int32Limiter {
    public static void LimitToRangeRef(this ref int value, int min, int max) {
        if (value < min) { value = min; }
        if (value > max) { value = max; }
    }

    public static int LimitToRange(this int value, int min, int max) {
        if (value < min) { return min; }
        if (value > max) { return max; }
        return value;
    }
}
