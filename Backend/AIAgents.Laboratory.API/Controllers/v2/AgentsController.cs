using System.Net.Mime;
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
using static AIAgents.Laboratory.API.Helpers.SwaggerConstants.AgentsController;

namespace AIAgents.Laboratory.API.Controllers.v2;

/// <summary>
/// The <c>AgentsController</c> class is an API controller responsible for handling HTTP requests related to agent management in the AIAgents Laboratory application. 
/// </summary>
/// <remarks>It provides endpoints for creating, retrieving, updating, and deleting agents, as well as downloading associated knowledgebase files. 
/// The controller interacts with the <see cref="IAgentsHandler"/> service to perform the necessary operations and returns appropriate HTTP responses based on the outcome of each request.</remarks>
/// <param name="httpContext">The http context accessor.</param>
/// <param name="configuration">The configuration.</param>
/// <param name="logger">The logger service.</param>
/// <param name="correlationContext">The correlation context used for logging.</param>
/// <param name="agentsHandler">The agents handler service.</param>
/// <seealso cref="BaseController" />
[ApiController]
[ApiVersion(ApiVersionsConstants.ApiVersionV2)]
[Route(ApiBaseRoute)]
public sealed class AgentsController(IHttpContextAccessor httpContext, IConfiguration configuration, ILogger<AgentsController> logger, ICorrelationContext correlationContext,
    IAgentsHandler agentsHandler) : BaseController(httpContext, configuration)
{
    /// <summary>
    /// Creates the new agent asynchronous.
    /// </summary>
    /// <param name="agentData">The agent data.</param>
    /// <returns>The boolean for success/failure.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    [HttpPost(AgentsRoutes.CreateNewAgent_Route)]
    [Consumes(MediaTypeNames.Multipart.FormData)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = CreateNewAgentAction.Summary, Description = CreateNewAgentAction.Description, OperationId = CreateNewAgentAction.OperationId)]
    public async Task<ResponseDto> CreateNewAgentAsync([FromForm] CreateAgentDTO agentData)
    {
        bool result = false;
        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(CreateNewAgentAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, agentData }));

            ArgumentNullException.ThrowIfNull(agentData);
            if (base.IsAuthorized(UserBased))
            {
                result = await agentsHandler.CreateNewAgentAsync(agentData, base.UserEmail).ConfigureAwait(false);
                if (result)
                    return HandleSuccessRequestResponse(result);
                else
                    return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.SomethingWentWrongDefaultMessage);
            }

            return HandleUnAuthorizedRequestResponse();
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(CreateNewAgentAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(CreateNewAgentAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, result }));
        }
    }

    /// <summary>
    /// Gets all agents data asynchronous.
    /// </summary>
    /// <returns>The list of <see cref="CreateAgentDTO"/></returns>
    [HttpGet(AgentsRoutes.GetAllAgents_Route)]
    [ProducesResponseType(typeof(IEnumerable<AgentDataDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = GetAllAgentsDataAction.Summary, Description = GetAllAgentsDataAction.Description, OperationId = GetAllAgentsDataAction.OperationId)]
    public async Task<ResponseDto> GetAllAgentsDataAsync()
    {
        IEnumerable<AgentDataDTO> result = [];
        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(GetAllAgentsDataAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail }));

            if (base.IsAuthorized(UserBased))
            {
                result = await agentsHandler.GetAllAgentsDataAsync(base.UserEmail).ConfigureAwait(false);
                if (result is not null)
                    return HandleSuccessRequestResponse(result);
                else
                    return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.SomethingWentWrongDefaultMessage);
            }

            return HandleUnAuthorizedRequestResponse();
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(GetAllAgentsDataAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetAllAgentsDataAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, result }));
        }
    }

    /// <summary>
    /// Gets the agent data by identifier asynchronous.
    /// </summary>
    /// <param name="agentId">The agent identifier.</param>
    /// <returns>The agent data dto model.</returns>
    [HttpGet(AgentsRoutes.GetAgentById_Route)]
    [ProducesResponseType(typeof(AgentDataDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = GetAgentDataByIdAction.Summary, Description = GetAgentDataByIdAction.Description, OperationId = GetAgentDataByIdAction.OperationId)]
    public async Task<ResponseDto> GetAgentDataByIdAsync([FromRoute] string agentId)
    {
        AgentDataDTO result = new();
        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(GetAgentDataByIdAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, agentId }));

            ArgumentException.ThrowIfNullOrEmpty(agentId);
            if (base.IsAuthorized(UserBased))
            {
                result = await agentsHandler.GetAgentDataByIdAsync(agentId, base.UserEmail).ConfigureAwait(false);
                if (result is not null)
                    return HandleSuccessRequestResponse(result);
                else
                    return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.SomethingWentWrongDefaultMessage);
            }

            return HandleUnAuthorizedRequestResponse();
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(GetAgentDataByIdAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetAgentDataByIdAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, agentId, result }));
        }
    }

    /// <summary>
    /// Updates the existing agent data asynchronous.
    /// </summary>
    /// <param name="updateAgentData">The update agent data.</param>
    /// <returns>The agent data dto.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    [HttpPut(AgentsRoutes.UpdateExistingAgent_Route)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = UpdateExistingAgentDataAction.Summary, Description = UpdateExistingAgentDataAction.Description, OperationId = UpdateExistingAgentDataAction.OperationId)]
    public async Task<ResponseDto> UpdateExistingAgentDataAsync([FromForm] AgentDataDTO updateAgentData)
    {
        bool result = false;
        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(UpdateExistingAgentDataAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, updateAgentData.AgentId }));

            ArgumentNullException.ThrowIfNull(updateAgentData);
            if (IsAuthorized(UserBased))
            {
                result = await agentsHandler.UpdateExistingAgentDataAsync(updateAgentData, base.UserEmail).ConfigureAwait(false);
                if (result)
                    return HandleSuccessRequestResponse(result);
                else
                    return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.SomethingWentWrongDefaultMessage);
            }

            return HandleUnAuthorizedRequestResponse();
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(UpdateExistingAgentDataAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(UpdateExistingAgentDataAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, updateAgentData.AgentId, result }));
        }
    }

    /// <summary>
    /// Deletes the existing agent data asynchronous.
    /// </summary>
    /// <param name="agentId">The agent identifier.</param>
    /// <returns>The boolean for success/failure.</returns>
    [HttpDelete(AgentsRoutes.DeleteExistingAgent_Route)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = DeleteExistingAgentDataAction.Summary, Description = DeleteExistingAgentDataAction.Description, OperationId = DeleteExistingAgentDataAction.OperationId)]
    public async Task<ResponseDto> DeleteExistingAgentDataAsync([FromRoute] string agentId)
    {
        bool result = false;
        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(DeleteExistingAgentDataAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, agentId }));

            ArgumentException.ThrowIfNullOrEmpty(agentId);
            if (IsAuthorized(UserBased))
            {
                result = await agentsHandler.DeleteExistingAgentDataAsync(agentId, base.UserEmail).ConfigureAwait(false);
                if (result)
                    return HandleSuccessRequestResponse(result);
                else
                    return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.SomethingWentWrongDefaultMessage);
            }

            return HandleUnAuthorizedRequestResponse();
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(DeleteExistingAgentDataAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(DeleteExistingAgentDataAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, agentId, result }));
        }
    }

    /// <summary>
    /// Downloads the knowledgebase file asynchronous.
    /// </summary>
    /// <param name="downloadFile">The download file dto model.</param>
    /// <returns>The downloaded file data.</returns>
    [HttpPost(AgentsRoutes.DownloadAssociatedDocuments_Route)]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = DownloadKnowledgebaseFileAction.Summary, Description = DownloadKnowledgebaseFileAction.Description, OperationId = DownloadKnowledgebaseFileAction.OperationId)]
    public async Task<ResponseDto> DownloadKnowledgebaseFileAsync([FromBody] DownloadFileDTO downloadFile)
    {
        string result = string.Empty;
        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(DownloadKnowledgebaseFileAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, downloadFile }));

            ArgumentNullException.ThrowIfNull(downloadFile);
            if (IsAuthorized(UserBased))
            {
                result = await agentsHandler.DownloadKnowledgebaseFileAsync(
                    agentGuid: downloadFile.AgentGuid,
                    fileName: downloadFile.FileName).ConfigureAwait(false);

                if (!string.IsNullOrWhiteSpace(result))
                    return HandleSuccessRequestResponse(result);
                else
                    return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.SomethingWentWrongDefaultMessage);
            }

            return HandleUnAuthorizedRequestResponse();
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(DownloadKnowledgebaseFileAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(DownloadKnowledgebaseFileAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, result }));
        }
    }
}
