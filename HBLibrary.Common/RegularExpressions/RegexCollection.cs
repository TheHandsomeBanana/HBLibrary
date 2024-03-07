using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HBLibrary.Common.RegularExpressions;
public static partial class RegexCollection {
    public readonly static Regex SimplePercentageValue =
#if NET7_0_OR_GREATER
        SimplePercentageRegex();
#elif NETFRAMEWORK
        new Regex("^(100|[0-9]|[1-9][0-9])%?$");
#endif

    public readonly static Regex SimplePercentagePValue =
#if NET7_0_OR_GREATER
        SimplePercentagePRegex();
#elif NETFRAMEWORK
        new Regex("^(100|[0-9]|[1-9][0-9])p$");
#endif


#if NET7_0_OR_GREATER
    [GeneratedRegex("^(100|[0-9]|[1-9][0-9])%$")]
    private static partial Regex SimplePercentageRegex();
    [GeneratedRegex("^(100|[0-9]|[1-9][0-9])p$")]
    private static partial Regex SimplePercentagePRegex();
#endif
}
