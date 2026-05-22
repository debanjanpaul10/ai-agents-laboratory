using AIAgents.Laboratory.API.Adapters.Contracts;
using AIAgents.Laboratory.API.Adapters.Models.Base;
using AIAgents.Laboratory.API.Adapters.Models.Response;
using AIAgents.Laboratory.Domain.Contracts;
using AIAgents.Laboratory.Domain.Helpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using static AIAgents.Laboratory.API.Helpers.AuthorizationTypes;
using static AIAgents.Laboratory.API.Helpers.Constants;
using static AIAgents.Laboratory.API.Helpers.RouteConstants;
using static AIAgents.Laboratory.API.Helpers.SwaggerConstants.ConversationsController;

namespace AIAgents.Laboratory.API.Controllers;

[ApiController]
[Route(ApiBaseRoute)]
public sealed class ConversationsController(
    IHttpContextAccessor httpContextAccessor,
    IConfiguration configuration,
    ILogger<ConversationsController> logger,
    ICorrelationContext correlationContext,
    IConversationsHandler conversationsHandler) : BaseController(httpContextAccessor, configuration)
{
    /// <summary>
    /// Gets the conversation history data for user.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token used to cancel the asynchronous operation. Optional.</param>
    /// <returns>The conversation history data for user.</returns>
    [HttpGet(ConversationsRoutes.GetConversationHistoryUser_Route)]
    [ProducesResponseType(typeof(ConversationHistoryDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = GetConversationHistoryDataForUserAction.Summary,
        Description = GetConversationHistoryDataForUserAction.Description,
        OperationId = GetConversationHistoryDataForUserAction.OperationId)]
    public async Task<ResponseDto> GetConversationHistoryDataForUserAsync(
        CancellationToken cancellationToken = default
    )
    {
        ConversationHistoryDTO result = new();
        try
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodStart,
                nameof(GetConversationHistoryDataForUserAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail })
            );

            if (base.IsAuthorized(authorizationType: UserBased))
            {
                result = await conversationsHandler.GetConversationHistoryDataAsync(
                    userName: base.UserEmail,
                    cancellationToken
                ).ConfigureAwait(false);

                if (result is not null)
                    return HandleSuccessRequestResponse(
                        responseData: result
                    );
                else
                    return HandleBadRequestResponse(
                        statusCode: StatusCodes.Status400BadRequest,
                        message: ExceptionConstants.ConversationHistoryCannotBeFetchedMessageConstant
                    );
            }

            return HandleUnAuthorizedRequestResponse();
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex,
                LoggingConstants.LogHelperMethodFailed,
                nameof(GetConversationHistoryDataForUserAsync), DateTime.UtcNow, ex.Message
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
                nameof(GetConversationHistoryDataForUserAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, result })
            );
        }
    }

    /// <summary>
    /// Gets the workspace conversation history asynchronous.
    /// </summary>
    /// <param name="workspaceGuid">The workspace unique identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The conversation history.</returns>
    [HttpGet(ConversationsRoutes.GetWorkspaceConversationHistory_Route)]
    [ProducesResponseType(typeof(ConversationHistoryDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = GetWorkspaceConversationHistoryAction.Summary,
        Description = GetWorkspaceConversationHistoryAction.Description,
        OperationId = GetWorkspaceConversationHistoryAction.OperationId)]
    public async Task<ResponseDto> GetWorkspaceConversationHistoryAsync(
        [FromQuery] string workspaceGuid,
        CancellationToken cancellationToken = default
    )
    {
        ConversationHistoryDTO result = new();
        try
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodStart,
                nameof(GetWorkspaceConversationHistoryAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, workspaceGuid })
            );

            ArgumentException.ThrowIfNullOrEmpty(workspaceGuid);
            if (base.IsAuthorized(authorizationType: ApplicationBased))
            {
                result = await conversationsHandler.GetWorkspaceConversationHistoryAsync(
                    workspaceId: workspaceGuid,
                    currentUserEmail: base.UserEmail,
                    cancellationToken
                ).ConfigureAwait(false);

                if (result is not null)
                    return HandleSuccessRequestResponse(
                        responseData: result
                    );
                else
                    return HandleBadRequestResponse(
                        statusCode: StatusCodes.Status400BadRequest,
                        message: ExceptionConstants.SomethingWentWrongDefaultMessage
                    );
            }

            return HandleUnAuthorizedRequestResponse();
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex,
                LoggingConstants.LogHelperMethodFailed,
                nameof(GetWorkspaceConversationHistoryAsync), DateTime.UtcNow, ex.Message
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
                nameof(GetWorkspaceConversationHistoryAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, workspaceGuid, result })
            );
        }
    }

    /// <summary>
    /// Clears the conversation history data for user.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token used to cancel the asynchronous operation. Optional.</param>
    /// <returns>The boolean for success/failure.</returns>
    [HttpPost(ConversationsRoutes.ClearConversationHistory_Route)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = ClearConversationHistoryForUserAction.Summary,
        Description = ClearConversationHistoryForUserAction.Description,
        OperationId = ClearConversationHistoryForUserAction.OperationId)]
    public async Task<ResponseDto> ClearConversationHistoryForUserAsync(
        CancellationToken cancellationToken = default
    )
    {
        bool result = false;
        try
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodStart, nameof(ClearConversationHistoryForUserAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail }));

            if (base.IsAuthorized(authorizationType: UserBased))
            {
                result = await conversationsHandler.ClearConversationHistoryForUserAsync(
                    userName: base.UserEmail,
                    cancellationToken
                ).ConfigureAwait(false);

                if (result)
                    return HandleSuccessRequestResponse(
                        responseData: result);
                else
                    return HandleBadRequestResponse(
                        statusCode: StatusCodes.Status400BadRequest,
                        message: ExceptionConstants.ConversationHistoryCannotBeClearedMessageConstant);
            }

            return HandleUnAuthorizedRequestResponse();
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(ClearConversationHistoryForUserAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(ClearConversationHistoryForUserAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, result }));
        }
    }

    /// <summary>
    /// Clears the conversation history of a workspace for a given conversation id.
    /// </summary>
    /// <param name="workspaceGuid">The GUID of the workspace.</param>
    /// <param name="conversationId">The ID of the conversation.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A boolean indicating whether the operation was successful.</returns>
    [HttpPost(ConversationsRoutes.ClearWorkspaceConversationHistory_Route)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = ClearWorkspaceConversationHistoryAction.Summary,
        Description = ClearWorkspaceConversationHistoryAction.Description,
        OperationId = ClearWorkspaceConversationHistoryAction.OperationId)]
    public async Task<ResponseDto> ClearWorkspaceConversationHistoryAsync(
        [FromQuery] string workspaceGuid,
        [FromQuery] string conversationId,
        CancellationToken cancellationToken = default
    )
    {
        bool result = false;
        try
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodStart,
                nameof(ClearWorkspaceConversationHistoryAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, workspaceGuid, conversationId })
            );

            ArgumentException.ThrowIfNullOrEmpty(workspaceGuid);
            ArgumentException.ThrowIfNullOrEmpty(conversationId);
            if (base.IsAuthorized(authorizationType: UserBased))
            {
                result = await conversationsHandler.ClearWorkspaceConversationHistoryAsync(
                    workspaceId: workspaceGuid,
                    currentUserEmail: base.UserEmail,
                    conversationId: conversationId,
                    cancellationToken
                ).ConfigureAwait(false);

                if (result)
                    return HandleSuccessRequestResponse(
                        responseData: result
                    );
                else
                    return HandleBadRequestResponse(
                        statusCode: StatusCodes.Status400BadRequest,
                        message: ExceptionConstants.SomethingWentWrongDefaultMessage
                    );
            }

            return HandleUnAuthorizedRequestResponse();
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex,
                LoggingConstants.LogHelperMethodFailed,
                nameof(ClearWorkspaceConversationHistoryAsync), DateTime.UtcNow, ex.Message
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
                nameof(ClearWorkspaceConversationHistoryAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, workspaceGuid, conversationId, result })
            );
        }
    }

    /// <summary>
    /// Initializes the workspace conversation asynchronous.
    /// </summary>
    /// <param name="workspaceGuid">The workspace unique identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The newly created conversation id.</returns>
    [HttpPost(ConversationsRoutes.InitializeWorkspaceConversation_Route)]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = InitializeWorkspaceConversationAction.Summary,
        Description = InitializeWorkspaceConversationAction.Description,
        OperationId = InitializeWorkspaceConversationAction.OperationId)]
    public async Task<ResponseDto> InitializeWorkspaceConversationAsync(
        [FromRoute] string workspaceGuid,
        [FromRoute] string? applicationName,
        CancellationToken cancellationToken = default
    )
    {
        string result = string.Empty;
        try
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodStart,
                nameof(InitializeWorkspaceConversationAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, workspaceGuid })
            );

            ArgumentException.ThrowIfNullOrWhiteSpace(workspaceGuid);
            if (base.IsAuthorized(authorizationType: ApplicationBased))
            {
                var userOrApplicationName = string.IsNullOrWhiteSpace(applicationName) ? base.UserEmail : applicationName;
                result = await conversationsHandler.InitializeWorkspaceConversationAsync(
                    workspaceGuid,
                    userOrApplicationName,
                    cancellationToken
                ).ConfigureAwait(false);

                if (result is not null)
                    return HandleSuccessRequestResponse(
                        responseData: result
                    );
                else
                    return HandleBadRequestResponse(
                        statusCode: StatusCodes.Status400BadRequest,
                        message: ExceptionConstants.SomethingWentWrongDefaultMessage
                    );
            }

            return HandleUnAuthorizedRequestResponse();
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex,
                LoggingConstants.LogHelperMethodFailed,
                nameof(InitializeWorkspaceConversationAsync), DateTime.UtcNow, ex.Message
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
                nameof(InitializeWorkspaceConversationAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, workspaceGuid, result })
            );
        }
    }
}