using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parser.Valve.Commands.Types
{
    public class Toggle : Command
    {
        public bool IsKeyDown { get; private set; }

        public Toggle(JsonCommand textCommand)
            : base(textCommand, textCommand.Name.Substring(1))
        {
            IsKeyDown = textCommand.Name.StartsWith("+");
        }
    }
}