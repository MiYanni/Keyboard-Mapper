using System;
using System.IO;
using TableToJson;

namespace TestTableToJson
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var test = new List(new DirectoryInfo(@"TestTable.html"));
            var json = test.Create();
            Console.WriteLine(json);
            File.WriteAllText(new DirectoryInfo(@"TestTable.json").ToString(), json);
            Console.WriteLine("Press any key to exit.");
            Console.ReadLine();
        }
    }
}