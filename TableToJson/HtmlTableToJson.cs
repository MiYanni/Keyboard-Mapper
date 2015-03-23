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

        private Dictionary<int, IDictionary<int, string>> ProcessRow(int rowIndex, CQ rowCq, Dictionary<int, IDictionary<int, string>> temp)
        {
            var cellIndex = 0;
            if (!temp.ContainsKey(rowIndex))
            {
                temp[rowIndex] = new Dictionary<int, string>();
            }
            if (Options.IncludeRowId)
            {
                cellIndex++;
                temp[rowIndex][cellIndex] = rowCq.Attr("id") ?? String.Empty;
            }

            rowCq.Children().Each(cell =>
            {
                var cellCq = cell.Cq();
                if (cellCq.Filter("[rowspan]").Length > 0)
                {
                    var length = Convert.ToInt32(cellCq.Attr("rowspan"), 10) - 1;
                    cellCq.RemoveAttr("rowspan");

                    var currentRow = rowCq;
                    foreach (var i in Enumerable.Range(0, length))
                    {
                        currentRow = currentRow.Next("tr");
                        currentRow.Children().Eq(cellIndex - 1).After(cell);
                    }
                }

                // Process rowspans
                //if (cellCq.Filter("[rowspan]").Length > 0)
                //{
                //    var length = Convert.ToInt32(cellCq.Attr("rowspan"), 10) - 1;
                //    var text = CellValues(cellIndex, cell);
                //    for (var i = 1; i <= length; i++)
                //    {
                //        if (!temp.ContainsKey(rowIndex + 1))
                //        {
                //            temp[rowIndex + i] = new Dictionary<int, string>();
                //        }
                //        temp[rowIndex + i][cellIndex] = text;
                //    }
                //}

                //// Process colspans
                //if (cellCq.Filter("[colspan]").Length > 0)
                //{
                //    var length = Convert.ToInt32(cellCq.Attr("colspan"), 10) - 1;
                //    var text = CellValues(cellIndex, cell);
                //    for (var i = 1; i <= length; i++)
                //    {
                //        // Cell has both col and row spans.
                //        if (cellCq.Filter("[rowspan]").Length > 0)
                //        {
                //            var length2 = Convert.ToInt32(cellCq.Attr("rowspan"), 10);
                //            for (var j = 0; j < length2; j++)
                //            {
                //                temp[rowIndex + j][cellIndex + i] = text;
                //            }
                //        }
                //        else
                //        {
                //            temp[rowIndex][cellIndex + i] = text;
                //        }
                //    }
                //}

                // Skip column if already defined.
                //while (temp[rowIndex].ContainsKey(cellIndex) && temp[rowIndex][cellIndex] != null)
                //{
                //    cellIndex++;
                //}

                var value = (temp[rowIndex].ContainsKey(cellIndex) ? temp[rowIndex][cellIndex] : null) ?? CellValues(cellIndex, cell);
                if (value != null)
                {
                    temp[rowIndex][cellIndex] = value;
                }
                cellIndex++;
            });
            return temp;
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
            table.Cq().Children("tbody,*").Children("tr").Elements
            .Select(row => row.Cq())
            .Where(IsValidRow)
            .ForEach((rowCq, rowIndex) =>
            {
                temp = ProcessRow(rowIndex, rowCq, temp);
            });

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