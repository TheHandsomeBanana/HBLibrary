﻿using System.Text.RegularExpressions;

namespace HBLibrary.Core.RegularExpressions;
public static partial class RegexCollection {
    public readonly static Regex SimplePercentageRegex =
#if NET5_0_OR_GREATER
        GenSimplePercentageRegex();
#elif NETFRAMEWORK
        new Regex("^(100|[0-9]|[1-9][0-9])%?$");
#endif

    public readonly static Regex SimplePercentagePRegex =
#if NET5_0_OR_GREATER
        GenSimplePercentagePRegex();
#elif NETFRAMEWORK
        new Regex("^(100|[0-9]|[1-9][0-9])p$");
#endif

    public readonly static Regex CommonCLIPasswordRegex =
#if NET5_0_OR_GREATER
        GenCommonPasswordRegex();
#elif NET472_OR_GREATER
        new Regex("^[\\w!@#$%^&*()-_=+[\\]{};:'\",.<>?/|`~]{8,32}$\r\n");
#endif


#if NET5_0_OR_GREATER
    [GeneratedRegex("^(100|[0-9]|[1-9][0-9])%$")]
    private static partial Regex GenSimplePercentageRegex();
    [GeneratedRegex("^(100|[0-9]|[1-9][0-9])p$")]
    private static partial Regex GenSimplePercentagePRegex();
    [GeneratedRegex("^[\\w!@#$%^&*()-_=+[\\]{};:'\",.<>?/|`~]{8,32}$\r\n")]
    private static partial Regex GenCommonPasswordRegex();
#endif
}
