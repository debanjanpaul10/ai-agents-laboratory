using AIAgents.Laboratory.Processor.Contracts;
using static AIAgents.Laboratory.Processor.Helpers.ProcessorConstants;

namespace AIAgents.Laboratory.Processor.Services.FileReaders;

/// <summary>
/// Provides functionality to resolve the appropriate file content reader based on file extensions. 
/// </summary>
/// <remarks>This factory class maintains a mapping of supported file extensions to their corresponding content readers, allowing for flexible handling of various document formats when processing knowledge base documents. 
/// If a requested file extension is not supported, the factory throws a FileFormatException with a message indicating that the file type is unsupported.</remarks>
/// <param name="readers">The collection of file content readers to be used by the factory. Each reader should specify the file extensions it supports through the SupportedExtensions property.</param>
public sealed class FileContentReaderFactory(IEnumerable<IFileContentReader> readers)
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
        if (this._readers.TryGetValue(fileExtension, out var reader))
            return reader;

        throw new FileFormatException(ExceptionConstants.UnsupportedFileTypeMessage);
    }
}
