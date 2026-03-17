using AIAgents.Laboratory.Domain.Contracts;
using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.Domain.DrivenPorts;
using AIAgents.Laboratory.Domain.Helpers;
using AIAgents.Laboratory.Persistence.SQLDatabase.Contracts;
using AIAgents.Laboratory.Persistence.SQLDatabase.Models;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using static AIAgents.Laboratory.Persistence.SQLDatabase.Helpers.Constants;

namespace AIAgents.Laboratory.Persistence.SQLDatabase.DataManagers;

/// <summary>
/// Provides the services for managing registered application data, including operations to create, read, update, and delete registered applications for the current logged in user.
/// </summary>
/// <param name="unitOfWork">The database unit of work service.</param>
/// <param name="logger">The application logger service.</param>
/// <param name="mapper">The auto mapper service.</param>
/// <param name="correlationContext">The correlation context service.</param>
/// <seealso cref="IRegisteredApplicationDataManager"/>
public sealed class RegisteredApplicationDataManager(IUnitOfWork unitOfWork, ILogger<RegisteredApplicationDataManager> logger, IMapper mapper, ICorrelationContext correlationContext) : IRegisteredApplicationDataManager
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
            logger.LogInformation(LoggingConstants.MethodStartedMessageConstant, nameof(CreateNewRegisteredApplicationAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedInUser, newApplicationData }));

            var dataEntityModel = mapper.Map<RegisteredApplicationEntity>(newApplicationData);
            await unitOfWork.Repository<RegisteredApplicationEntity>().AddAsync(dataEntityModel).ConfigureAwait(false);
            await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, LoggingConstants.MethodFailedWithMessageConstant, nameof(CreateNewRegisteredApplicationAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message);
        }
        finally
        {
            logger.LogInformation(LoggingConstants.MethodEndedMessageConstant, nameof(CreateNewRegisteredApplicationAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedInUser, newApplicationData }));
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
            logger.LogInformation(LoggingConstants.MethodStartedMessageConstant, nameof(DeleteRegisteredApplicationByIdAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedInUser, applicationId }));
            var result = await unitOfWork.Repository<RegisteredApplicationEntity>().FirstOrDefaultAsync(x => x.IsActive && x.Id == applicationId).ConfigureAwait(false);
            if (result is not null)
            {
                result.IsActive = false;
                result.DateModified = DateTime.UtcNow;
                result.ModifiedBy = currentLoggedInUser;

                unitOfWork.Repository<RegisteredApplicationEntity>().Update(result);
                await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, LoggingConstants.MethodFailedWithMessageConstant, nameof(DeleteRegisteredApplicationByIdAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message);
        }
        finally
        {
            logger.LogInformation(LoggingConstants.MethodEndedMessageConstant, nameof(DeleteRegisteredApplicationByIdAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedInUser, applicationId }));
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
            logger.LogInformation(LoggingConstants.MethodStartedMessageConstant, nameof(GetRegisteredApplicationByIdAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedInUser, applicationId }));
            var result = await unitOfWork.Repository<RegisteredApplicationEntity>().FirstOrDefaultAsync(x => x.IsActive && x.Id == applicationId).ConfigureAwait(false);
            return mapper.Map<RegisteredApplication>(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, LoggingConstants.MethodFailedWithMessageConstant, nameof(GetRegisteredApplicationByIdAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message);
        }
        finally
        {
            logger.LogInformation(LoggingConstants.MethodEndedMessageConstant, nameof(GetRegisteredApplicationByIdAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedInUser, applicationId }));
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
            logger.LogInformation(LoggingConstants.MethodStartedMessageConstant, nameof(GetRegisteredApplicationsAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedInUser }));

            var result = await unitOfWork.Repository<RegisteredApplicationEntity>().GetAllAsync(x => x.IsActive).ConfigureAwait(false);
            return mapper.Map<IEnumerable<RegisteredApplication>>(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, LoggingConstants.MethodFailedWithMessageConstant, nameof(GetRegisteredApplicationsAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message);
        }
        finally
        {
            logger.LogInformation(LoggingConstants.MethodEndedMessageConstant, nameof(GetRegisteredApplicationsAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedInUser }));
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
            logger.LogInformation(LoggingConstants.MethodStartedMessageConstant, nameof(UpdateExistingRegisteredApplicationAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedInUser, updateApplicationData }));

            var result = await unitOfWork.Repository<RegisteredApplicationEntity>().GetAllAsync(x => x.IsActive && x.Id == updateApplicationData.Id).ConfigureAwait(false);
            if (result is not null)
            {
                var dataEntityModel = mapper.Map<RegisteredApplicationEntity>(updateApplicationData);
                unitOfWork.Repository<RegisteredApplicationEntity>().Update(dataEntityModel);
                await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, LoggingConstants.MethodFailedWithMessageConstant, nameof(UpdateExistingRegisteredApplicationAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message);
        }
        finally
        {
            logger.LogInformation(LoggingConstants.MethodEndedMessageConstant, nameof(UpdateExistingRegisteredApplicationAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedInUser, updateApplicationData }));
        }
    }
}
