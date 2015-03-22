#if UNIT_TEST
#pragma warning disable 1591
// ReSharper disable InconsistentNaming

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

namespace Common.Extensions
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    internal sealed class ThrowExtensionsTests
    {
        private enum TestEnum { One, Two, Three }

        [Test]
        public void ThrowExtensions_ThrowIfNotEnum_Positive()
        {
            Assert.DoesNotThrow(() => typeof(TestEnum).ThrowIfNotEnum());
            Assert.DoesNotThrow(() => typeof(StringComparison).ThrowIfNotEnum());
            Assert.DoesNotThrow(() => typeof(DayOfWeek).ThrowIfNotEnum());

            var testEnum = TestEnum.One;
            var stringComparison = StringComparison.CurrentCulture;
            var dayOfWeek = DayOfWeek.Friday;

            Assert.DoesNotThrow(() => testEnum = testEnum.ThrowIfNotEnum());
            Assert.DoesNotThrow(() => stringComparison = stringComparison.ThrowIfNotEnum());
            Assert.DoesNotThrow(() => dayOfWeek = dayOfWeek.ThrowIfNotEnum());

            Assert.AreEqual(TestEnum.One, testEnum);
            Assert.AreEqual(StringComparison.CurrentCulture, stringComparison);
            Assert.AreEqual(DayOfWeek.Friday, dayOfWeek);

            Assert.DoesNotThrow(ThrowExtensions.ThrowIfNotEnum<TestEnum>);
            Assert.DoesNotThrow(ThrowExtensions.ThrowIfNotEnum<StringComparison>);
            Assert.DoesNotThrow(ThrowExtensions.ThrowIfNotEnum<DayOfWeek>);
        }

        [Test]
        public void ThrowExtensions_ThrowIfNotEnum_Negative()
        {
            Assert.Throws<ArgumentException>(() => typeof(int).ThrowIfNotEnum());
            Assert.Throws<ArgumentException>(() => typeof(DateTime).ThrowIfNotEnum());
            Assert.Throws<ArgumentException>(() => typeof(TimeZone).ThrowIfNotEnum());

            string text = null;
            Assert.Throws<ArgumentException>(() => 1.ThrowIfNotEnum());
            Assert.Throws<ArgumentException>(() => (new DateTime()).ThrowIfNotEnum());
            Assert.Throws<ArgumentException>(() => true.ThrowIfNotEnum());
            Assert.Throws<ArgumentException>(() => text.ThrowIfNotEnum());

            Assert.Throws<ArgumentException>(ThrowExtensions.ThrowIfNotEnum<int>);
            Assert.Throws<ArgumentException>(ThrowExtensions.ThrowIfNotEnum<DateTime>);
            Assert.Throws<ArgumentException>(ThrowExtensions.ThrowIfNotEnum<string>);
        }

        [Test]
        public void ThrowExtensions_ThrowIfNull_Positive()
        {
            const string text = "text";
            var random = new Random();
            var argumentException = new ArgumentException();

            Assert.DoesNotThrow(() => 1.ThrowIfNull("int"));
            Assert.DoesNotThrow(() => text.ThrowIfNull(text));
            Assert.DoesNotThrow(() => random.ThrowIfNull("random"));
            Assert.DoesNotThrow(() => argumentException.ThrowIfNull("argument exception"));
            Assert.DoesNotThrow(() => 5.5.ThrowIfNull());

            Assert.DoesNotThrow(() => text.ThrowIfNull(null));
        }

        [Test]
        public void ThrowExtensions_ThrowIfNull_Negative()
        {
            string text = null;
            Random random = null;
            ArgumentException argumentException = null;

            Assert.Throws<ArgumentNullException>(() => text.ThrowIfNull("text"));
            Assert.Throws<ArgumentNullException>(() => random.ThrowIfNull("random"));
            Assert.Throws<ArgumentNullException>(() => argumentException.ThrowIfNull("argument exception"));

            Assert.Throws<ArgumentNullException>(() => text.ThrowIfNull(text));
            Assert.Throws<ArgumentNullException>(() => text.ThrowIfNull());
        }

        [Test]
        public void ThrowExtensions_ThrowIfOutOfRange_Positive()
        {
            const int integer = 5;
            const bool boolean = true;
            var dateTime = new DateTime(45, 5, 2);

            Assert.DoesNotThrow(() => integer.ThrowIfOutOfRange("integer", 3, 300));
            Assert.DoesNotThrow(() => boolean.ThrowIfOutOfRange("boolean", false, true));
            Assert.DoesNotThrow(() => dateTime.ThrowIfOutOfRange("datetime", new DateTime(3, 2, 23), new DateTime(765, 4, 9)));

            Assert.DoesNotThrow(() => integer.ThrowIfOutOfRange("integer", -341, null));
            Assert.DoesNotThrow(() => boolean.ThrowIfOutOfRange("boolean", false, null));
            Assert.DoesNotThrow(() => dateTime.ThrowIfOutOfRange("datetime", new DateTime(33, 6, 7), null));

            Assert.DoesNotThrow(() => integer.ThrowIfOutOfRange("integer", null, 14));
            Assert.DoesNotThrow(() => boolean.ThrowIfOutOfRange("boolean", null, true));
            Assert.DoesNotThrow(() => dateTime.ThrowIfOutOfRange("datetime", null, new DateTime(68, 5, 15)));

            Assert.AreEqual(integer, integer.ThrowIfOutOfRange("integer", null, null));
            Assert.AreEqual(boolean, boolean.ThrowIfOutOfRange("boolean", null, null));
            Assert.AreEqual(dateTime, dateTime.ThrowIfOutOfRange("datetime", null, null));

            Assert.AreEqual(integer, integer.ThrowIfOutOfRange(null, null, null));
            Assert.AreEqual(boolean, boolean.ThrowIfOutOfRange(null, null, null));
            Assert.AreEqual(dateTime, dateTime.ThrowIfOutOfRange(null, null, null));
        }

        [Test]
        public void ThrowExtensions_ThrowIfOutOfRange_Negative()
        {
            const int integer = 5;
            const bool boolean = true;
            var dateTime = new DateTime(45, 5, 2);

            Assert.Throws<ArgumentOutOfRangeException>(() => integer.ThrowIfOutOfRange("integer", 300, 3));
            Assert.Throws<ArgumentOutOfRangeException>(() => boolean.ThrowIfOutOfRange("boolean", true, false));
            Assert.Throws<ArgumentOutOfRangeException>(() => dateTime.ThrowIfOutOfRange("datetime", new DateTime(765, 4, 9), new DateTime(3, 2, 23)));

            Assert.Throws<ArgumentOutOfRangeException>(() => integer.ThrowIfOutOfRange("integer", null, -341));
            Assert.Throws<ArgumentOutOfRangeException>(() => boolean.ThrowIfOutOfRange("boolean", null, false));
            Assert.Throws<ArgumentOutOfRangeException>(() => dateTime.ThrowIfOutOfRange("datetime", null, new DateTime(33, 6, 7)));

            Assert.Throws<ArgumentOutOfRangeException>(() => integer.ThrowIfOutOfRange("integer", 14, null));
            Assert.Throws<ArgumentOutOfRangeException>(() => dateTime.ThrowIfOutOfRange("datetime", new DateTime(68, 5, 15), null));
        }

        // http://msdn.microsoft.com/en-us/library/z5z9kes2.aspx
        internal sealed class TestDigit
        {
            public TestDigit(double value)
            {
                Value = value;
            }

            public readonly double Value;

            // User-defined conversion from Digit to double
            public static implicit operator double(TestDigit testDigit)
            {
                return testDigit.Value;
            }

            //  User-defined conversion from double to Digit
            public static implicit operator TestDigit(double value)
            {
                return new TestDigit(value);
            }
        }

        [Test]
        public void ThrowExtensions_ThrowIfCannotCast_Positive()
        {
            Assert.DoesNotThrow(ThrowExtensions.ThrowIfCannotCast<short, int>);
            Assert.DoesNotThrow(ThrowExtensions.ThrowIfCannotCast<List<int>, IEnumerable>);

            Assert.DoesNotThrow(() => 1.ThrowIfCannotCast(typeof(double)));
            Assert.DoesNotThrow(() => new DateTime().ThrowIfCannotCast(typeof(IConvertible)));
            Assert.DoesNotThrow(() => new TestDigit(47).ThrowIfCannotCast(typeof(double)));

            Assert.DoesNotThrow(() => typeof(decimal).ThrowIfCannotCast(typeof(float)));
            Assert.DoesNotThrow(() => typeof(TimeSpan).ThrowIfCannotCast(typeof(IComparable)));
        }

        [Test]
        public void ThrowExtensions_ThrowIfCannotCast_Negative()
        {
            Assert.Throws<InvalidCastException>(ThrowExtensions.ThrowIfCannotCast<int, short>);
            Assert.Throws<InvalidCastException>(ThrowExtensions.ThrowIfCannotCast<DateTime, IEnumerable>);

            TimeZone chair = null;
            Assert.Throws<ArgumentNullException>(() => chair.ThrowIfCannotCast(typeof(double)));
            Assert.Throws<InvalidCastException>(() => new DateTime().ThrowIfCannotCast(typeof(TimeZone)));

            Type cat = null;
            Assert.Throws<InvalidCastException>(() => typeof(double).ThrowIfCannotCast(typeof(bool)));
            Assert.Throws<InvalidCastException>(() => typeof(TimeSpan).ThrowIfCannotCast(typeof(InvalidCastException)));
            Assert.Throws<ArgumentNullException>(() => cat.ThrowIfCannotCast(typeof(bool)));
            Assert.Throws<ArgumentNullException>(() => typeof(double).ThrowIfCannotCast(cat));
        }
    }
}

// ReSharper restore InconsistentNaming
#pragma warning restore 1591
#endif