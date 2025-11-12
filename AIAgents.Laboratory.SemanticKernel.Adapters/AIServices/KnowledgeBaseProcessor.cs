using System.Globalization;
using AIAgents.Laboratory.Domain.DrivingPorts;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel.Memory;
using Microsoft.SemanticKernel.Text;
using static AIAgents.Laboratory.SemanticKernel.Adapters.Helpers.Constants;

namespace AIAgents.Laboratory.SemanticKernel.Adapters.AIServices;

/// <summary>
/// The knowledge base processor class.
/// </summary>
/// <param name="memoryStore">The memory store service.</param>
/// <param name="embeddingGeneratorService">The embedding generation service.</param>
/// <param name="logger">The logger service.</param>
/// <seealso cref="IKnowledgeBaseProcessor"/>
#pragma warning disable SKEXP0001
#pragma warning disable SKEXP0050
public class KnowledgeBaseProcessor(IMemoryStore memoryStore, IEmbeddingGenerator<string, Embedding<float>> embeddingGeneratorService, ILogger<KnowledgeBaseProcessor> logger) : IKnowledgeBaseProcessor
{
    /// <summary>
    /// Gets the relevant knowledge asynchronous.
    /// </summary>
    /// <param name="query">The query.</param>
    /// <param name="agentId">The agent identifier.</param>
    /// <returns>
    /// The knowledge base data.
    /// </returns>
    public async Task<string> GetRelevantKnowledgeAsync(string query, string agentId)
    {
        ArgumentException.ThrowIfNullOrEmpty(query);
        ArgumentException.ThrowIfNullOrEmpty(agentId);

        try
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodStart, nameof(GetRelevantKnowledgeAsync), DateTime.UtcNow, agentId));

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
            logger.LogError(ex, string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(GetRelevantKnowledgeAsync), DateTime.UtcNow, ex.Message));
            throw;
        }
        finally
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodEnd, nameof(GetRelevantKnowledgeAsync), DateTime.UtcNow, agentId));
        }
    }

    /// <summary>
    /// Processes the knowledge base document asynchronous.
    /// </summary>
    /// <param name="content">The content.</param>
    /// <param name="agentId">The agent identifier.</param>
    public async Task ProcessKnowledgeBaseDocumentAsync(string content, string agentId)
    {
        ArgumentException.ThrowIfNullOrEmpty(content);
        ArgumentException.ThrowIfNullOrEmpty(agentId);

        try
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodStart, nameof(ProcessKnowledgeBaseDocumentAsync), DateTime.UtcNow, agentId));

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
            logger.LogError(ex, string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(ProcessKnowledgeBaseDocumentAsync), DateTime.UtcNow, ex.Message));
            throw;
        }
        finally
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodEnd, nameof(ProcessKnowledgeBaseDocumentAsync), DateTime.UtcNow, agentId));
        }
    }
}
