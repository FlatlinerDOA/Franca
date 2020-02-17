using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Franca.UnitTests
{
    [TestClass]
    public class RepeatSpec
    {
        public static readonly RepeatTokenizer Digits = new RepeatTokenizer(CharTokenizer.Digit);

        [TestMethod]
        public void RepeatMatchesSingle() => Assert.AreEqual(1, Digits.Parse("1").Length);

        [TestMethod]
        public void RepeatMatchesTwo() => Assert.AreEqual(2, Digits.Parse("12").Length);

        [TestMethod]
        public void RepeatMatchesThree() => Assert.AreEqual(3, Digits.Parse("123").Length);

        [TestMethod]
        public void RepeatMatchesThreeCorrectChars() => Assert.AreEqual("123", new string(Digits.Parse("123").Span));
    }
}
