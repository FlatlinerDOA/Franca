using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Franca
{
    /// <summary>
    /// Can be used to encapsulate something that has an optional start token and/or an end token.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class BracketedParser<T> : IParser<T>
    {
        public ITokenizer Start { get; }

        public IParser<T> Content { get; }
        
        public ITokenizer End { get; }

        public BracketedParser(ITokenizer start, ITokenizer content, ITokenizer end, Selector<T> selector)
        {
            this.Start = start;
            this.Content = new SelectParser<T>(content, selector);
            this.End = end;
        }

        public BracketedParser(ITokenizer content, ITokenizer end, Selector<T> selector)
        {
            this.Content = new SelectParser<T>(content, selector);
            this.End = end;
        }

        public BracketedParser(ITokenizer start, IParser<T> content, ITokenizer end)
        {
            this.Start = start;
            this.Content = content;
            this.End = end;
        }

        public BracketedParser(IParser<T> content, ITokenizer end)
        {
            this.Content = content;
            this.End = end;
        }

        public Token Parse(ReadOnlySpan<char> source, ReadOnlySpanAction<char, T> observer)
        {
            if (this.Start != null) 
            {   
                var open = this.Start.Parse(source);
                if (open.IsSuccess)
                {
                    source = open.Remaining;
                } 
                else 
                { 
                    return Token.Fail(source);
                }
            }

            T result = default;
            var contentToken = this.Content.Parse(source, (i, r) =>
            {
                result = r;
            });

            if (!contentToken.IsSuccess)
            {
                return Token.Fail(source);
            }

            if (this.End != null)
            {
                var close = this.End.Parse(contentToken.Remaining);
                if (!close.IsSuccess)
                {
                    return Token.Fail(source);
                }
            }

            observer(contentToken.Span, result);
            return Token.Success(contentToken.Remaining, contentToken.Remaining.Length);
        }

        public override string ToString() =>
            string.Join(' ', this.Start?.ToString(), this.Content.ToString(), this.End?.ToString());

        public bool TryParseBuffer(in ReadOnlySequence<char> buffer)
        {
            // TODO: Add an overload that is more efficient.
            return this.Parse(buffer.FirstSpan, (_, __) => { }).IsSuccess;
        }
    }
}
