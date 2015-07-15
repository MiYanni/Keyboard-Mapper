using System.Runtime.Serialization;

namespace Parser.Valve.Commands
{
    [DataContract]
    public class JsonCommand
    {
        [DataMember(Name = "Command")]
        public string Name { get; set; }

        [DataMember(Name = "Default")]
        public string Default { get; set; }

        [DataMember(Name = "Cheat?")]
        public string IsCheat { get; set; }

        [DataMember(Name = "Help Text")]
        public string HelpText { get; set; }
    }
}