using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Franca.Parsers
{
    public class CsvParser
    {
        public static readonly CharTokenizer Comma = new CharTokenizer(',');

        public static readonly CharTokenizer Tab = new CharTokenizer('\t');

        public static readonly CharTokenizer DoubleQuote = new CharTokenizer('"');

        public static readonly CharTokenizer SingleQuote = new CharTokenizer('\'');

        public static readonly CharTokenizer Symbols = CharTokenizer.Any('|', '(', ')', '*', '&');

        public static readonly ChoiceTokenizer CommonCellCharacters = CharTokenizer.LetterOrDigit | CharTokenizer.Whitespace | Symbols;

        public static readonly SequenceTokenizer DoubleQuotedCell = DoubleQuote.Skip() +
                                                            (CommonCellCharacters | Comma | Tab | SingleQuote).Many() +
                                                            DoubleQuote.Skip();

        public static readonly SequenceTokenizer SingleQuotedCell = SingleQuote.Skip() +
                                                             (CommonCellCharacters | Comma | Tab | DoubleQuote).Many() +
                                                             SingleQuote.Skip();

        public static readonly CharTokenizer RowDelimiter = CharTokenizer.LineFeed;
        public static readonly ChoiceTokenizer Cell = (DoubleQuotedCell |
                                                       SingleQuotedCell |
                                                       CommonCellCharacters.Many());

        public static readonly IParser<string> CellParser = from cell in Cell
                                                            from _ in (Comma | RowDelimiter.Until()).Skip()
                                                            select cell.Text();

        ////public static readonly IParser<string> RowParser = from row in new RepeatParser<string>(CellParser, cell => new string(cell.Span))
        ////                                                   from delim in RowDelimiter.Skip()
        ////                                                   select line;

        public IEnumerable<IReadOnlyDictionary<string, string>> CsvWithHeaders(ReadOnlySpan<char> rows)
        {
            yield break;
        }
    }
}
