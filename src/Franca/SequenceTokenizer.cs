using System;
using System.Collections.Generic;
using System.Linq;

namespace Franca;

public sealed class SequenceTokenizer : ITokenizer
{
	private bool ignoreFailure;

	public SequenceTokenizer(IEnumerable<ITokenizer> inputs)
	{
		this.Inputs = inputs;
	}

	public SequenceTokenizer(IEnumerable<ITokenizer> inputs, bool ignoreFailure)
	{
		this.Inputs = inputs;
		this.ignoreFailure = ignoreFailure;
	}

	public IEnumerable<ITokenizer> Inputs { get; }

	public Token Parse(ReadOnlySpan<char> span)
	{
		////var ambiguous = new List<IParser<T>>();
		var accumulated = Token.Fail(span);
		var remainderSpan = span;
		foreach (var parser in this.Inputs)
		{
			var result = parser.Parse(remainderSpan);
			if (result.IsSuccess)
			{
				accumulated = accumulated + result;
				remainderSpan = result.Remaining;
			}
			else
			{
				if (this.ignoreFailure)
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

	public override string ToString() => "( " + string.Join(" + ", this.Inputs.Select(i => i.ToString())) + " )";

	public static ChoiceTokenizer operator |(SequenceTokenizer left, ITokenizer right)
	{
		return new ChoiceTokenizer(new[] { left, right });
	}

	public static SequenceTokenizer operator +(SequenceTokenizer left, SequenceTokenizer right)
	{
		return new SequenceTokenizer(left.Inputs.Concat(right.Inputs));
	}

	public static SequenceTokenizer operator +(SequenceTokenizer left, ITokenizer right)
	{
		return new SequenceTokenizer(left.Inputs.Append(right));
	}
}