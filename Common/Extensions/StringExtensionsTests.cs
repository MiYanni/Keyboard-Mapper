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
    internal sealed class StringExtensionsTests
    {
        [TestCase(",", 20, 1337, -216, 8696354, 0, Result = "20,1337,-216,8696354,0"),
        TestCase(".", "Calculate", "Taco", "Band Wagon", "Hello", "Soup Lottery", Result = "Calculate.Taco.Band Wagon.Hello.Soup Lottery"),
        TestCase("%", 5.557, -75.2145, .00241765, Result = "5.557%-75.2145%0.00241765"),
        TestCase("-", (byte)0x04, (byte)0xB5, (byte)0xFF, (byte)5, Result = "0x04-0xB5-0xFF-0x05")]
        public string StringExtensions_ToElementString_ObjectPositive(string delimiter, params object[] elements)
        {
            return StringExtensions_ToElementString_Helper(elements, delimiter);
        }

        [TestCase("è", Result = "00:00:01è00:00:00.0000050"),
        TestCase(" ", Result = "00:00:01 00:00:00.0000050")]
        public string StringExtensions_ToElementString_ClassPositive(string delimiter)
        {
            var elementList = new List<TimeSpan> { TimeSpan.FromMilliseconds(1000), new TimeSpan(50) };
            return StringExtensions_ToElementString_Helper(elementList, delimiter);
        }

        [TestCase(true, Result = ""),
        TestCase(false, Result = "")]
        public string StringExtensions_ToElementString_EmptyPositive(bool nullOrZero)
        {
            var elementList = nullOrZero ? null : new List<byte>();
            var result = string.Empty;
            Assert.DoesNotThrow(() => result = elementList.ToElementString());
            return result;
        }

        private static string StringExtensions_ToElementString_Helper<T>(IEnumerable<T> elements, string delimiter)
        {
            var result = string.Empty;
            Assert.DoesNotThrow(() => result = elements.ToElementString(delimiter));
            Assert.NotNull(result);
            Assert.IsFalse(string.IsNullOrEmpty(result));
            return result;
        }

        [TestCase(new object[] { 1, 2.4, "Cancel" }, "*", true, Result = "{Count=3}*1*2.4*Cancel"),
        TestCase(new object[] { "%", false, 1.0 / 3.0 }, "!", false, Result = "%!False!0.333333333333333"),
        TestCase(new object[] { null, "", 0 }, " ", false, Result = "null  0")]
        public string StringExtensions_ToElementString_Positive(object[] elements, string delimiter, bool showCount)
        {
            var result = string.Empty;
            Assert.DoesNotThrow(() => result = elements.ToElementString(delimiter, showCount));
            Assert.NotNull(result);
            Assert.IsFalse(string.IsNullOrEmpty(result));
            return result;
        }

        [TestCase(new[] { "Crab", "Cheese Cake", "Quality" }, new object[] { 15, -11.4564, "Hello Cat" }, Result = "[Crab:15, Cheese Cake:-11.4564, Quality:Hello Cat]"),
        TestCase(new[] { "A", "B", "C" }, new object[] { null, null, null }, Result = "[A:null, B:null, C:null]"),
        TestCase(new string[] { null, null, null }, new object[] { null, null, null }, Result = "[:null, :null, :null]")]
        public string StringExtensions_ToNameValueString_Positive(string[] names, object[] values)
        {
            var classList = names.Zip(values, (n, v) => new Tuple<string, object>(n, v)).ToList();
            var result = String.Empty;
            Assert.DoesNotThrow(() => result = classList.ToNameValueString());
            Assert.IsFalse(String.IsNullOrEmpty(result));
            return result;
        }

        [Test]
        public void StringExtensions_ToNameValueString_Negative()
        {
            List<Tuple<string, object>> nullList = null;
            Assert.AreEqual(String.Empty, nullList.ToNameValueString());
            nullList = new List<Tuple<string, object>> { null, null, null, null };
            Assert.AreEqual("[]", nullList.ToNameValueString());
        }

        private static readonly Dictionary<string, object>[] DictPositiveArray = new[]
        {
            new Dictionary<string, object>
            {
                { "Hang Gliders", TimeSpan.FromDays(10) },
                { "Mangos", new List<int> { 3421, 435 } },
                { "Someday", 42.11213 }
            },
            new Dictionary<string, object>
            {
                { "A", null },
                { "B", true },
                { "C", new DateTime(100) },
                { "D", new Tuple<int, bool>(1234, false) }
            },
            null
        };

        [TestCase(0, Result = "[Hang Gliders:10.00:00:00, Mangos:System.Collections.Generic.List`1[System.Int32], Someday:42.11213]"),
        TestCase(1, Result = "[A:null, B:True, C:1/1/0001 12:00:00 AM, D:(1234, False)]"),
        TestCase(2, Result = "")]
        public string StringExtensions_ToNameValueString_DictPositive(int arrayIndex)
        {
            var result = string.Empty;
            Assert.DoesNotThrow(() => result = DictPositiveArray[arrayIndex].ToNameValueString());
            return result;
        }

        [TestCase("Bacon", Result = "\"Bacon\""),
        TestCase("HorseShoe", Result = "\"HorseShoe\""),
        TestCase(null, Result = "\"\""),
        TestCase("", Result = "\"\""),
        TestCase("\"Cake\"", Result = "\"\"Cake\"\"")]
        public string StringExtensions_ToQuotedString_Positive(string textToQuote)
        {
            var result = string.Empty;
            Assert.DoesNotThrow(() => result = textToQuote.ToQuotedString());
            Assert.IsFalse(string.IsNullOrEmpty(result));
            return result;
        }

        public enum TestEnum { First = 0, Second = 1, Third = 2, Fourth = 3 }

        [TestCase(TestEnum.First, "=", Result = "First=0"),
        TestCase(TestEnum.Second, "&", Result = "Second&1"),
        TestCase(TestEnum.Third, "^", Result = "Third^2"),
        TestCase(TestEnum.Fourth, "#", Result = "Fourth#3")]
        public string StringExtensions_CreateEnumString_Positive(TestEnum testEnum, string delim)
        {
            var result = string.Empty;
            Assert.DoesNotThrow(() => result = testEnum.ToEnumString(delim));
            return result;
        }

        [TestCase(true),
        TestCase(42),
        TestCase(55.66)]
        public void StringExtensions_CreateEnumString_Negative<T>(T testNotEnum) where T : struct, IConvertible
        {
            Assert.Throws<ArgumentException>(() => testNotEnum.ToEnumString());
        }

        [TestCase("tacoSalad", Result = "taco Salad"),
        TestCase("MrMimeIsAwesome", Result = "Mr Mime Is Awesome"),
        TestCase("", Result = ""),
        TestCase("WWFIsTotallyRAD", Result = "WWF Is Totally RAD"),
        TestCase("nothingtoseehere", Result = "nothingtoseehere")]
        public string StringExtensions_ToSpacedStringFromCamelCase_Positive(string camelCased)
        {
            var result = string.Empty;
            Assert.DoesNotThrow(() => result = camelCased.ToSpacedStringFromCamelCase());
            return result;
        }

        private sealed class ReflectedClass
        {
            public object Germs { get; set; }

            public static int Fifty { get { return 50; } }

            private static double Hello { get { return 43110.0; } }

            internal TimeSpan Houses { get; set; }

            public List<bool> Bacon { get; set; }

            public string[] NewOne { get; set; }
        }

        private static readonly object[] ReflectedArray = new object[]
        {
            new { DateTime = new DateTime(55, 2, 12), Interview = 55 },
            new Tuple<string, float>("Banana", (float)4.4),
            new List<short> { 22, 55, 700, 3421},
            new ReflectedClass { Germs = null, Houses = new TimeSpan(4,3,2,1), Bacon = new List<bool>{true, false, true}, NewOne = new[] {"Say So", "Please"}},
            null
        };

        [TestCase(0, "4", "^", Result = "Class^AnonymousType4DateTime^2/12/0055 12:00:00 AM4Interview^55"),
        TestCase(1, "Oh", "MM", Result = "ClassMMTuple`2OhItem1MMBananaOhItem2MM4.4"),
        TestCase(2, "*&", "!", Result = "Class!List`1*&Capacity!4*&Count!4*&Items!22,55,700,3421"),
        TestCase(3, "Law 2", "C~", Result = "ClassC~ReflectedClassLaw 2GermsC~nullLaw 2BaconC~True,False,TrueLaw 2NewOneC~Say So,Please"),
        TestCase(4, "", "", Result = "")]
        public string StringExtensions_ToReflectedString_Positive(int arrayIndex, string propDelim, string valueDelim)
        {
            var result = string.Empty;
            Assert.DoesNotThrow(() => result = ReflectedArray[arrayIndex].ToReflectedString(propDelim, valueDelim));
            Console.WriteLine(result);
            return result;
        }
    }
}

// ReSharper restore InconsistentNaming
#pragma warning restore 1591
#endif