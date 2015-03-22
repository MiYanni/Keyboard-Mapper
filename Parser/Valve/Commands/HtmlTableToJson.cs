using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using CsQuery;

namespace Parser.Valve.Commands
{
    internal sealed class HtmlTableToJson
    {
        public IDomObject Table { get; private set; }
        public TableOptions Options { get; private set; }

        public HtmlTableToJson(IDomObject table, TableOptions options = null)
        {
            Table = table;
            Options = options ?? new TableOptions();
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

                if(!(useGlobal || useCell)) return null;

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
            if (Options.IncludeRowId && row.Attr("id") == null) {
                yield return Options.RowIdHeader;
            }
            foreach (var value in row.Children("td,th").Elements
                .Select((cell, cellIndex) => CellValues(cellIndex, cell, isHeader)))
            {
                yield return value;
            }
        }
        
        private IEnumerable<string> GetHeadings()
        {
            var firstRow = Table.Cq().Find("tr:first").First();
            if (Options.Headings.Any())
            {
                return Options.Headings;
            }
            else
            {
                return RowValues(firstRow, true);
            }
            //return Options.Headings.Any() ? 
            //    Options.Headings : 
            //    RowValues(firstRow, true);
        }

        private bool IgnoredColumn(int index)
        {
            if(Options.OnlyColumns.Any()) 
            {
                return !Options.OnlyColumns.Contains(index);
            }
            return Options.IgnoreColumns.Contains(index);
        }

        private IEnumerable<KeyValuePair<string, object>> ArraysToHash(IEnumerable<string> keys, IEnumerable<string> values)
        {
            var index = 0;
            
            foreach(var value in values)
            {
                // When ignoring columns, the header option still starts with the first defined column.
                if (index < keys.Count() && value != null) 
                {
                    yield return new KeyValuePair<string, object>(keys.ElementAt(index), value);
                    index++;
                }
            }
        }

        private Dictionary<int, Dictionary<int, string>> ProcessRow(int rowIndex, IDomObject row, Dictionary<int, Dictionary<int, string>> temp)
        {
            if (rowIndex > 0 || Options.Headings.Any())
            {
                var rowCq = row.Cq();

                var isEmpty = rowCq.Find("td").Length == rowCq.Find("td:empty").Length;

                if ((rowCq.Is(":visible") || !Options.IgnoreHiddenRows) &&
                    (!isEmpty || !Options.IgnoreEmptyRows) &&
                    !Convert.ToBoolean(rowCq.Data("ignore")))
                {
                    var cellIndex = 0;
                    if (!temp.ContainsKey(rowIndex))
                    {
                        temp[rowIndex] = new Dictionary<int, string>();
                    }
                    if (Options.IncludeRowId)
                    {
                        cellIndex = cellIndex + 1;
                        temp[rowIndex][cellIndex] = rowCq.Attr("id") ?? String.Empty;
                    }

                    rowCq.Children().Each(cell =>
                    {
                        var cellCq = cell.Cq();

                        // Process rowspans
                        var length = 0;
                        string text;
                        if (cellCq.Filter("[rowspan]").Length > 0)
                        {
                            length = Convert.ToInt32(cellCq.Attr("rowspan"), 10) - 1;
                            text = CellValues(cellIndex, cell);
                            for (var i = 1; i <= length; i++)
                            {
                                if (temp[rowIndex + i] == null)
                                {
                                    temp[rowIndex + i] = new Dictionary<int, string>();
                                }
                                temp[rowIndex + i][cellIndex] = text;
                            }
                        }

                        // Process colspans
                        if (cellCq.Filter("[colspan]").Length > 0)
                        {
                            length = Convert.ToInt32(cellCq.Attr("colspan"), 10) - 1;
                            text = CellValues(cellIndex, cell);
                            for (var i = 1; i <= length; i++)
                            {
                                // Cell has both col and row spans.
                                if (cellCq.Filter("[rowspan]").Length > 0)
                                {
                                    var length2 = Convert.ToInt32(cellCq.Attr("rowspan"), 10);
                                    for (var j = 0; j < length2; j++)
                                    {
                                        temp[rowIndex + j][cellIndex + i] = text;
                                    }
                                }
                                else
                                {
                                    temp[rowIndex][cellIndex + i] = text;
                                }
                            }
                        }

                        // Skip column if already defined
                        //while (temp[rowIndex][cellIndex] != null)
                        //while (cellIndex < temp[rowIndex].Count && temp[rowIndex][cellIndex] != null)
                        while (temp[rowIndex].ContainsKey(cellIndex) && temp[rowIndex][cellIndex] != null)
                        {
                            cellIndex++;
                        }

                        //text = temp[rowIndex][cellIndex] ?? CellValues(cellIndex, cell);
                        //text = ((cellIndex < temp[rowIndex].Count) ? temp[rowIndex][cellIndex] : null) ?? CellValues(cellIndex, cell);
                        text = (temp[rowIndex].ContainsKey(cellIndex) ? temp[rowIndex][cellIndex] : null) ?? CellValues(cellIndex, cell);
                        if (text != null)
                        {
                            temp[rowIndex][cellIndex] = text;
                        }
                        cellIndex++;
                    });
                }
            }
            return temp;
        }

        private string Construct(IEnumerable<string> headings)
        {
            var temp = new Dictionary<int, Dictionary<int, string>>();
            Table.Cq().Children("tbody,*").Children("tr").Each((rowIndex, row) =>
            {
                temp = ProcessRow(rowIndex, row, temp);
            });

            var result = new List<IDictionary<string, object>>();
            foreach (var row in temp.OrderBy(r => r.Key).Select(r => r.Value.ToDictionary(x => x.Key, x => x.Value)))
            {
                if (row != null)
                {
                    // Remove ignoredColumns / add onlyColumns
                    var newRow = (Options.OnlyColumns.Any() || Options.IgnoreColumns.Any())
                        ? row.Where((v, index) => !IgnoredColumn(index))
                        : row;

                    // Remove ignoredColumns / add onlyColumns if headings is not defined.
                    var newHeadings = Options.Headings.Any() ? headings :
                        headings.Where((v, index) => !IgnoredColumn(index));

                    result.Add(ArraysToHash(newHeadings, newRow.Select(r => r.Value)).ToDictionary(x => x.Key, x => x.Value));
                }
            }

            var resultString = "";
            var serializer = new JavaScriptSerializer();
            resultString += serializer.Serialize(result);
            //foreach (var row in result)
            //{
            //    resultString += serializer.Serialize(row);
            //}
            return resultString;
        }


        public string ToJson()
        {
            var headings = GetHeadings();
            return Construct(headings);
        }
    }
    internal sealed class TableOptions
    {
        /// <summary>
        /// Array of column indexes to ignore.
        /// Type: Array
        /// Default: []
        /// </summary>
        public IEnumerable<int> IgnoreColumns { get; set; }

        /// <summary>
        /// Array of column indexes to include, all other columns are ignored. This takes presidence over ignoreColumns when provided.
        /// This takes presidence over ignoreColumns when both are provided.
        /// Type: Array
        /// Default: null - all columns
        /// </summary>
        public IEnumerable<int> OnlyColumns { get; set; }

        /// <summary>
        /// Boolean if hidden rows should be ignored or not.
        /// Type: Boolean
        /// Default: true
        /// </summary>
        public bool IgnoreHiddenRows { get; set; }

        /// <summary>
        /// Boolean if empty rows should be ignored or not.
        /// Type: Boolean
        /// Default: true
        /// </summary>
        public bool IgnoreEmptyRows { get; set; }

        /// <summary>
        /// Array of column headings to use. When supplied, all table rows are treated as values.
        /// Type: Array
        /// Default: null
        /// </summary>
        public IEnumerable<string> Headings { get; set; }

        /// <summary>
        /// Boolean if HTML tags in table cells should be preserved.
        /// Type: Boolean
        /// Default: false
        /// </summary>
        public bool AllowHtml { get; set; }

        /// <summary>
        /// Determines if the id attribute of each &lt;tr&gt; element is included in the JSON.
        /// Type: Boolean
        /// Default: false
        /// </summary>
        public bool IncludeRowId { get; set; }

        /// <summary>
        /// The header for the id attribute. Only used if includeRowId is true.
        /// Type: String
        /// Default: rowId
        /// </summary>
        public string RowIdHeader { get; set; }

        /// <summary>
        /// The cell attribute name which contains the text data override value.
        /// If this attribute exists, the value of this attribute it used instead of the cell's actual value.
        /// Type: String
        /// Default: data-override
        /// </summary>
        public string TextDataOverride { get; set; }

        /// <summary>
        /// A function to process a cell's value.
        /// If the cell's data override exists, then that value is used as the cell's value.
        /// If the global extractor is null, then the cell extractor is used.
        /// If the cell extractor doesn't exist for the cell index, then the normal value of the cell is used. 
        /// Type: Function
        /// Default: null
        /// </summary>
        public Extractor GlobalTextExtractor { get; set; }

        /// <summary>
        /// A function to process a cell's value.
        /// If the cell's data override exists, then that value is used as the cell's value.
        /// If the global extractor is null, then the cell extractor is used.
        /// If the cell extractor doesn't exist for the cell index, then the normal value of the cell is used. 
        /// Type: Function
        /// Default: null
        /// </summary>
        public IDictionary<int, Extractor> CellTextExtractor { get; set; }

        public TableOptions()
        {
            IgnoreColumns = new List<int>();
            OnlyColumns = new List<int>();
            IgnoreHiddenRows = true;
            IgnoreEmptyRows = false;
            Headings = new List<string>();
            AllowHtml = false;
            IncludeRowId = false;
            RowIdHeader = "rowId";
            TextDataOverride = "data-override";
            GlobalTextExtractor = null;
            CellTextExtractor = new Dictionary<int, Extractor>();
        }
    }

    /// <summary>
    /// A method for extracting a cell's value instead of using the value of the cell directly.
    /// </summary>
    /// <param name="cellIndex">The index of the cell in row.</param>
    /// <param name="cellCq">The CQ of the cell.</param>
    /// <returns>The value of the cell as determined by the extractor.</returns>
    public delegate string Extractor(int cellIndex, CQ cellCq);
}