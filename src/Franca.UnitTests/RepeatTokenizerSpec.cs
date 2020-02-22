using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Franca.UnitTests
{
    [TestClass]
    public class RepeatTokenizerSpec
    {
        public static readonly ITokenizer Digits = new RepeatTokenizer(CharTokenizer.Digit);

        public static readonly IParser<int> DigitsToInteger = from digit in CharTokenizer.Digit.Many()
                                                              select int.Parse(digit);

        public static readonly ITokenizer UpToFiveDigits = new RepeatTokenizer(CharTokenizer.Digit, 1, 5);

        public static readonly ITokenizer ExactlyThreeDigits = new RepeatTokenizer(CharTokenizer.Digit, 3, 4);

        public static readonly ITokenizer ZeroOrOne = new RepeatTokenizer(new CharTokenizer('('), 0, 2);

        [TestMethod]
        public void RepeatMatchesSingle() => Assert.AreEqual(1, Digits.Parse("1").Length);

        [TestMethod]
        public void RepeatMatchesTwo() => Assert.AreEqual(2, Digits.Parse("12").Length);

        [TestMethod]
        public void RepeatMatchesThree() => Assert.AreEqual(3, Digits.Parse("123").Length);

        [TestMethod]
        public void RepeatMatchesThreeCorrectChars() => Assert.AreEqual("123", new string(Digits.Parse("123").Span));

        [TestMethod]
        public void DigitsToIntegerMapsCorrectly() => Assert.AreEqual(123, DigitsToInteger.SelectMany("123").First());

        [TestMethod]
        public void UpToFiveDigitsMatchesFiveWithTwoRemaining() 
        {
            var actual = UpToFiveDigits.Parse("1234567");
            Assert.AreEqual("12345", new string(actual.Span));
            Assert.AreEqual("67", new string(actual.Remaining));
        }

        [TestMethod]
        public void ExactlyThreeDoesNotMatchTwo()
        {
            var actual = ExactlyThreeDigits.Parse("12");
            Assert.IsFalse(actual.IsSuccess);
            Assert.AreEqual("12", new string(actual.Remaining));
        }

        [TestMethod]
        public void ExactlyThreeMatchesThree()
        {
            var actual = ExactlyThreeDigits.Parse("123");
            Assert.IsTrue(actual.IsSuccess);
            Assert.AreEqual("123", new string(actual.Span));
        }

        [TestMethod]
        public void ZeroOrOneCanBeUsedForOptionalTokens()
        {
            var actual = ZeroOrOne.Parse("(ABC");
            Assert.IsTrue(actual.IsSuccess);
            Assert.AreEqual("(", new string(actual.Span));
            Assert.AreEqual("ABC", new string(actual.Remaining));

            var actual2 = ZeroOrOne.Parse("ABC");
            Assert.IsFalse(actual2.IsSuccess);
            Assert.AreEqual(string.Empty, new string(actual2.Span));
            Assert.AreEqual("ABC", new string(actual2.Remaining));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ConstructingRepeatWithMaxExclusiveLessThanMinMinusOneErrors() => new RepeatTokenizer(new CharTokenizer('('), 0, 1);

        [TestMethod] 
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ConstructingRepeatWithNegativeMinErrors() => new RepeatTokenizer(new CharTokenizer('('), -1, 1);
    }
}
