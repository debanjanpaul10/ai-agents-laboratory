using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.Domain.Ports.Out;
using AIAgents.Laboratory.Persistence.MongoDatabase.Contracts;
using AIAgents.Laboratory.Persistence.MongoDatabase.Models;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using static AIAgents.Laboratory.Persistence.MongoDatabase.Helpers.Constants;

namespace AIAgents.Laboratory.Persistence.MongoDatabase.DataManager;

/// <summary>
/// Implements the IConversationHistoryDataManager interface to manage conversation history data using MongoDB as the underlying data store.
/// </summary>
/// <remarks>This class provides methods to retrieve, save, and clear conversation history for chat users, interacting with the MongoDB database through the IMongoDatabaseRepository abstraction. 
/// It uses AutoMapper for mapping between domain entities and MongoDB models, and relies on configuration settings for database and collection names. 
/// All operations are asynchronous to ensure non-blocking data access and manipulation.</remarks>
/// <param name="configuration">The configuration settings for the MongoDB database.</param>
/// <param name="mapper">The AutoMapper instance for mapping between domain entities and MongoDB models.</param>
/// <param name="mongoDatabaseRepository">The MongoDB database repository for data access operations.</param>
public sealed class ConversationHistoryDataManager(IConfiguration configuration, IMapper mapper, IMongoDatabaseRepository mongoDatabaseRepository) : IConversationHistoryDataManager
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

    /// <summary>
    /// Clears the conversation history data for the user.
    /// </summary>
    /// <param name="userName">The user name for user.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The boolean for success/failure.</returns>
    public async Task<bool> ClearConversationHistoryForUserAsync(string userName, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ConversationHistoryModel>.Filter.Where(x => x.UserName == userName && x.IsActive);
        var allConversationHistoryData = await mongoDatabaseRepository.GetDataFromCollectionAsync(
            databaseName: this.MongoDatabaseName,
            collectionName: this.ConversationHistoryCollectionName,
            filter: filter,
            cancellationToken
        ).ConfigureAwait(false);

        return allConversationHistoryData.Any() && await mongoDatabaseRepository.DeleteDataFromCollectionAsync(
            filter: filter,
            databaseName: this.MongoDatabaseName,
            collectionName: this.ConversationHistoryCollectionName,
            cancellationToken
        ).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the conversation history data for current chat.
    /// </summary>
    /// <param name="userName">The user name.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The conversation history domain.</returns>
    public async Task<ConversationHistoryDomain> GetConversationHistoryAsync(string userName, CancellationToken cancellationToken = default)
    {
        var allConversationHistoryData = await mongoDatabaseRepository.GetDataFromCollectionAsync(
                databaseName: this.MongoDatabaseName,
                collectionName: this.ConversationHistoryCollectionName,
                filter: Builders<ConversationHistoryModel>.Filter.Where(x => x.UserName == userName && x.IsActive),
                cancellationToken
            ).ConfigureAwait(false);

        if (allConversationHistoryData.Any())
        {
            return mapper.Map<ConversationHistoryDomain>(allConversationHistoryData.First());
        }
        else
        {
            var newConversationHistory = new ConversationHistoryModel()
            {
                UserName = userName,
                ChatHistory = [],
                ConversationId = Guid.NewGuid().ToString(),
                IsActive = true,
                LastModifiedOn = DateTime.UtcNow
            };

            await mongoDatabaseRepository.SaveDataAsync(
                data: newConversationHistory,
                databaseName: this.MongoDatabaseName,
                collectionName: this.ConversationHistoryCollectionName,
                cancellationToken
            ).ConfigureAwait(false);
            return mapper.Map<ConversationHistoryDomain>(newConversationHistory);
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
        var filter = Builders<ConversationHistoryModel>.Filter
            .Where(x => x.ConversationId == conversationHistory.ConversationId && x.UserName == conversationHistory.UserName);

        var allConversationHistoryData = await mongoDatabaseRepository.GetDataFromCollectionAsync(
            databaseName: this.MongoDatabaseName,
            collectionName: this.ConversationHistoryCollectionName,
            filter: filter,
            cancellationToken
        ).ConfigureAwait(false);

        var conversationHistoryData = allConversationHistoryData.FirstOrDefault() ?? throw new KeyNotFoundException(ExceptionConstants.DataNotFoundExceptionMessage);
        var dbConversationHistory = mapper.Map<ConversationHistoryModel>(conversationHistory);

        // Add the new message to the existing chat history
        var updatedChatHistory = conversationHistoryData.ChatHistory.ToList();
        updatedChatHistory.Add(dbConversationHistory.ChatHistory[^1]);

        // Update the existing document with the new chat history
        var update = Builders<ConversationHistoryModel>.Update
            .Set(x => x.ChatHistory, updatedChatHistory)
            .Set(x => x.LastModifiedOn, DateTime.UtcNow)
            .Set(x => x.IsActive, true);

        return await mongoDatabaseRepository.UpdateDataInCollectionAsync(
            filter: filter,
            update: update,
            databaseName: this.MongoDatabaseName,
            collectionName: this.ConversationHistoryCollectionName,
            cancellationToken
        ).ConfigureAwait(false);
    }
}
