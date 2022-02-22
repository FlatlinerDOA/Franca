using System;
using System.Buffers;
using System.Collections.Generic;

namespace Franca;

/// <summary>
/// Can be used to encapsulate something that has an optional start token and/or an end token.
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed class BracketedParser<T> : IParser<T>
{
    public ITokenizer Open { get; }

    public IParser<T> Content { get; }
        
    public ITokenizer Close { get; }

    public BracketedParser(ITokenizer open, ITokenizer content, ITokenizer close, Selector<T> selector)
    {
        this.Open = open;
        this.Content = new SelectParser<T>(content, selector);
        this.Close = close;
    }

    public BracketedParser(ITokenizer content, ITokenizer close, Selector<T> selector)
    {
        this.Content = new SelectParser<T>(content, selector);
        this.Close = close;
    }

    public BracketedParser(ITokenizer open, IParser<T> content, ITokenizer close)
    {
        this.Open = open;
        this.Content = content;
        this.Close = close;
    }

    public BracketedParser(IParser<T> content, ITokenizer close)
    {
        this.Content = content;
        this.Close = close;
    }
/*
    public BracketedEnumerator Parse(ref ReadOnlySpan<char> source)
    {
        return new BracketedEnumerator(ref source, this.Open, this.Content, this.Close);
    }

    public readonly ref struct BracketedEntry
    {
        private readonly ReadOnlySpan<char> content;
        private readonly T value;

        public BracketedEntry(ReadOnlySpan<char> content, T value)
        {
            this.content = content;
            this.value = value;
            this.Error = null;
        }
        
        public BracketedEntry(ReadOnlySpan<char> source, int start, int end, string errorMessage)
        {
            this.content = source;
            this.value = default;
            var line = 0;
            var column = 0;
            for (var offset = source.IndexOf('\n'); offset != -1 && offset < start; offset = source.Slice(offset).IndexOf('\n'))
            {
                line++;
                column = offset - start;
            }
            
            this.Error = new ParseException(errorMessage, line, column);
        }

        public T Value
        {
            get => this.Error != null ? throw this.Error : value;
            init => this.value = value;
        }

        public ParseException Error { get; init; }
    }
    
    public readonly ref struct BracketedEnumerator
    {
        private readonly ReadOnlySpan<char> baseSpan;
        private readonly ITokenizer open;
        private readonly IParser<T> content;
        private readonly ITokenizer close;

        public BracketedEnumerator(
            ref ReadOnlySpan<char> baseSpan,
            ITokenizer open,
            IParser<T> content,
            ITokenizer close)
        {
            this.baseSpan = baseSpan;
            this.open = open;
            this.content = content;
            this.close = close;
            this.Current = default;
        }
        
        public BracketedEntry Current { get; init; }

        // Needed to be compatible with the foreach operator
        public BracketedEnumerator GetEnumerator() => this;
       
        public bool MoveNext()
        {
            var source = this.baseSpan;
            if (this.open != null) 
            {   
                var open = this.open.Parse(source);
                if (open.IsSuccess)
                {
                    source = open.Remaining;
                } 
                else 
                { 
                    return new BracketedEntry(this.baseSpan,  "Missing " + this.open.ToString());
                }
            }

            T result = default;
            var contentToken = this.Content.Parse(source, (i, r) =>
            {
                result = r;
            });

            if (!contentToken.IsSuccess)
            {
                return TokenSegment.Fail(source);
            }

            if (this.Close != null)
            {
                var close = this.Close.Parse(contentToken.Remaining);
                if (!close.IsSuccess)
                {
                    return TokenSegment.Fail(source);
                }
            }

            observer(contentToken.Span, result);
            return TokenSegment<T>.Success(contentToken.Remaining, contentToken.Remaining.Length);
        }
    }
    */
    public Token Parse(ReadOnlySpan<char> source, ParserObserver<T> observer)
    {
        if (this.Open != null) 
        {   
            var open = this.Open.Parse(source);
            if (open.IsSuccess)
            {
                source = open.Remaining;
            } 
            else 
            { 
                return observer(Token.Fail(source), default);
            }
        }

        T result = default;
        var remaining = this.Content.Parse(source, ResultObserver);

        if (!remaining.IsSuccess)
        {
            return observer(Token.Fail(source), default);
        }

        if (this.Close != null)
        {
            var close = this.Close.Parse(remaining.Span);
            if (!close.IsSuccess)
            {
                return observer(Token.Fail(source), default);
            }
        }

        return observer(remaining, result);
        Token ResultObserver(Token token, T arg)
        {
            result = arg;
            return Token.Success(token.Remaining);
        }
    }

    public override string ToString() =>
        string.Join(' ', this.Open?.ToString(), this.Content.ToString(), this.Close?.ToString());
}