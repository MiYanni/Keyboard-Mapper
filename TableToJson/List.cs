using System;
using System.IO;
using CsQuery;

namespace TableToJson
{
    public class List
    {
        public string Url { get; private set; }

        public DirectoryInfo FilePath { get; private set; }

        private readonly CQ _dom;

        public List(string url)
        {
            Url = url;
            _dom = CQ.CreateFromUrl(Url);
        }

        public List(DirectoryInfo filePath)
        {
            FilePath = filePath;
            _dom = CQ.CreateDocumentFromFile(FilePath.ToString());
        }

        public string Create()
        {
            //var dom = CQ.CreateFromUrl(Url);
            //var dom = CQ.CreateDocumentFromFile(@"..\TableToJson\TestTable.html");
            var tables = _dom["table"];
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