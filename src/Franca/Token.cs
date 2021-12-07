using System;

namespace Franca
{
	/// <summary>
	/// A Token is merely a pointer to a span of remaining characters from an input, and a consuming length.
	/// </summary>
    public readonly ref struct Token
	{
    	/// <summary>
		/// The length of characters consumed by this token. -1 denotes a failed parse.
		/// </summary>
		private readonly int length;

		private readonly ReadOnlySpan<char> baseSpan;

		private Token(in ReadOnlySpan<char> baseSpan, int length)
		{
			this.baseSpan = baseSpan;
			this.length = length;
		}

		public readonly ReadOnlySpan<char> Base => this.baseSpan;

		public readonly int Length => this.length == -1 ? 0 : this.length;

		public readonly bool IsSuccess => this.length != -1;

		public readonly ReadOnlySpan<char> Remaining => this.baseSpan.Slice(this.Length);

		public readonly ReadOnlySpan<char> Span => this.Length > 0 ? 
			this.Base.Slice(0, this.Length) :
			ReadOnlySpan<char>.Empty;

		public static Token Success(ReadOnlySpan<char> baseSpan, int length)
		{
			return new Token(baseSpan, length);
		}

		/// <summary>
		/// Returns a successful token with the remaining content.
		/// </summary>
		/// <param name="remaining">The source span</param>
		/// <returns>A success token for the source space</returns>
		public static Token Success(ReadOnlySpan<char> remaining)
		{
			return new Token(remaining, remaining.Length);
		}

		public static Token Fail(ReadOnlySpan<char> baseSpan)
		{
			return new Token(baseSpan, -1);
		}

		public static Token operator +(Token left, Token right) 
		{
			return new Token(left.Base, left.Length + right.Length);
		}
	}
}
