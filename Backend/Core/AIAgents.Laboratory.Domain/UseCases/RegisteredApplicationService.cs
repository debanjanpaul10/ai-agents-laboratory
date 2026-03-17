using AIAgents.Laboratory.Domain.Contracts;
using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.Domain.DrivenPorts;
using AIAgents.Laboratory.Domain.DrivingPorts;
using AIAgents.Laboratory.Domain.Helpers;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using static AIAgents.Laboratory.Domain.Helpers.Constants;

namespace AIAgents.Laboratory.Domain.UseCases;

/// <summary>
/// Provides the services for the Registered Application Service, which provides operations to manage registered applications for the current logged in user, 
/// including creating, reading, updating, and deleting registered applications. 
/// </summary>
/// <remarks>This service acts as a driving port in the application architecture, orchestrating the business logic and interactions with the underlying data management layer through the use of data managers.</remarks>
/// <param name="logger">The logger service.</param>
/// <param name="registeredApplicationDataManager">The registered application data manager service.</param>
/// <param name="correlationContext">The correlation context service.</param>
/// <seealso cref="IRegisteredApplicationService"/>
public sealed class RegisteredApplicationService(ILogger<RegisteredApplicationService> logger, IRegisteredApplicationDataManager registeredApplicationDataManager, ICorrelationContext correlationContext) : IRegisteredApplicationService
{
    /// <summary>
    /// Creates a new registered application for the current logged in user with the provided application data.
    /// </summary>
    /// <param name="currentLoggedInUser">The current logged in user.</param>
    /// <param name="newApplicationData">The new application creation data model.</param>
    /// <returns>A boolean for success/failure.</returns>
    public async Task<bool> CreateNewRegisteredApplicationAsync(string currentLoggedInUser, RegisteredApplication newApplicationData)
    {
        try
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(CreateNewRegisteredApplicationAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedInUser, newApplicationData }));

            newApplicationData.PrepareAuditEntityData(currentLoggedInUser);
            return await registeredApplicationDataManager.CreateNewRegisteredApplicationAsync(currentLoggedInUser, newApplicationData).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, LoggingConstants.LogHelperMethodFailed, nameof(CreateNewRegisteredApplicationAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message);
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(CreateNewRegisteredApplicationAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedInUser, newApplicationData }));
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
        try
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(DeleteRegisteredApplicationByIdAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedInUser, applicationId }));
            return await registeredApplicationDataManager.DeleteRegisteredApplicationByIdAsync(currentLoggedInUser, applicationId).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, LoggingConstants.LogHelperMethodFailed, nameof(DeleteRegisteredApplicationByIdAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message);
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(DeleteRegisteredApplicationByIdAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedInUser, applicationId }));
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
        try
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(GetRegisteredApplicationByIdAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedInUser, applicationId }));
            return await registeredApplicationDataManager.GetRegisteredApplicationByIdAsync(currentLoggedInUser, applicationId).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, LoggingConstants.LogHelperMethodFailed, nameof(GetRegisteredApplicationByIdAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message);
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetRegisteredApplicationByIdAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedInUser, applicationId }));
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
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(GetRegisteredApplicationsAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedInUser }));
            return await registeredApplicationDataManager.GetRegisteredApplicationsAsync(currentLoggedInUser).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, LoggingConstants.LogHelperMethodFailed, nameof(GetRegisteredApplicationsAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message);
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetRegisteredApplicationsAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedInUser }));
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
        try
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(UpdateExistingRegisteredApplicationAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedInUser, updateApplicationData }));

            updateApplicationData.ModifiedBy = currentLoggedInUser;
            updateApplicationData.DateModified = DateTime.UtcNow;
            return await registeredApplicationDataManager.UpdateExistingRegisteredApplicationAsync(currentLoggedInUser, updateApplicationData).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, LoggingConstants.LogHelperMethodFailed, nameof(UpdateExistingRegisteredApplicationAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message);
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(UpdateExistingRegisteredApplicationAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedInUser, updateApplicationData }));
        }
    }
}
