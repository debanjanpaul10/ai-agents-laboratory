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
/// Provides an implementation of the INotificationsDataManager interface, responsible for handling data access operations related to notifications.
/// </summary>
/// <param name="correlationContext">The correlation context used to track and correlate logs and operations.</param>
/// <param name="logger">The ILogger instance used for logging information, warnings, and errors that occur within the data manager's methods. This helps in monitoring the application's behavior and diagnosing issues when they arise.</param>
/// <param name="configuration">The configuration instance used to access application settings.</param>
/// <param name="mongoDatabaseRepository">The MongoDB database repository used for data access operations.</param>
/// <seealso cref="INotificationsDataManager"/>
public sealed class NotificationsDataManager(
    ILogger<NotificationsDataManager> logger,
    ICorrelationContext correlationContext,
    IConfiguration configuration,
    IMongoDatabaseRepository mongoDatabaseRepository) : INotificationsDataManager
{
    /// <summary>
    /// The mongo database name configuration value.
    /// </summary>
    private readonly string MongoDatabaseName = configuration[MongoDbCollectionConstants.AiAgentsPrimaryDatabase]
        ?? throw new KeyNotFoundException(ExceptionConstants.ConfigurationKeyNotFoundExceptionMessage);

    /// <summary>
    /// The conversation history collection name configuration value.
    /// </summary>
    private readonly string NotificationsCollectionName = configuration[MongoDbCollectionConstants.NotificationsCollectionName]
        ?? throw new KeyNotFoundException(ExceptionConstants.ConfigurationKeyNotFoundExceptionMessage);

    /// <inheritdoc/>
    public async Task<bool> DeleteAllNotificationsForUserAsync(
        string currentLoggedInUser,
        CancellationToken cancellationToken = default
    )
    {
        bool response = false;
        try
        {
            logger.LogAppInformation(
                LoggingConstants.MethodStartedMessageConstant,
                nameof(DeleteAllNotificationsForUserAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedInUser })
            );

            var filter = Builders<NotificationsModel>.Filter.And(
                Builders<NotificationsModel>.Filter.Eq(n => n.RecipientUserName, currentLoggedInUser),
                Builders<NotificationsModel>.Filter.Eq(n => n.IsActive, true)
            );
            var updates = new List<UpdateDefinition<NotificationsModel>>
            {
                Builders<NotificationsModel>.Update.Set(field => field.IsActive, false),
                Builders<NotificationsModel>.Update.Set(field => field.DateModified, DateTime.UtcNow),
                Builders<NotificationsModel>.Update.Set(field => field.ModifiedBy, currentLoggedInUser)
            };
            var update = Builders<NotificationsModel>.Update.Combine(updates);
            response = await mongoDatabaseRepository.UpdateDataInCollectionAsync(
                databaseName: MongoDatabaseName,
                collectionName: NotificationsCollectionName,
                filter: filter,
                update: update,
                cancellationToken: cancellationToken
            ).ConfigureAwait(false);
            return response;
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex,
                LoggingConstants.MethodFailedWithMessageConstant,
                nameof(DeleteAllNotificationsForUserAsync), DateTime.UtcNow, ex.Message
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
                nameof(DeleteAllNotificationsForUserAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedInUser, response })
            );
        }
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<NotificationsDomain>> GetNotificationsForUserAsync(
        string recipientUserName,
        CancellationToken cancellationToken = default
    )
    {
        IEnumerable<NotificationsDomain>? response = null;
        try
        {
            logger.LogAppInformation(
                LoggingConstants.MethodStartedMessageConstant,
                nameof(GetNotificationsForUserAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, recipientUserName })
            );

            var filter = Builders<NotificationsModel>.Filter.And(
                Builders<NotificationsModel>.Filter.Eq(n => n.RecipientUserName, recipientUserName),
                Builders<NotificationsModel>.Filter.Eq(n => n.IsActive, true)
            );

            var allData = await mongoDatabaseRepository.GetDataFromCollectionAsync(
                databaseName: MongoDatabaseName,
                collectionName: NotificationsCollectionName,
                filter: filter,
                cancellationToken: cancellationToken
            ).ConfigureAwait(false);
            return [.. allData.Select(MongoDataMapperProfile.MapToDomain)];
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex,
                LoggingConstants.MethodFailedWithMessageConstant,
                nameof(GetNotificationsForUserAsync), DateTime.UtcNow, ex.Message
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
                nameof(GetNotificationsForUserAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, recipientUserName, response })
            );
        }
    }

    /// <inheritdoc/>
    public async Task<bool> MarkExistingNotificationAsReadAsync(
        string currentLoggedInUser,
        Guid notificationId,
        CancellationToken cancellationToken = default
    )
    {
        bool response = false;
        try
        {
            logger.LogAppInformation(
                LoggingConstants.MethodStartedMessageConstant,
                nameof(MarkExistingNotificationAsReadAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedInUser, notificationId })
            );

            var filter = Builders<NotificationsModel>.Filter.And(
                Builders<NotificationsModel>.Filter.Eq(n => n.RecipientUserName, currentLoggedInUser),
                Builders<NotificationsModel>.Filter.Eq(n => n.Id, notificationId),
                Builders<NotificationsModel>.Filter.Eq(n => n.IsActive, true)
            );
            var updates = new List<UpdateDefinition<NotificationsModel>>
            {
                Builders<NotificationsModel>.Update.Set(field => field.IsRead, true),
                Builders<NotificationsModel>.Update.Set(field => field.DateModified, DateTime.UtcNow),
                Builders<NotificationsModel>.Update.Set(field => field.ModifiedBy, currentLoggedInUser)
            };
            var update = Builders<NotificationsModel>.Update.Combine(updates);
            response = await mongoDatabaseRepository.UpdateDataInCollectionAsync(
                databaseName: MongoDatabaseName,
                collectionName: NotificationsCollectionName,
                filter: filter,
                update: update,
                cancellationToken: cancellationToken
            ).ConfigureAwait(false);
            return response;
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex,
                LoggingConstants.MethodFailedWithMessageConstant,
                nameof(MarkExistingNotificationAsReadAsync), DateTime.UtcNow, ex.Message
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
                nameof(MarkExistingNotificationAsReadAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedInUser, notificationId, response })
            );
        }
    }
}
