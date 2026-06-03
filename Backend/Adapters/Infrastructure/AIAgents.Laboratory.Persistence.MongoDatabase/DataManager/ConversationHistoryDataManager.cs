using AIAgents.Laboratory.Domain.Contracts;
using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.Domain.Helpers;
using AIAgents.Laboratory.Domain.Ports.Out;
using AIAgents.Laboratory.Persistence.MongoDatabase.Contracts;
using AIAgents.Laboratory.Persistence.MongoDatabase.Mapper;
using AIAgents.Laboratory.Persistence.MongoDatabase.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Newtonsoft.Json;
using static AIAgents.Laboratory.Persistence.MongoDatabase.Helpers.Constants;

namespace AIAgents.Laboratory.Persistence.MongoDatabase.DataManager;

/// <summary>
/// Implements the IConversationHistoryDataManager interface to manage conversation history data using MongoDB as the underlying data store.
/// </summary>
/// <remarks>This class provides methods to retrieve, save, and clear conversation history for chat users, interacting with the MongoDB database through the IMongoDatabaseRepository abstraction. 
/// It uses <see cref="MongoDataMapperProfile"/> for mapping between domain entities and MongoDB models, and relies on configuration settings for database and collection names. 
/// All operations are asynchronous to ensure non-blocking data access and manipulation.</remarks>
/// <param name="configuration">The configuration settings for the MongoDB database.</param>
/// <param name="correlationContext">The correlation context for logging and tracing.</param>
/// <param name="logger">The logger instance for logging information and errors.</param>
/// <param name="mongoDatabaseRepository">The MongoDB database repository for data access operations.</param>
/// <see cref="IConversationHistoryDataManager"/>
public sealed class ConversationHistoryDataManager(
    IConfiguration configuration,
    ICorrelationContext correlationContext,
    ILogger<ConversationHistoryDataManager> logger,
    IMongoDatabaseRepository mongoDatabaseRepository) : IConversationHistoryDataManager
{
    /// <summary>
    /// The mongo database name configuration value.
    /// </summary>
    private readonly string MongoDatabaseName = configuration[MongoDbCollectionConstants.AiAgentsPrimaryDatabase]
        ?? throw new KeyNotFoundException(ExceptionConstants.ConfigurationKeyNotFoundExceptionMessage);

    /// <summary>
    /// The conversation history collection name configuration value.
    /// </summary>
    private readonly string ConversationHistoryCollectionName = configuration[MongoDbCollectionConstants.ConversationHistoryCollectionName]
        ?? throw new KeyNotFoundException(ExceptionConstants.ConfigurationKeyNotFoundExceptionMessage);

    /// <inheritdoc />
    public async Task<bool> ClearConversationHistoryByWorkspaceAsync(
        string workspaceId,
        string conversationId,
        string currentUserEmail,
        CancellationToken cancellationToken = default
    )
    {
        bool response = false;
        try
        {
            logger.LogAppInformation(
                LoggingConstants.MethodStartedMessageConstant,
                nameof(ClearConversationHistoryByWorkspaceAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { correlationContext.CorrelationId, workspaceId, conversationId, currentUserEmail })
            );

            var filter = Builders<ConversationHistoryModel>.Filter.Where(
                x => x.ConversationId == conversationId && x.IsActive && x.WorkspaceId == workspaceId && x.UserName == currentUserEmail
            );
            var allConversationHistoryData = await mongoDatabaseRepository.GetDataFromCollectionAsync(
                databaseName: this.MongoDatabaseName,
                collectionName: this.ConversationHistoryCollectionName,
                filter: filter,
                cancellationToken
            ).ConfigureAwait(false);

            response = allConversationHistoryData.Any() && await mongoDatabaseRepository.DeleteDataFromCollectionAsync(
                filter: filter,
                databaseName: this.MongoDatabaseName,
                collectionName: this.ConversationHistoryCollectionName,
                cancellationToken
            ).ConfigureAwait(false);
            return response;
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex,
                LoggingConstants.MethodFailedWithMessageConstant,
                nameof(ClearConversationHistoryByWorkspaceAsync), DateTime.UtcNow, ex.Message
            );
            throw new AIAgentsBusinessException(
                message: ex.Message,
                correlationId: correlationContext.CorrelationId
            );
        }
        finally
        {
            logger.LogAppInformation(
                LoggingConstants.MethodEndedMessageConstant,
                nameof(ClearConversationHistoryByWorkspaceAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { correlationContext.CorrelationId, workspaceId, conversationId, currentUserEmail, response })
            );
        }
    }

    /// <inheritdoc />
    public async Task<bool> ClearConversationHistoryForUserAsync(
        string userName,
        CancellationToken cancellationToken = default
    )
    {
        bool response = false;
        try
        {
            logger.LogAppInformation(
                LoggingConstants.MethodStartedMessageConstant,
                nameof(ClearConversationHistoryForUserAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { correlationContext.CorrelationId, userName })
            );

            var filter = Builders<ConversationHistoryModel>.Filter.Where(
                x => x.UserName == userName && x.IsActive && string.IsNullOrWhiteSpace(x.WorkspaceId)
            );
            var allConversationHistoryData = await mongoDatabaseRepository.GetDataFromCollectionAsync(
                databaseName: this.MongoDatabaseName,
                collectionName: this.ConversationHistoryCollectionName,
                filter: filter,
                cancellationToken
            ).ConfigureAwait(false);

            response = allConversationHistoryData.Any() && await mongoDatabaseRepository.DeleteDataFromCollectionAsync(
                filter: filter,
                databaseName: this.MongoDatabaseName,
                collectionName: this.ConversationHistoryCollectionName,
                cancellationToken
            ).ConfigureAwait(false);
            return response;
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex,
                LoggingConstants.MethodFailedWithMessageConstant,
                nameof(ClearConversationHistoryForUserAsync), DateTime.UtcNow, ex.Message
            );
            throw new AIAgentsBusinessException(
                message: ex.Message,
                correlationId: correlationContext.CorrelationId
            );
        }
        finally
        {
            logger.LogAppInformation(
                LoggingConstants.MethodEndedMessageConstant,
                nameof(ClearConversationHistoryForUserAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { correlationContext.CorrelationId, userName, response })
            );
        }
    }

    /// <inheritdoc />
    public async Task<ConversationHistoryDomain> GetConversationHistoryAsync(
        string userName,
        CancellationToken cancellationToken = default
    )
    {
        ConversationHistoryDomain response = new();
        try
        {
            logger.LogAppInformation(
                LoggingConstants.MethodStartedMessageConstant,
                nameof(GetConversationHistoryAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { correlationContext.CorrelationId, userName })
            );

            var allConversationHistoryData = await mongoDatabaseRepository.GetDataFromCollectionAsync(
                databaseName: this.MongoDatabaseName,
                collectionName: this.ConversationHistoryCollectionName,
                filter: Builders<ConversationHistoryModel>.Filter.Where(x => x.UserName == userName && x.IsActive && string.IsNullOrWhiteSpace(x.WorkspaceId)),
                cancellationToken
            ).ConfigureAwait(false);

            if (!allConversationHistoryData.Any())
                response = await this.InitializeWorkspaceConversationAsync(
                    workspaceGuid: string.Empty,
                    userOrApplicationName: userName,
                    conversationId: string.Empty,
                    cancellationToken
                ).ConfigureAwait(false);
            else
                response = MongoDataMapperProfile.MapToDomain(model: allConversationHistoryData.First());

            return response;
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex,
                LoggingConstants.MethodFailedWithMessageConstant,
                nameof(GetConversationHistoryAsync), DateTime.UtcNow, ex.Message
            );
            throw new AIAgentsBusinessException(
                message: ex.Message,
                correlationId: correlationContext.CorrelationId
            );
        }
        finally
        {
            logger.LogAppInformation(
                LoggingConstants.MethodEndedMessageConstant,
                nameof(GetConversationHistoryAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { correlationContext.CorrelationId, userName, response })
            );
        }
    }

    /// <inheritdoc />
    public async Task<ConversationHistoryDomain> GetConversationHistoryByWorkspaceAsync(
        string workspaceId,
        string conversationId,
        string currentUserEmail,
        CancellationToken cancellationToken = default
    )
    {
        ConversationHistoryDomain response = new();
        try
        {
            logger.LogAppInformation(
                LoggingConstants.MethodStartedMessageConstant,
                nameof(GetConversationHistoryByWorkspaceAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { correlationContext.CorrelationId, workspaceId, conversationId, currentUserEmail })
            );

            var conversationHistoryFilter = Builders<ConversationHistoryModel>.Filter
             .Where(item => item.IsActive && item.WorkspaceId == workspaceId);

            if (!string.IsNullOrWhiteSpace(currentUserEmail))
                conversationHistoryFilter &= Builders<ConversationHistoryModel>.Filter
                    .Where(item => item.UserName == currentUserEmail);

            if (!string.IsNullOrWhiteSpace(conversationId))
                conversationHistoryFilter &= Builders<ConversationHistoryModel>.Filter
                    .Where(item => item.ConversationId == conversationId);

            var allConversationHistoryData = await mongoDatabaseRepository.GetDataFromCollectionAsync(
                databaseName: this.MongoDatabaseName,
                collectionName: this.ConversationHistoryCollectionName,
                filter: conversationHistoryFilter,
                cancellationToken
            ).ConfigureAwait(false);

            if (!allConversationHistoryData.Any())
                response = await this.InitializeWorkspaceConversationAsync(
                    workspaceGuid: workspaceId,
                    userOrApplicationName: currentUserEmail,
                    conversationId,
                    cancellationToken
                ).ConfigureAwait(false);
            else
                response = allConversationHistoryData.Select(selector: MongoDataMapperProfile.MapToDomain).First();

            return response;
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex,
                LoggingConstants.MethodFailedWithMessageConstant,
                nameof(GetConversationHistoryByWorkspaceAsync), DateTime.UtcNow, ex.Message
            );
            throw new AIAgentsBusinessException(
                message: ex.Message,
                correlationId: correlationContext.CorrelationId
            );
        }
        finally
        {
            logger.LogAppInformation(
                LoggingConstants.MethodEndedMessageConstant,
                nameof(GetConversationHistoryByWorkspaceAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { correlationContext.CorrelationId, workspaceId, conversationId, currentUserEmail, response })
            );
        }
    }

    /// <inheritdoc />
    public async Task<ConversationHistoryDomain> InitializeWorkspaceConversationAsync(
        string workspaceGuid,
        string userOrApplicationName,
        string conversationId = "",
        CancellationToken cancellationToken = default
    )
    {
        ConversationHistoryDomain result = new();
        try
        {
            logger.LogAppInformation(
                LoggingConstants.MethodStartedMessageConstant,
                nameof(InitializeWorkspaceConversationAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { correlationContext.CorrelationId, workspaceGuid })
            );

            var conversationHistoryData = new ConversationHistoryModel()
            {
                UserName = userOrApplicationName,
                ChatHistory = [],
                ConversationId = string.IsNullOrWhiteSpace(conversationId) ? Guid.NewGuid().ToString() : conversationId,
                IsActive = true,
                LastModifiedOn = DateTime.UtcNow
            };

            await mongoDatabaseRepository.SaveDataAsync(
                data: conversationHistoryData,
                databaseName: this.MongoDatabaseName,
                collectionName: this.ConversationHistoryCollectionName,
                bypassDocumentValidation: false,
                cancellationToken
            ).ConfigureAwait(false);

            result = MongoDataMapperProfile.MapToDomain(model: conversationHistoryData);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex,
                LoggingConstants.MethodFailedWithMessageConstant,
                nameof(InitializeWorkspaceConversationAsync), DateTime.UtcNow, ex.Message
            );
            throw new AIAgentsBusinessException(
                message: ex.Message,
                correlationId: correlationContext.CorrelationId
            );
        }
        finally
        {
            logger.LogAppInformation(
                LoggingConstants.MethodEndedMessageConstant,
                nameof(InitializeWorkspaceConversationAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { correlationContext.CorrelationId, workspaceGuid, result })
            );
        }
    }

    /// <inheritdoc />
    public async Task<bool> SaveMessageToConversationHistoryAsync(
        ConversationHistoryDomain conversationHistory,
        CancellationToken cancellationToken = default
    )
    {
        bool response = false;
        try
        {
            logger.LogAppInformation(
                LoggingConstants.MethodStartedMessageConstant,
                nameof(GetConversationHistoryAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { correlationContext.CorrelationId, conversationHistory })
            );

            var filter = Builders<ConversationHistoryModel>.Filter
                .Where(x => x.ConversationId == conversationHistory.ConversationId && x.UserName == conversationHistory.UserName && x.IsActive);

            var allConversationHistoryData = await mongoDatabaseRepository.GetDataFromCollectionAsync(
                databaseName: this.MongoDatabaseName,
                collectionName: this.ConversationHistoryCollectionName,
                filter: filter,
                cancellationToken
            ).ConfigureAwait(false);

            var conversationHistoryData = allConversationHistoryData.FirstOrDefault()
                ?? throw new KeyNotFoundException(ExceptionConstants.DataNotFoundExceptionMessage);

            var dbConversationHistory = MongoDataMapperProfile.MapToModel(domain: conversationHistory);

            // Append any new chat history entries from the incoming conversation history.
            var updatedChatHistory = conversationHistoryData.ChatHistory.ToList();
            if (dbConversationHistory.ChatHistory.Count > updatedChatHistory.Count)
                updatedChatHistory.AddRange(
                    dbConversationHistory.ChatHistory.Skip(updatedChatHistory.Count)
                );

            // Update the existing document with the new chat history
            var update = Builders<ConversationHistoryModel>.Update
                .Set(x => x.ChatHistory, updatedChatHistory)
                .Set(x => x.LastModifiedOn, DateTime.UtcNow)
                .Set(x => x.IsActive, true);

            response = await mongoDatabaseRepository.UpdateDataInCollectionAsync(
                filter: filter,
                update: update,
                databaseName: this.MongoDatabaseName,
                collectionName: this.ConversationHistoryCollectionName,
                cancellationToken
            ).ConfigureAwait(false);
            return response;
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex,
                LoggingConstants.MethodFailedWithMessageConstant,
                nameof(SaveMessageToConversationHistoryAsync), DateTime.UtcNow, ex.Message
            );
            throw new AIAgentsBusinessException(
                message: ex.Message,
                correlationId: correlationContext.CorrelationId
            );
        }
        finally
        {
            logger.LogAppInformation(
                LoggingConstants.MethodEndedMessageConstant,
                nameof(SaveMessageToConversationHistoryAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { correlationContext.CorrelationId, conversationHistory, response })
            );
        }
    }
}
