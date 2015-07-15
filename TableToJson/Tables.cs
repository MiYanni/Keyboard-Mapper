using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Script.Serialization;
using CsQuery;
using TableType = System.Collections.Generic.IEnumerable<System.Collections.Generic.IDictionary<string, object>>;
using TablesType = System.Collections.Generic.IEnumerable<System.Collections.Generic.IEnumerable<System.Collections.Generic.IDictionary<string, object>>>;

namespace TableToJson
{
    public class Tables : TablesType
    {
        public string Url { get; private set; }

        public DirectoryInfo FilePath { get; private set; }

        private readonly CQ _dom;

        private readonly TablesType _tables;

        public Tables(string url)
            : this(CQ.CreateFromUrl(url))
        {
            Url = url;
        }

        public Tables(DirectoryInfo filePath)
            : this(CQ.CreateDocumentFromFile(filePath.ToString()))
        {
            FilePath = filePath;
        }

        private Tables(CQ dom)
        {
            _dom = dom;
            var converter = new HtmlTableToJson(_dom["table"], new TableOptions
            {
                // http://api.jquery.com/text/
                GlobalTextExtractor = (i, c) => c.Text()
            });
            _tables = converter.Generate();
        }

        public string ToJson()
        {
            var serializer = new JavaScriptSerializer();
            //return serializer.Serialize(new
            //{
            //    tables = _tables.Select((t, i) => new { index = i, table = t }).ToDictionary(x => "table" + (x.index + 1), x => x.table)
            //});

            // TEMPORARY FOR VALVE
            var tableTitles = _dom["table"].Siblings("h2").Select(h => h.Cq().Text()).ToList();
            tableTitles.RemoveAt(0);

            return serializer.Serialize(new
            {
                tables = _tables.Zip(tableTitles, (t, tt) => new {Title = tt, Table = t}).ToDictionary(x => x.Title, x => x.Table)
            });
        }

        //public string Create()
        //{
        //    //var dom = CQ.CreateFromUrl(Url);
        //    //var dom = CQ.CreateDocumentFromFile(@"..\TableToJson\TestTable.html");
        //    //var tables = _dom["table"];
        //    //var result = String.Empty;
        //    //tables.Each(table =>
        //    //{
        //    //    var converter = new HtmlTableToJson(table);
        //    //    result += converter.ToJson();
        //    //});
        //    return "";
        //}

        public IEnumerator<TableType> GetEnumerator()
        {
            return _tables.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}