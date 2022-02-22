using System;
using System.Collections.Generic;
using System.Linq;

namespace Franca;

//   public sealed class Choice<T> : IParser<T>
//{
//	public Choice(IEnumerable<IParser<T>> choices)
//	{
//		this.Choices = choices;
//	}

//	public IEnumerable<IParser<T>> Choices { get; }

//	public Token SelectMany(ReadOnlySpan<char> span)
//	{
//		var ambiguous = new List<ITokenizer>();
//		foreach (var parser in this.Choices)
//		{
//			var result = parser.SelectMany(span);
//			if (result.IsSuccess)
//			{
//				return result;
//			}
//		}

//		return Token.Fail(span);
//	}

//	public override string ToString() => string.Join(" | ", this.Choices.Select(i => i.ToString()));

//	public static Choice<T> operator |(Choice<T> left, IParser<T> right)
//	{
//		return new Choice<T>(left.Choices.Append(right));
//	}

//	public static Choice<T> operator |(IParser<T> left, Choice<T> right)
//	{
//		return new Choice<T>(new[] { left }.Concat(right.Choices));
//	}
//}