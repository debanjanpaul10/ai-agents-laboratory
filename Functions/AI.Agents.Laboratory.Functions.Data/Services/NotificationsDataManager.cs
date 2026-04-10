using AI.Agents.Laboratory.Functions.Data.Contracts;
using AI.Agents.Laboratory.Functions.Data.Models;
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
    /// The name of the MongoDB database and collection are retrieved from the configuration settings, with error handling to manage missing configuration values. The class uses these values to interact with the MongoDB database for saving push notification data. The SavePushNotificationsDataAsync method implements the logic to save a NotificationRequest to the database, including logging at the start and end of the operation, as well as error logging in case of exceptions. It constructs a NotificationModel from the request data and uses the mongoDatabaseRepository to persist it to the specified collection. The method returns a boolean indicating the success of the operation.
    /// </summary>
    private readonly string MongoDatabaseName = configuration[MongoDbConfigurationConstants.AiAgentsPrimaryDatabase]
        ?? throw new KeyNotFoundException(ExceptionConstants.MissingConfigurationMessage);

    /// <summary>
    /// The name of the MongoDB collection for notifications is retrieved from the configuration settings, with error handling to manage missing configuration values. This collection name is used in the SavePushNotificationsDataAsync method to specify where the notification data should be saved in the MongoDB database. The method constructs a NotificationModel from the incoming NotificationRequest and uses the mongoDatabaseRepository to save it to the specified collection, returning a boolean indicating the success of the operation. Logging is implemented throughout the method to track the processing flow and any issues that arise during data persistence.
    /// </summary>
    private readonly string NotificationsCollectionName = configuration[MongoDbConfigurationConstants.NotificationsCollectionName]
        ?? throw new KeyNotFoundException(ExceptionConstants.MissingConfigurationMessage);

    /// <summary>
    /// Saves push notification data to a data store, returning a boolean indicating the success of the operation. The method takes a NotificationRequest object as input, which contains the details of the notification to be saved. The implementation of this method is responsible for handling the actual data persistence logic, including any necessary transformations or validations before saving the data. It constructs a NotificationModel from the request data and uses the mongoDatabaseRepository to persist it to the specified collection in the MongoDB database. The method includes logging at the start and end of the operation, as well as error logging in case of exceptions, and throws an AIAgentsBusinessException if any issues occur during the process.
    /// </summary>
    /// <param name="request">The notification request containing the details to be saved.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation, with a boolean indicating success.</returns>
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

            var document = PrepareNotificationDataModel(request);
            response = await mongoDatabaseRepository.SaveDataAsync(
                data: document,
                databaseName: MongoDatabaseName,
                collectionName: NotificationsCollectionName,
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

    #region PRIVATE METHODS

    /// <summary>
    /// Prepares a <c>NotificationModel</c> from a given NotificationRequest, mapping the relevant properties and setting the creation and modification timestamps. 
    /// This method is used to transform the incoming request data into a format suitable for persistence in the MongoDB database. 
    /// </summary>
    /// <param name="request">The notification request containing the details to be transformed into a NotificationModel.</param>
    /// <returns>A <c>NotificationModel</c> object populated with data from the request and additional metadata such as timestamps.</returns>
    private static NotificationModel PrepareNotificationDataModel(NotificationRequest request) =>
        new()
        {
            Title = request.Title,
            Message = request.Message,
            RecipientUserName = request.RecipientUserName,
            NotificationType = request.NotificationType,
            CreatedBy = request.CreatedBy,
            IsGlobal = request.IsGlobal,
            DateCreated = DateTime.UtcNow,
            DateModified = DateTime.UtcNow,
            ModifiedBy = request.CreatedBy
        };

    #endregion
}
