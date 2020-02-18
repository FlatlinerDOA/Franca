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

		public static Token Success(ReadOnlySpan<char> span, int length)
		{
			return new Token(span, length);
		}

		public static Token Fail(ReadOnlySpan<char> span)
		{
			return new Token(span, -1);
		}

		public static Token operator +(Token left, Token right) 
		{
			return new Token(left.Base, left.Length + right.Length);
		}
	}
}
