using System;
using System.IO;
using Parser.Valve.Commands;
using Parser.Valve.Commands.Types;

namespace Parser.Valve
{
    public class Reader
    {
        public DirectoryInfo ConfigDirectory { get; private set; }

        public Reader(DirectoryInfo configDirectory)
        {
            ConfigDirectory = configDirectory;
        }

        public Command ReadNext()
        {
            return new Value();
        }
    }

    internal static class StringExtensions
    {
        public static Command ToCommand(this string line)
        {
            var parts = line.Split(' ');
            return new Value();
        }
    }
}