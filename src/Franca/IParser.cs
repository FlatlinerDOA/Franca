using System;
using System.Collections.Generic;
using System.Buffers;

namespace Franca
{
	/// <summary>
	/// Equivalent to an IObservable{<typeparamref name="T"/>} parsing from a span of characters.
	/// </summary>
	/// <typeparam name="T">The resulting type being projected from the span of char</typeparam>
    public interface IParser<T>
	{
		/// <summary>
		/// Parses the input sequence and outputs a token with success + remaining unprocessed portion.
		/// </summary>
		/// <param name="source">The source to parse</param>
		/// <param name="observer">
		/// The observer of the parsed result, 
		/// and the span it was based on (which allows for contextual nesting)
		/// </param>
		/// <returns>A token with a boolean indicating success or failure, and a span of the remaining unprocessed characters.</returns>
		Token Parse(ReadOnlySpan<char> source, ReadOnlySpanAction<char, T> observer);
	}
}
