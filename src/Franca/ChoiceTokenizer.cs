using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Franca;

public sealed class ChoiceTokenizer : ITokenizer
{
	public ChoiceTokenizer(IEnumerable<ITokenizer> choices)
	{
		this.Choices = choices;
	}

	public IEnumerable<ITokenizer> Choices { get; }

	public Token Parse(ReadOnlySpan<char> span)
	{
		var ambiguous = new List<ITokenizer>();
		foreach (var parser in this.Choices)
		{
			var result = parser.Parse(span);
			if (result.IsSuccess)
			{
				return result;
			}
		}

		return Token.Fail(span);
	}

	public override string ToString() => "( " + string.Join(" | ", this.Choices.Select(i => i.ToString())) + " )";

	public static ChoiceTokenizer operator |(ChoiceTokenizer left, ChoiceTokenizer right)
	{
		return new ChoiceTokenizer(left.Choices.Concat(right.Choices));
	}

	public static ChoiceTokenizer operator |(ChoiceTokenizer left, ITokenizer right)
	{
		return new ChoiceTokenizer(left.Choices.Append(right));
	}

	public static ChoiceTokenizer operator |(ITokenizer left, ChoiceTokenizer right)
	{
		return new ChoiceTokenizer(new[] { left }.Concat(right.Choices));
	}
}