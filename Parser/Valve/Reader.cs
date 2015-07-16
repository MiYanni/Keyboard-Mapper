using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Parser.Valve.Commands;
using Parser.Valve.Commands.Types;

namespace Parser.Valve
{
    public static class Reader
    {
        //public DirectoryInfo ConfigDirectory { get; private set; }

        //public Reader(DirectoryInfo configDirectory)
        //{
        //    ConfigDirectory = configDirectory;
        //}

        //public Command ReadNext()
        //{
        //    return new Value();
        //}

        public static IEnumerable<Command> Parse(JsonFile file)
        {
            return file.Commands.Select(Factory.Create);
        } 
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