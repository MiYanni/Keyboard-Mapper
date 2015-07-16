using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Extensions;

namespace Parser.Valve.Commands.Types
{
    public class Basic : Command
    {
        public Basic(JsonCommand textCommand)
            : base(textCommand)
        {

        }
    }
}