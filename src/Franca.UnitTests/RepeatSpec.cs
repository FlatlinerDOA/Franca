using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Franca.UnitTests
{
    [TestClass]
    public class RepeatSpec
    {
        public static readonly ITokenizer Digits = new RepeatTokenizer(CharTokenizer.Digit);

        public static readonly IParser<int> DigitsToInteger = from digit in CharTokenizer.Digit.Many()
                                                              select int.Parse(digit.Span);

        [TestMethod]
        public void RepeatMatchesSingle() => Assert.AreEqual(1, Digits.Parse("1").Length);

        [TestMethod]
        public void RepeatMatchesTwo() => Assert.AreEqual(2, Digits.Parse("12").Length);

        [TestMethod]
        public void RepeatMatchesThree() => Assert.AreEqual(3, Digits.Parse("123").Length);

        [TestMethod]
        public void RepeatMatchesThreeCorrectChars() => Assert.AreEqual("123", new string(Digits.Parse("123").Span));

        [TestMethod]
        public void DigitsToIntegerMapsCorrectly() => Assert.AreEqual(123, DigitsToInteger.Parse("123"));

    }
}
