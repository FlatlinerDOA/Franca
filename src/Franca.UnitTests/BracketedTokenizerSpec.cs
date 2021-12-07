using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Franca.UnitTests
{
    [TestClass]
    public class BracketedTokenizerSpec
    {
        public static BracketedParser<string> AngleBrackets = new BracketedParser<string>((CharTokenizer)'<', CharTokenizer.LetterOrDigit, (CharTokenizer)'>', chars => new string(chars));

		[TestMethod]
		public void SingleBracketShouldNotMatch() => Assert.IsFalse(AngleBrackets.Parse("<A", (_, __) => { }).IsSuccess);

		[TestMethod]
		public void BracketedCharShouldMatch() => Assert.IsTrue(AngleBrackets.Parse("<A>", (_, __) => { }).IsSuccess);

		[TestMethod]
		public void BracketedCharShouldNotIncludeBrackets() => AngleBrackets.Parse(
            "<A>",
            (a, result) => Assert.AreEqual("A", result));
    }
}
