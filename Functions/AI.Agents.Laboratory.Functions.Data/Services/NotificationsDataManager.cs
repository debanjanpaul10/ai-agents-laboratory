using AI.Agents.Laboratory.Functions.Data.Contracts;
using AI.Agents.Laboratory.Functions.Data.Helpers;
using AI.Agents.Laboratory.Functions.Shared.Constants;
using AI.Agents.Laboratory.Functions.Shared.Exceptions;
using AI.Agents.Laboratory.Functions.Shared.Helpers;
using AI.Agents.Laboratory.Functions.Shared.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AI.Agents.Laboratory.Functions.Data.Services;

/// <summary>
/// Implements the INotificationsDataManager to provide methods for saving push notification data to a data store. 
/// </summary>
/// <remarks>
/// This class abstracts the underlying data access implementation, allowing for flexibility and separation of concerns in the business logic layer. 
/// It uses an instance of IMongoDatabaseRepository to interact with a MongoDB database for data persistence, and includes logging and exception handling to ensure robust operations.
/// </remarks>
/// <param name="configuration">The configuration object.</param>
/// <param name="mongoDatabaseRepository">The MongoDB database repository.</param>
/// <param name="correlationContext">The correlation context.</param>
/// <param name="logger">The logger.</param>
/// <seealso cref="INotificationsDataManager"/>
public sealed class NotificationsDataManager(
    IConfiguration configuration,
    IMongoDatabaseRepository mongoDatabaseRepository,
    ICorrelationContext correlationContext,
    ILogger<NotificationsDataManager> logger) : INotificationsDataManager
{
    /// <summary>
    /// The name of the MongoDB database and collection are retrieved from the configuration settings, with error handling to manage missing configuration values. 
    /// </summary>
    private readonly string MongoDatabaseName = configuration[MongoDbConfigurationConstants.AiAgentsPrimaryDatabase]
        ?? throw new KeyNotFoundException(ExceptionConstants.MissingConfigurationMessage);

    /// <summary>
    /// The name of the MongoDB collection for notifications is retrieved from the configuration settings, with error handling to manage missing configuration values. 
    /// </summary>
    private readonly string NotificationsCollectionName = configuration[MongoDbConfigurationConstants.NotificationsCollectionName]
        ?? throw new KeyNotFoundException(ExceptionConstants.MissingConfigurationMessage);

    /// <inheritdoc/>
    public async Task<bool> SavePushNotificationsDataAsync(
        NotificationRequest request,
        CancellationToken cancellationToken = default
    )
    {
        bool response = false;
        try
        {
            logger.LogAppInformation(
                LoggerConstants.LogHelperMethodStart,
                nameof(SavePushNotificationsDataAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, request })
            );

            var document = DatabaseUtilities.PrepareNotificationDataModel(
                request
            );
            response = await mongoDatabaseRepository.SaveDataAsync(
                data: document,
                databaseName: MongoDatabaseName,
                collectionName: NotificationsCollectionName,
                bypassDocumentValidation: true,
                cancellationToken: cancellationToken
            ).ConfigureAwait(false);
            return response;
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex,
                LoggerConstants.LogHelperMethodFailed,
                nameof(SavePushNotificationsDataAsync), DateTime.UtcNow, ex.Message
            );
            throw new AIAgentsBusinessException(
                message: ex.Message,
                correlationId: correlationContext.CorrelationId
            );
        }
        finally
        {
            logger.LogAppInformation(
                LoggerConstants.LogHelperMethodEnd,
                nameof(SavePushNotificationsDataAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, request, response })
            );
        }
    }
}
