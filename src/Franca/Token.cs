using System;

namespace Franca
{
    public ref struct Token
	{
		private int length;

		private Token(ReadOnlySpan<char> baseSpan, int length)
		{
			this.Base = baseSpan;
			this.length = length;
		}

		public ReadOnlySpan<char> Base;

		public int Length => this.length == -1 ? 0 : this.length;

		////public bool IsEmpty => this.length < 1;

		public bool IsSuccess => this.length != -1;

		public ReadOnlySpan<char> Remaining => this.Base.Slice(this.Length);

		public ReadOnlySpan<char> Span => this.Length > 0 ? 
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
