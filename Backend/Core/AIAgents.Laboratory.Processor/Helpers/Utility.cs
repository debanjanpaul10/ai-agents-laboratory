using DocumentFormat.OpenXml.Packaging;
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
        if (cell.DataType is not null && cell.DataType.Value == CellValues.SharedString && sharedStringTable is not null && int.TryParse(value, out var index)
            && index >= 0 && index < sharedStringTable.ChildElements.Count)
        {
            var item = sharedStringTable.ChildElements[index] as SharedStringItem;
            return item?.InnerText ?? string.Empty;
        }

        return value;
    }

    /// <summary>
    /// Prepares the excel data from workbook.
    /// </summary>
    /// <param name="workbookPart">The workbook part.</param>
    /// <returns>The string data from the excel workbook.</returns>
    internal static string PrepareExcelData(WorkbookPart workbookPart)
    {
        ArgumentNullException.ThrowIfNull(workbookPart.Workbook);
        ArgumentNullException.ThrowIfNull(workbookPart.Workbook.Sheets);

        var sharedStringTable = workbookPart.SharedStringTablePart?.SharedStringTable;
        var stringBuilder = new System.Text.StringBuilder();

        foreach (Sheet sheet in workbookPart.Workbook.Sheets.OfType<Sheet>())
        {
            // Add sheet name as a simple header separator
            var sheetName = sheet.Name?.Value;
            if (!string.IsNullOrWhiteSpace(sheetName))
            {
                stringBuilder.AppendLine(sheetName);
                stringBuilder.AppendLine(new string('-', sheetName.Length));
            }

            var worksheetPart = workbookPart.GetPartById(sheet.Id!) as WorksheetPart;
            var rows = worksheetPart?.Worksheet?.GetFirstChild<SheetData>()?.Elements<Row>();
            if (rows is null)
            {
                stringBuilder.AppendLine();
                continue;
            }

            foreach (var row in rows)
            {
                var cellValues = new List<string>();
                foreach (var cell in row.Elements<Cell>())
                    cellValues.Add(Utility.GetCellText(cell, sharedStringTable));

                // Join cell values with tabs to preserve some structure
                if (cellValues.Count > 0)
                    stringBuilder.AppendLine(string.Join('\t', cellValues));
            }

            stringBuilder.AppendLine();
        }

        return stringBuilder.ToString();
    }
}
