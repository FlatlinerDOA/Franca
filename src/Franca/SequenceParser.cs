using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;

namespace Franca;

////public delegate IEnumerable<T> SingleSelector<out T>(Token token);
/*
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

	public void Parse(ReadOnlySpan<char> source, ReadOnlySpanAction<char, IReadOnlyList<T>> observer)
	{
		var accumulated = new List<T>();
		foreach (var tokenizer in this.Inputs)
		{
			var result = tokenizer.Parse(source);
			if (result.IsSuccess)
			{
				accumulated.Add(this.selector(result.Span));
				source = result.Remaining;
			}
			else
			{
				if (this.ignoreFailure)
				{
					break;
				}
				else
				{
					return;
				}
			}
		}

		observer(source, accumulated);
	}

	public override string ToString() => string.Join(" + ", this.Inputs.Select(i => i.ToString()));

	public static SequenceParser<T> operator +(SequenceParser<T> left, SequenceParser<T> right)
	{
		return new SequenceParser<T>(left.Inputs.Concat(right.Inputs), left.selector);
	}

	public static SequenceParser<T> operator +(SequenceParser<T> left, ITokenizer right)
	{
		return new SequenceParser<T>(left.Inputs.Append(right), left.selector);
	}
}*/