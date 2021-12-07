# Franca
A Span&lt;T> based text parser

Inspired by Sprache with a goal for efficiency, performance and support for asynchronous parsing.

## Design Goals

* As close to zero allocations as possible.
* Ability to do stream parsing of multi-gigabyte files with via System.IO.Pipelines support.
* Improve code readability by making the grammar declarations appear as close to EBNF as possible.
* Allow for dynamic asynchronously constructed grammar trees for more advanced auto-complete suggestion support.
* Push based architecture where the parser pushes results rather than a consumer pulling them. Resembling more of an `IObservable<T>` pattern instead of `IEnumerable<T>` like Sprache.

## Example CSV Grammar

```
    CharTokenizer Comma = ',';

    CharTokenizer Tab = '\t';

    CharTokenizer DoubleQuote = '"';

    CharTokenizer SingleQuote = '\'';

    ITokenizer Symbols = CharTokenizer.Any('|', '(', ')', '*', '&');

    ITokenizer CommonCellCharacters = CharTokenizer.LetterOrDigit | CharTokenizer.Whitespace | Symbols;

    ITokenizer RowDelimiter = CharTokenizer.SingleLineFeed | EndTokenizer.EOF;

    ITokenizer DoubleQuotedCell = DoubleQuote.Skip() +
                                  (CommonCellCharacters | Comma | Tab | SingleQuote).Many() +
                                   DoubleQuote.Skip();

    ITokenizer SingleQuotedCell = SingleQuote.Skip() +
                                  (CommonCellCharacters | Comma | Tab | DoubleQuote).Many() +
                                   SingleQuote.Skip();

    ITokenizer Cell = (DoubleQuotedCell |
                       SingleQuotedCell |
                       CommonCellCharacters.Many());

    IParser<string> CellParser = Cell.DelimitedBy(Comma | RowDelimiter.Until(), cell => new string(cell));

    IParser<IReadOnlyList<string>> RowParser = CellParser.DelimitedBy(RowDelimiter, cell => new string(cell));
```


## Architecture

The parsing logic is split into two phases - tokenization and parsing.

A tokenizer is a special case of parser dedicated to character tokenization where a stream of characters is split up into constituent spans of chars. This is for efficiency so that token spans can be calculated efficiently prior to allocating objects during streaming.

A parser takes one or more tokenizers and constructs a strongly typed resulting object or value from the `ReadOnlySpan<char>` provided to it by the tokenizer.