using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Parser.Valve.Commands.Types;
using CommandDecider = System.Tuple<System.Func<Parser.Valve.Commands.JsonCommand, bool>, System.Func<Parser.Valve.Commands.JsonCommand, Parser.Valve.Commands.Command>>;

namespace Parser.Valve.Commands
{
    public static class Factory
    {
        //private static readonly Dictionary<string, Func<JsonCommand, Command>> Binder =
        //new Dictionary<string, Func<JsonCommand, Command>> {
        //    { "unbindall", j => new Value(j) }
        //};
        private static readonly CommandDecider[] Binder = {
            new CommandDecider(j => j.Name.StartsWith("+") || j.Name.StartsWith("-"), j => new Toggle(j)),
            new CommandDecider(j => j.Default == String.Empty, j => new Basic(j)),
            new CommandDecider(j => { double result; return Double.TryParse(j.Default, out result); }, j => new Value(j)),
            new CommandDecider(j => true, j => new Special(j))
        };
        public static Command Create(JsonCommand command)
        {
            return Binder.First(d => d.Item1(command)).Item2(command);
        }
    }
}