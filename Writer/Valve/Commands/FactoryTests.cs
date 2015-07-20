#if UNIT_TEST
#pragma warning disable 1591
// ReSharper disable InconsistentNaming

using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

namespace Writer.Valve.Commands
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    internal sealed class FactoryTests
    {
        [Test]
        public void FactoryTests_MethodName_Condition()
        {
        }
    }
}

// ReSharper restore InconsistentNaming
#pragma warning restore 1591
#endif