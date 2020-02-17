using System;

namespace Franca
{
    public ref struct Token
	{
		private Token(ReadOnlySpan<char> baseSpan, int length)
		{
			this.Base = baseSpan;
			this.Length = length;
		}

		public ReadOnlySpan<char> Base;

		public int Length;

		public bool IsEmpty => this.Length == 0;

		public ReadOnlySpan<char> Remaining => this.Base.Slice(this.Length);

		public ReadOnlySpan<char> Span => this.Base.Slice(0, this.Length);

		public static Token Success(ReadOnlySpan<char> span, int length)
		{
			return new Token(span, length);
		}

		public static Token Fail(ReadOnlySpan<char> span)
		{
			return new Token(span, 0);
		}

		public static Token operator +(Token left, Token right) 
		{
			return new Token(left.Base, left.Length + right.Length);
		}
	}
}
