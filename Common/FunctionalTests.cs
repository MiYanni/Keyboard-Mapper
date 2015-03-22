#if UNIT_TEST
#pragma warning disable 1591
// ReSharper disable InconsistentNaming

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Common.Extensions;
using NUnit.Framework;

namespace Common
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    internal sealed class FunctionalTests
    {
        [Test]
        public void FunctionalTests_ForLoop_Positive()
        {
            var value = 0;
            Assert.DoesNotThrow(() => Functional.ForLoop(0, i => i < 5, i => ++i, i => value = i));
            Assert.AreEqual(4, value);

            var value2 = "Bacon";
            Assert.DoesNotThrow(() => Functional.ForLoop(0, i => i < 10, i => ++i, i => value2 += i.ToString(CultureInfo.InvariantCulture)));
            Assert.AreEqual("Bacon0123456789", value2);

            var value3 = "Bacon";
            Assert.DoesNotThrow(() => Functional.ForLoop(0, i => i < 10, i => ++i, i =>
            {
                if (i.IsDivisibleBy(2)) return;
                value3 += i.ToString(CultureInfo.InvariantCulture);
            }));
            Assert.AreEqual("Bacon13579", value3);

            var chart = 1;
            Assert.DoesNotThrow(() => Functional.ForLoop(1, i => i < 5, i => ++i, i =>
            {
                if (i.IsDivisibleBy(2)) return false;
                chart += i;
                return true;
            }));
            Assert.AreEqual(2, chart);
        }

        [Test]
        public void FunctionalTests_ForLoop_Negative()
        {
            Assert.Throws<ArgumentNullException>(() => Functional.ForLoop(null, null, null, (Action<TimeZone>)null));
            Assert.Throws<ArgumentNullException>(() => Functional.ForLoop(null, null, null, (Func<TimeZone, bool>)null));

            int? garbage;
            Assert.Throws<ArgumentNullException>(() => Functional.ForLoop(new int?(0), null, i => ++i, i => garbage = i));
            Assert.Throws<ArgumentNullException>(() => Functional.ForLoop(new int?(0), i => i == 10, null, i => garbage = i));
            Assert.Throws<ArgumentNullException>(() => Functional.ForLoop(0, i => i == 10, i => ++i, (Action<int?>)null));
            Assert.Throws<ArgumentNullException>(() => Functional.ForLoop(0, i => i == 10, i => ++i, (Func<int?, bool>)null));
        }

        [Test]
        public void FunctionalTests_UsingStatement_Positive()
        {
            var value = -500;
            Assert.DoesNotThrow(() => Functional.UsingStatement(new MemoryStream(), i => { value = (int)i.Length; }));
            Assert.AreEqual(0, value);

            Assert.DoesNotThrow(() => Assert.AreEqual(0, Functional.UsingStatement(new MemoryStream(), i => (int)i.Length)));
        }

        [Test]
        public void FunctionalTests_UsingStatement_Negative()
        {
            Assert.Throws<ArgumentNullException>(() => Functional.UsingStatement(new MemoryStream(), null));
            Assert.Throws<ArgumentNullException>(() => Functional.UsingStatement(new MemoryStream(), (Func<MemoryStream, int>)null));
        }
    }
}

// ReSharper restore InconsistentNaming
#pragma warning restore 1591
#endif