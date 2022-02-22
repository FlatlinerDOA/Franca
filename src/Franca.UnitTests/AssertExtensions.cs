using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Franca.UnitTests;

public static class AssertExtensions
{
    public static void SequenceEquals<T>(this Assert assert, IEnumerable<T> expected, IEnumerable<T> actual)
    {
        Assert.IsTrue(expected.SequenceEqual(actual));
    }

    public static void SequenceEquals<T>(this Assert assert, IEnumerable<T> expected, IEnumerable<T> actual, string message, params object[] args)
    {
        Assert.IsTrue(expected.SequenceEqual(actual), message, args);
    }
}