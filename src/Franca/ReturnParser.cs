using System;
using System.Collections.Generic;
using System.Linq;

namespace Franca
{
	public delegate T Selector<out T>(Token token);

	/// <summary>
	/// Reutrns the first result from parser
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public sealed class ReturnParser<T> : IParser<T>
	{
		private readonly ITokenizer input;
		private Selector<T> selector;

		public ReturnParser(ITokenizer input, Selector<T> selector)
		{
			this.input = input;
			this.selector = selector;
		}

		public IEnumerable<T> Parse(Token token)
		{
			var result = input.Parse(token.Span);
			if (result.IsSuccess)
			{
				return new[] { this.selector(result) };
			}

			return Array.Empty<T>();
		}

		public override string ToString() => "Return<"+ typeof(T).Name + ">";
	}
}
