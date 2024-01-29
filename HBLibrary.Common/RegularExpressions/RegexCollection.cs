using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HBLibrary.Common.RegularExpressions;
public static partial class RegexCollection {
    public readonly static Regex SimplePercentageValue = CreateSimplePercentageRegex();

    private static Regex CreateSimplePercentageRegex() {
#if NET7_0_OR_GREATER
        return SimplePercentageRegex();
#elif NETFRAMEWORK
        return new Regex("^(100|[0-9]|[1-9][0-9])%$");
#endif
    }

#if NET7_0_OR_GREATER
    [GeneratedRegex("^(100|[0-9]|[1-9][0-9])%$")]
    private static partial Regex SimplePercentageRegex();
#endif
}
