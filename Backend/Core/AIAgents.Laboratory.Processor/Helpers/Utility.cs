using DocumentFormat.OpenXml.Spreadsheet;

namespace AIAgents.Laboratory.Processor.Helpers;

/// <summary>
/// The utility class.
/// </summary>
internal static class Utility
{
    /// <summary>
    /// Gets the text value of a cell, resolving shared strings if necessary.
    /// </summary>
    /// <param name="cell">The spreadsheet cell.</param>
    /// <param name="sharedStringTable">The shared string table.</param>
    /// <returns>The cell value.</returns>
    internal static string GetCellText(Cell cell, SharedStringTable? sharedStringTable)
    {
        if (cell is null) return string.Empty;

        var value = cell.CellValue?.InnerText;
        if (string.IsNullOrEmpty(value)) return string.Empty;

        // If the cell uses shared strings, resolve them
        if (cell.DataType is not null && cell.DataType.Value == CellValues.SharedString && sharedStringTable != null)
        {
            if (int.TryParse(value, out var index) && index >= 0 && index < sharedStringTable.ChildElements.Count)
            {
                var item = sharedStringTable.ChildElements[index] as SharedStringItem;
                return item?.InnerText ?? string.Empty;
            }
        }

        return value;
    }
}
