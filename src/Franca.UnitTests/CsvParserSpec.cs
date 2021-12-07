using Franca.Parsers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Franca.UnitTests
{
    [TestClass]
    public sealed class CsvParserSpec
    {
        [TestMethod]
        public void SimpleParseOfTwoCells() => Assert.That.SequenceEquals(new[] { "ABC", "XYZ" }, CsvParser.RowParser.SelectMany("ABC,XYZ").First());

        [TestMethod]
        public void SimpleParseOfTwoCellsTwoRows() => Assert.That.SequenceEquals(new[] { "ABC", "XYZ" }, CsvParser.RowParser.SelectMany("ABC,XYZ\nTUB,QWE").First());

        [TestMethod]
        public void SimpleParseOfTwoCellsTwoRowsSecondCell() => Assert.That.SequenceEquals(new[] { "TUB", "QWE" }, CsvParser.RowParser.SelectMany("ABC,XYZ\nTUB,QWE").Skip(1).First());

        [TestMethod]
        public void ParseWithHeaders() => Assert.That.SequenceEquals(new[] { "XYZ", "QWE" }, CsvParser.CsvWithHeaders("ABC,XYZ\nTUB,QWE".AsMemory()).First().Values);

        [TestMethod]
        public void CanParseInfiniteCsv()
        {
            // var stream = new MemoryStream(100000);
            // System.Buffers.BuffersExtensions.CopyTo
        }
    }
}
