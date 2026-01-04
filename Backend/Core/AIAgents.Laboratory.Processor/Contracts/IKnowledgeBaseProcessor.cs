using AIAgents.Laboratory.Processor.Models;

namespace AIAgents.Laboratory.Processor.Contracts;

/// <summary>
/// The interface for processing knowledge base documents and retrieving relevant knowledge.
/// </summary>
public interface IKnowledgeBaseProcessor
{
    /// <summary>
    /// Processes a knowledge base document by splitting its content into chunks, generating embeddings, and storing them in the memory store for the specified agent.
    /// </summary>
    /// <remarks>If the specified agent's collection does not exist in the memory store, it will be created automatically. Each chunk of the document is processed to generate an embedding and is then upserted into the
    /// agent's collection.</remarks>
    /// <param name="content">The plain text content of the knowledge base document to process. Cannot be null or empty.</param>
    /// <param name="agentId">The unique identifier of the agent whose knowledge base will be updated. Cannot be null or empty.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task ProcessKnowledgeBaseDocumentAsync(string content, string agentId);

    /// <summary>
    /// Asynchronously retrieves relevant knowledge entries for a given query and agent identifier.
    /// </summary>
    /// <remarks>The method uses semantic search to identify and return up to five of the most relevant knowledge entries for the specified agent. The returned string may contain multiple entries, each separated by
    /// two newline characters. If no relevant knowledge is found, the method returns an empty string.</remarks>
    /// <param name="query">The search query used to find relevant knowledge. Cannot be null or empty.</param>
    /// <param name="agentId">The unique identifier of the agent whose knowledge base is searched. Cannot be null or empty.</param>
    /// <returns>A string containing the most relevant knowledge entries separated by double newlines, or an empty string if no relevant entries are found.</returns>
    Task<string> GetRelevantKnowledgeAsync(string query, string agentId);

    /// <summary>
    /// Detects the file type of the specified knowledge base document and reads its content accordingly.
    /// </summary>
    /// <remarks>Supported file types include plain text, PDF, Excel, and Word documents. If the file type is not supported, an exception is thrown.</remarks>
    /// <param name="knowledgeBaseDocument">The knowledge base document to read. Must not be null and must have a valid file name with a supported extension.</param>
    /// <returns>A string containing the content of the file. The format of the content depends on the file type.</returns>
    string DetectAndReadFileContent(KnowledgeBaseDocumentDomain knowledgeBaseDocumentDomain);
}
