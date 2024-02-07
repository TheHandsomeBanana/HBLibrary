using HBLibrary.Common.RegularExpressions;
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
        Assert.IsFalse(RegexCollection.SimplePercentageValue.Match("100").Success);
        Assert.IsFalse(RegexCollection.SimplePercentageValue.Match("101%").Success);
        Assert.IsFalse(RegexCollection.SimplePercentageValue.Match("01%").Success);
        Assert.IsFalse(RegexCollection.SimplePercentageValue.Match("abc").Success);
        Assert.IsTrue(RegexCollection.SimplePercentageValue.Match("100%").Success);
        Assert.IsTrue(RegexCollection.SimplePercentageValue.Match("1%").Success);
        Assert.IsTrue(RegexCollection.SimplePercentageValue.Match("95%").Success);
    }
}
