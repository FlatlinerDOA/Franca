using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Franca.UnitTests;

[TestClass]
public class CharTokenizerSpec
{
	[TestMethod]
	public void SingleDigitChar() => Assert.AreEqual(CharTokenizer.Digit.Parse("1").Length, 1);

	[TestMethod]
	public void SingleDigitGetsChar() => Assert.AreEqual('1', CharTokenizer.Digit.Parse("1").Span[0]);

	[TestMethod]
	public void MultiDigitChar() => Assert.AreEqual(1, CharTokenizer.Digit.Parse("12").Length);

	[TestMethod]
	public void AnyCharMatchesSecond() => Assert.AreEqual(1, CharTokenizer.Any('A', 'B').Parse("B").Length);

	[TestMethod]
	public void EverythingElse() 
	{
		// Observable Action<Action<T>> Subscribe
		// Observer Action<Token> OnNextOrCompleted

		////var x = new CharParser<char>("ABC".AsMemory(), () => new[] { 'Y' });
		////var y = x.Parse("ABC".AsSpan());
		//var csv = CharTokenizer.LetterOrDigit.Many() + new CharTokenizer(',');


		//CharTokenizer.Digit.Parse("1").IsSuccess.Dump();

		//var or = CharTokenizer.Digit | CharTokenizer.Whitespace;
		//or.Parse("1").IsSuccess.Dump();
		//or.Parse(" ").IsSuccess.Dump();

		//var digits = CharTokenizer.Digit.Many().Text();
		//digits.Parse("1234").Span.Dump();
		//var equation = "1+2*4";

		//var choice = new CharTokenizer('(') + (CharTokenizer.Digit | CharTokenizer.Whitespace).Many() + new CharTokenizer(')');

		//choice.Parse("(1234)").Span.Dump("Bracketed");

		//var twoDigits = CharTokenizer.Digit + CharTokenizer.Digit;

		//twoDigits.Parse("12").Span.Dump();
	}
}