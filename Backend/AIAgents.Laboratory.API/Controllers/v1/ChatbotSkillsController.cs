using AIAgents.Laboratory.API.Adapters.Contracts;
using AIAgents.Laboratory.API.Adapters.Models.Base;
using AIAgents.Laboratory.API.Adapters.Models.Request;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using static AIAgents.Laboratory.API.Helpers.Constants;
using static AIAgents.Laboratory.API.Helpers.RouteConstants;
using static AIAgents.Laboratory.API.Helpers.SwaggerConstants.ChatbotSkillsController;

namespace AIAgents.Laboratory.API.Controllers.v1;

/// <summary>
/// The AI Chatbot skills controller.
/// </summary>
/// <param name="httpContextAccessor">The http context accessor.</param>
/// <param name="skillsHandler">The skills handler adapter.</param>
/// <seealso cref="BaseController" />
[ApiController]
[ApiVersion(ApiVersionsConstants.ApiVersionV1)]
[Route("aiagentsapi/v{version:apiVersion}/[controller]")]
public class ChatbotSkillsController(IHttpContextAccessor httpContextAccessor, IChatbotSkillsHandler skillsHandler) : BaseController(httpContextAccessor)
{
    /// <summary>
    /// Detects the user intent asynchronous.
    /// </summary>
    /// <param name="userQueryRequest">The user query request.</param>
    /// <returns>The ai response string.</returns>
    [HttpPost(AISkillsRoutes.DetectUserIntent_Route)]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = DetectUserIntentAction.Summary, Description = DetectUserIntentAction.Description, OperationId = DetectUserIntentAction.OperationId)]
    public async Task<ResponseDTO> DetectUserIntentAsync([FromBody] UserQueryRequestDTO userQueryRequest)
    {
        var result = await skillsHandler.DetectUserIntentAsync(userQueryRequest).ConfigureAwait(false);
        if (!string.IsNullOrEmpty(result))
            return HandleSuccessRequestResponse(result);

        return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.AiServicesDownMessage);
    }

    /// <summary>
    /// Gets the user greeting response asynchronous.
    /// </summary>
    /// <returns>The ai response string.</returns>
    [HttpPost(AISkillsRoutes.GetUserGreetingResponse_Route)]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = GetUserGreetingResponseAction.Summary, Description = GetUserGreetingResponseAction.Description, OperationId = GetUserGreetingResponseAction.OperationId)]
    public async Task<ResponseDTO> GetUserGreetingResponseAsync()
    {
        var result = await skillsHandler.GetUserGreetingResponseAsync().ConfigureAwait(false);
        if (!string.IsNullOrEmpty(result))
            return HandleSuccessRequestResponse(result);

        return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.AiServicesDownMessage);
    }

    /// <summary>
    /// Gets the rag text response asynchronous.
    /// </summary>
    /// <param name="ragTextInput">The rag text input.</param>
    /// <returns>The ai response string.</returns>
    [HttpPost(AISkillsRoutes.GetRAGTextResponse_Route)]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = GetRAGTextResponseAction.Summary, Description = GetRAGTextResponseAction.Description, OperationId = GetRAGTextResponseAction.OperationId)]
    public async Task<ResponseDTO> GetRAGTextResponseAsync([FromBody] SkillsInputDTO ragTextInput)
    {
        var result = await skillsHandler.GetRAGTextResponseAsync(ragTextInput).ConfigureAwait(false);
        if (!string.IsNullOrEmpty(result))
            return HandleSuccessRequestResponse(result);

        return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.AiServicesDownMessage);
    }

    /// <summary>
    /// Gets the nl to SQL response asynchronous.
    /// </summary>
    /// <param name="nltosqlInput">The nltosql input.</param>
    /// <returns>The ai response string.</returns>
    [HttpPost(AISkillsRoutes.GetNlToSqlResponse_Route)]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = GetNLToSQLResponseAction.Summary, Description = GetNLToSQLResponseAction.Description, OperationId = GetNLToSQLResponseAction.OperationId)]
    public async Task<ResponseDTO> GetNLToSQLResponseAsync([FromBody] NltosqlInputDTO nltosqlInput)
    {
        var result = await skillsHandler.GetNLToSQLResponseAsync(nltosqlInput).ConfigureAwait(false);
        if (!string.IsNullOrEmpty(result))
            return HandleSuccessRequestResponse(result);

        return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.AiServicesDownMessage);
    }

    /// <summary>
    /// Gets the SQL query markdown response asynchronous.
    /// </summary>
    /// <param name="sqlQueryResult">The SQL query.</param>
    /// <returns>The AI formatted response.</returns>
    [HttpPost(AISkillsRoutes.GetSQLQueryMarkdownResponse_Route)]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = GetSQLQueryMarkdownResponseAction.Summary, Description = GetSQLQueryMarkdownResponseAction.Description, OperationId = GetSQLQueryMarkdownResponseAction.OperationId)]
    public async Task<ResponseDTO> GetSQLQueryMarkdownResponseAsync([FromBody] SqlQueryResultDTO sqlQueryResult)
    {
        var result = await skillsHandler.GetSQLQueryMarkdownResponseAsync(sqlQueryResult.JsonQuery).ConfigureAwait(false);
        if (!string.IsNullOrEmpty(result))
            return HandleSuccessRequestResponse(result);

        return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.AiServicesDownMessage);
    }

    /// <summary>
    /// Gets the list of followup questions based on ai response and user query.
    /// </summary>
    /// <param name="followupQuestionsRequest">The followup questions request.</param>
    /// <returns>The list of ai responses.</returns>
    [HttpPost(AISkillsRoutes.GetFollowupQuestionsResponse_Route)]
    [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = GetFollowupQuestionsResponseAction.Summary, Description = GetFollowupQuestionsResponseAction.Description, OperationId = GetFollowupQuestionsResponseAction.OperationId)]
    public async Task<ResponseDTO> GetFollowupQuestionsResponseAsync([FromBody] FollowupQuestionsRequestDTO followupQuestionsRequest)
    {
        var result = await skillsHandler.GetFollowupQuestionsResponseAsync(followupQuestionsRequest).ConfigureAwait(false);
        if (result is not null && result.Any())
            return HandleSuccessRequestResponse(result);

        return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.AiServicesDownMessage);
    }
}

