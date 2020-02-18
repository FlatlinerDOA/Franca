using System;
using System.Collections.Generic;
using System.Text;

namespace Franca
{
	public sealed class RepeatParser<T> : IParser<T>
	{
		private readonly Selector<T> selector;

		public RepeatParser(ITokenizer input, Selector<T> selector, int min = 0, int maxExclusive = int.MaxValue)
		{
			this.Input = input;
			this.selector = selector;
		}

		public ITokenizer Input { get; }

		public IEnumerable<T> Parse(Token token)
		{
			var accumulated = new List<T>();

			var remainderSpan = token.Span;
			
			var resultToken = this.Input.Parse(remainderSpan);
			while (resultToken.IsSuccess)
			{
				accumulated.Add(this.selector(resultToken));
				resultToken = this.Input.Parse(resultToken.Remaining);
			}

			return accumulated;
		}

		public override string ToString() => this.Input.ToString();
	}
}
