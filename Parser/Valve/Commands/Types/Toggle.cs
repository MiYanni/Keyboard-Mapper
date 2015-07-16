using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parser.Valve.Commands.Types
{
    public class Toggle : Command
    {
        public Toggle(JsonCommand textCommand)
            : base(textCommand)
        {
            
        }
    }
}