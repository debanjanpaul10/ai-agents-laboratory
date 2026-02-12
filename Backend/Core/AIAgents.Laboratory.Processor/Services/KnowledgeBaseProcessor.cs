using AIAgents.Laboratory.Processor.Contracts;
using AIAgents.Laboratory.Processor.Helpers;
using AIAgents.Laboratory.Processor.Models;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel.Memory;
using Microsoft.SemanticKernel.Text;
using Newtonsoft.Json;
using static AIAgents.Laboratory.Processor.Helpers.ProcessorConstants;

namespace AIAgents.Laboratory.Processor.Services;

/// <summary>
/// The knowledge base processor class.
/// </summary>
/// <param name="memoryStore">The memory store service.</param>
/// <param name="embeddingGeneratorService">The embedding generation service.</param>
/// <param name="logger">The logger service.</param>
/// <seealso cref="IKnowledgeBaseProcessor"/>
#pragma warning disable SKEXP0050
#pragma warning disable SKEXP0001
public sealed class KnowledgeBaseProcessor(IMemoryStore memoryStore, IEmbeddingGenerator<string, Embedding<float>> embeddingGeneratorService, ILogger<KnowledgeBaseProcessor> logger) : IKnowledgeBaseProcessor
{
    /// <summary>
    /// Detects the file type of the specified knowledge base document and reads its content accordingly.
    /// </summary>
    /// <remarks>Supported file types include plain text, PDF, Excel, and Word documents. If the file type is not supported, an exception is thrown.</remarks>
    /// <param name="knowledgeBaseDocumentDomain">The knowledge base document to read. Must not be null and must have a valid file name with a supported extension.</param>
    /// <returns>A string containing the content of the file. The format of the content depends on the file type.</returns>
    public string DetectAndReadFileContent(KnowledgeBaseDocumentDomain knowledgeBaseDocumentDomain)
    {
        ArgumentNullException.ThrowIfNull(knowledgeBaseDocumentDomain);

        try
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(GetRelevantKnowledgeAsync), DateTime.UtcNow, knowledgeBaseDocumentDomain.FileName);

            var fileType = Path.GetExtension(knowledgeBaseDocumentDomain.FileName);
            if (string.Equals(KnowledgeBaseConstants.FileContentTypes.PlainTextFiles, fileType, StringComparison.OrdinalIgnoreCase))
                return this.ReadTextFileData(knowledgeBaseDocumentDomain);

            else if (string.Equals(KnowledgeBaseConstants.FileContentTypes.PdfFiles, fileType, StringComparison.OrdinalIgnoreCase))
                return this.ReadPdfFileData(knowledgeBaseDocumentDomain);

            else if (KnowledgeBaseConstants.FileContentTypes.ExcelFiles.Split(KnowledgeBaseConstants.CommaSeparator).Contains(fileType))
                return this.ReadSpreadsheetData(knowledgeBaseDocumentDomain);

            else if (KnowledgeBaseConstants.FileContentTypes.WordFiles.Split(KnowledgeBaseConstants.CommaSeparator).Contains(fileType))
                return this.ReadWordFileData(knowledgeBaseDocumentDomain);

            else if (string.Equals(KnowledgeBaseConstants.FileContentTypes.JsonFiles, fileType, StringComparison.OrdinalIgnoreCase))
                return this.ReadTextFileData(knowledgeBaseDocumentDomain);

            else
                throw new FileFormatException(ExceptionConstants.UnsupportedFileTypeMessage);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, LoggingConstants.LogHelperMethodFailed, nameof(GetRelevantKnowledgeAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsException(ex);
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetRelevantKnowledgeAsync), DateTime.UtcNow, knowledgeBaseDocumentDomain.FileName);
        }
    }

    /// <summary>
    /// Asynchronously retrieves relevant knowledge entries for a given query and agent identifier.
    /// </summary>
    /// <remarks>The method uses semantic search to identify and return up to five of the most relevant knowledge entries for the specified agent. The returned string may contain multiple entries, each separated by
    /// two newline characters. If no relevant knowledge is found, the method returns an empty string.</remarks>
    /// <param name="query">The search query used to find relevant knowledge. Cannot be null or empty.</param>
    /// <param name="agentId">The unique identifier of the agent whose knowledge base is searched. Cannot be null or empty.</param>
    /// <returns>A string containing the most relevant knowledge entries separated by double newlines, or an empty string if no relevant entries are found.</returns>
    public async Task<string> GetRelevantKnowledgeAsync(string query, string agentId)
    {
        ArgumentException.ThrowIfNullOrEmpty(query);
        ArgumentException.ThrowIfNullOrEmpty(agentId);

        try
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(GetRelevantKnowledgeAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { query, agentId }));

            var queryEmbeddingResult = await embeddingGeneratorService.GenerateAsync(query).ConfigureAwait(false);
            var queryEmbedding = queryEmbeddingResult.Vector;
            var relevantChunks = new List<(MemoryRecord Record, double Score)>();

            await foreach (var chunk in memoryStore.GetNearestMatchesAsync(agentId, queryEmbedding, 5))
                relevantChunks.Add(chunk);

            if (relevantChunks.Count == 0)
                return string.Empty;

            return string.Join("\n\n", relevantChunks.Where(c => !string.IsNullOrEmpty(c.Record.Metadata.Text)).Select(c => c.Record.Metadata.Text));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, LoggingConstants.LogHelperMethodFailed, nameof(GetRelevantKnowledgeAsync), DateTime.UtcNow, ex.Message);
            return string.Empty;
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetRelevantKnowledgeAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { query, agentId }));
        }
    }

    /// <summary>
    /// Processes a knowledge base document by splitting its content into chunks, generating embeddings, and storing them in the memory store for the specified agent.
    /// </summary>
    /// <remarks>If the specified agent's collection does not exist in the memory store, it will be created automatically. Each chunk of the document is processed to generate an embedding and is then upserted into the
    /// agent's collection.</remarks>
    /// <param name="content">The plain text content of the knowledge base document to process. Cannot be null or empty.</param>
    /// <param name="agentId">The unique identifier of the agent whose knowledge base will be updated. Cannot be null or empty.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the document content cannot be split into valid chunks, or if the number of generated embeddings does not match the number of chunks.</exception>
    public async Task ProcessKnowledgeBaseDocumentAsync(string content, string agentId)
    {
        ArgumentException.ThrowIfNullOrEmpty(content);
        ArgumentException.ThrowIfNullOrEmpty(agentId);

        try
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(ProcessKnowledgeBaseDocumentAsync), DateTime.UtcNow, agentId);

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
            logger.LogError(ex, LoggingConstants.LogHelperMethodFailed, nameof(ProcessKnowledgeBaseDocumentAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsException(ex);
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(ProcessKnowledgeBaseDocumentAsync), DateTime.UtcNow, agentId);
        }
    }

    #region PRIVATE METHODS

    /// <summary>
    /// Reads the contents of a spreadsheet file from the specified knowledge base document and returns its data as a formatted string.
    /// </summary>
    /// <remarks>The returned string includes each sheet's name as a header, followed by its rows with cell
    /// values separated by tabs. This method does not modify the input document. Only basic formatting is applied; cell
    /// formulas and advanced formatting are not preserved.</remarks>
    /// <param name="knowledgeBaseFile">The knowledge base document containing the spreadsheet file to read. The document's FileContent property must contain the binary content of a valid spreadsheet file. Cannot be null.</param>
    /// <returns>A string containing the tab-delimited contents of the spreadsheet, including sheet names as headers. Returns an empty string if the file content is null, empty, or the spreadsheet contains no readable data.</returns>
    private string ReadSpreadsheetData(KnowledgeBaseDocumentDomain knowledgeBaseFile)
    {
        ArgumentNullException.ThrowIfNull(knowledgeBaseFile);
        ArgumentNullException.ThrowIfNull(knowledgeBaseFile.FileContent);

        try
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(ReadSpreadsheetData), DateTime.UtcNow, knowledgeBaseFile.FileName);

            if (knowledgeBaseFile.FileContent.Length == 0)
                return string.Empty;

            using var memoryStream = new MemoryStream(knowledgeBaseFile.FileContent);
            using var spreadsheetDocument = SpreadsheetDocument.Open(memoryStream, false);

            var workbookPart = spreadsheetDocument.WorkbookPart;
            if (workbookPart?.Workbook?.Sheets is null)
                return string.Empty;

            return Utility.PrepareExcelData(workbookPart);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, LoggingConstants.LogHelperMethodFailed, nameof(ReadSpreadsheetData), DateTime.UtcNow, ex.Message);
            throw new AIAgentsException(ex);
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(ReadSpreadsheetData), DateTime.UtcNow, knowledgeBaseFile.FileName);
        }
    }

    /// <summary>
    /// Extracts and returns the plain text content from the body of a Word document provided as a knowledge base file.
    /// </summary>
    /// <remarks>This method reads only the main body text of the Word document and ignores formatting,
    /// images, and other non-text elements. The returned text preserves paragraph breaks as line breaks. If the file
    /// content is not a valid WordprocessingML document, an exception may be thrown.</remarks>
    /// <param name="knowledgeBaseFile">The knowledge base document containing the Word file data to read. Cannot be null. The file content must represent a valid WordprocessingML (.docx) file.</param>
    /// <returns>A string containing the concatenated plain text from all non-empty paragraphs in the document body. Returns an empty string if the file content is null, empty, or does not contain any readable text.</returns>
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
            if (body is null)
                return string.Empty;

            var stringBuilder = new System.Text.StringBuilder();
            foreach (var paragraph in body.Elements<Paragraph>().Select(p => p.InnerText).Where(item => !string.IsNullOrWhiteSpace(item.Trim())))
                stringBuilder.AppendLine(paragraph);

            return stringBuilder.ToString();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, LoggingConstants.LogHelperMethodFailed, nameof(ReadWordFileData), DateTime.UtcNow, ex.Message);
            throw new AIAgentsException(ex);
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(ReadWordFileData), DateTime.UtcNow, knowledgeBaseFile.FileName);
        }
    }

    /// <summary>
    /// Extracts and returns the text content from a PDF file represented by the specified knowledge base document.
    /// </summary>
    /// <param name="knowledgeBaseFile">The knowledge base document containing the PDF file data to be read. Cannot be null. The FileContent property
    /// must contain the PDF file's binary data.</param>
    /// <returns>A string containing the extracted text from the PDF file. Returns an empty string if the file content is null or empty.</returns>
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
            throw new AIAgentsException(ex);
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

            if (knowledgeBaseFile.FileContent is null || knowledgeBaseFile.FileContent.Length == 0)
                return string.Empty;

            return System.Text.Encoding.UTF8.GetString(knowledgeBaseFile.FileContent);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, LoggingConstants.LogHelperMethodFailed, nameof(ReadTextFileData), DateTime.UtcNow, ex.Message);
            throw new AIAgentsException(ex);
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(ReadTextFileData), DateTime.UtcNow, knowledgeBaseFile.FileName);
        }
    }

    #endregion
}
