#if UNIT_TEST
#pragma warning disable 1591
// ReSharper disable InconsistentNaming

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using NUnit.Framework;

namespace Common.Extensions
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    internal sealed class SerializationExtensionsTests
    {
        [Test]
        public void SerializationExtensions_Serialize_Positive()
        {
            var number = new byte[] { };
            var text = new byte[] { };
            var date = new byte[] { };

            Assert.DoesNotThrow(() => number = 1.Serialize());
            Assert.DoesNotThrow(() => text = "Copy".Serialize());
            Assert.DoesNotThrow(() => date = new DateTime(1954, 3, 23).Serialize());

            Assert.AreEqual(new byte[] { 0, 1, 0, 0, 0, 255, 255, 255, 255, 1, 0, 0, 0, 0, 0, 0, 0, 4, 1, 0, 0, 0,
                12, 83, 121, 115, 116, 101, 109, 46, 73, 110, 116, 51, 50, 1, 0, 0, 0, 7, 109, 95, 118, 97, 108, 117,
                101, 0, 8, 1, 0, 0, 0, 11 }, number);
            Assert.AreEqual(new byte[] { 0, 1, 0, 0, 0, 255, 255, 255, 255, 1, 0, 0, 0, 0, 0, 0, 0, 6, 1, 0, 0, 0, 4,
                67, 111, 112, 121, 11 }, text);
            Assert.AreEqual(new byte[] { 0, 1, 0, 0, 0, 255, 255, 255, 255, 1, 0, 0, 0, 0, 0, 0, 0, 4, 1, 0, 0, 0, 15,
                83, 121, 115, 116, 101, 109, 46, 68, 97, 116, 101, 84, 105, 109, 101, 2, 0, 0, 0, 5, 116, 105, 99, 107,
                115, 8, 100, 97, 116, 101, 68, 97, 116, 97, 0, 0, 9, 16, 0, 64, 24, 45, 96, 207, 141, 8, 0, 64, 24, 45,
                96, 207, 141, 8, 11 }, date);
        }

        public class Bacon
        {
            public int Shoe;
            public string Tower;
        }

        [Test]
        public void SerializationExtensions_Serialize_Negative()
        {
            int? number = null;
            Assert.Throws<ArgumentException>(() => new { Cheese = 43, Hamburger = "Cake" }.Serialize());
            Assert.Throws<ArgumentException>(() => new Bacon().Serialize());
            Assert.Throws<ArgumentNullException>(() => number.Serialize());
        }

        [Test]
        public void SerializationExtensions_Deserialize_Positive()
        {
            const int number = 1;
            var serializedNumber = number.Serialize();
            const string text = "Copy";
            var serializedText = text.Serialize();
            var date = new DateTime(1954, 3, 23);
            var serializedDate = date.Serialize();

            var deserializedNumber = 0;
            var deserializedText = String.Empty;
            var deserializedDate = new DateTime();

            Assert.DoesNotThrow(() => deserializedNumber = serializedNumber.Deserialize<int>());
            Assert.DoesNotThrow(() => deserializedText = serializedText.Deserialize<string>());
            Assert.DoesNotThrow(() => deserializedDate = serializedDate.Deserialize<DateTime>());

            Assert.AreEqual(number, deserializedNumber);
            Assert.AreEqual(text, deserializedText);
            Assert.AreEqual(date, deserializedDate);
            //// http://stackoverflow.com/questions/15003335/javascriptserializer-is-subtracting-one-day-from-date
            //Assert.AreEqual(date, deserializedDate.ToLocalTime());
        }

        [Test]
        public void SerializationExtensions_Deserialize_Negative()
        {
            byte[] blank = null;
            var wrongData = new byte[] { 0x04, 0x43, 0x12, 0x01, 0x01, 0x05, 0x045 };
            Assert.Throws<ArgumentException>(() => new byte[] { }.Deserialize());
            Assert.Throws<ArgumentNullException>(() => blank.Deserialize());
            Assert.Throws<SerializationException>(() => wrongData.Deserialize());
        }
    }
}

// ReSharper restore InconsistentNaming
#pragma warning restore 1591
#endif