using AIAgents.Laboratory.Processor.Contracts;
using AIAgents.Laboratory.Processor.Helpers;
using AIAgents.Laboratory.Processor.Models;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using Microsoft.Extensions.Logging;
using static AIAgents.Laboratory.Processor.Helpers.ProcessorConstants;

namespace AIAgents.Laboratory.Processor.Services.FileReaders;

/// <summary>
/// Provides functionality to read the content of PDF files. 
/// This class uses the iText library to extract text from PDF documents, handling multiple pages and ensuring that the extracted text is properly formatted. 
/// </summary>
/// <remarks>It implements the IFileContentReader interface, allowing it to be used in a flexible manner within the knowledge base processing workflow. 
/// The class also includes robust error handling and logging to facilitate troubleshooting and ensure reliability when processing PDF files.</remarks>
/// <param name="logger">The logger service.</param>
/// <seealso cref="IFileContentReader"/>
internal sealed class PdfFileContentReader(ILogger<PdfFileContentReader> logger) : IFileContentReader
{
    /// <summary>
    /// The file extensions supported by this reader. In this case, it supports only PDF files with the ".pdf" extension.
    /// </summary>
    public IReadOnlyCollection<string> SupportedExtensions { get; } = [KnowledgeBaseConstants.FileContentTypes.PdfFiles];

    /// <summary>
    /// Reads the content of a PDF file and returns it as a string. The method takes a <see cref="KnowledgeBaseDocumentDomain"/> object as input, which contains the file content in byte array format.
    /// </summary>
    /// <param name="knowledgeBaseDocument">The object containing the PDF file content to be read.</param>
    /// <returns>The string content of the file.</returns>
    public string Read(KnowledgeBaseDocumentDomain knowledgeBaseDocument)
    {
        ArgumentNullException.ThrowIfNull(knowledgeBaseDocument);

        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(Read), DateTime.UtcNow, knowledgeBaseDocument.FileName);

            if (knowledgeBaseDocument.FileContent is null || knowledgeBaseDocument.FileContent.Length == 0)
                return string.Empty;

            using var memoryStream = new MemoryStream(knowledgeBaseDocument.FileContent);
            using var reader = new PdfReader(memoryStream);
            using var pdfDocument = new PdfDocument(reader);

            var stringBuilder = new System.Text.StringBuilder();
            for (int pageNumber = 1; pageNumber <= pdfDocument.GetNumberOfPages(); pageNumber++)
            {
                var page = pdfDocument.GetPage(pageNumber);
                var pageText = PdfTextExtractor.GetTextFromPage(page, new SimpleTextExtractionStrategy());

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
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(Read), DateTime.UtcNow, ex.Message);
            throw new AIAgentsException(ex);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(Read), DateTime.UtcNow, knowledgeBaseDocument.FileName);
        }
    }
}
