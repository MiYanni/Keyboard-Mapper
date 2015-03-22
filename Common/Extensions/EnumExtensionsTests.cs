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
    internal sealed class EnumExtensionsTests
    {
        [Test]
        public void EnumExtensions_AllValues_Positive()
        {
            Assert.DoesNotThrow(() => Assert.AreEqual(
                new[] { DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday },
                EnumExtensions.AllValues<DayOfWeek>()));
            Assert.DoesNotThrow(() => Assert.AreEqual(
                new[] { StringComparison.CurrentCulture, StringComparison.CurrentCultureIgnoreCase, StringComparison.InvariantCulture,
                    StringComparison.InvariantCultureIgnoreCase, StringComparison.Ordinal, StringComparison.OrdinalIgnoreCase },
                EnumExtensions.AllValues<StringComparison>()));
        }

        [Test]
        public void EnumExtensions_AllValues_Negative()
        {
            Assert.Throws<ArgumentException>(() => EnumExtensions.AllValues<int>());
            Assert.Throws<ArgumentException>(() => EnumExtensions.AllValues<DateTime>());
        }

        [Test]
        public void EnumExtensions_GetName_Positive()
        {
            Assert.DoesNotThrow(() => Assert.AreEqual("Tuesday", DayOfWeek.Tuesday.GetName()));
            Assert.DoesNotThrow(() => Assert.AreEqual("CurrentCulture", StringComparison.CurrentCulture.GetName()));
        }

        [Test]
        public void EnumExtensions_GetName_Negative()
        {
            Assert.Throws<ArgumentException>(() => 5.GetName());
            Assert.Throws<ArgumentException>(() => DateTime.MinValue.GetName());
        }

        [Flags]
        private enum Coat { Red = 1, Green = 2, Blue = 4, Pancakes = 8 }

        [Flags]
        private enum Simple { Catsup = 1, Mister = 2, Belle = 4, Perch = 8 }

        [Test]
        public void EnumExtensions_HasFlag_Positive()
        {
            Assert.DoesNotThrow(() => Assert.AreEqual(true, EnumExtensions.HasFlag((Coat.Red | Coat.Pancakes), Coat.Pancakes)));
            Assert.DoesNotThrow(() => Assert.AreEqual(true, EnumExtensions.HasFlag((Simple.Catsup | Simple.Belle | Simple.Mister), (Simple.Catsup | Simple.Mister))));

            Assert.DoesNotThrow(() => Assert.AreEqual(false, EnumExtensions.HasFlag((Coat.Red | Coat.Pancakes), Coat.Green)));
            Assert.DoesNotThrow(() => Assert.AreEqual(false, EnumExtensions.HasFlag((Simple.Catsup | Simple.Belle | Simple.Mister), (Simple.Catsup | Simple.Mister | Simple.Perch))));
        }

        [Test]
        public void EnumExtensions_HasFlag_Negative()
        {
            Assert.Throws<ArgumentException>(() => 5.HasFlag(2));
            Assert.Throws<ArgumentException>(() => DateTime.MinValue.HasFlag(DateTime.MaxValue));
        }

        [Test]
        public void EnumExtensions_HasFlags_Positive()
        {
            Assert.DoesNotThrow(() => Assert.AreEqual(true, (Coat.Red | Coat.Pancakes).HasFlags(Coat.Pancakes)));
            Assert.DoesNotThrow(() => Assert.AreEqual(true, (Simple.Catsup | Simple.Belle | Simple.Mister).HasFlags(Simple.Catsup, Simple.Mister)));

            Assert.DoesNotThrow(() => Assert.AreEqual(false, (Coat.Red | Coat.Pancakes).HasFlags(Coat.Green)));
            Assert.DoesNotThrow(() => Assert.AreEqual(false, (Simple.Catsup | Simple.Belle | Simple.Mister).HasFlags(Simple.Catsup, Simple.Mister, Simple.Perch)));
        }

        [Test]
        public void EnumExtensions_HasFlags_Negative()
        {
            Assert.Throws<ArgumentException>(() => 5.HasFlags(2, 6));
            Assert.Throws<ArgumentException>(() => DateTime.MinValue.HasFlags(DateTime.MaxValue, DateTime.MinValue));
        }

        [Test]
        public void EnumExtensions_AllFlags_Positive()
        {
            Assert.DoesNotThrow(() => Assert.AreEqual((Coat.Red | Coat.Green | Coat.Blue | Coat.Pancakes), EnumExtensions.AllFlags<Coat>()));
            Assert.DoesNotThrow(() => Assert.AreEqual((Simple.Catsup | Simple.Perch | Simple.Belle | Simple.Mister), EnumExtensions.AllFlags<Simple>()));
        }

        [Test]
        public void EnumExtensions_AllFlags_Negative()
        {
            Assert.Throws<ArgumentException>(() => EnumExtensions.AllFlags<int>());
            Assert.Throws<ArgumentException>(() => EnumExtensions.AllFlags<DateTime>());
        }

        [Test]
        public void EnumExtensions_AllFlagsExcept_Positive()
        {
            Assert.DoesNotThrow(() => Assert.AreEqual((Coat.Red | Coat.Green | Coat.Pancakes), EnumExtensions.AllFlagsExcept(Coat.Blue)));
            Assert.DoesNotThrow(() => Assert.AreEqual((Simple.Catsup | Simple.Belle), EnumExtensions.AllFlagsExcept(Simple.Perch, Simple.Mister)));
        }

        [Test]
        public void EnumExtensions_AllFlagsExcept_Negative()
        {
            Assert.Throws<ArgumentException>(() => EnumExtensions.AllFlagsExcept(5, 7));
            Assert.Throws<ArgumentException>(() => EnumExtensions.AllFlagsExcept(DateTime.MinValue, DateTime.MaxValue));
        }

        [Test]
        public void EnumExtensions_SplitFlags_Positive()
        {
            Assert.DoesNotThrow(() => Assert.AreEqual(new[] { Coat.Red, Coat.Green, Coat.Pancakes }, (Coat.Red | Coat.Green | Coat.Pancakes).SplitFlags()));
            Assert.DoesNotThrow(() => Assert.AreEqual(new[] { Simple.Catsup, Simple.Belle }, (Simple.Catsup | Simple.Belle).SplitFlags()));
        }

        [Test]
        public void EnumExtensions_SplitFlags_Negative()
        {
            Assert.Throws<ArgumentException>(() => 55.SplitFlags());
            Assert.Throws<ArgumentException>(() => DateTime.MinValue.SplitFlags());
        }

        [Test]
        public void EnumExtensions_CombineFlags_Positive()
        {
            Assert.DoesNotThrow(() => Assert.AreEqual((Coat.Red | Coat.Green | Coat.Pancakes), new[] { Coat.Red, Coat.Green, Coat.Pancakes }.CombineFlags()));
            Assert.DoesNotThrow(() => Assert.AreEqual((Simple.Catsup | Simple.Belle), new[] { Simple.Catsup, Simple.Belle }.CombineFlags()));
            Assert.DoesNotThrow(() => Assert.AreEqual((Simple)0, new Simple[] { }.CombineFlags()));
        }

        [Test]
        public void EnumExtensions_CombineFlags_Negative()
        {
            Assert.Throws<ArgumentException>(() => new[] { 5, 7 }.CombineFlags());
            Assert.Throws<ArgumentException>(() => new[] { DateTime.MinValue, DateTime.MaxValue }.CombineFlags());
            Simple[] things = null;
            Assert.Throws<ArgumentNullException>(() => things.CombineFlags());
        }

        [Test]
        public void EnumExtensions_GetFlagBitIndex_Positive()
        {
            Assert.DoesNotThrow(() => Assert.AreEqual(2, Coat.Blue.GetFlagBitIndex()));
            Assert.DoesNotThrow(() => Assert.AreEqual(0, Simple.Catsup.GetFlagBitIndex()));

            Assert.DoesNotThrow(() => Assert.AreEqual(2, Coat.Blue.GetFlagBitIndexLowest()));
            Assert.DoesNotThrow(() => Assert.AreEqual(0, Simple.Catsup.GetFlagBitIndexLowest()));
            Assert.DoesNotThrow(() => Assert.AreEqual(-1, ((Simple)0).GetFlagBitIndexLowest()));

            Assert.DoesNotThrow(() => Assert.AreEqual(2, Coat.Blue.GetFlagBitIndexHighest()));
            Assert.DoesNotThrow(() => Assert.AreEqual(0, Simple.Catsup.GetFlagBitIndexHighest()));
            Assert.DoesNotThrow(() => Assert.AreEqual(-1, ((Simple)0).GetFlagBitIndexHighest()));
        }

        [Test]
        public void EnumExtensions_GetFlagBitIndex_Negative()
        {
            Assert.Throws<ArgumentException>(() => 5.GetFlagBitIndex());
            Assert.Throws<ArgumentException>(() => DateTime.MinValue.GetFlagBitIndex());

            Assert.Throws<ArgumentException>(() => 5.GetFlagBitIndexLowest());
            Assert.Throws<ArgumentException>(() => DateTime.MinValue.GetFlagBitIndexLowest());

            Assert.Throws<ArgumentException>(() => 5.GetFlagBitIndexHighest());
            Assert.Throws<ArgumentException>(() => DateTime.MinValue.GetFlagBitIndexHighest());
        }
    }
}

// ReSharper restore InconsistentNaming
#pragma warning restore 1591
#endif