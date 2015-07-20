using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

#if UNIT_TEST
#pragma warning disable 1591
// ReSharper disable InconsistentNaming

namespace Writer.Valve.Commands
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    internal sealed class CommandTests
    {
        [Test]
        public void CommandTests_MethodName_Condition()
        {
        }
    }
}

// ReSharper restore InconsistentNaming
#pragma warning restore 1591
#endif