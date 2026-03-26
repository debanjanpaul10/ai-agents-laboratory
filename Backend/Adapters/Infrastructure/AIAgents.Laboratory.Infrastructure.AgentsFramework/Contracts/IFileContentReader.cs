using AIAgents.Laboratory.Domain.DomainEntities.AgentsEntities;

namespace AIAgents.Laboratory.Infrastructure.AgentsFramework.Contracts;

/// <summary>
/// Defines a contract for reading the content of a file based on its type. 
/// Implementations of this interface should specify the supported file extensions and provide logic to read the content of the file accordingly. 
/// This allows for a flexible and extensible way to handle different file types when processing knowledge base documents.
/// </summary>
/// <remarks>Implementations of this interface should ensure that the Read method can handle the specific file types they support, and should throw appropriate 
/// exceptions if the file type is not supported or if there are issues reading the file content.</remarks>
public interface IFileContentReader
{
    /// <summary>
    /// The collection of file extensions that this reader supports. Each extension should be in the format ".ext" (e.g., ".txt", ".pdf").
    /// </summary>
    IReadOnlyCollection<string> SupportedExtensions { get; }

    /// <summary>
    /// Reads the content of the provided knowledge base document and returns it as a string. The implementation should determine how to read the content based on the file type and may involve parsing or processing the file content as needed.
    /// </summary>
    /// <remarks>Implementations should handle any necessary parsing or processing of the file content based on its type. For example, if the file is a PDF, the implementation might need to extract text from the PDF format. 
    /// If the file type is not supported, the implementation should throw an appropriate exception.</remarks>
    /// <param name="knowledgeBaseDocument">The knowledge base document domain model.</param>
    /// <returns>The string content of the file.</returns>
    string Read(KnowledgeBaseDocumentDomain knowledgeBaseDocument);
}
