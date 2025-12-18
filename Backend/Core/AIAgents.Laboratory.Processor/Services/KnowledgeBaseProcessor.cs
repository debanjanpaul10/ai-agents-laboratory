using System.Globalization;
using AIAgents.Laboratory.Processor.Contracts;
using AIAgents.Laboratory.Processor.Helpers;
using AIAgents.Laboratory.Processor.Models;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel.Memory;
using Microsoft.SemanticKernel.Text;
using static AIAgents.Laboratory.Processor.Helpers.ProcessorConstants;

namespace AIAgents.Laboratory.Processor.Services;

/// <summary>
/// The knowledge base processor class.
/// </summary>
/// <param name="memoryStore">The memory store service.</param>
/// <param name="embeddingGeneratorService">The embedding generation service.</param>
/// <param name="logger">The logger service.</param>
/// <seealso cref="IKnowledgeBaseProcessor"/>
#pragma warning disable SKEXP0001
#pragma warning disable SKEXP0050
public class KnowledgeBaseProcessor(IMemoryStore memoryStore, IEmbeddingGenerator<string, Embedding<float>> embeddingGeneratorService, ILogger<KnowledgeBaseProcessor> logger) : IKnowledgeBaseProcessor
{
    /// <summary>
    /// Detects and reads the file content for the knowledgebase.
    /// </summary>
    /// <param name="knowledgeBaseDocument">The knowledge base document domain model.</param>
    /// <returns>The file content string.</returns>
    public string DetectAndReadFileContent(KnowledgeBaseDocumentDomain knowledgeBaseDocument)
    {
        ArgumentNullException.ThrowIfNull(knowledgeBaseDocument);

        try
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(GetRelevantKnowledgeAsync), DateTime.UtcNow, knowledgeBaseDocument.FileName);

            var fileType = Path.GetExtension(knowledgeBaseDocument.FileName);
            if (string.Equals(KnowledgeBaseConstants.FileContentTypes.PlainTextFiles, fileType, StringComparison.OrdinalIgnoreCase))
                return this.ReadTextFileData(knowledgeBaseDocument);
            else if (string.Equals(KnowledgeBaseConstants.FileContentTypes.PdfFiles, fileType, StringComparison.OrdinalIgnoreCase))
                return this.ReadPdfFileData(knowledgeBaseDocument);
            else if (KnowledgeBaseConstants.FileContentTypes.ExcelFiles.Split(KnowledgeBaseConstants.CommaSeparator).Contains(fileType))
                return this.ReadSpreadsheetData(knowledgeBaseDocument);
            else if (KnowledgeBaseConstants.FileContentTypes.WordFiles.Split(KnowledgeBaseConstants.CommaSeparator).Contains(fileType))
                return this.ReadWordFileData(knowledgeBaseDocument);
            else
                throw new Exception(ExceptionConstants.UnsupportedFileTypeMessage);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, LoggingConstants.LogHelperMethodFailed, nameof(GetRelevantKnowledgeAsync), DateTime.UtcNow, ex.Message);
            throw;
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetRelevantKnowledgeAsync), DateTime.UtcNow, knowledgeBaseDocument.FileName);
        }
    }

    /// <summary>
    /// Gets the relevant knowledge asynchronous.
    /// </summary>
    /// <param name="query">The query.</param>
    /// <param name="agentId">The agent identifier.</param>
    /// <returns>
    /// The knowledge base data.
    /// </returns>
    public async Task<string> GetRelevantKnowledgeAsync(string query, string agentId)
    {
        ArgumentException.ThrowIfNullOrEmpty(query);
        ArgumentException.ThrowIfNullOrEmpty(agentId);

        try
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(GetRelevantKnowledgeAsync), DateTime.UtcNow, agentId);

            var queryEmbeddingResult = await embeddingGeneratorService.GenerateAsync(query).ConfigureAwait(false);
            var queryEmbedding = queryEmbeddingResult.Vector;
            var relevantChunks = new List<(MemoryRecord Record, double Score)>();

            await foreach (var chunk in memoryStore.GetNearestMatchesAsync(agentId, queryEmbedding, 5))
                relevantChunks.Add(chunk);

            if (relevantChunks.Count == 0) return string.Empty;

            return string.Join("\n\n", relevantChunks.Where(c => !string.IsNullOrEmpty(c.Record.Metadata.Text)).Select(c => c.Record.Metadata.Text));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(GetRelevantKnowledgeAsync), DateTime.UtcNow, ex.Message));
            throw;
        }
        finally
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodEnd, nameof(GetRelevantKnowledgeAsync), DateTime.UtcNow, agentId));
        }
    }

    /// <summary>
    /// Processes the knowledge base document asynchronous.
    /// </summary>
    /// <param name="content">The content.</param>
    /// <param name="agentId">The agent identifier.</param>
    public async Task ProcessKnowledgeBaseDocumentAsync(string content, string agentId)
    {
        ArgumentException.ThrowIfNullOrEmpty(content);
        ArgumentException.ThrowIfNullOrEmpty(agentId);

        try
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodStart, nameof(ProcessKnowledgeBaseDocumentAsync), DateTime.UtcNow, agentId));

            // Ensure the collection exists before upserting records
            var collections = new List<string>();
            await foreach (var collection in memoryStore.GetCollectionsAsync())
                collections.Add(collection);

            if (!collections.Contains(agentId))
                await memoryStore.CreateCollectionAsync(agentId).ConfigureAwait(false);

            var chunks = TextChunker.SplitPlainTextLines(content, 512);
            if (chunks.Count == 0)
                throw new InvalidOperationException(ExceptionConstants.NoValidChunksGenerated);

            var embeddingResults = await embeddingGeneratorService.GenerateAsync(chunks).ConfigureAwait(false);
            var embeddings = embeddingResults.Select(e => e.Vector).ToList();
            if (embeddings.Count != chunks.Count)
                throw new InvalidOperationException(ExceptionConstants.NumberOfEmbeddingsMismatch);

            for (int i = 0; i < chunks.Count; i++)
            {
                var chunkDescription = string.Format(KnowledgeBaseConstants.ChunkDescriptionTemplate, i + 1, chunks.Count);
                var metadata = new MemoryRecordMetadata(false, agentId, chunks[i], chunkDescription, string.Empty, string.Empty);
                var record = new MemoryRecord(metadata, embeddings[i], null, null);

                await memoryStore.UpsertAsync(agentId, record).ConfigureAwait(false);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(ProcessKnowledgeBaseDocumentAsync), DateTime.UtcNow, ex.Message));
            throw;
        }
        finally
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodEnd, nameof(ProcessKnowledgeBaseDocumentAsync), DateTime.UtcNow, agentId));
        }
    }

    #region PRIVATE METHODS

    /// <summary>
    /// Reads the spreadsheet file.
    /// </summary>
    /// <param name="knowledgeBaseFile">The knowledge base file.</param>
    /// <returns>The knowledge base document in string.</returns>
    private string ReadSpreadsheetData(KnowledgeBaseDocumentDomain knowledgeBaseFile)
    {
        ArgumentNullException.ThrowIfNull(knowledgeBaseFile);
        try
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(ReadSpreadsheetData), DateTime.UtcNow, knowledgeBaseFile.FileName);

            if (knowledgeBaseFile.FileContent is null || knowledgeBaseFile.FileContent.Length == 0) return string.Empty;

            using var memoryStream = new MemoryStream(knowledgeBaseFile.FileContent);
            using var spreadsheetDocument = SpreadsheetDocument.Open(memoryStream, false);

            var workbookPart = spreadsheetDocument.WorkbookPart;
            if (workbookPart is null || workbookPart.Workbook.Sheets is null) return string.Empty;

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
        catch (Exception ex)
        {
            logger.LogError(ex, LoggingConstants.LogHelperMethodFailed, nameof(ReadSpreadsheetData), DateTime.UtcNow, ex.Message);
            throw;
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(ReadSpreadsheetData), DateTime.UtcNow, knowledgeBaseFile.FileName);
        }
    }

    /// <summary>
    /// Reads the word file.
    /// </summary>
    /// <param name="knowledgeBaseFile">The knowledge base file.</param>
    /// <returns>The knowledge base document in string.</returns>
    private string ReadWordFileData(KnowledgeBaseDocumentDomain knowledgeBaseFile)
    {
        ArgumentNullException.ThrowIfNull(knowledgeBaseFile);

        try
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(ReadWordFileData), DateTime.UtcNow, knowledgeBaseFile.FileName);

            if (knowledgeBaseFile.FileContent is null || knowledgeBaseFile.FileContent.Length == 0) return string.Empty;

            using var memoryStream = new MemoryStream(knowledgeBaseFile.FileContent);
            using var wordDocument = WordprocessingDocument.Open(memoryStream, false);

            var body = wordDocument.MainDocumentPart?.Document?.Body;
            if (body is null) return string.Empty;

            var stringBuilder = new System.Text.StringBuilder();
            foreach (var paragraph in body.Elements<Paragraph>())
            {
                var text = paragraph.InnerText;
                if (!string.IsNullOrWhiteSpace(text)) stringBuilder.AppendLine(text.Trim());
            }

            return stringBuilder.ToString();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, LoggingConstants.LogHelperMethodFailed, nameof(ReadWordFileData), DateTime.UtcNow, ex.Message);
            throw;
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(ReadWordFileData), DateTime.UtcNow, knowledgeBaseFile.FileName);
        }
    }

    /// <summary>
    /// Reads the PDF file data.
    /// </summary>
    /// <param name="knowledgeBaseFile">The knowledge base file.</param>
    /// <returns>The knowledge base document in string.</returns>
    private string ReadPdfFileData(KnowledgeBaseDocumentDomain knowledgeBaseFile)
    {
        ArgumentNullException.ThrowIfNull(knowledgeBaseFile);

        try
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(ReadPdfFileData), DateTime.UtcNow, knowledgeBaseFile.FileName);

            if (knowledgeBaseFile.FileContent is null || knowledgeBaseFile.FileContent.Length == 0) return string.Empty;

            using var memoryStream = new MemoryStream(knowledgeBaseFile.FileContent);
            using var reader = new PdfReader(memoryStream);
            using var pdfDocument = new PdfDocument(reader);

            var stringBuilder = new System.Text.StringBuilder();
            for (int pageNumber = 1; pageNumber <= pdfDocument.GetNumberOfPages(); pageNumber++)
            {
                var page = pdfDocument.GetPage(pageNumber);
                var strategy = new SimpleTextExtractionStrategy();
                var pageText = PdfTextExtractor.GetTextFromPage(page, strategy);

                if (!string.IsNullOrWhiteSpace(pageText))
                {
                    stringBuilder.AppendLine(pageText.Trim());
                    stringBuilder.AppendLine();
                }
            }

            return stringBuilder.ToString();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, LoggingConstants.LogHelperMethodFailed, nameof(ReadPdfFileData), DateTime.UtcNow, ex.Message);
            throw;
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(ReadPdfFileData), DateTime.UtcNow, knowledgeBaseFile.FileName);
        }
    }

    /// <summary>
    /// Reads the text file data.
    /// </summary>
    /// <param name="knowledgeBaseFile">The knowledge base file.</param>
    /// <returns>The knowledge base document in string.</returns>
    private string ReadTextFileData(KnowledgeBaseDocumentDomain knowledgeBaseFile)
    {
        ArgumentNullException.ThrowIfNull(knowledgeBaseFile);

        try
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(ReadTextFileData), DateTime.UtcNow, knowledgeBaseFile.FileName);

            if (knowledgeBaseFile.FileContent is null || knowledgeBaseFile.FileContent.Length == 0) return string.Empty;
            return System.Text.Encoding.UTF8.GetString(knowledgeBaseFile.FileContent);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, LoggingConstants.LogHelperMethodFailed, nameof(ReadTextFileData), DateTime.UtcNow, ex.Message);
            throw;
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(ReadTextFileData), DateTime.UtcNow, knowledgeBaseFile.FileName);
        }
    }

    #endregion
}
