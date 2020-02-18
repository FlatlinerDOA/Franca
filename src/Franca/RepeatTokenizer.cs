using System;

namespace Franca
{
    public sealed class RepeatTokenizer : ITokenizer
	{
		private int min;

		private int maxExclusive;

		public RepeatTokenizer(ITokenizer input, int min = 1, int maxExclusive = int.MaxValue)
		{
			if (min < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(min));
			}

			if (min >= maxExclusive || maxExclusive <= 1)
			{
				throw new ArgumentOutOfRangeException(nameof(maxExclusive));
			}

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
					// TODO: Fail or break? return Token.Fail(span);
					break;
				}

				var result = this.Input.Parse(remainderSpan);
				if (result.IsSuccess)
				{
					accumulated = accumulated + result;
					remainderSpan = result.Remaining;
				}
				else
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
