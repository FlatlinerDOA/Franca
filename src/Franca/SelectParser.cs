using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;

namespace Franca
{
	public delegate T Selector<out T>(ReadOnlySpan<char> token);

	/// <summary>
	/// Reutrns the first result from parser
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public sealed class SelectParser<T> : IParser<T>
	{
		private readonly ITokenizer input;
		private Selector<T> selector;

		public SelectParser(ITokenizer input, Selector<T> selector)
		{
			this.input = input;
			this.selector = selector;
		}

		public Token Parse(ReadOnlySpan<char> sequence, ReadOnlySpanAction<char, T> observer)
		{
			var result = input.Parse(sequence);
			if (result.IsSuccess)
			{
				observer(result.Span, this.selector(result.Span));
				sequence = result.Remaining;
			} 
			else
			{
				return Token.Fail(sequence);
			}

			return Token.Success(result.Remaining);
		}

		public override string ToString() => "<"+ typeof(T).Name + "> = " + this.input.ToString();
	}
}
