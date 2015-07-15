using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Parser.Valve.Commands
{
    [DataContract]
    public class JsonFile
    {
        [DataMember(Name = "table")]
        public List<JsonCommand> Commands { get; set; } 
    }
}
