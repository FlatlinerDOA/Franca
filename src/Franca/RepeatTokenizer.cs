using System;

namespace Franca
{
    public sealed class RepeatTokenizer : ITokenizer
	{
		private int min;

		private int maxExclusive;

		public RepeatTokenizer(ITokenizer input, int min = 1, int maxExclusive = int.MaxValue)
		{
			this.Input = input;
			this.min = min;
			this.maxExclusive = maxExclusive;
		}

		public ITokenizer Input { get; }

		public Token Parse(ReadOnlySpan<char> span)
		{
			var remainderSpan = span;
			var accumulated = Token.Fail(span);
			for (int i = 0; i <= this.maxExclusive; i++)
			{
				if (i == this.maxExclusive)
				{
					return Token.Fail(span);
				}

				var result = this.Input.Parse(remainderSpan);
				if (result.IsEmpty)
				{
					if (i >= this.min)
					{
						break;
					}
					else
					{
						return Token.Fail(span);
					}
				}
				else
				{
					accumulated = accumulated + result;
					remainderSpan = result.Remaining;
				}
			}

			return accumulated;
		}

		public static SequenceTokenizer operator +(RepeatTokenizer left, ITokenizer right)
		{
			return new SequenceTokenizer(new[] { left, right });
		}

		public static ChoiceTokenizer operator |(RepeatTokenizer left, ITokenizer right)
		{
			return new ChoiceTokenizer(new[] { left, right });
		}

		public override string ToString() => this.min == 0 ? "[ " + this.Input.ToString() + " ]" : "{ " + this.Input + " }";
	}
}
