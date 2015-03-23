using CsQuery;

namespace TableToJson
{
    /// <summary>
    /// A method for extracting a cell's value instead of using the value of the cell directly.
    /// </summary>
    /// <param name="cellIndex">The index of the cell in row.</param>
    /// <param name="cellCq">The CQ of the cell.</param>
    /// <returns>The value of the cell as determined by the extractor.</returns>
    public delegate string Extractor(int cellIndex, CQ cellCq);
}