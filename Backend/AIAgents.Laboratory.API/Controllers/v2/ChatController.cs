using AIAgents.Laboratory.API.Adapters.Contracts;
using AIAgents.Laboratory.API.Adapters.Models.Base;
using AIAgents.Laboratory.API.Adapters.Models.Request;
using AIAgents.Laboratory.API.Adapters.Models.Response;
using AIAgents.Laboratory.Domain.Contracts;
using AIAgents.Laboratory.Domain.Helpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using static AIAgents.Laboratory.API.Helpers.AuthorizationTypes;
using static AIAgents.Laboratory.API.Helpers.Constants;
using static AIAgents.Laboratory.API.Helpers.RouteConstants;
using static AIAgents.Laboratory.API.Helpers.SwaggerConstants.ChatController;

namespace AIAgents.Laboratory.API.Controllers.v2;

/// <summary>
/// Provides API endpoints for interacting with AI chat agents, including invoking chat operations, retrieving direct responses, managing conversation history, and clearing user data.
/// </summary>
/// <remarks>This controller is versioned and secured, exposing endpoints for AI chat functionality. All actions require proper authorization and may return standard HTTP status codes for success, 
/// unauthorized access, bad requests, or not found scenarios. Usage of these endpoints is intended for clients needing conversational AI features and conversation history management.</remarks>
/// <param name="httpContextAccessor">The accessor used to obtain HTTP context information for the current request.</param>
/// <param name="configuration">The application configuration instance used to access settings and options.</param>
/// <param name="logger">The logger used for recording diagnostic and operational information for the controller.</param>
/// <param name="correlationContext">The context object used to track correlation identifiers for request tracing and logging.</param>
/// <param name="chatHandler">The handler responsible for processing chat-related operations and communication with AI agents.</param>
/// <seealso cref="BaseController"/>
[ApiController]
[ApiVersion(ApiVersionsConstants.ApiVersionV2)]
[Route("aiagentsapi/v{version:apiVersion}/[controller]")]
public sealed class ChatController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, ILogger<ChatController> logger, ICorrelationContext correlationContext, IChatHandler chatHandler) : BaseController(httpContextAccessor, configuration)
{
    /// <summary>
    /// Invokes the chat agent asynchronous.
    /// </summary>
    /// <param name="chatRequestDTO">The chat request dto.</param>
    /// <returns>The AI response.</returns>
    /// <exception cref="System.ArgumentNullException"></exception>
    [HttpPost(ChatRoutes.InvokeAgent_Route)]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = InvokeAgentAction.Summary, Description = InvokeAgentAction.Description, OperationId = InvokeAgentAction.OperationId)]
    public async Task<ResponseDto> InvokeChatAgentAsync([FromBody] ChatRequestDTO chatRequestDTO, CancellationToken cancellationToken = default)
    {
        string result = string.Empty;
        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(InvokeChatAgentAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, chatRequestDTO }));

            ArgumentNullException.ThrowIfNull(chatRequestDTO);
            if (base.IsAuthorized(UserBased))
            {
                result = await chatHandler.InvokeChatAgentAsync(chatRequestDTO, cancellationToken).ConfigureAwait(false);
                if (!string.IsNullOrEmpty(result))
                    return HandleSuccessRequestResponse(result);
                else
                    return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.SomethingWentWrongDefaultMessage);
            }

            return HandleUnAuthorizedRequestResponse();
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(InvokeChatAgentAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(InvokeChatAgentAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, result }));
        }
    }

    /// <summary>
    /// Gets the direct chat response from chatbot async.
    /// </summary>
    /// <param name="userChatMessage">The user chat request dto model.</param>
    /// <returns>The AI response string.</returns>
    [HttpPost(ChatRoutes.GetDirectChatResponse_Route)]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = GetDirectChatResponseAction.Summary, Description = GetDirectChatResponseAction.Description, OperationId = GetDirectChatResponseAction.OperationId)]
    public async Task<ResponseDto> GetDirectChatResponseAsync([FromBody] DirectChatRequestDTO userChatMessage, CancellationToken cancellationToken = default)
    {
        string result = string.Empty;
        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(GetDirectChatResponseAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, userChatMessage }));

            ArgumentNullException.ThrowIfNull(userChatMessage);
            ArgumentException.ThrowIfNullOrEmpty(userChatMessage.UserMessage);
            if (base.IsAuthorized(UserBased))
            {
                result = await chatHandler.GetDirectChatResponseAsync(
                    userQuery: userChatMessage.UserMessage,
                    userEmail: base.UserEmail,
                    cancellationToken: cancellationToken).ConfigureAwait(false);

                if (!string.IsNullOrEmpty(result))
                    return HandleSuccessRequestResponse(result);
                else
                    return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.SomethingWentWrongDefaultMessage);
            }

            return HandleUnAuthorizedRequestResponse();
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(GetDirectChatResponseAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetDirectChatResponseAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, result }));
        }
    }

    /// <summary>
    /// Clears the conversation history data for user.
    /// </summary>
    /// <returns>The boolean for success/failure.</returns>
    [HttpPost(ChatRoutes.ClearConversationHistory_Route)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = ClearConversationHistoryForUserAction.Summary, Description = ClearConversationHistoryForUserAction.Description, OperationId = ClearConversationHistoryForUserAction.OperationId)]
    public async Task<ResponseDto> ClearConversationHistoryForUserAsync()
    {
        bool result = false;
        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(ClearConversationHistoryForUserAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail }));
            if (base.IsAuthorized(UserBased))
            {
                result = await chatHandler.ClearConversationHistoryForUserAsync(base.UserEmail).ConfigureAwait(false);
                if (result)
                    return HandleSuccessRequestResponse(result);
                else
                    return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.ConversationHistoryCannotBeClearedMessageConstant);
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
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(ClearConversationHistoryForUserAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, result }));
        }
    }

    /// <summary>
    /// Gets the conversation history data for user.
    /// </summary>
    /// <returns>The conversation history data for user.</returns>
    [HttpGet(ChatRoutes.GetConversationHistoryUser_Route)]
    [ProducesResponseType(typeof(ConversationHistoryDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = GetConversationHistoryDataForUserAction.Summary, Description = GetConversationHistoryDataForUserAction.Description, OperationId = GetConversationHistoryDataForUserAction.OperationId)]
    public async Task<ResponseDto> GetConversationHistoryDataForUserAsync()
    {
        ConversationHistoryDTO result = new();
        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(GetConversationHistoryDataForUserAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail }));
            if (base.IsAuthorized(UserBased))
            {
                result = await chatHandler.GetConversationHistoryDataAsync(base.UserEmail).ConfigureAwait(false);
                if (result is not null)
                    return HandleSuccessRequestResponse(result);
                else
                    return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.ConversationHistoryCannotBeFetchedMessageConstant);
            }

            return HandleUnAuthorizedRequestResponse();
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(GetConversationHistoryDataForUserAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetConversationHistoryDataForUserAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, result }));
        }
    }
}
