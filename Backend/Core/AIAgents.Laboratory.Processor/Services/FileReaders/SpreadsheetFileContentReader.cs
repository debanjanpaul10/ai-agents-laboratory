using AIAgents.Laboratory.Processor.Contracts;
using AIAgents.Laboratory.Processor.Helpers;
using AIAgents.Laboratory.Processor.Models;
using DocumentFormat.OpenXml.Packaging;
using Microsoft.Extensions.Logging;
using static AIAgents.Laboratory.Processor.Helpers.ProcessorConstants;

namespace AIAgents.Laboratory.Processor.Services.FileReaders;

/// <summary>
/// Provides functionality to read the content of spreadsheet files, including Excel files with extensions such as .xls, .xlsx, and .xlsm.
/// </summary>
/// <remarks>This class uses the Open XML SDK to extract text from spreadsheet documents, handling multiple sheets and ensuring that the extracted text is properly formatted.</remarks>
/// <param name="logger">The logger service.</param>
/// <seealso cref="IFileContentReader"/>
internal sealed class SpreadsheetFileContentReader(ILogger<SpreadsheetFileContentReader> logger) : IFileContentReader
{
    /// <summary>
    /// The file extensions supported by this reader. In this case, it supports Excel files with the extensions ".xls", ".xlsx", and ".xlsm".
    /// </summary>
    public IReadOnlyCollection<string> SupportedExtensions { get; } = KnowledgeBaseConstants.FileContentTypes.ExcelFiles.Split(KnowledgeBaseConstants.CommaSeparator);

    /// <summary>
    /// Reads the content of a spreadsheet file and returns it as a string. The method takes a <see cref="KnowledgeBaseDocumentDomain"/> object as input, which contains the file content in byte array format. 
    /// </summary>
    /// <remarks>It uses the Open XML SDK to open the spreadsheet document, extract text from all sheets, and concatenate it into a single string. If the file content is empty, it returns an empty string. 
    /// The method also includes error handling to log any exceptions that occur during the reading process and rethrows them as <see cref="AIAgentsException"/>.</remarks>
    /// <param name="knowledgeBaseDocument">The object containing the PDF file content to be read.</param>
    /// <returns>The string content of the file.</returns>
    public string Read(KnowledgeBaseDocumentDomain knowledgeBaseDocument)
    {
        ArgumentNullException.ThrowIfNull(knowledgeBaseDocument);
        ArgumentNullException.ThrowIfNull(knowledgeBaseDocument.FileContent);

        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(Read), DateTime.UtcNow, knowledgeBaseDocument.FileName);

            if (knowledgeBaseDocument.FileContent.Length == 0)
                return string.Empty;

            using var memoryStream = new MemoryStream(knowledgeBaseDocument.FileContent);
            using var spreadsheetDocument = SpreadsheetDocument.Open(memoryStream, false);

            var workbookPart = spreadsheetDocument.WorkbookPart;
            if (workbookPart?.Workbook?.Sheets is null)
                return string.Empty;

            return Utility.PrepareExcelData(workbookPart);
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(Read), DateTime.UtcNow, ex.Message);
            throw new AIAgentsException(ex);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(Read), DateTime.UtcNow, knowledgeBaseDocument.FileName);
        }
    }
}
