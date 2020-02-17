using System;
using System.Collections.Generic;
using System.Linq;

namespace Franca
{
	public static class ParseExtensions
	{
		/*public static IParser<TResult> Select<T, TResult>(this ParserResult<T> result, Func<T, TResult> selector)
		{
			var e = result.Results.GetEnumerator();
			while (e.MoveNext())
			{
				yield return selector(e.Current);
			}
		}*/

		public static T Parse<T>(this IParser<T> parser, ReadOnlySpan<char> span)
		{
			var result = parser.Parse(Token.Success(span, span.Length));
			if (!result.IsSuccess)
			{
				throw new Exception("We couldn't parse, sorry... we'll give better errors in a future version.");
			}

			return result.Value;
		}

		public static IParser<string> Text(this ITokenizer source)
		{
			return new ReturnParser<string>(
				source, 
				token => new string(token.Span));
		}

		public static IParser<T> Select<T>(this ITokenizer source, Selector<T> selector)
		{
			return new ReturnParser<T>(source, selector);
		}

		public static T Select<T>(this Token token, Selector<T> selector)
		{
			return selector(token);
		}

		//public static IParser<TResult> SelectMany<TResult>(this ITokenizer source, IParser<TResult> parser, Selector<TResult> selector)
		//{
		//	return new SequenceParser<TResult>(parser, selector);
		//		new ReturnParser<TResult>(source, selector);
		//}

		public static ITokenizer SelectMany<TResult>(this ITokenizer source, Func<ITokenizer, ITokenizer> selector)
		{ 		
			return new SequenceTokenizer(new[] { source, selector(source) });
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
			return new RepeatTokenizer(input, 0);
		}

		public static RepeatTokenizer AtLeastOnce(this ITokenizer input)
		{
			return new RepeatTokenizer(input);
		}
	}
}
