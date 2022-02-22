using System;

namespace Franca;

public sealed class EndTokenizer : ITokenizer
{
    public static EndTokenizer EOF = new EndTokenizer();

    public Token Parse(ReadOnlySpan<char> span)
    {
        return span.IsEmpty ? Token.Success(span) : Token.Fail(span);
    }

    public override string ToString() => "<EOF>";
}