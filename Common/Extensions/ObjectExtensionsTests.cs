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
    internal sealed class ObjectExtensionsTests
    {
        private static TCast TestOne<T, TCast>(T value)
        {
            return (TCast)(dynamic)value;
        }

        [Test]
        public void ObjectExtensions_CastTo_Positive()
        {
            Assert.DoesNotThrow(() => Assert.AreEqual(1.0, 1.CastTo<double>()));
            Assert.DoesNotThrow(() => Assert.AreEqual(47.0, new ThrowExtensionsTests.TestDigit(47).CastTo<double>()));
            Assert.DoesNotThrow(() => Assert.AreEqual((float)34.0, new decimal(34).CastTo<float>()));

            Assert.DoesNotThrow(() => Assert.AreEqual(1.0, 1.CastToOrDefault<double>()));
            Assert.DoesNotThrow(() => Assert.AreEqual(47.0, new ThrowExtensionsTests.TestDigit(47).CastToOrDefault<double>()));
            Assert.DoesNotThrow(() => Assert.AreEqual((float)34.0, new decimal(34).CastToOrDefault<float>()));

            Assert.DoesNotThrow(() => Assert.AreEqual(false, 55.45.CastToOrDefault<bool>()));
            Assert.DoesNotThrow(() => Assert.AreEqual(null, "pink".CastToOrDefault<ThrowExtensionsTests.TestDigit>()));
            Assert.DoesNotThrow(() => Assert.AreEqual(DateTime.MinValue, new InvalidCastException().CastToOrDefault<DateTime>()));
        }

        [Test]
        public void ObjectExtensions_CastTo_Negative()
        {
            Type cat = null;
            Assert.Throws<InvalidCastException>(() => 1.CastTo<string>());
            Assert.Throws<InvalidCastException>(() => new ThrowExtensionsTests.TestDigit(47).CastTo<TimeSpan>());
            Assert.Throws<ArgumentNullException>(() => cat.CastTo<float>());

            Exception salad = null;
            Assert.Throws<ArgumentNullException>(() => salad.CastToOrDefault<DateTime>());
        }

        [Test]
        public void ObjectExtensions_IsNull_Positive()
        {
            const string text = "text";
            var random = new Random();
            var argumentException = new ArgumentException();

            Assert.DoesNotThrow(() => Assert.AreEqual(false, 1.IsNull()));
            Assert.DoesNotThrow(() => Assert.AreEqual(false, text.IsNull()));
            Assert.DoesNotThrow(() => Assert.AreEqual(false, random.IsNull()));
            Assert.DoesNotThrow(() => Assert.AreEqual(false, argumentException.IsNull()));
            Assert.DoesNotThrow(() => Assert.AreEqual(false, 5.5.IsNull()));

            Assert.DoesNotThrow(() => Assert.AreEqual(false, text.IsNull()));
        }

        [Test]
        public void ObjectExtensions_IsNull_Negative()
        {
            string text = null;
            Random random = null;
            ArgumentException argumentException = null;

            Assert.DoesNotThrow(() => Assert.AreEqual(true, text.IsNull()));
            Assert.DoesNotThrow(() => Assert.AreEqual(true, random.IsNull()));
            Assert.DoesNotThrow(() => Assert.AreEqual(true, argumentException.IsNull()));

            Assert.DoesNotThrow(() => Assert.AreEqual(true, text.IsNull()));
            Assert.DoesNotThrow(() => Assert.AreEqual(true, text.IsNull()));
        }
    }
}

// ReSharper restore InconsistentNaming
#pragma warning restore 1591
#endif