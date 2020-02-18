using Franca.Parsers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Franca.UnitTests
{
    [TestClass]
    public sealed class CsvParserSpec
    {
        [TestMethod]
        public void SimpleParseOfTwoCells() => Assert.AreEqual("ABC", CsvParser.CellParser.Parse("ABC,XYZ"));
    }
}
