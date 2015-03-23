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
            Console.WriteLine(test.Create());
            Console.WriteLine("Press any key to exit.");
            Console.ReadLine();
        }
    }
}