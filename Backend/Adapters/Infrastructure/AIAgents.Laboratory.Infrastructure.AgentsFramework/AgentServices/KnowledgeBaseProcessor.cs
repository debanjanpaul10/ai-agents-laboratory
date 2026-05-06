using AIAgents.Laboratory.Domain.Contracts;
using AIAgents.Laboratory.Domain.DomainEntities.AgentsEntities;
using AIAgents.Laboratory.Domain.Helpers;
using AIAgents.Laboratory.Domain.Ports.Out;
using AIAgents.Laboratory.Infrastructure.AgentsFramework.AgentServices.FileReaders;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel.Memory;
using Microsoft.SemanticKernel.Text;
using Newtonsoft.Json;
using static AIAgents.Laboratory.Infrastructure.AgentsFramework.Helpers.Constants;

namespace AIAgents.Laboratory.Infrastructure.AgentsFramework.AgentServices;

/// <summary>
/// Provides functionality to process knowledge base documents, including reading file content, generating embeddings, and retrieving relevant knowledge based on queries. 
/// </summary>
/// <remarks>This class interacts with a memory store to manage knowledge entries and uses an embedding generator service to create vector representations of text for semantic search. 
/// It also utilizes a factory to read different types of files based on their extensions, allowing for flexible handling of various document formats.</remarks>
/// <param name="logger">The logging service.</param>
/// <param name="correlationContext">The correlation context used to track and correlate logs and exceptions across different components of the application.</param>
/// <param name="memoryStore">The memory store used to manage knowledge entries.</param>
/// <param name="embeddingGeneratorService">The embedding generator service used to create vector representations of text for semantic search.</param>
/// <param name="fileContentReaderFactory">The factory used to resolve the appropriate file content reader based on file extensions.</param>
/// <seealso cref="IKnowledgeBaseProcessor"/>
#pragma warning disable SKEXP0050
#pragma warning disable SKEXP0001
public sealed class KnowledgeBaseProcessor(
    ILogger<KnowledgeBaseProcessor> logger,
    ICorrelationContext correlationContext,
    IMemoryStore memoryStore,
    IEmbeddingGenerator<string, Embedding<float>> embeddingGeneratorService,
    FileContentReaderFactory fileContentReaderFactory) : IKnowledgeBaseProcessor
{
    /// <inheritdoc />
    public string DetectAndReadFileContent(KnowledgeBaseDocumentDomain knowledgeBaseDocumentDomain)
    {
        ArgumentNullException.ThrowIfNull(knowledgeBaseDocumentDomain);

        try
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodStart,
                nameof(DetectAndReadFileContent), DateTime.UtcNow, knowledgeBaseDocumentDomain.FileName
            );

            var fileExtension = Path.GetExtension(knowledgeBaseDocumentDomain.FileName);
            var reader = fileContentReaderFactory.Resolve(fileExtension);
            return reader.Read(knowledgeBaseDocumentDomain);
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex,
                LoggingConstants.LogHelperMethodFailed,
                nameof(DetectAndReadFileContent), DateTime.UtcNow, ex.Message
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
                nameof(DetectAndReadFileContent), DateTime.UtcNow, knowledgeBaseDocumentDomain.FileName
            );
        }
    }

    /// <inheritdoc />
    public async Task<string> GetRelevantKnowledgeAsync(
        string query,
        string agentId,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentException.ThrowIfNullOrEmpty(query);
        ArgumentException.ThrowIfNullOrEmpty(agentId);

        try
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodStart,
                nameof(GetRelevantKnowledgeAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { query, agentId })
            );

            var queryEmbeddingResult = await embeddingGeneratorService.GenerateAsync(
                value: query,
                cancellationToken: cancellationToken
            ).ConfigureAwait(false);

            var queryEmbedding = queryEmbeddingResult.Vector;
            var relevantChunks = new List<(MemoryRecord Record, double Score)>();

            await foreach (var chunk in memoryStore.GetNearestMatchesAsync(
                collectionName: agentId,
                embedding: queryEmbedding,
                limit: 5,
                cancellationToken: cancellationToken
            ).ConfigureAwait(false))
            {
                relevantChunks.Add(chunk);
            }

            if (relevantChunks.Count == 0)
                return string.Empty;

            return string.Join(
                "\n\n",
                relevantChunks.Where(c => !string.IsNullOrEmpty(c.Record.Metadata.Text)).Select(c => c.Record.Metadata.Text)
            );
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex,
                LoggingConstants.LogHelperMethodFailed,
                nameof(GetRelevantKnowledgeAsync), DateTime.UtcNow, ex.Message
            );
            return string.Empty;
        }
        finally
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodEnd,
                nameof(GetRelevantKnowledgeAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { query, agentId })
            );
        }
    }

    /// <inheritdoc />
    public async Task ProcessKnowledgeBaseDocumentAsync(
        string content,
        string agentId,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentException.ThrowIfNullOrEmpty(content);
        ArgumentException.ThrowIfNullOrEmpty(agentId);

        try
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodStart,
                nameof(ProcessKnowledgeBaseDocumentAsync), DateTime.UtcNow, agentId
            );

            // Ensure the collection exists before upserting records
            var collections = new List<string>();
            await foreach (var collection in memoryStore.GetCollectionsAsync(
                cancellationToken: cancellationToken
            ).ConfigureAwait(false))
            {
                collections.Add(collection);
            }

            if (!collections.Contains(agentId))
                await memoryStore.CreateCollectionAsync(
                    collectionName: agentId,
                    cancellationToken: cancellationToken
                ).ConfigureAwait(false);

            var chunks = TextChunker.SplitPlainTextLines(content, 512);
            if (chunks.Count == 0)
                throw new InvalidOperationException(ExceptionConstants.NoValidChunksGenerated);

            var embeddingResults = await embeddingGeneratorService.GenerateAsync(
                values: chunks,
                cancellationToken: cancellationToken
            ).ConfigureAwait(false);

            var embeddings = embeddingResults.Select(e => e.Vector).ToList();
            if (embeddings.Count != chunks.Count)
                throw new InvalidOperationException(ExceptionConstants.NumberOfEmbeddingsMismatch);

            for (int i = 0; i < chunks.Count; i++)
            {
                var chunkDescription = string.Format(KnowledgeBaseConstants.ChunkDescriptionTemplate, i + 1, chunks.Count);
                var metadata = new MemoryRecordMetadata(
                    isReference: false,
                    id: agentId,
                    text: chunks[i],
                    description: chunkDescription,
                    externalSourceName: string.Empty,
                    additionalMetadata: string.Empty
                );
                var record = new MemoryRecord(
                    metadata,
                    embedding: embeddings[i],
                    key: null,
                    timestamp: null
                );

                await memoryStore.UpsertAsync(
                    collectionName: agentId,
                    record,
                    cancellationToken
                ).ConfigureAwait(false);
            }
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex,
                LoggingConstants.LogHelperMethodFailed,
                nameof(ProcessKnowledgeBaseDocumentAsync), DateTime.UtcNow, ex.Message
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
                nameof(ProcessKnowledgeBaseDocumentAsync), DateTime.UtcNow, agentId
            );
        }
    }
}
