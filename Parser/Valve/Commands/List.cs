using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CsQuery;
using NUnit.Framework;

namespace Parser.Valve.Commands
{
    public class List
    {
        //public string Url { get; private set; }

        //public List(string url)
        //{
        //    Url = url;
        //}

        public string Create()
        {
            //var dom = CQ.CreateFromUrl(Url);
            var dom = CQ.CreateDocumentFromFile(@"D:\Projects\Keyboard Mapper\Parser\Valve\Commands\TestTable.html");
            var tables = dom["table"];
            var result = String.Empty;
            tables.Each(table =>
            {
                var converter = new HtmlTableToJson(table);
                result += converter.ToJson();
            });
            return result;
        }
    }
}