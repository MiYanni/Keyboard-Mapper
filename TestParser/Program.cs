using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.Script.Serialization;
using Parser.Valve.Commands;
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

            return;
        }
    }
}