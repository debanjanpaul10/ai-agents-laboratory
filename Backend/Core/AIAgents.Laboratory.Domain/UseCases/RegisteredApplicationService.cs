using AIAgents.Laboratory.Domain.Contracts;
using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.Domain.Helpers;
using AIAgents.Laboratory.Domain.Ports.In;
using AIAgents.Laboratory.Domain.Ports.Out;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using static AIAgents.Laboratory.Domain.Helpers.Constants;

namespace AIAgents.Laboratory.Domain.UseCases;

/// <summary>
/// Provides the services for the Registered Application Service, which provides operations to manage registered applications for the current logged in user, 
/// including creating, reading, updating, and deleting registered applications. 
/// </summary>
/// <remarks>This service acts as a driving port in the application architecture, orchestrating the business logic and interactions with the underlying data management layer through the use of data managers.</remarks>
/// <param name="correlationContext">The correlation context for logging and tracing purposes.</param>
/// <param name="logger">The logger instance for logging information and errors.</param>
/// <param name="registeredApplicationDataManager">The registered application data manager for performing data operations related to registered applications.</param>
/// <param name="notificationsService">The notifications service for sending notifications related to registered application operations.</param>
/// <seealso cref="IRegisteredApplicationService"/>
public sealed class RegisteredApplicationService(
    ILogger<RegisteredApplicationService> logger,
    ICorrelationContext correlationContext,
    IRegisteredApplicationDataManager registeredApplicationDataManager,
    INotificationsService notificationsService) : IRegisteredApplicationService
{
    /// <summary>
    /// Creates a new registered application for the current logged in user with the provided application data.
    /// </summary>
    /// <param name="currentLoggedInUser">The current logged in user.</param>
    /// <param name="newApplicationData">The new application creation data model.</param>
    /// <param name="cancellationToken">The cancellation token used to cancel the asynchronous operation. Optional.</param>
    /// <returns>A boolean for success/failure.</returns>
    public async Task<bool> CreateNewRegisteredApplicationAsync(
        string currentLoggedInUser,
        RegisteredApplicationDomain newApplicationData,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(newApplicationData);
        ArgumentException.ThrowIfNullOrWhiteSpace(currentLoggedInUser);
        bool response = false;
        try
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodStart,
                nameof(CreateNewRegisteredApplicationAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedInUser, newApplicationData })
            );

            newApplicationData.PrepareAuditEntityData(currentUser: currentLoggedInUser);
            response = await registeredApplicationDataManager.CreateNewRegisteredApplicationAsync(
                currentLoggedInUser,
                newApplicationData,
                cancellationToken
            ).ConfigureAwait(false);
            return response;
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex,
                LoggingConstants.LogHelperMethodFailed, nameof(CreateNewRegisteredApplicationAsync), DateTime.UtcNow, ex.Message
            );
            throw new AIAgentsBusinessException(
                message: ex.Message,
                correlationId: correlationContext.CorrelationId
            );
        }
        finally
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodEnd,
                nameof(CreateNewRegisteredApplicationAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedInUser, response })
            );
        }
    }

    /// <summary>
    /// Deletes a registered application by its ID for the current logged in user. 
    /// </summary>
    /// <remarks>The deletion is performed as a soft delete, where the IsActive property of the application is set to false instead of physically removing the record from the database.</remarks>
    /// <param name="currentLoggedInUser">The current logged in user.</param>
    /// <param name="applicationId">The application id for which data is to be deleted.</param>
    /// <param name="cancellationToken">The cancellation token used to cancel the asynchronous operation. Optional.</param>
    /// <returns>A boolean for success/failure.</returns>
    public async Task<bool> DeleteRegisteredApplicationByIdAsync(
        string currentLoggedInUser,
        int applicationId,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(currentLoggedInUser);

        bool response = false;
        try
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodStart,
                nameof(DeleteRegisteredApplicationByIdAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedInUser, applicationId })
            );

            response = await registeredApplicationDataManager.DeleteRegisteredApplicationByIdAsync(
                currentLoggedInUser,
                applicationId,
                cancellationToken
            ).ConfigureAwait(false);
            if (response)
                await this.SendRegisteredApplicationUpdateNotificationAsync(
                    userToBeNotified: currentLoggedInUser,
                    currentUserEmail: currentLoggedInUser,
                    applicationName: applicationId.ToString(),
                    applicationGuid: applicationId.ToString(),
                    cancellationToken: cancellationToken
                ).ConfigureAwait(false);

            return response;
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex,
                LoggingConstants.LogHelperMethodFailed, nameof(DeleteRegisteredApplicationByIdAsync), DateTime.UtcNow, ex.Message
            );
            throw new AIAgentsBusinessException(
                message: ex.Message,
                correlationId: correlationContext.CorrelationId
            );
        }
        finally
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodEnd,
                nameof(DeleteRegisteredApplicationByIdAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedInUser, applicationId })
            );
        }
    }

    /// <summary>
    /// Gets the details of a specific registered application by its ID for the current logged in user.
    /// </summary>
    /// <param name="currentLoggedInUser">The current logged in user.</param>
    /// <param name="applicationId">The application id to be searched for.</param>
    /// <param name="cancellationToken">The cancellation token used to cancel the asynchronous operation. Optional.</param>
    /// <returns>The registered application data model.</returns>
    public async Task<RegisteredApplicationDomain> GetRegisteredApplicationByIdAsync(
        string currentLoggedInUser,
        int applicationId,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(currentLoggedInUser);
        RegisteredApplicationDomain? result = null;
        try
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodStart,
                nameof(GetRegisteredApplicationByIdAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedInUser, applicationId })
            );

            result = await registeredApplicationDataManager.GetRegisteredApplicationByIdAsync(
                currentLoggedInUser,
                applicationId,
                cancellationToken
            ).ConfigureAwait(false);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex,
                LoggingConstants.LogHelperMethodFailed,
                nameof(GetRegisteredApplicationByIdAsync), DateTime.UtcNow, ex.Message
            );
            throw new AIAgentsBusinessException(
                message: ex.Message,
                correlationId: correlationContext.CorrelationId
            );
        }
        finally
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodEnd,
                nameof(GetRegisteredApplicationByIdAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedInUser, applicationId, result })
            );
        }
    }

    /// <summary>
    /// Gets the list of registered applications for the current logged in user.
    /// </summary>
    /// <param name="currentLoggedInUser">The current logged in user.</param>
    /// <param name="cancellationToken">The cancellation token used to cancel the asynchronous operation. Optional.</param>
    /// <returns>The list of <see cref="RegisteredApplicationDomain"/></returns>
    public async Task<IEnumerable<RegisteredApplicationDomain>> GetRegisteredApplicationsAsync(
        string currentLoggedInUser,
        CancellationToken cancellationToken = default
    )
    {
        IEnumerable<RegisteredApplicationDomain>? result = null;
        try
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodStart,
                nameof(GetRegisteredApplicationsAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedInUser })
            );

            result = await registeredApplicationDataManager.GetRegisteredApplicationsAsync(
                currentLoggedInUser,
                cancellationToken
            ).ConfigureAwait(false);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex,
                LoggingConstants.LogHelperMethodFailed,
                nameof(GetRegisteredApplicationsAsync), DateTime.UtcNow, ex.Message
            );
            throw new AIAgentsBusinessException(
                message: ex.Message,
                correlationId: correlationContext.CorrelationId
            );
        }
        finally
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodEnd,
                nameof(GetRegisteredApplicationsAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedInUser, result })
            );
        }
    }

    /// <summary>
    /// Updates an existing registered application for the current logged in user with the provided application data. 
    /// </summary>
    /// <remarks>The application to be updated is identified by the Id property of the provided application data model.</remarks>
    /// <param name="currentLoggedInUser">The current logged in user.</param>
    /// <param name="updateApplicationData">The update application data model.</param>
    /// <param name="cancellationToken">The cancellation token used to cancel the asynchronous operation. Optional.</param>
    /// <returns>A boolean for success/failure.</returns>
    public async Task<bool> UpdateExistingRegisteredApplicationAsync(
        string currentLoggedInUser,
        RegisteredApplicationDomain updateApplicationData,
        CancellationToken cancellationToken = default
    )
    {
        bool response = false;
        try
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodStart,
                nameof(UpdateExistingRegisteredApplicationAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedInUser, updateApplicationData })
            );

            response = await registeredApplicationDataManager.UpdateExistingRegisteredApplicationAsync(
                currentLoggedInUser,
                updateApplicationData,
                cancellationToken
            ).ConfigureAwait(false);
            if (response)
                await this.SendRegisteredApplicationUpdateNotificationAsync(
                    userToBeNotified: currentLoggedInUser,
                    currentUserEmail: currentLoggedInUser,
                    applicationName: updateApplicationData.ApplicationName,
                    applicationGuid: updateApplicationData.Id.ToString(),
                    cancellationToken: cancellationToken
                ).ConfigureAwait(false);

            return response;
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex,
                LoggingConstants.LogHelperMethodFailed,
                nameof(UpdateExistingRegisteredApplicationAsync), DateTime.UtcNow, ex.Message
            );
            throw new AIAgentsBusinessException(
                message: ex.Message,
                correlationId: correlationContext.CorrelationId
            );
        }
        finally
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodEnd,
                nameof(UpdateExistingRegisteredApplicationAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedInUser, response })
            );
        }
    }

    /// <summary>
    /// Sends a notification to a user about the update of a registered application's data. The notification includes the name and ID of the updated application, and is sent as a push notification.
    /// </summary>
    /// <param name="userToBeNotified">The user to be notified about the registered application data update.</param>
    /// <param name="currentUserEmail">The email of the current user who performed the update operation on the registered application, which will be included in the notification as the creator of the notification.</param>
    /// <param name="applicationName">The name of the registered application that has been updated, which will be included in the notification message.</param>
    /// <param name="applicationGuid">The GUID of the registered application that has been updated, which will be included in the notification message.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation of sending the notification. Optional.</param>
    /// <returns>A task that represents the asynchronous operation of sending the notification.</returns>
    private async Task SendRegisteredApplicationUpdateNotificationAsync(
        string userToBeNotified,
        string currentUserEmail,
        string applicationName,
        string applicationGuid,
        CancellationToken cancellationToken = default
    )
    {
        var notificationsDomainModel = new NotificationsDomain
        {
            RecipientUserName = userToBeNotified,
            Title = string.Format(NotificationMessagesConstants.RegisteredApplicationDataUpdateTitleTemplate, applicationName),
            Message = string.Format(NotificationMessagesConstants.RegisteredApplicationDataHasBeenUpdatedMessageTemplate, applicationName, applicationGuid),
            IsGlobal = false,
            NotificationType = nameof(NotificationTypes.Push),
            CreatedBy = currentUserEmail
        };
        await notificationsService.CreateNewNotificationAsync(
            request: notificationsDomainModel,
            cancellationToken: cancellationToken
        ).ConfigureAwait(false);
    }
}
