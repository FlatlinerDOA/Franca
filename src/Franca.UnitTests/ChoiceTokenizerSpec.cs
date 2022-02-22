using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Franca.UnitTests;

[TestClass]
public sealed class ChoiceTokenizerSpec
{
    public static ITokenizer DigitOrComma = CharTokenizer.Digit | new CharTokenizer(',');

    [TestMethod]
    public void DigitOrCommaMatchesDigit() => Assert.AreEqual("1", new string(DigitOrComma.Parse("1,").Span));

    [TestMethod]
    public void DigitOrCommaMatchesComma() => Assert.AreEqual(",", new string(DigitOrComma.Parse(",1").Span));

    [TestMethod]
    public void DigitOrCommaDoesNotMatchLetter() => Assert.AreEqual(string.Empty, new string(DigitOrComma.Parse("A,1").Span));

    [TestMethod]
    public void LetterDoesNotMatch() => Assert.IsFalse(DigitOrComma.Parse("A").IsSuccess);
}