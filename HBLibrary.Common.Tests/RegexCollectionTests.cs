﻿using HBLibrary.Common.RegularExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common.Tests;
[TestClass]
public class RegexCollectionTests {
    [TestMethod]
    public void Regex_SimplePercentage() {
        Assert.IsFalse(RegexCollection.SimplePercentageRegex.Match("100").Success);
        Assert.IsFalse(RegexCollection.SimplePercentageRegex.Match("101%").Success);
        Assert.IsFalse(RegexCollection.SimplePercentageRegex.Match("01%").Success);
        Assert.IsFalse(RegexCollection.SimplePercentageRegex.Match("abc").Success);
        Assert.IsTrue(RegexCollection.SimplePercentageRegex.Match("100%").Success);
        Assert.IsTrue(RegexCollection.SimplePercentageRegex.Match("1%").Success);
        Assert.IsTrue(RegexCollection.SimplePercentageRegex.Match("95%").Success);
    }
}
