using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parser.Valve.Commands.Types
{
    public class Value : Command
    {
        public double Default { get; private set; }

        public Value(JsonCommand textCommand)
            : base(textCommand)
        {
            Default = Convert.ToDouble(textCommand.Default);
        }
    }
}