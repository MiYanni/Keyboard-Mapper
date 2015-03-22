using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Parser.Valve.Commands.Types;

namespace Parser.Valve.Commands
{
    public static class Factory
    {
        private static readonly Dictionary<string, Func<Command>> Binder =
        new Dictionary<string, Func<Command>> {
            { "unbindall", () => new Value() }
        };
        public static Command Create(string command)
        {
            return Binder[command]();
        }
    }
}