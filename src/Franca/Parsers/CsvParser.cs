using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Threading;
using System.Linq;
using System.Text;

namespace Franca.Parsers
{
    public sealed class CsvParser
    {
        public static readonly CharTokenizer Comma = ',';

        public static readonly CharTokenizer Tab = '\t';

        public static readonly CharTokenizer DoubleQuote = '"';

        public static readonly CharTokenizer SingleQuote = '\'';

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

        public static readonly RepeatParser<string> RowParser = CellParser.DelimitedBy(RowDelimiter, cell => new string(cell));
        
        public static IEnumerable<IReadOnlyDictionary<string, string>> CsvWithHeaders(ReadOnlyMemory<char> rows)
        {
            IReadOnlyList<string> headers = Array.Empty<string>();
            foreach (var row in RowParser.SelectMany(rows.Span))
            {
                var dictionary = new Dictionary<string, string>(headers.Count);
                for (int col = 0; col < Math.Min(headers.Count, row.Count); col++)
                {
                    dictionary[headers[col]] = row[col];
                }

                yield return dictionary;
            }
        }

        public static async IAsyncEnumerable<IReadOnlyDictionary<string, string>> CsvWithHeadersAsync(PipeReader reader, CancellationToken cancellationToken)
        {
            try
            {
                while (true)
                {
                    var result = await reader.ReadAsync(cancellationToken);
                    var buffer = result.Buffer;

                    try
                    {
                        // Process all messages from the buffer, modifying the input buffer on each
                        // iteration.
                        IReadOnlyDictionary<string, string> d = null;
                        while (RowParser.TryParseBuffer(Encoding.UTF8.GetChars(buffer), r => {
                            d = r;
                        }))
                        {
                            yield return d;
                        }

                        // There's no more data to be processed.
                        if (result.IsCompleted)
                        {
                            if (buffer.Length > 0)
                            {
                                // The message is incomplete and there's no more data to process.
                                throw new Exception($"Incomplete CSV at offset {buffer.Length}");
                            }
                            
                            break;
                        }
                    }
                    finally
                    {
                        // Since all messages in the buffer are being processed, you can use the
                        // remaining buffer's Start and End position to determine consumed and examined.
                        reader.AdvanceTo(buffer.Start, buffer.End);
                    }
                }
            }
            finally
            {
                await reader.CompleteAsync();
	        }
        }

        public static IEnumerable<IReadOnlyList<string>> CsvNoHeaders(ReadOnlySpan<char> rows)
        {
            return RowParser.SelectMany(rows);
        }
    }
}
