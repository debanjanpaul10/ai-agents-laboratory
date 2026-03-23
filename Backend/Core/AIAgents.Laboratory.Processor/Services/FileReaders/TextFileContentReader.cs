using AIAgents.Laboratory.Processor.Contracts;
using AIAgents.Laboratory.Processor.Helpers;
using AIAgents.Laboratory.Processor.Models;
using Microsoft.Extensions.Logging;
using static AIAgents.Laboratory.Processor.Helpers.ProcessorConstants;

namespace AIAgents.Laboratory.Processor.Services.FileReaders;

/// <summary>
/// Provides functionality to read the content of plain text files, including JSON files.
/// </summary>
/// <remarks>This class reads the content of plain text files by converting the byte array content into a UTF-8 encoded string.
/// It supports file types such as .txt and .json, allowing for flexible handling of various text-based document formats. 
/// The class includes error handling to log any exceptions that occur during the reading process and rethrows them as <see cref="AIAgentsException"/>.</remarks>
/// <param name="logger">The logger service.</param>
/// <seealso cref="IFileContentReader"/>
internal sealed class TextFileContentReader(ILogger<TextFileContentReader> logger) : IFileContentReader
{
    /// <summary>
    /// The file extensions supported by this reader. In this case, it supports plain text files with the ".txt" extension and JSON files with the ".json" extension.
    /// </summary>
    public IReadOnlyCollection<string> SupportedExtensions { get; } = [KnowledgeBaseConstants.FileContentTypes.JsonFiles, KnowledgeBaseConstants.FileContentTypes.PlainTextFiles];

    /// <summary>
    /// Reads the content of a plain text file, including JSON files, by converting the byte array content into a UTF-8 encoded string.
    /// </summary>
    /// <remarks>This method checks if the file content is null or empty and returns an empty string in such cases. 
    /// It logs the start and end of the reading process, as well as any exceptions that occur, which are rethrown as <see cref="AIAgentsException"/>.</remarks>
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

            return System.Text.Encoding.UTF8.GetString(knowledgeBaseDocument.FileContent);
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
