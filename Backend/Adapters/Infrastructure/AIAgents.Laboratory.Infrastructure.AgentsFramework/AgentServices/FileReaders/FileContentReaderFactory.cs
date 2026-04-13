using AIAgents.Laboratory.Domain.Contracts;
using AIAgents.Laboratory.Domain.Helpers;
using AIAgents.Laboratory.Infrastructure.AgentsFramework.Contracts;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SharpCompress.Common;
using static AIAgents.Laboratory.Infrastructure.AgentsFramework.Helpers.Constants;

namespace AIAgents.Laboratory.Infrastructure.AgentsFramework.AgentServices.FileReaders;

/// <summary>
/// Provides functionality to resolve the appropriate file content reader based on file extensions. 
/// </summary>
/// <remarks>This factory class maintains a mapping of supported file extensions to their corresponding content readers, allowing for flexible handling of various document formats when processing knowledge base documents. 
/// If a requested file extension is not supported, the factory throws a FileFormatException with a message indicating that the file type is unsupported.</remarks>
/// <param name="readers">The collection of file content readers to be used by the factory. Each reader should specify the file extensions it supports through the SupportedExtensions property.</param>
public sealed class FileContentReaderFactory(
    ILogger<FileContentReaderFactory> logger,
    ICorrelationContext correlationContext,
    IEnumerable<IFileContentReader> readers)
{
    /// <summary>
    /// The dictionary that maps file extensions to their corresponding file content readers. The keys are the file extensions (e.g., ".txt", ".pdf") and the values are the instances of IFileContentReader that can handle those extensions.
    /// </summary>
    private readonly Dictionary<string, IFileContentReader> _readers = readers
        .SelectMany(r => r.SupportedExtensions.Select(ext => (ext, r)))
        .ToDictionary(x => x.ext, x => x.r, StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Resolves the appropriate file content reader based on the provided file extension. If a reader for the specified extension is found in the dictionary, it is returned; 
    /// otherwise, a FileFormatException is thrown indicating that the file type is unsupported.
    /// </summary>
    /// <param name="fileExtension">The file extension for which to resolve the content reader (e.g., ".txt", ".pdf"). The extension should include the leading dot and is case-insensitive.</param>
    /// <returns>The <see cref="IFileContentReader"/> instance that can handle the specified file extension.</returns>
    public IFileContentReader Resolve(string fileExtension)
    {
        try
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodStart,
                nameof(Resolve), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, fileExtension })
            );

            if (this._readers.TryGetValue(fileExtension, out var reader))
                return reader;

            throw new InvalidFormatException(ExceptionConstants.UnsupportedFileTypeMessage);
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex,
                LoggingConstants.LogHelperMethodFailed,
                nameof(Resolve), DateTime.UtcNow, ex.Message
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
                nameof(Resolve), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, fileExtension })
            );
        }
    }
}
