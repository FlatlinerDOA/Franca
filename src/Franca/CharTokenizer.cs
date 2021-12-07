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

		public CharTokenizer(char match, string name)
		{
			this.match = c => c == match;
			this.Name = "<" + name + ">";
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

			return new CharTokenizer(c => anyChar.AsSpan().IndexOf(c) != -1, sb.ToString());
		}

		/// <summary>
		/// Single character digits 0 - 9
		/// </summary>
		public static readonly CharTokenizer Digit = new CharTokenizer(char.IsDigit, "Digit");

		/// <summary>
		/// Any single character unicode letter or digits.
		/// </summary>
		public static readonly CharTokenizer LetterOrDigit = new CharTokenizer(char.IsLetterOrDigit, "LetterOrDigit");

		/// <summary>
		/// Any whitespace including spaces, tabs, line feeds and carriage returns.
		/// </summary>
		public static readonly CharTokenizer Whitespace = new CharTokenizer(char.IsWhiteSpace, "Whitespace");

		public static readonly CharTokenizer LowSurrogate = new CharTokenizer(char.IsLowSurrogate, "LowSurrogate");

		/// <summary>
		/// Any lf or crlf
		/// </summary>
		public static readonly ChoiceTokenizer SingleLineFeed = new CharTokenizer('\n', "LF") | (new CharTokenizer('\r', "CR") + new CharTokenizer('\n', "LF"));

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

		public static implicit operator CharTokenizer(char match) => new CharTokenizer(match);

		public override string ToString() => this.Name;
	}
}
