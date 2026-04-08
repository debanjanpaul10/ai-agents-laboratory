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
/// Provides an implementation of the INotificationsDataManager interface, responsible for handling data access operations related to notifications.
/// </summary>
/// <param name="configuration">The configuration instance used to access application settings.</param>
/// <param name="mapper">The AutoMapper instance used for object mapping.</param>
/// <param name="mongoDatabaseRepository">The MongoDB database repository used for data access operations.</param>
/// <seealso cref="INotificationsDataManager"/>
public sealed class NotificationsDataManager(
    IConfiguration configuration,
    IMapper mapper,
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

    /// <summary>
    /// Retrieves a list of notifications for a specific user based on their username. 
    /// This method allows clients to fetch all notifications that are relevant to a particular user
    /// </summary>
    /// <param name="recipientUserName">The username of the user for whom to retrieve notifications.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation if needed.</param>
    /// <returns>A list of notifications relevant to the specified user.</returns>
    public async Task<IEnumerable<NotificationsDomain>> GetNotificationsForUserAsync(
        string recipientUserName,
        CancellationToken cancellationToken = default)
    {
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
        return mapper.Map<IEnumerable<NotificationsDomain>>(allData);
    }
}
