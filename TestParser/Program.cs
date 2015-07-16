using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.Script.Serialization;
using Parser.Valve;
using Parser.Valve.Commands;
using Parser.Valve.Commands.Types;
using TableToJson;

namespace TestParser
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            //var test = new List();
            //Console.WriteLine(test.Create());
            //Console.WriteLine("Press any key to exit.");
            //Console.ReadLine();


            var jsonFile = new DirectoryInfo(@"ValveConsoleCommands.json");
            var file = File.ReadAllText(jsonFile.ToString());
            //var serializer = new JavaScriptSerializer();
            //var commands = serializer.Deserialize<JsonFile>(file);

            var serializer = new DataContractJsonSerializer(typeof(JsonFile));
            var ms = new MemoryStream(Encoding.Unicode.GetBytes(file));
            var result = serializer.ReadObject(ms) as JsonFile;

            var commands = Reader.Parse(result).ToList();
            //commands.ToList().ForEach(Console.WriteLine);
            var values = commands.Where(c => c is Value);
            var basics = commands.Where(c => c is Basic);
            var toggles = commands.Where(c => c is Toggle);
            var specials = commands.Where(c => c is Special);

            var commandGroups = new List<IEnumerable<Command>> {values, basics, toggles, specials};
            commandGroups.ForEach(cg =>
            {
                Console.WriteLine(cg.First().GetType().Name + ": " + cg.Count());
                //if (cg.First().GetType().Name == "Toggle")
                //{
                //    cg.ToList().ForEach(Console.WriteLine);
                //}
            });
            Console.WriteLine("Cheats: " + commandGroups.SelectMany(c => c).Count(c => c.IsCheat));

            Console.WriteLine("Press any key to exit.");
            Console.ReadLine();
        }
    }
}