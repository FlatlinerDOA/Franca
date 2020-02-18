using System;
using System.Collections.Generic;
using System.Text;

namespace Franca
{
    /// <summary>
    /// Skips some or all of an input tokenizers result.
    /// Always returns an empty result regardless of success or failure.
    /// </summary>
    public sealed class SkipTokenizer : ITokenizer
    {
        private readonly ITokenizer source;

        private readonly int skip;

        public SkipTokenizer(ITokenizer source, int skip = -1)
        {
            this.source = source;
            this.skip = skip;
        }

        public Token Parse(ReadOnlySpan<char> span)
        {
            var result = source.Parse(span);
            if (result.IsSuccess)
            {
                return Token.Success(
                    skip == -1 ? 
                        result.Remaining : 
                        result.Base.Slice(skip), 0);
            }

            return Token.Fail(span);
        }

        public static SequenceTokenizer operator +(SkipTokenizer left, ITokenizer right)
        {
            return new SequenceTokenizer(new[] { left, right });
        }

        public static ChoiceTokenizer operator |(SkipTokenizer left, ITokenizer right)
        {
            return new ChoiceTokenizer(new[] { left, right });
        }
    }
}
