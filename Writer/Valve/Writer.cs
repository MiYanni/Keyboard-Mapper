//using Parser.Valve.Commands.Types;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Writer.Valve.Commands;

namespace Writer.Valve
{
    public class Writer
    {
        public DirectoryInfo ConfigDirectory { get; private set; }

        public FileInfo ConfigFile { get; set; }

        public static string ConfigFileName = "config.cfg";

        public FileInfo AutoExecFile { get; set; }

        public static string AutoExecFileName = "autoexec.cfg";

        public Writer(DirectoryInfo configDirectory)
        {
            ConfigDirectory = configDirectory;
            ConfigFile = new FileInfo(ConfigDirectory + ConfigFileName);
            AutoExecFile = new FileInfo(ConfigDirectory + AutoExecFileName);
        }

        public void WriteConfig(IEnumerable<Command> commands)
        {
            File.WriteAllText(ConfigFile.ToString(),
                commands.Select(c => c.ToString()).Aggregate((c1, c2) => c1 + Environment.NewLine + c2));
        }

        //public Command ReadNext()
        //{
        //    return new Value();
        //}

        //public static IEnumerable<Command> Parse(JsonFile file)
        //{
        //    return file.Commands.Select(Factory.Create);
        //}
    }

    //internal static class StringExtensions
    //{
    //    public static Command ToCommand(this string line)
    //    {
    //        var parts = line.Split(' ');
    //        return new Value();
    //    }
    //}
}