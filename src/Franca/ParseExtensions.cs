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
			return parser.Parse(Token.Success(span, span.Length)).Single();
		}

		public static SkipTokenizer Skip(this ITokenizer tokenizer)
		{
			return new SkipTokenizer(tokenizer);
		}

		public static SkipTokenizer Until(this ITokenizer tokenizer)
		{
			return new SkipTokenizer(tokenizer, 0);
		}

		public static ReturnParser<T> Return<T>(this ITokenizer tokenizer, T returnValue)
		{
			return new ReturnParser<T>(tokenizer, _ => returnValue);
		}

		public static ReturnParser<string> Text(this ITokenizer source)
		{
			return new ReturnParser<string>(
				source, 
				token => new string(token.Span));
		}

		public static ReturnParser<T> Select<T>(this ITokenizer source, Selector<T> selector)
		{
			return new ReturnParser<T>(source, selector);
		}


		public static RepeatTokenizer Optional(this ITokenizer source)
		{
			return new RepeatTokenizer(source, 0, 2);
		}

		public static IParser<T> SelectMany<T>(this ITokenizer source, Func<ITokenizer, ITokenizer> selector, Func<ITokenizer, IParser<T>> resultSelector)
		{
			return resultSelector(new SequenceTokenizer(new[] { source, selector(source) }));
		}

		public static IParser<T> SelectMany<T>(this ITokenizer source, Func<ITokenizer, ITokenizer> selector, Func<ITokenizer, ITokenizer, IParser<T>> resultSelector)
		{
			return resultSelector(source, selector(source));
		}


		public static SequenceTokenizer SelectMany(this ITokenizer source, Func<ITokenizer, ITokenizer> selector)
		{ 		
			return new SequenceTokenizer(new[] { source, selector(source) });
		}

		//public static IParser<T> SelectMany<T, TResult>(this IParser<T> source, Func<IParser<T>, IParser<T>> selector)
		//{
		//	return new SequenceParser<T, TResult>(new[] { source, selector(source) });
		//}

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
