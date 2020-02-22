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
    }
}
