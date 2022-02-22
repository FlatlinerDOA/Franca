using System;

namespace Franca;

public sealed class ParseException : Exception
{
    public ParseException(string message, int line, int column) : base(message)
    {
        this.Data["line"] = line;
        this.Data["column"] = column;
    }

    public int Line => this.Data["line"] is int l ? l : -1;

    public int Column => this.Data["column"] is int l ? l : -1;
}