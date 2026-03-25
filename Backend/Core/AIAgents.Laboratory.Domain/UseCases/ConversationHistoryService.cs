using AIAgents.Laboratory.Domain.Contracts;
using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.Domain.Helpers;
using AIAgents.Laboratory.Domain.Ports.Out;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Newtonsoft.Json;
using static AIAgents.Laboratory.Domain.Helpers.Constants;

namespace AIAgents.Laboratory.Domain.UseCases;

/// <summary>
/// Provides services for managing and persisting conversation history for chat users.
/// </summary>
/// <remarks>This service enables retrieval, storage, and clearing of conversation history for individual users.
/// It relies on MongoDB for data persistence and supports correlation tracking for distributed operations. All operations are asynchronous and log relevant events for monitoring and troubleshooting.</remarks>
/// <param name="logger">The logger used for recording diagnostic and operational information.</param>
/// <param name="configuration">The application configuration source used to retrieve database and collection settings.</param>
/// <param name="correlationContext">The correlation context used for tracking request and operation correlation across services.</param>
/// <param name="mongoDatabaseService">The MongoDB database service used for data access and persistence operations.</param>
/// <seealso cref="IConversationHistoryService"/>
public sealed class ConversationHistoryService(ILogger<ConversationHistoryService> logger, IConfiguration configuration, ICorrelationContext correlationContext, IMongoDatabaseService mongoDatabaseService) : IConversationHistoryService
{
    /// <summary>
    /// The mongo database name configuration value.
    /// </summary>
    private readonly string MongoDatabaseName = configuration[MongoDbCollectionConstants.AiAgentsPrimaryDatabase] ?? throw new KeyNotFoundException(ExceptionConstants.ConfigurationKeyNotFoundExceptionMessage);

    /// <summary>
    /// The conversation history collection name configuration value.
    /// </summary>
    private readonly string ConversationHistoryCollectionName = configuration[MongoDbCollectionConstants.ConversationHistoryCollectionName] ?? throw new KeyNotFoundException(ExceptionConstants.ConfigurationKeyNotFoundExceptionMessage);

    /// <summary>
    /// Gets the conversation history data for current chat.
    /// </summary>
    /// <param name="userName">The user name.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The conversation history domain.</returns>
    public async Task<ConversationHistoryDomain> GetConversationHistoryAsync(string userName, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userName);

        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(GetConversationHistoryAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, userName }));

            var allConversationHistoryData = await mongoDatabaseService.GetDataFromCollectionAsync(
                databaseName: this.MongoDatabaseName,
                collectionName: this.ConversationHistoryCollectionName,
                filter: Builders<ConversationHistoryDomain>.Filter.Where(x => x.UserName == userName && x.IsActive),
                cancellationToken
            ).ConfigureAwait(false);

            if (allConversationHistoryData.Any())
            {
                return allConversationHistoryData.First();
            }
            else
            {
                var newConversationHistory = new ConversationHistoryDomain()
                {
                    UserName = userName,
                    ChatHistory = [],
                    ConversationId = Guid.NewGuid().ToString(),
                    IsActive = true,
                    LastModifiedOn = DateTime.UtcNow
                };

                await mongoDatabaseService.SaveDataAsync(
                    data: newConversationHistory,
                    databaseName: this.MongoDatabaseName,
                    collectionName: this.ConversationHistoryCollectionName,
                    cancellationToken
                ).ConfigureAwait(false);
                return newConversationHistory;
            }
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(GetConversationHistoryAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetConversationHistoryAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, userName }));
        }
    }

    /// <summary>
    /// Saves the current message data to conversation history.
    /// </summary>
    /// <param name="conversationHistory">The conversation history.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The boolean for success/failure.</returns>
    public async Task<bool> SaveMessageToConversationHistoryAsync(ConversationHistoryDomain conversationHistory, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(conversationHistory);

        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(SaveMessageToConversationHistoryAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, conversationHistory.ConversationId }));

            var filter = Builders<ConversationHistoryDomain>.Filter
                .Where(x => x.ConversationId == conversationHistory.ConversationId && x.UserName == conversationHistory.UserName);

            var allConversationHistoryData = await mongoDatabaseService.GetDataFromCollectionAsync(
                databaseName: this.MongoDatabaseName,
                collectionName: this.ConversationHistoryCollectionName,
                filter: filter,
                cancellationToken
            ).ConfigureAwait(false);

            var conversationHistoryData = allConversationHistoryData.FirstOrDefault() ?? throw new KeyNotFoundException(ExceptionConstants.DataNotFoundExceptionMessage);

            // Add the new message to the existing chat history
            var updatedChatHistory = conversationHistoryData.ChatHistory.ToList();
            updatedChatHistory.Add(conversationHistory.ChatHistory[^1]);

            // Update the existing document with the new chat history
            var update = Builders<ConversationHistoryDomain>.Update
                .Set(x => x.ChatHistory, updatedChatHistory)
                .Set(x => x.LastModifiedOn, DateTime.UtcNow)
                .Set(x => x.IsActive, true);

            return await mongoDatabaseService.UpdateDataInCollectionAsync(
                filter: filter,
                update: update,
                databaseName: this.MongoDatabaseName,
                collectionName: this.ConversationHistoryCollectionName,
                cancellationToken
            ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(SaveMessageToConversationHistoryAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(SaveMessageToConversationHistoryAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, conversationHistory.ConversationId }));
        }
    }

    /// <summary>
    /// Clears the conversation history data for the user.
    /// </summary>
    /// <param name="userName">The user name for user.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The boolean for success/failure.</returns>
    public async Task<bool> ClearConversationHistoryForUserAsync(string userName, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userName);

        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(ClearConversationHistoryForUserAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, userName }));

            var filter = Builders<ConversationHistoryDomain>.Filter.Where(x => x.UserName == userName && x.IsActive);
            var allConversationHistoryData = await mongoDatabaseService.GetDataFromCollectionAsync(
                databaseName: this.MongoDatabaseName,
                collectionName: this.ConversationHistoryCollectionName,
                filter: filter,
                cancellationToken
            ).ConfigureAwait(false);

            return allConversationHistoryData.Any() && await mongoDatabaseService.DeleteDataFromCollectionAsync(
                filter: filter,
                databaseName: this.MongoDatabaseName,
                collectionName: this.ConversationHistoryCollectionName,
                cancellationToken
            ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(ClearConversationHistoryForUserAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(ClearConversationHistoryForUserAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, userName }));
        }
    }
}
