using System;
using System.Collections.Generic;
using System.Linq;

namespace Franca
{
	public static class ParseExtensions
	{
		public static readonly IParser<string> Text = new ReturnParser<string>(token => new[] { new string(token.Span) });

		/*public static IParser<TResult> Select<T, TResult>(this ParserResult<T> result, Func<T, TResult> selector)
		{
			var e = result.Results.GetEnumerator();
			while (e.MoveNext())
			{
				yield return selector(e.Current);
			}
		}*/

		public static T Select<T>(this Token token, Selector<T> selector)
		{
			return selector(token).FirstOrDefault();
		}

		public static IEnumerable<TResult> SelectMany<T, TResult>(this IEnumerable<ITokenizer> source, Token span, Selector<TResult> selector)
		{
			var result = new SequenceParser<TResult>(source, selector).Parse(span);
			if (result.IsSuccess)
			{
				return result.Value;
			}

			return Enumerable.Empty<TResult>();
		}

		////public static IEnumerable<T> SelectMany<T>(this ITokenizer tokenizer, Token token, Selector<T> selector)
		////{
		////	if (token.IsEmpty)
		////	{
		////		return new T[0];
		////	}

		////	return selector(token);
		////}

		public static RepeatTokenizer Many(this ITokenizer input)
		{
			return new RepeatTokenizer(input);
		}

	}
}
