using AIAgents.Laboratory.API.Adapters.Contracts;
using AIAgents.Laboratory.API.Adapters.Models.Request;
using AIAgents.Laboratory.API.Adapters.Models.Response;
using AIAgents.Laboratory.API.Helpers;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using static AIAgents.Laboratory.API.Helpers.Constants;
using static AIAgents.Laboratory.API.Helpers.RouteConstants;
using static AIAgents.Laboratory.API.Helpers.SwaggerConstants.ChatController;

namespace AIAgents.Laboratory.API.Controllers.v2;

/// <summary>
/// The Chat API Controller class.
/// </summary>
/// <param name="httpContextAccessor">The http context accessor.</param>
/// <param name="chatHandler">The Chat API adapter handler.</param>
/// <seealso cref="AIAgents.Laboratory.API.Controllers.BaseController" />
[ApiController]
[ApiVersion(ApiVersionsConstants.ApiVersionV2)]
[Route("aiagentsapi/v{version:apiVersion}/[controller]")]
public class ChatController(IHttpContextAccessor httpContextAccessor, IChatHandler chatHandler) : BaseController(httpContextAccessor)
{
    /// <summary>
    /// Invokes the chat agent asynchronous.
    /// </summary>
    /// <param name="chatRequestDTO">The chat request dto.</param>
    /// <returns>The AI response.</returns>
    /// <exception cref="System.ArgumentNullException"></exception>
    [HttpPost(RouteConstants.ChatRoutes.InvokeAgent_Route)]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = InvokeAgentAction.Summary, Description = InvokeAgentAction.Description, OperationId = InvokeAgentAction.OperationId)]
    public async Task<ResponseDTO> InvokeChatAgentAsync([FromBody] ChatRequestDTO chatRequestDTO)
    {
        ArgumentNullException.ThrowIfNull(chatRequestDTO);

        var result = await chatHandler.InvokeChatAgentAsync(chatRequestDTO).ConfigureAwait(false);
        if (!string.IsNullOrEmpty(result))
            return HandleSuccessRequestResponse(result);

        return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.AiServicesDownMessage);
    }

    /// <summary>
    /// Gets the direct chat response from chatbot async.
    /// </summary>
    /// <param name="userChatMessage">The user chat request dto model.</param>
    /// <returns>The AI response string.</returns>
    [HttpPost(RouteConstants.ChatRoutes.GetDirectChatResponse_Route)]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = GetDirectChatResponseAction.Summary, Description = GetDirectChatResponseAction.Description, OperationId = GetDirectChatResponseAction.OperationId)]
    public async Task<ResponseDTO> GetDirectChatResponseAsync([FromBody] DirectChatRequestDTO userChatMessage)
    {
        ArgumentNullException.ThrowIfNull(userChatMessage);
        ArgumentException.ThrowIfNullOrEmpty(userChatMessage.UserMessage);

        var result = await chatHandler.GetDirectChatResponseAsync(userChatMessage.UserMessage, UserEmail).ConfigureAwait(false);
        if (!string.IsNullOrEmpty(result))
            return HandleSuccessRequestResponse(result);

        return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.AiServicesDownMessage);
    }

    /// <summary>
    /// Clears the conversation history data for user.
    /// </summary>
    /// <returns>The boolean for success/failure.</returns>
    [HttpPost(RouteConstants.ChatRoutes.ClearConversationHistory_Route)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = ClearConversationHistoryForUserAction.Summary, Description = ClearConversationHistoryForUserAction.Description, OperationId = ClearConversationHistoryForUserAction.OperationId)]
    public async Task<ResponseDTO> ClearConversationHistoryForUserAsync()
    {
        var result = await chatHandler.ClearConversationHistoryForUserAsync(base.UserEmail).ConfigureAwait(false);
        if (result)
            return HandleSuccessRequestResponse(result);

        return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.ConversationHistoryCannotBeClearedMessageConstant);
    }

    /// <summary>
    /// Gets the conversation history data for user.
    /// </summary>
    /// <returns>The conversation history data for user.</returns>
    [HttpGet(RouteConstants.ChatRoutes.GetConversationHistoryUser_Route)]
    [ProducesResponseType(typeof(ConversationHistoryDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = GetConversationHistoryDataForUserAction.Summary, Description = GetConversationHistoryDataForUserAction.Description, OperationId = GetConversationHistoryDataForUserAction.OperationId)]
    public async Task<ResponseDTO> GetConversationHistoryDataForUserAsync()
    {
        var result = await chatHandler.GetConversationHistoryDataAsync(base.UserEmail).ConfigureAwait(false);
        if (result is not null)
            return HandleSuccessRequestResponse(result);

        return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.ConversationHistoryCannotBeFetchedMessageConstant);
    }
}
