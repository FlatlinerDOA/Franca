using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Franca.Parsers
{
    public sealed class CsvParser
    {
        public static readonly CharTokenizer Comma = new CharTokenizer(',');

        public static readonly CharTokenizer Tab = new CharTokenizer('\t');

        public static readonly CharTokenizer DoubleQuote = new CharTokenizer('"');

        public static readonly CharTokenizer SingleQuote = new CharTokenizer('\'');

        public static readonly CharTokenizer Symbols = CharTokenizer.Any('|', '(', ')', '*', '&');

        public static readonly ChoiceTokenizer CommonCellCharacters = CharTokenizer.LetterOrDigit | CharTokenizer.Whitespace | Symbols;

        public static readonly ITokenizer RowDelimiter = CharTokenizer.SingleLineFeed | EndTokenizer.EOF;

        public static readonly SequenceTokenizer DoubleQuotedCell = DoubleQuote.Skip() +
                                                                    (CommonCellCharacters | Comma | Tab | SingleQuote).Many() +
                                                                    DoubleQuote.Skip();

        public static readonly SequenceTokenizer SingleQuotedCell = SingleQuote.Skip() +
                                                                    (CommonCellCharacters | Comma | Tab | DoubleQuote).Many() +
                                                                    SingleQuote.Skip();

        public static readonly ChoiceTokenizer Cell = (DoubleQuotedCell |
                                                       SingleQuotedCell |
                                                       CommonCellCharacters.Many());

        public static readonly IParser<string> CellParser = Cell.DelimitedBy(Comma | RowDelimiter.Until(), cell => new string(cell));

        public static readonly IParser<IReadOnlyList<string>> RowParser = CellParser.DelimitedBy(RowDelimiter, cell => new string(cell));
        
        public IEnumerable<IReadOnlyDictionary<string, string>> CsvWithHeaders(ReadOnlySpan<char> rows)
        {
            yield break;
        }

        public IEnumerable<IReadOnlyList<string>> CsvNoHeaders(ReadOnlySpan<char> rows)
        {
            return RowParser.SelectMany(rows);
        }

    }
}
