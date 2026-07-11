using AIAgents.Laboratory.Domain.Contracts;
using AIAgents.Laboratory.Domain.Helpers;
using AIAgents.Laboratory.Domain.Models.Agents;
using AIAgents.Laboratory.Infrastructure.AgentsFramework.Contracts;
using AIAgents.Laboratory.Infrastructure.AgentsFramework.Helpers;
using DocumentFormat.OpenXml.Packaging;
using Microsoft.Extensions.Logging;
using static AIAgents.Laboratory.Infrastructure.AgentsFramework.Helpers.Constants;

namespace AIAgents.Laboratory.Infrastructure.AgentsFramework.AgentServices.FileReaders;

/// <summary>
/// Provides functionality to read the content of spreadsheet files, including Excel files with extensions such as .xls, .xlsx, and .xlsm.
/// </summary>
/// <remarks>This class uses the Open XML SDK to extract text from spreadsheet documents, handling multiple sheets and ensuring that the extracted text is properly formatted.</remarks>
/// <param name="logger">The logger service.</param>
/// <param name="correlationContext">The correlation context for logging and exception handling.</param>
/// <seealso cref="IFileContentReader"/>
internal sealed class SpreadsheetFileContentReader(
    ILogger<SpreadsheetFileContentReader> logger,
    ICorrelationContext correlationContext) : IFileContentReader
{
    /// <summary>
    /// The file extensions supported by this reader. In this case, it supports Excel files with the extensions ".xls", ".xlsx", and ".xlsm".
    /// </summary>
    public IReadOnlyCollection<string> SupportedExtensions { get; } = KnowledgeBaseConstants.FileContentTypes.ExcelFiles.Split(KnowledgeBaseConstants.CommaSeparator);

    /// <inheritdoc />
    public string Read(
        KnowledgeBaseDocumentDomain knowledgeBaseDocument
    )
    {
        ArgumentNullException.ThrowIfNull(knowledgeBaseDocument);
        ArgumentNullException.ThrowIfNull(knowledgeBaseDocument.FileContent);

        try
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodStart,
                nameof(Read), DateTime.UtcNow, knowledgeBaseDocument.FileName
            );

            if (knowledgeBaseDocument.FileContent.Length == 0)
                return string.Empty;

            using var memoryStream = new MemoryStream(knowledgeBaseDocument.FileContent);
            using var spreadsheetDocument = SpreadsheetDocument.Open(memoryStream, false);

            var workbookPart = spreadsheetDocument.WorkbookPart;
            if (workbookPart?.Workbook?.Sheets is null)
                return string.Empty;

            return Utilities.PrepareExcelData(workbookPart);
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex,
                LoggingConstants.LogHelperMethodFailed,
                nameof(Read), DateTime.UtcNow, ex.Message
            );
            throw new AIAgentsBusinessException(
                message: ex.Message,
                correlationId: correlationContext.CorrelationId
            );
        }
        finally
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodEnd,
                nameof(Read), DateTime.UtcNow, knowledgeBaseDocument.FileName
            );
        }
    }
}
