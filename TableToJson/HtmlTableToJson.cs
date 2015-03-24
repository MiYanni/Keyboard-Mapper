using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using CsQuery;
using CsQuery.ExtensionMethods;

namespace TableToJson
{
    internal sealed class HtmlTableToJson
    {
        public IEnumerable<IDomObject> Tables { get; private set; }

        public TableOptions Options { get; private set; }

        public HtmlTableToJson(IEnumerable<IDomObject> tables, TableOptions options = null)
        {
            Tables = tables;
            Options = options ?? new TableOptions();
        }

        public HtmlTableToJson(IDomObject table, TableOptions options = null)
            : this(new List<IDomObject> { table }, options)
        {
        }

        private string CellValues(int cellIndex, IDomObject cell, bool isHeader = false)
        {
            var cellCq = cell.Cq();
            Func<string> getExtractorValue = () =>
            {
                // Don't use extractor for header cells.
                if (isHeader) return null;

                var useGlobal = Options.GlobalTextExtractor != null;
                var useCell = Options.CellTextExtractor.ContainsKey(cellIndex);

                if (!(useGlobal || useCell)) return null;

                return useGlobal
                    ? Options.GlobalTextExtractor(cellIndex, cellCq)
                    : Options.CellTextExtractor[cellIndex](cellIndex, cellCq);
            };
            Func<string> getDefaultValue = () =>
                (Options.AllowHtml ? cellCq.Html() : (cell.InnerText ?? cellCq.Text())) ?? String.Empty;

            var cellOverrideValue = cellCq.Attr(Options.TextDataOverride);
            return (cellOverrideValue ?? getExtractorValue() ?? getDefaultValue()).Trim();
        }

        private IEnumerable<string> RowValues(CQ row, bool isHeader = false)
        {
            if (Options.IncludeRowId && row.Attr("id") == null)
            {
                yield return Options.RowIdHeader;
            }
            foreach (var value in row.Children("td,th").Elements
                .Select((cell, cellIndex) => CellValues(cellIndex, cell, isHeader)))
            {
                yield return value;
            }
        }

        private IEnumerable<string> GetHeadings(IDomObject table)
        {
            var firstRow = table.Cq().Find("tr:first").First();
            return Options.Headings.Any() ?
                Options.Headings :
                RowValues(firstRow, true);
        }

        private bool IgnoredColumn(int index)
        {
            if (Options.OnlyColumns.Any())
            {
                return !Options.OnlyColumns.Contains(index);
            }
            return Options.IgnoreColumns.Contains(index);
        }

        private IEnumerable<KeyValuePair<string, object>> ArraysToHash(IEnumerable<string> keys, IEnumerable<string> values)
        {
            var index = 0;

            foreach (var value in values)
            {
                // When ignoring columns, the header option still starts with the first defined column.
                if (index < keys.Count() && value != null)
                {
                    yield return new KeyValuePair<string, object>(keys.ElementAt(index), value);
                    index++;
                }
            }
        }

        private static void InsertAtIndex(CQ collection, int index, IEnumerable<IDomObject> newElement)
        {
            if (index == 0)
            {
                collection.Prepend(newElement);
                return;
            }
            collection.Children().Eq(index - 1).After(newElement);
        }

        private static int SpanAdditionalCount(CQ cq, string span)
        {
            return Convert.ToInt32(cq.Attr(span), 10) - 1;
        }

        private static void FlattenSpans(CQ rowCq, string span, Action<int, int, CQ> process)
        {
            rowCq.Children().Elements
                .Select((c, ci) => new { CellCq = c.Cq(), ColumnIndex = ci })
                .Where(cc => cc.CellCq.Attr(span) != null)
                .ForEach(cc =>
                {
                    var count = SpanAdditionalCount(cc.CellCq, span);
                    cc.CellCq.RemoveAttr(span);

                    process(count, cc.ColumnIndex, cc.CellCq);
                });
        }

        private static void FlattenAllSpans(CQ rowCq)
        {
            FlattenSpans(rowCq, "colspan", (c, ci, cc) =>
            {
                for (var i = 0; i < c; ++i)
                {
                    cc.After(CQ.Create(cc));
                }
            });

            FlattenSpans(rowCq, "rowspan", (c, ci, cc) =>
            {
                var currentRow = rowCq;
                for (var i = 0; i < c; ++i)
                {
                    currentRow = currentRow.Next("tr");
                    InsertAtIndex(currentRow, ci, CQ.Create(cc));
                }
            });
        }

        private Dictionary<int, string> ProcessRow(CQ rowCq)
        {
            var result = new Dictionary<int, string>();
            var indexShift = 0;
            if (Options.IncludeRowId)
            {
                result[0] = rowCq.Attr("id") ?? String.Empty;
                indexShift++;
            }

            rowCq.Children().Each((index, cell) =>
            {
                var adjustedIndex = index + indexShift;
                var value = CellValues(adjustedIndex, cell);
                if (value != null)
                {
                    result[adjustedIndex] = value;
                }
            });
            return result;
        }

        private bool IsValidRow(CQ rowCq, int rowIndex)
        {
            var isEmpty = rowCq.Find("td").Length == rowCq.Find("td:empty").Length;
            return (rowIndex > 0 || Options.Headings.Any()) &&
                ((rowCq.Is(":visible") || !Options.IgnoreHiddenRows) &&
                (!isEmpty || !Options.IgnoreEmptyRows) &&
                !Convert.ToBoolean(rowCq.Data("ignore")));
        }

        private IEnumerable<IDictionary<string, object>> Construct(IEnumerable<string> headings, IDomObject table)
        {
            var temp = new Dictionary<int, IDictionary<int, string>>();
            Console.WriteLine(table.Cq().Html());
            table.Cq().Children("tbody,*").Children("tr").Elements
                .Select(row => row.Cq())
                .Where(IsValidRow)
                .ForEach((rowCq, rowIndex) =>
                {
                    FlattenAllSpans(rowCq);
                    temp[rowIndex] = ProcessRow(rowCq);
                });
            Console.WriteLine(table.Cq().Html());
            temp.ForEach(a => a.Value.ForEach(b => Console.WriteLine(b.Key + " " + b.Value)));

            var result = new List<IDictionary<string, object>>();
            foreach (var row in temp.OrderBy(r => r.Key).Select(r => r.Value.ToDictionary(x => x.Key, x => x.Value)))
            {
                // Remove ignoredColumns / add onlyColumns
                var newRow = (Options.OnlyColumns.Any() || Options.IgnoreColumns.Any())
                    ? row.Where((v, index) => !IgnoredColumn(index))
                    : row;

                // Remove ignoredColumns / add onlyColumns if headings is not defined.
                var newHeadings = Options.Headings.Any() ? headings :
                    headings.Where((v, index) => !IgnoredColumn(index));

                result.Add(ArraysToHash(newHeadings, newRow.OrderBy(r => r.Key).Select(r => r.Value)).ToDictionary(x => x.Key, x => x.Value));
            }
            return result;
        }

        public string ToJson()
        {
            var serializer = new JavaScriptSerializer();
            return serializer.Serialize(new { tables = Tables.Select(t => Construct(GetHeadings(t), t)).Select((t, i) => new { index = i, table = t }).ToDictionary(x => "table" + (x.index + 1), x => x.table) });
        }
    }
}