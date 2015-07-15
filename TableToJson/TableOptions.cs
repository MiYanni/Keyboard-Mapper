using System.Collections.Generic;

namespace TableToJson
{
    /// <summary>
    /// The options for deserializing an HTML table.
    /// </summary>
    public sealed class TableOptions
    {
        /// <summary>
        /// Array of column indexes to ignore.
        /// Default: []
        /// </summary>
        public IEnumerable<int> IgnoreColumns { get; set; }

        /// <summary>
        /// Array of column indexes to include, all other columns are ignored.
        /// This takes presidence over ignoreColumns when provided.
        /// This takes presidence over ignoreColumns when both are provided.
        /// Default: null (all columns)
        /// </summary>
        public IEnumerable<int> OnlyColumns { get; set; }

        /// <summary>
        /// Boolean if hidden rows should be ignored or not.
        /// Default: true
        /// </summary>
        public bool IgnoreHiddenRows { get; set; }

        /// <summary>
        /// Boolean if empty rows should be ignored or not.
        /// Default: true
        /// </summary>
        public bool IgnoreEmptyRows { get; set; }

        /// <summary>
        /// Array of column headings to use.
        /// When supplied, all table rows are treated as values.
        /// Default: null
        /// </summary>
        public IEnumerable<string> Headings { get; set; }

        /// <summary>
        /// Boolean if HTML tags in table cells should be preserved.
        /// Default: false
        /// </summary>
        public bool AllowHtml { get; set; }

        /// <summary>
        /// Determines if the id attribute of each &lt;tr&gt; element is included in the JSON.
        /// Default: false
        /// </summary>
        public bool IncludeRowId { get; set; }

        /// <summary>
        /// The header for the id attribute. Only used if includeRowId is true.
        /// Default: rowId
        /// </summary>
        public string RowIdHeader { get; set; }

        /// <summary>
        /// The cell attribute name which contains the text data override value.
        /// If this attribute exists, the value of this attribute it used instead of the cell's actual value.
        /// Default: data-override
        /// </summary>
        public string TextDataOverride { get; set; }

        /// <summary>
        /// A function to process a cell's value.
        /// If the cell's data override exists, then that value is used as the cell's value.
        /// If the global extractor is null, then the cell extractor is used.
        /// If the cell extractor doesn't exist for the cell index, then the normal value of the cell is used.
        /// Default: null
        /// </summary>
        public Extractor GlobalTextExtractor { get; set; }

        /// <summary>
        /// A function to process a cell's value.
        /// If the cell's data override exists, then that value is used as the cell's value.
        /// If the global extractor is null, then the cell extractor is used.
        /// If the cell extractor doesn't exist for the cell index, then the normal value of the cell is used.
        /// Default: null
        /// </summary>
        public IDictionary<int, Extractor> CellTextExtractor { get; set; }

        /// <summary>
        /// Creates the table options for deserializing an HTML table.
        /// </summary>
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
            GlobalTextExtractor = (i, c) =>
            {
                //c.Children("p,pre,a").Contents().Unwrap();
                // http://api.jquery.com/text/
                return c.Text();
            };
            CellTextExtractor = new Dictionary<int, Extractor>();
        }
    }
}