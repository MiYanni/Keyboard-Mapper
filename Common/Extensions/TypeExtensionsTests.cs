#if UNIT_TEST
#pragma warning disable 1591
// ReSharper disable InconsistentNaming

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Common.Extensions
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    internal sealed class TypeExtensionsTests
    {
        [Test]
        public void TypeExtensions_IsCastableTo_Positive()
        {
            Assert.DoesNotThrow(() => Assert.IsTrue(typeof(short).IsCastableTo(typeof(int))));
            Assert.DoesNotThrow(() => Assert.IsTrue(typeof(List<int>).IsCastableTo(typeof(IEnumerable))));
            Assert.DoesNotThrow(() => Assert.IsTrue(typeof(decimal).IsCastableTo(typeof(float))));
            Assert.DoesNotThrow(() => Assert.IsTrue(typeof(TimeSpan).IsCastableTo(typeof(IComparable))));

            Assert.DoesNotThrow(() => Assert.IsFalse(typeof(int).IsCastableTo(typeof(short))));
            Assert.DoesNotThrow(() => Assert.IsFalse(typeof(DateTime).IsCastableTo(typeof(IEnumerable))));
            Assert.DoesNotThrow(() => Assert.IsFalse(typeof(double).IsCastableTo(typeof(bool))));
            Assert.DoesNotThrow(() => Assert.IsFalse(typeof(TimeSpan).IsCastableTo(typeof(InvalidCastException))));
        }

        [Test]
        public void TypeExtensions_IsCastableTo_Negative()
        {
            Type cat = null;
            Assert.Throws<ArgumentNullException>(() => cat.IsCastableTo(typeof(bool)));
            Assert.Throws<ArgumentNullException>(() => typeof(double).IsCastableTo(cat));
        }

        [Test]
        public void TypeExtensions_GetDefault_Positive()
        {
            Assert.DoesNotThrow(() => Assert.AreEqual(0, typeof(short).GetDefault()));
            Assert.DoesNotThrow(() => Assert.AreEqual(0, typeof(double).GetDefault<double>()));
            Assert.DoesNotThrow(() => Assert.AreEqual(TimeSpan.Zero, typeof(TimeSpan).GetDefault()));
            Assert.DoesNotThrow(() => Assert.AreEqual(DateTime.MinValue, typeof(DateTime).GetDefault<DateTime>()));
            Assert.DoesNotThrow(() => Assert.AreEqual(null, typeof(List<int>).GetDefault()));
            Assert.DoesNotThrow(() => Assert.AreEqual(null, typeof(IComparable).GetDefault<IComparable>()));
            Assert.DoesNotThrow(() => Assert.AreEqual(null, typeof(InvalidCastException).GetDefault<InvalidCastException>()));
        }

        [Test]
        public void TypeExtensions_GetDefault_Negative()
        {
            Type cat = null;
            Assert.Throws<ArgumentNullException>(() => cat.GetDefault());
            Assert.Throws<ArgumentNullException>(() => cat.GetDefault<int>());

            cat = typeof(int);
            Assert.Throws<InvalidCastException>(() => cat.GetDefault<short>());
            Assert.Throws<InvalidCastException>(() => cat.GetDefault<IEnumerable>());
        }
    }
}

// ReSharper restore InconsistentNaming
#pragma warning restore 1591
#endif