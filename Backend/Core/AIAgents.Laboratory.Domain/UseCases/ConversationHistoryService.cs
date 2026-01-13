using System.Globalization;
using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.Domain.DrivenPorts;
using AIAgents.Laboratory.Domain.DrivingPorts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using static AIAgents.Laboratory.Domain.Helpers.Constants;

namespace AIAgents.Laboratory.Domain.UseCases;

/// <summary>
/// The conversation history service.
/// </summary>
/// <param name="logger">The logger service.</param>
/// <param name="mongoDatabaseService">The mongo database service.</param>
/// <seealso cref="IConversationHistoryService"/>
public class ConversationHistoryService(ILogger<ConversationHistoryService> logger, IConfiguration configuration, IMongoDatabaseService mongoDatabaseService) : IConversationHistoryService
{
    /// <summary>
    /// The mongo database name configuration value.
    /// </summary>
    private readonly string MongoDatabaseName = configuration[MongoDbCollectionConstants.AiAgentsPrimaryDatabase] ?? throw new Exception(ExceptionConstants.ConfigurationKeyNotFoundExceptionMessage);

    /// <summary>
    /// The conversation history collection name configuration value.
    /// </summary>
    private readonly string ConversationHistoryCollectionName = configuration[MongoDbCollectionConstants.ConversationHistoryCollectionName] ?? throw new Exception(ExceptionConstants.ConfigurationKeyNotFoundExceptionMessage);

    /// <summary>
    /// Gets the conversation history data for current chat.
    /// </summary>
    /// <param name="userName">The user name.</param>
    /// <returns>The conversation history domain.</returns>
    public async Task<ConversationHistoryDomain> GetConversationHistoryAsync(string userName)
    {
        try
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodStart, nameof(GetConversationHistoryAsync), DateTime.UtcNow, userName));
            var allConversationHistoryData = await mongoDatabaseService.GetDataFromCollectionAsync(MongoDatabaseName, ConversationHistoryCollectionName,
                Builders<ConversationHistoryDomain>.Filter.Where(x => x.UserName == userName && x.IsActive)).ConfigureAwait(false);

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

                await mongoDatabaseService.SaveDataAsync(newConversationHistory, MongoDatabaseName, ConversationHistoryCollectionName).ConfigureAwait(false);
                return newConversationHistory;
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(GetConversationHistoryAsync), DateTime.UtcNow, ex.Message));
            throw;
        }
        finally
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodEnd, nameof(GetConversationHistoryAsync), DateTime.UtcNow, userName));
        }
    }

    /// <summary>
    /// Saves the current message data to conversation history.
    /// </summary>
    /// <param name="conversationHistory">The conversation history.</param>
    /// <returns>The boolean for success/failure.</returns>
    public async Task<bool> SaveMessageToConversationHistoryAsync(ConversationHistoryDomain conversationHistory)
    {
        try
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodStart, nameof(SaveMessageToConversationHistoryAsync), DateTime.UtcNow, conversationHistory.ConversationId));

            var filter = Builders<ConversationHistoryDomain>.Filter.Where(x => x.ConversationId == conversationHistory.ConversationId && x.UserName == conversationHistory.UserName);
            var allConversationHistoryData = await mongoDatabaseService.GetDataFromCollectionAsync(
                MongoDatabaseName, ConversationHistoryCollectionName, filter).ConfigureAwait(false);

            var conversationHistoryData = allConversationHistoryData.FirstOrDefault() ?? throw new Exception(ExceptionConstants.DataNotFoundExceptionMessage);

            // Add the new message to the existing chat history
            var updatedChatHistory = conversationHistoryData.ChatHistory.ToList();
            updatedChatHistory.Add(conversationHistory.ChatHistory.Last());

            // Update the existing document with the new chat history
            var update = Builders<ConversationHistoryDomain>.Update
                .Set(x => x.ChatHistory, updatedChatHistory)
                .Set(x => x.LastModifiedOn, DateTime.UtcNow)
                .Set(x => x.IsActive, true);
            return await mongoDatabaseService.UpdateDataInCollectionAsync(filter, update, MongoDatabaseName, ConversationHistoryCollectionName).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(SaveMessageToConversationHistoryAsync), DateTime.UtcNow, ex.Message));
            throw;
        }
        finally
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodEnd, nameof(SaveMessageToConversationHistoryAsync), DateTime.UtcNow, conversationHistory.ConversationId));
        }
    }

    /// <summary>
    /// Clears the conversation history data for the user.
    /// </summary>
    /// <param name="userName">The user name for user.</param>
    /// <returns>The boolean for success/failure.</returns>
    public async Task<bool> ClearConversationHistoryForUserAsync(string userName)
    {
        try
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodStart, nameof(ClearConversationHistoryForUserAsync), DateTime.UtcNow, userName));
            ArgumentException.ThrowIfNullOrEmpty(userName);

            var filter = Builders<ConversationHistoryDomain>.Filter.Where(x => x.UserName == userName && x.IsActive);
            var allConversationHistoryData = await mongoDatabaseService.GetDataFromCollectionAsync(
                MongoDatabaseName, ConversationHistoryCollectionName, filter).ConfigureAwait(false);
            return allConversationHistoryData.Any() && await mongoDatabaseService.DeleteDataFromCollectionAsync(filter, MongoDatabaseName, ConversationHistoryCollectionName).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(ClearConversationHistoryForUserAsync), DateTime.UtcNow, ex.Message));
            throw;
        }
        finally
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodEnd, nameof(ClearConversationHistoryForUserAsync), DateTime.UtcNow, userName));
        }
    }
}
