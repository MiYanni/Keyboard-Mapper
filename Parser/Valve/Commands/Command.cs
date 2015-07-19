using System;
using System.Runtime.Serialization;
using Common.Extensions;

namespace Parser.Valve.Commands
{
    public abstract class Command
    {
        //[DataMember(Name = "Command")]
        //public string Name { get; set; }

        //[DataMember(Name = "Default")]
        //public string Default { get; set; }

        //[DataMember(Name = "Cheat?")]
        //public string IsCheat { get; set; }

        //[DataMember(Name = "Help Text")]
        //public string HelpText { get; set; }

        protected JsonCommand TextCommand { get; set; }

        public string Name { get; private set; }

        public string Description { get; private set; }

        public bool IsCheat { get; private set; }

        private const string CheatIndicator = "Yes";

        protected Command(JsonCommand textCommand, string name = null)
        {
            TextCommand = textCommand;
            Name = name ?? textCommand.Name;
            //if (Name == "[mp_autokick]]")
            //    Name = "mp_autokick";
            Description = textCommand.HelpText;
            IsCheat = String.Equals(textCommand.IsCheat, CheatIndicator, StringComparison.InvariantCultureIgnoreCase);
        }

        public override string ToString()
        {
            return this.ToReflectedString();
        }
    }
}