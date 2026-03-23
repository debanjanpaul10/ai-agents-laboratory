using AIAgents.Laboratory.Domain.Contracts;
using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.Domain.DrivenPorts;
using AIAgents.Laboratory.Domain.DrivingPorts;
using AIAgents.Laboratory.Domain.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Newtonsoft.Json;
using static AIAgents.Laboratory.Domain.Helpers.Constants;

namespace AIAgents.Laboratory.Domain.UseCases;

/// <summary>
/// Provides the services for the Registered Application Service, which provides operations to manage registered applications for the current logged in user, 
/// including creating, reading, updating, and deleting registered applications. 
/// </summary>
/// <remarks>This service acts as a driving port in the application architecture, orchestrating the business logic and interactions with the underlying data management layer through the use of data managers.</remarks>
/// <param name="logger">The logger service.</param>
/// <param name="configuration">The configuration service.</param>
/// <param name="correlationContext">The correlation context service.</param>
/// <param name="mongoDatabaseService">The mongo db database service.</param>
/// <seealso cref="IRegisteredApplicationService"/>
public sealed class RegisteredApplicationService(ILogger<RegisteredApplicationService> logger, ICorrelationContext correlationContext,
    IConfiguration configuration, IMongoDatabaseService mongoDatabaseService) : IRegisteredApplicationService
{
    /// <summary>
    /// The mongo database name configuration value.
    /// </summary>
    private readonly string MongoDatabaseName = configuration[MongoDbCollectionConstants.AiAgentsPrimaryDatabase]
        ?? throw new KeyNotFoundException(ExceptionConstants.ConfigurationKeyNotFoundExceptionMessage);

    /// <summary>
    /// The agents data collection name configuration value.
    /// </summary>
    private readonly string RegisteredApplicationsCollectionName = configuration[MongoDbCollectionConstants.RegisteredApplicationsCollectionName]
        ?? throw new KeyNotFoundException(ExceptionConstants.ConfigurationKeyNotFoundExceptionMessage);

    /// <summary>
    /// Creates a new registered application for the current logged in user with the provided application data.
    /// </summary>
    /// <param name="currentLoggedInUser">The current logged in user.</param>
    /// <param name="newApplicationData">The new application creation data model.</param>
    /// <returns>A boolean for success/failure.</returns>
    public async Task<bool> CreateNewRegisteredApplicationAsync(string currentLoggedInUser, RegisteredApplication newApplicationData)
    {
        ArgumentNullException.ThrowIfNull(newApplicationData);
        ArgumentException.ThrowIfNullOrWhiteSpace(currentLoggedInUser);

        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(CreateNewRegisteredApplicationAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedInUser, newApplicationData }));

            newApplicationData.PrepareAuditEntityData(currentLoggedInUser);
            return await mongoDatabaseService.SaveDataAsync(
                data: newApplicationData,
                databaseName: MongoDatabaseName,
                collectionName: RegisteredApplicationsCollectionName).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(CreateNewRegisteredApplicationAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(CreateNewRegisteredApplicationAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedInUser, newApplicationData }));
        }
    }

    /// <summary>
    /// Deletes a registered application by its ID for the current logged in user. 
    /// </summary>
    /// <remarks>The deletion is performed as a soft delete, where the IsActive property of the application is set to false instead of physically removing the record from the database.</remarks>
    /// <param name="currentLoggedInUser">The current logged in user.</param>
    /// <param name="applicationId">The application id for which data is to be deleted.</param>
    /// <returns>A boolean for success/failure.</returns>
    public async Task<bool> DeleteRegisteredApplicationByIdAsync(string currentLoggedInUser, int applicationId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(currentLoggedInUser);

        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(DeleteRegisteredApplicationByIdAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedInUser, applicationId }));

            var filter = Builders<RegisteredApplication>.Filter.Where(item => item.IsActive && item.Id == applicationId);
            var allApplications = await mongoDatabaseService.GetDataFromCollectionAsync(
                databaseName: this.MongoDatabaseName,
                collectionName: this.RegisteredApplicationsCollectionName,
                filter).ConfigureAwait(false);
            var updateApplication = allApplications.FirstOrDefault() ?? throw new KeyNotFoundException(ExceptionConstants.DataNotFoundExceptionMessage);

            if (updateApplication.CreatedBy != currentLoggedInUser)
                throw new UnauthorizedAccessException(ExceptionConstants.UnauthorizedUserExceptionMessage);

            var updates = new List<UpdateDefinition<RegisteredApplication>>
            {
                Builders<RegisteredApplication>.Update.Set(x => x.IsActive, false),
                Builders<RegisteredApplication>.Update.Set(x => x.DateModified, DateTime.UtcNow)
            };
            var update = Builders<RegisteredApplication>.Update.Combine(updates);
            return await mongoDatabaseService.UpdateDataInCollectionAsync(
                filter,
                update,
                databaseName: this.MongoDatabaseName,
                collectionName: this.RegisteredApplicationsCollectionName).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(DeleteRegisteredApplicationByIdAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(DeleteRegisteredApplicationByIdAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedInUser, applicationId }));
        }
    }

    /// <summary>
    /// Gets the details of a specific registered application by its ID for the current logged in user.
    /// </summary>
    /// <param name="currentLoggedInUser">The current logged in user.</param>
    /// <param name="applicationId">The application id to be searched for.</param>
    /// <returns>The registered application data model.</returns>
    public async Task<RegisteredApplication> GetRegisteredApplicationByIdAsync(string currentLoggedInUser, int applicationId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(currentLoggedInUser);

        RegisteredApplication? result = null;
        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(GetRegisteredApplicationByIdAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedInUser, applicationId }));

            var filter = Builders<RegisteredApplication>.Filter.And(
                Builders<RegisteredApplication>.Filter.Eq(x => x.IsActive, true),
                Builders<RegisteredApplication>.Filter.Eq(x => x.Id, applicationId));

            var allData = await mongoDatabaseService.GetDataFromCollectionAsync(
                databaseName: this.MongoDatabaseName,
                collectionName: this.RegisteredApplicationsCollectionName,
                filter).ConfigureAwait(false);

            result = allData.FirstOrDefault() ?? throw new KeyNotFoundException(ExceptionConstants.DataNotFoundExceptionMessage);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(GetRegisteredApplicationByIdAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetRegisteredApplicationByIdAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedInUser, applicationId, result }));
        }
    }

    /// <summary>
    /// Gets the list of registered applications for the current logged in user.
    /// </summary>
    /// <param name="currentLoggedInUser">The current logged in user.</param>
    /// <returns>The list of <see cref="RegisteredApplication"/></returns>
    public async Task<IEnumerable<RegisteredApplication>> GetRegisteredApplicationsAsync(string currentLoggedInUser)
    {
        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(GetRegisteredApplicationsAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedInUser }));

            var filter = Builders<RegisteredApplication>.Filter.And(
                Builders<RegisteredApplication>.Filter.Eq(x => x.IsActive, true));

            return await mongoDatabaseService.GetDataFromCollectionAsync(
                databaseName: this.MongoDatabaseName,
                collectionName: this.RegisteredApplicationsCollectionName,
                filter).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(GetRegisteredApplicationsAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetRegisteredApplicationsAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedInUser }));
        }
    }

    /// <summary>
    /// Updates an existing registered application for the current logged in user with the provided application data. 
    /// </summary>
    /// <remarks>The application to be updated is identified by the Id property of the provided application data model.</remarks>
    /// <param name="currentLoggedInUser">The current logged in user.</param>
    /// <param name="updateApplicationData">The update application data model.</param>
    /// <returns>A boolean for success/failure.</returns>
    public async Task<bool> UpdateExistingRegisteredApplicationAsync(string currentLoggedInUser, RegisteredApplication updateApplicationData)
    {
        bool response = false;
        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(UpdateExistingRegisteredApplicationAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedInUser, updateApplicationData }));

            var filter = Builders<RegisteredApplication>.Filter.And(
                Builders<RegisteredApplication>.Filter.Eq(x => x.IsActive, true), Builders<RegisteredApplication>.Filter.Eq(x => x.Id, updateApplicationData.Id));
            var updates = new List<UpdateDefinition<RegisteredApplication>>
            {
                Builders<RegisteredApplication>.Update.Set(ra => ra.ApplicationName, updateApplicationData.ApplicationName),
                Builders<RegisteredApplication>.Update.Set(ra => ra.Description, updateApplicationData.Description),
                Builders<RegisteredApplication>.Update.Set(ra => ra.ApplicationRegistrationGuid, updateApplicationData.ApplicationRegistrationGuid),
                Builders<RegisteredApplication>.Update.Set(ra => ra.DateModified, DateTime.UtcNow),
                Builders<RegisteredApplication>.Update.Set(ra => ra.ModifiedBy, currentLoggedInUser)
            };

            var update = Builders<RegisteredApplication>.Update.Combine(updates);
            response = await mongoDatabaseService.UpdateDataInCollectionAsync(
                filter,
                update,
                databaseName: this.MongoDatabaseName,
                collectionName: this.RegisteredApplicationsCollectionName).ConfigureAwait(false);
            return response;
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(UpdateExistingRegisteredApplicationAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(UpdateExistingRegisteredApplicationAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedInUser, response }));
        }
    }
}
