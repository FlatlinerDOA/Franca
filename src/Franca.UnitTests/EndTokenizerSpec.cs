using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Franca.UnitTests;

[TestClass]
public class EndTokenizerSpec
{
	[TestMethod]
	public void EndOfFileShouldMatchEmptyString() => Assert.IsTrue(EndTokenizer.EOF.Parse("").IsSuccess);

	[TestMethod]
	public void EndOfFileShouldNotMatchNonEmptyString() => Assert.IsFalse(EndTokenizer.EOF.Parse("1").IsSuccess);

	[TestMethod]
	public void EndOfFileLengthIsZero() => Assert.AreEqual(EndTokenizer.EOF.Parse("").Length, 0);
}