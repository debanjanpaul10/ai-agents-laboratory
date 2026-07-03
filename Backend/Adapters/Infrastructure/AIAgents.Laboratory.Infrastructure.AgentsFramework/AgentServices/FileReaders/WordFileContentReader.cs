using AIAgents.Laboratory.Domain.Contracts;
using AIAgents.Laboratory.Domain.Helpers;
using AIAgents.Laboratory.Domain.Models.Agents;
using AIAgents.Laboratory.Infrastructure.AgentsFramework.Contracts;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Extensions.Logging;
using static AIAgents.Laboratory.Infrastructure.AgentsFramework.Helpers.Constants;

namespace AIAgents.Laboratory.Infrastructure.AgentsFramework.AgentServices.FileReaders;

/// <summary>
/// Provides functionality to read the content of Word documents, including files with extensions such as .doc and .docx.
/// </summary>
/// <remarks>This class uses the Open XML SDK to extract text from Word documents, handling multiple paragraphs and ensuring that the extracted text is properly formatted.
/// <param name="logger">The logger service.</param>
/// <param name="correlationContext">The correlation context for logging and exception handling.</param>
/// <seealso cref="IFileContentReader"/>
internal sealed class WordFileContentReader(
    ILogger<WordFileContentReader> logger,
    ICorrelationContext correlationContext) : IFileContentReader
{
    /// <summary>
    /// The file extensions supported by this reader. In this case, it supports Word documents with the extensions ".doc" and ".docx".
    /// </summary>
    public IReadOnlyCollection<string> SupportedExtensions { get; } = KnowledgeBaseConstants.FileContentTypes.WordFiles.Split(KnowledgeBaseConstants.CommaSeparator);

    /// <inheritdoc />
    public string Read(
        KnowledgeBaseDocumentDomain knowledgeBaseDocument
    )
    {
        ArgumentNullException.ThrowIfNull(knowledgeBaseDocument);

        try
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodStart,
                nameof(Read), DateTime.UtcNow, knowledgeBaseDocument.FileName
            );

            if (knowledgeBaseDocument.FileContent is null || knowledgeBaseDocument.FileContent.Length == 0)
                return string.Empty;

            using var memoryStream = new MemoryStream(knowledgeBaseDocument.FileContent);
            using var wordDocument = WordprocessingDocument.Open(memoryStream, false);

            var body = wordDocument.MainDocumentPart?.Document?.Body;
            if (body is null)
                return string.Empty;

            var stringBuilder = new System.Text.StringBuilder();
            foreach (var paragraph in body.Elements<Paragraph>().Select(p => p.InnerText).Where(t => !string.IsNullOrWhiteSpace(t)))
                stringBuilder.AppendLine(paragraph);

            return stringBuilder.ToString();
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
