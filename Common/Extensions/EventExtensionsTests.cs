#if UNIT_TEST
#pragma warning disable 1591
// ReSharper disable InconsistentNaming

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
using NUnit.Framework;

namespace Common.Extensions
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    internal sealed class EventExtensionsTests
    {
        public sealed class TestEventArgs : EventArgs
        {
            public int Taco { get; set; }

            public string Centipede { get; set; }
        }

        public event EventHandler Cake;

        public event EventHandler<TestEventArgs> Bingo;

        public event GenericEventHandler Blessing;

        public event GenericEventHandler<int> Testatrix;

        public event GenericEventHandler<int?> TestatrixNewAge;

        [Test]
        public void EventExtensions_Raise_Positive()
        {
            var eventTest = new EventExtensionsTests();
            RaiseEvent(() => eventTest.Cake.Raise(eventTest), m => eventTest.Cake += (s, e) =>
            {
                Assert.AreEqual(eventTest, s);
                TestEventArgs eventArg = null;
                Assert.DoesNotThrow(() => eventArg = e.CastTo<TestEventArgs>());
                Assert.AreEqual(3, eventArg.Taco);
                Assert.AreEqual("Misaligned", eventArg.Centipede);
                m.Set();
            }, () => eventTest.Cake.Raise(eventTest, new TestEventArgs { Taco = 3, Centipede = "Misaligned" }));

            RaiseEvent(() => eventTest.Bingo.Raise(eventTest), m => eventTest.Bingo += (s, e) =>
            {
                Assert.AreEqual(eventTest, s);
                Assert.AreEqual(1337, e.Taco);
                Assert.AreEqual("Malware", e.Centipede);
                m.Set();
            }, () => eventTest.Bingo.Raise(eventTest, new TestEventArgs { Taco = 1337, Centipede = "Malware" }));

            RaiseEvent(() => eventTest.Blessing.Raise(eventTest), m => eventTest.Blessing += s =>
            {
                Assert.AreEqual(eventTest, s);
                m.Set();
            }, () => eventTest.Blessing.Raise(eventTest));

            RaiseEvent(() => eventTest.Testatrix.Raise(eventTest, -123), m => eventTest.Testatrix += (s, e) =>
            {
                Assert.AreEqual(eventTest, s);
                Assert.AreEqual(42, e);
                m.Set();
            }, () => eventTest.Testatrix.Raise(eventTest, 42));
        }

        private static void RaiseEvent(Func<bool> raiseFalse, Action<ManualResetEvent> addEvent, Func<bool> raiseTrue)
        {
            var manualEvent = new ManualResetEvent(false);
            Assert.DoesNotThrow(() => Assert.IsFalse(raiseFalse()));
            addEvent(manualEvent);
            Assert.DoesNotThrow(() => Assert.IsTrue(raiseTrue()));
            Assert.IsTrue(manualEvent.WaitOne(1000));
        }

        [Test]
        public void EventExtensions_Raise_Negative()
        {
            var eventTest = new EventExtensionsTests();
            RaiseEvent(() => eventTest.Cake.Raise(null), m => eventTest.Cake += (s, e) =>
            {
                Assert.IsNull(s);
                Assert.IsNull(e);
                m.Set();
            }, () => eventTest.Cake.Raise(null));

            RaiseEvent(() => eventTest.Bingo.Raise(null), m => eventTest.Bingo += (s, e) =>
            {
                Assert.IsNull(s);
                Assert.IsNull(e);
                m.Set();
            }, () => eventTest.Bingo.Raise(null));

            RaiseEvent(() => eventTest.Blessing.Raise(null), m => eventTest.Blessing += s =>
            {
                Assert.IsNull(s);
                m.Set();
            }, () => eventTest.Blessing.Raise(null));

            RaiseEvent(() => eventTest.TestatrixNewAge.Raise(null, null), m => eventTest.TestatrixNewAge += (s, e) =>
            {
                Assert.IsNull(s);
                Assert.IsNull(e);
                m.Set();
            }, () => eventTest.TestatrixNewAge.Raise(null, null));
        }
    }
}

// ReSharper restore InconsistentNaming
#pragma warning restore 1591
#endif