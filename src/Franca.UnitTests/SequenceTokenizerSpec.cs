using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Franca.UnitTests;

[TestClass]
public sealed class SequenceTokenizerSpec
{
    public static ITokenizer DigitThenComma = CharTokenizer.Digit + new CharTokenizer(',');

    [TestMethod]
    public void DigitThenCommaMatches() => Assert.AreEqual(2, DigitThenComma.Parse("1,").Length);

    [TestMethod]
    public void TwoDigitsThenCommaDoesNotMatch() => Assert.IsFalse(DigitThenComma.Parse("12,").IsSuccess);
}