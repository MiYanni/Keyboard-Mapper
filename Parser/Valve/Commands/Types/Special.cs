using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parser.Valve.Commands.Types
{
    public class Special : Command
    {
        public Special(JsonCommand textCommand)
            : base(textCommand)
        {
            
        }
    }
}