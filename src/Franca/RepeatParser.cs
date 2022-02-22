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

	public Token Parse(ReadOnlySpan<char> source, ReadOnlySpanAction<char, IReadOnlyList<T>> observer)
	{
		var accumulated = this.count == int.MaxValue ? new List<T>() : new List<T>(this.count);
		var result = this.Input.Parse(
			source,
			(s, t) =>
			{
				accumulated.Add(t);
			});
		while (result.IsSuccess)
		{
			if (accumulated.Count >= this.count)
			{
				break;
			}

			source = result.Remaining;
			result = this.Input.Parse(
				source,
				(s, t) =>
				{
					accumulated.Add(t);
				});
		}

		if (!result.IsSuccess)
		{
			return Token.Fail(source);
		}

		observer(source, accumulated);
		return result;
	}

	public override string ToString() => "{ " + this.Input.ToString() + " }";

	public bool TryParseBuffer(in ReadOnlySequence<char> buffer)
	{
		return this.Parse(buffer.FirstSpan, (_, __) => { }).IsSuccess;
	}
}