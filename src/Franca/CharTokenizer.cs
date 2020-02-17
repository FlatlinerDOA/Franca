using System;
using System.Collections.Generic;

namespace Franca
{
	public sealed class CharTokenizer : ITokenizer
	{
		private Func<char, bool> match;

		public CharTokenizer(char match)
		{
			this.match = c => c == match;
			this.Name = "'" + match + "'";
		}

		public CharTokenizer(Func<char, bool> match, string name)
		{
			this.match = match;
			this.Name = "<" + name + ">";
		}

		public static CharTokenizer Digit = new CharTokenizer(char.IsDigit, "Digit");

		public static CharTokenizer LetterOrDigit = new CharTokenizer(char.IsLetterOrDigit, "LetterOrDigit");

		public static CharTokenizer Whitespace = new CharTokenizer(char.IsWhiteSpace, "Whitespace");

		public static CharTokenizer LowSurrogate = new CharTokenizer(char.IsLowSurrogate, "LowSurrogate");

		public string Name { get; }

		public Token Parse(ReadOnlySpan<char> span)
		{
			if (!span.IsEmpty && this.match(span[0]))
			{
				return Token.Success(span, 1);
			}

			return Token.Fail(span);
		}

		public static implicit operator CharTokenizer(char character)
		{
			return new CharTokenizer(character);
		}

		public static ChoiceTokenizer operator |(CharTokenizer left, ITokenizer right)
		{
			return new ChoiceTokenizer(new[] { left, right });
		}

		public static SequenceTokenizer operator +(CharTokenizer left, ITokenizer right)
		{
			return new SequenceTokenizer(new[] { left, right });
		}

		public override string ToString() => this.Name;
	}
}
