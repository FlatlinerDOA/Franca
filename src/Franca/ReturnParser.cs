using System;
using System.Collections.Generic;

namespace Franca
{
	public delegate IEnumerable<T> Selector<T>(Token token);

	/// <summary>
	/// Reutrns the first result from parser
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public sealed class ReturnParser<T> : IParser<T>
	{
		private Selector<T> selector;

		public ReturnParser(Selector<T> selector)
		{
			this.selector = selector;
		}

		public Result<T> Parse(Token token)
		{
			foreach (var output in this.selector(token))
			{
				return Result<T>.Success(output);
			}

			return Result<T>.Fail;
		}

		public override string ToString() => "Return<"+ typeof(T).Name + ">";
	}
}
