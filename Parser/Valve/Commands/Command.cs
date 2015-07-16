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

        protected Command(JsonCommand textCommand)
        {
            TextCommand = textCommand;
        }

        public override string ToString()
        {
            return "Class: " + GetType().Name + ", " + 
                TextCommand.ToReflectedString(hasClassName: false);
        }
    }
}