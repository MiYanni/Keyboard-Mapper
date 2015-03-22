#if UNIT_TEST
#pragma warning disable 1591
// ReSharper disable InconsistentNaming

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Common.Extensions
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    internal sealed class NumericExtensionsTests
    {
        [Test]
        public void NumericExtensions_GetDigitCount_Positive()
        {
            Assert.DoesNotThrow(() => Assert.AreEqual(3, 312L.GetDigitCount()));
            Assert.DoesNotThrow(() => Assert.AreEqual(1, 0L.GetDigitCount()));
            Assert.DoesNotThrow(() => Assert.AreEqual(1, 1L.GetDigitCount()));
            Assert.DoesNotThrow(() => Assert.AreEqual(12, 769305703284L.GetDigitCount()));
        }

        [Test]
        public void NumericExtensions_GetDigits_Positive()
        {
            Assert.DoesNotThrow(() => Assert.AreEqual(new List<byte> { 3, 1, 2 }, 312L.GetDigits().ToList()));
            Assert.DoesNotThrow(() => Assert.AreEqual(new List<byte> { 0 }, 0L.GetDigits().ToList()));
            Assert.DoesNotThrow(() => Assert.AreEqual(new List<byte> { 1 }, 1L.GetDigits().ToList()));
            Assert.DoesNotThrow(() => Assert.AreEqual(new List<byte> { 7, 6, 9, 3, 0, 5, 7, 0, 3, 2, 8, 4 }, 769305703284L.GetDigits().ToList()));
        }

        [Test]
        public void NumericExtensions_IsDivisibleBy_Positive()
        {
            Assert.DoesNotThrow(() => Assert.AreEqual(true, 312.IsDivisibleBy(2)));
            Assert.DoesNotThrow(() => Assert.AreEqual(true, 0.IsDivisibleBy(2)));
            Assert.DoesNotThrow(() => Assert.AreEqual(true, 1.IsDivisibleBy(1)));
            Assert.DoesNotThrow(() => Assert.AreEqual(true, 769305704.IsDivisibleBy(4)));
            Assert.DoesNotThrow(() => Assert.AreEqual(true, 1.IsDivisibleBy(-1)));
            Assert.DoesNotThrow(() => Assert.AreEqual(true, (-44).IsDivisibleBy(-4)));

            Assert.DoesNotThrow(() => Assert.AreEqual(false, 13.IsDivisibleBy(2)));
            Assert.DoesNotThrow(() => Assert.AreEqual(false, 4.IsDivisibleBy(769305704)));
        }

        [Test]
        public void NumericExtensions_IsDivisibleBy_Negative()
        {
            Assert.DoesNotThrow(() => Assert.AreEqual(false, 1.IsDivisibleBy(0)));
            Assert.DoesNotThrow(() => Assert.AreEqual(false, 0.IsDivisibleBy(0)));
            Assert.DoesNotThrow(() => Assert.AreEqual(false, (-1).IsDivisibleBy(0)));
        }
    }
}

// ReSharper restore InconsistentNaming
#pragma warning restore 1591
#endif