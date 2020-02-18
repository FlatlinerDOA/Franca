using System;
using System.Collections.Generic;
using System.Text;

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

		public static CharTokenizer Any(params char[] anyChar)
		{
			var sb = new StringBuilder();
			foreach (var m in anyChar)
			{
				if (sb.Length != 0)
				{
					sb.Append(", ");
				}

				sb.Append('\'').Append(m).Append('\'');
			}

			return new CharTokenizer(c => Array.IndexOf(anyChar, c) != -1, sb.ToString());
		}

		public static readonly CharTokenizer Digit = new CharTokenizer(char.IsDigit, "Digit");

		public static readonly CharTokenizer LetterOrDigit = new CharTokenizer(char.IsLetterOrDigit, "LetterOrDigit");

		public static readonly CharTokenizer Whitespace = new CharTokenizer(char.IsWhiteSpace, "Whitespace");

		public static readonly CharTokenizer LowSurrogate = new CharTokenizer(char.IsLowSurrogate, "LowSurrogate");

		public static readonly CharTokenizer LineFeed = CharTokenizer.Any('\n', '\r');

		public string Name { get; }

		public Token Parse(ReadOnlySpan<char> span)
		{
			if (!span.IsEmpty && this.match(span[0]))
			{
				return Token.Success(span, 1);
			}

			return Token.Fail(span);
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
