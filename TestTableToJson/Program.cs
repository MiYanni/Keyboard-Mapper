using System;
using System.Dynamic;
using System.IO;
using TableToJson;

namespace TestTableToJson
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            //var test = new List(new DirectoryInfo(@"TestTable.html"));
            //var test = new List(@"https://developer.valvesoftware.com/wiki/Console_Command_List");
            //var json = test.Create();
            //Console.WriteLine(json);
            //File.WriteAllText(new DirectoryInfo(@"TestTable.json").ToString(), json);
            //Console.WriteLine("Press any key to exit.");
            //Console.ReadLine();'
            CreateValveConsoleCommandList();
        }

        private static void CreateValveConsoleCommandList()
        {
            var test = new Tables(@"https://developer.valvesoftware.com/wiki/Console_Command_List");
            var json = test.ToJsonUngrouped();
            Console.WriteLine(json);
            File.WriteAllText(new DirectoryInfo(@"ValveConsoleCommands.json").ToString(), json);
            Console.WriteLine("Press any key to exit.");
            Console.ReadLine();
        }
    }
}