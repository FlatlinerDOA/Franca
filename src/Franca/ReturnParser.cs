using System;
using System.Collections.Generic;

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

		public Result<T> Parse(Token token)
		{
			var result = input.Parse(token.Span);
			if (!result.IsEmpty)
			{
				return Result<T>.Success(this.selector(result));
			}

			return Result<T>.Fail;
		}

		public override string ToString() => "Return<"+ typeof(T).Name + ">";
	}
}
