#if UNIT_TEST
#pragma warning disable 1591
// ReSharper disable InconsistentNaming

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NUnit.Framework;

namespace Common.Extensions
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    internal sealed class EnumerableExtensionsTests
    {
        [Test]
        public void EnumerableExtensions_Replace_Positive()
        {
            var collection = Enumerable.Range(0, 16).ToList();
            Assert.DoesNotThrow(() => collection = collection.Replace(v => v % 5 == 0, v => v - 2).ToList());
            Assert.AreEqual(new List<int> { -2, 1, 2, 3, 4, 3, 6, 7, 8, 9, 8, 11, 12, 13, 14, 13 }, collection);

            var collection2 = Enumerable.Range(0, 7).ToList();
            Assert.DoesNotThrow(() => collection2 = collection2.Replace((v, i) => i % 2 == 0, (v, i) => v + i).ToList());
            Assert.AreEqual(new List<int> { 0, 1, 4, 3, 8, 5, 12 }, collection2);

            var collection3 = new List<int>();
            Assert.DoesNotThrow(() => collection3 = collection3.Replace((v, i) => i % 2 == 0, (v, i) => v + i).ToList());
            Assert.AreEqual(new List<int>(), collection3);
        }

        [Test]
        public void EnumerableExtensions_Replace_Negative()
        {
            List<string> collection = null;
            Assert.Throws<ArgumentNullException>(() => collection = collection.Replace(v => v == "cat", v => v + "show").ToList());
        }
    }
}

// ReSharper restore InconsistentNaming
#pragma warning restore 1591
#endif