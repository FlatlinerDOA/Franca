using System;
using System.Collections.Generic;
using System.Linq;

namespace Franca
{
	////public delegate IEnumerable<T> SingleSelector<out T>(Token token);

	public sealed class SequenceParser<T> : IParser<IReadOnlyList<T>>
	{
		private readonly Selector<T> selector;

		private bool ignoreFailure;

		public SequenceParser(IEnumerable<ITokenizer> inputs, Selector<T> selector)
		{
			this.Inputs = inputs;
			this.selector = selector;
		}

		public SequenceParser(IEnumerable<ITokenizer> inputs, Selector<T> selector, bool ignoreFailure)
		{
			this.Inputs = inputs;
			this.selector = selector;
			this.ignoreFailure = ignoreFailure;
		}

		public IEnumerable<ITokenizer> Inputs { get; }

		public Result<IReadOnlyList<T>> Parse(Token token)
		{
			var ambiguous = new List<IParser<T>>();
			var accumulated = new List<T>();
			var remainderSpan = token.Span;
			foreach (var tokenizer in this.Inputs)
			{
				var result = tokenizer.Parse(remainderSpan);
				if (result.IsEmpty)
				{
					if (this.ignoreFailure)
					{
						break;
					}
					else
					{
						return Result<IReadOnlyList<T>>.Fail;
					}
				}
				else
				{
					accumulated.Add(this.selector(result));
					remainderSpan = result.Remaining;
				}
			}

			return Result<IReadOnlyList<T>>.Success(accumulated);
		}

		public override string ToString() => string.Join(" ", this.Inputs.Select(i => i.ToString()));

		public static SequenceParser<T> operator +(SequenceParser<T> left, SequenceParser<T> right)
		{
			return new SequenceParser<T>(left.Inputs.Concat(right.Inputs), left.selector);
		}

		public static SequenceParser<T> operator +(SequenceParser<T> left, ITokenizer right)
		{
			return new SequenceParser<T>(left.Inputs.Append(right), left.selector);
		}
	}
}
