using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;

namespace Franca;

/// <summary>
/// Repeatedly calls a tokenizer and accumulates the results, finally outputing the accumulation upon failure to parse
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed class RepeatParser<T> : IParser<IReadOnlyList<T>>
{
	private readonly int count;

	public RepeatParser(ITokenizer input, Selector<T> selector, int count = int.MaxValue)
	{
		this.Input = new SelectParser<T>(input, selector);
		this.count = count;
	}

	public RepeatParser(IParser<T> input, int count = int.MaxValue)
	{
		this.Input = input;
		this.count = count;
	}

	public IParser<T> Input { get; }

	public Token Parse(ReadOnlySpan<char> source, ParserObserver<T> observer)
	{
		int counter = 0;
		var result = this.Input.Parse(
			source,
			(s, t) =>
			{
				counter++;
				observer(s, t);
				return s;
			});
		while (result.IsSuccess)
		{
			if (counter >= this.count)
			{
				break;
			}

			source = result.Remaining;
			result = this.Input.Parse(
				source,
				(s, t) =>
				{
					counter++;
					observer(s, t);
					return s;
				});
		}

		if (!result.IsSuccess)
		{
			return Token.Fail(source);
		}

		return result;
	}

	public override string ToString() => "{ " + this.Input.ToString() + " }";
}