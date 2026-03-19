using AIAgents.Laboratory.Processor.Contracts;
using AIAgents.Laboratory.Processor.Helpers;
using AIAgents.Laboratory.Processor.Models;
using AIAgents.Laboratory.Processor.Services.FileReaders;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel.Memory;
using Microsoft.SemanticKernel.Text;
using Newtonsoft.Json;
using static AIAgents.Laboratory.Processor.Helpers.ProcessorConstants;

namespace AIAgents.Laboratory.Processor.Services;

/// <summary>
/// Provides functionality to process knowledge base documents, including reading file content, generating embeddings, and retrieving relevant knowledge based on queries. 
/// </summary>
/// <remarks>This class interacts with a memory store to manage knowledge entries and uses an embedding generator service to create vector representations of text for semantic search. 
/// It also utilizes a factory to read different types of files based on their extensions, allowing for flexible handling of various document formats.</remarks>
/// <param name="logger">The logging service.</param>
/// <param name="memoryStore">The memory store used to manage knowledge entries.</param>
/// <param name="embeddingGeneratorService">The embedding generator service used to create vector representations of text for semantic search.</param>
/// <param name="fileContentReaderFactory">The factory used to resolve the appropriate file content reader based on file extensions.</param>
/// <seealso cref="IKnowledgeBaseProcessor"/>
#pragma warning disable SKEXP0050
#pragma warning disable SKEXP0001
public sealed class KnowledgeBaseProcessor(ILogger<KnowledgeBaseProcessor> logger, IMemoryStore memoryStore, IEmbeddingGenerator<string, Embedding<float>> embeddingGeneratorService,
    FileContentReaderFactory fileContentReaderFactory) : IKnowledgeBaseProcessor
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
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(DetectAndReadFileContent), DateTime.UtcNow, knowledgeBaseDocumentDomain.FileName);

            var fileExtension = Path.GetExtension(knowledgeBaseDocumentDomain.FileName);
            var reader = fileContentReaderFactory.Resolve(fileExtension);
            return reader.Read(knowledgeBaseDocumentDomain);
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(DetectAndReadFileContent), DateTime.UtcNow, ex.Message);
            throw new AIAgentsException(ex);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(DetectAndReadFileContent), DateTime.UtcNow, knowledgeBaseDocumentDomain.FileName);
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
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(GetRelevantKnowledgeAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { query, agentId }));

            var queryEmbeddingResult = await embeddingGeneratorService.GenerateAsync(query).ConfigureAwait(false);
            var queryEmbedding = queryEmbeddingResult.Vector;
            var relevantChunks = new List<(MemoryRecord Record, double Score)>();

            await foreach (var chunk in memoryStore.GetNearestMatchesAsync(collectionName: agentId, embedding: queryEmbedding, limit: 5).ConfigureAwait(false))
                relevantChunks.Add(chunk);

            if (relevantChunks.Count == 0)
                return string.Empty;

            return string.Join("\n\n", relevantChunks.Where(c => !string.IsNullOrEmpty(c.Record.Metadata.Text)).Select(c => c.Record.Metadata.Text));
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(GetRelevantKnowledgeAsync), DateTime.UtcNow, ex.Message);
            return string.Empty;
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetRelevantKnowledgeAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { query, agentId }));
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
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(ProcessKnowledgeBaseDocumentAsync), DateTime.UtcNow, agentId);

            // Ensure the collection exists before upserting records
            var collections = new List<string>();
            await foreach (var collection in memoryStore.GetCollectionsAsync().ConfigureAwait(false))
                collections.Add(collection);

            if (!collections.Contains(agentId))
                await memoryStore.CreateCollectionAsync(agentId).ConfigureAwait(false);

            var chunks = TextChunker.SplitPlainTextLines(content, 512);
            if (chunks.Count == 0)
                throw new InvalidOperationException(ExceptionConstants.NoValidChunksGenerated);

            var embeddingResults = await embeddingGeneratorService.GenerateAsync(chunks);
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
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(ProcessKnowledgeBaseDocumentAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsException(ex);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(ProcessKnowledgeBaseDocumentAsync), DateTime.UtcNow, agentId);
        }
    }
}
