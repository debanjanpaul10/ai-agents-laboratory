using System.Globalization;
using AIAgents.Laboratory.Domain.Contracts;
using AIAgents.Laboratory.Domain.DomainEntities.AgentsEntities;
using AIAgents.Laboratory.Domain.DomainEntities.Workspaces;
using AIAgents.Laboratory.Domain.Helpers;
using AIAgents.Laboratory.Domain.Ports.In;
using AIAgents.Laboratory.Domain.Ports.Out;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Newtonsoft.Json;
using static AIAgents.Laboratory.Domain.Helpers.Constants;

namespace AIAgents.Laboratory.Domain.UseCases;

/// <summary>
/// The Workspace Service implementation.
/// </summary>
/// <remarks>This service provides functionalities to manage workspaces, including creating, updating, deleting, and retrieving workspaces.</remarks>
/// <param name="logger">The logger service.</param>
/// <param name="correlationContext">The correlation context for logging.</param>
/// <param name="workspacesDataManager">The workspaces data manager.</param>
/// <param name="agentChatService">The agent chat service.</param>
/// <param name="orchestratorService">The orchestrator service.</param>
/// <seealso cref="IWorkspacesService"/>
public sealed class WorkspacesService(ILogger<WorkspacesService> logger, IWorkspacesDataManager workspacesDataManager, ICorrelationContext correlationContext,
    IAgentChatService agentChatService, IOrchestratorService orchestratorService) : IWorkspacesService
{
    /// <summary>
    /// Creates a new workspace.
    /// </summary>
    /// <param name="agentsWorkspaceData">The agents workspace data.</param>
    /// <param name="currentUserEmail">The current user email address.</param>
    /// <param name="cancellationToken">The cancellation token used to cancel the asynchronous operation. Optional.</param>
    /// <returns>A boolean for <c>success/failure.</c></returns>
    public async Task<bool> CreateNewWorkspaceAsync(AgentsWorkspaceDomain agentsWorkspaceData, string currentUserEmail, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(currentUserEmail);
        ArgumentNullException.ThrowIfNull(agentsWorkspaceData);

        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(CreateNewWorkspaceAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, agentsWorkspaceData, currentUserEmail }));

            agentsWorkspaceData.AgentWorkspaceGuid = Guid.NewGuid().ToString();
            agentsWorkspaceData.PrepareAuditEntityData(currentUser: currentUserEmail);
            return await workspacesDataManager.CreateNewWorkspaceAsync(
                agentsWorkspaceData,
                currentUserEmail,
                cancellationToken
            ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(CreateNewWorkspaceAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(CreateNewWorkspaceAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, agentsWorkspaceData, currentUserEmail }));
        }
    }

    /// <summary>
    /// Deletes the existing workspace by workspace guid id.
    /// </summary>
    /// <param name="workspaceGuidId">The workspace guid id.</param>
    /// <param name="currentUserEmail">The current logged in user email address.</param>
    /// <param name="cancellationToken">The cancellation token used to cancel the asynchronous operation. Optional.</param>
    /// <returns>A boolean for <c>success/failure.</c></returns>
    public async Task<bool> DeleteExistingWorkspaceAsync(string workspaceGuidId, string currentUserEmail, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(workspaceGuidId);
        ArgumentException.ThrowIfNullOrWhiteSpace(currentUserEmail);

        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(DeleteExistingWorkspaceAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, workspaceGuidId, currentUserEmail }));

            return await workspacesDataManager.DeleteExistingWorkspaceAsync(
                workspaceGuidId,
                currentUserEmail,
                cancellationToken
            ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(DeleteExistingWorkspaceAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(DeleteExistingWorkspaceAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, workspaceGuidId, currentUserEmail }));
        }
    }

    /// <summary>
    /// Gets the collection of all available workspaces.
    /// </summary>
    /// <param name="currentUserEmail">The current logged in user name.</param>
    /// <param name="cancellationToken">The cancellation token used to cancel the asynchronous operation. Optional.</param>
    /// <returns>The list of <see cref="AgentsWorkspaceDomain"/></returns>
    public async Task<IEnumerable<AgentsWorkspaceDomain>> GetAllWorkspacesAsync(string currentUserEmail, CancellationToken cancellationToken = default)
    {
        IEnumerable<AgentsWorkspaceDomain>? result = null;
        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(GetAllWorkspacesAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentUserEmail }));

            result = await workspacesDataManager.GetAllWorkspacesAsync(
                currentUserEmail,
                cancellationToken
            ).ConfigureAwait(false);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(GetAllWorkspacesAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetAllWorkspacesAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentUserEmail, result }));
        }
    }

    /// <summary>
    /// Gets the workspace by workspace id.
    /// </summary>
    /// <param name="workspaceId">The workspace id.</param>
    /// <param name="currentUserEmail">The current logged in user email</param>
    /// <param name="cancellationToken">The cancellation token used to cancel the asynchronous operation. Optional.</param>
    /// <returns>The agent workspace domain model.</returns>
    public async Task<AgentsWorkspaceDomain> GetWorkspaceByWorkspaceIdAsync(string workspaceId, string currentUserEmail, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(workspaceId);
        AgentsWorkspaceDomain? result = null;
        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(GetWorkspaceByWorkspaceIdAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, workspaceId, currentUserEmail }));

            result = await workspacesDataManager.GetWorkspaceByWorkspaceIdAsync(
                workspaceId,
                currentUserEmail,
                cancellationToken
            ).ConfigureAwait(false);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(GetWorkspaceByWorkspaceIdAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetWorkspaceByWorkspaceIdAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, workspaceId, currentUserEmail, result }));
        }
    }

    /// <summary>
    /// Gets the workspace group chat response.
    /// </summary>
    /// <param name="chatRequest">The workspace agent chat request dto model.</param>
    /// <param name="cancellationToken">The cancellation token used to cancel the asynchronous operation. Optional.</param>
    /// <returns>The group chat response.</returns>
    public async Task<GroupChatResponseDomain> GetWorkspaceGroupChatResponseAsync(WorkspaceAgentChatRequestDomain chatRequest, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(chatRequest);
        ArgumentException.ThrowIfNullOrWhiteSpace(chatRequest.WorkspaceId);
        ArgumentException.ThrowIfNullOrWhiteSpace(chatRequest.UserMessage);

        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(GetWorkspaceGroupChatResponseAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, chatRequest }));

            if (string.IsNullOrWhiteSpace(chatRequest.ConversationId))
                chatRequest.ConversationId = Guid.NewGuid().ToString();

            var workspaceDetails = await this.GetWorkspaceByWorkspaceIdAsync(
                workspaceId: chatRequest.WorkspaceId,
                currentUserEmail: chatRequest.ApplicationName,
                cancellationToken
            ).ConfigureAwait(false);

            if (workspaceDetails is null || string.IsNullOrWhiteSpace(workspaceDetails.Id))
                throw new FileNotFoundException(string.Format(CultureInfo.InvariantCulture, ExceptionConstants.WorkspaceNotFoundExceptionMessage, chatRequest.WorkspaceId));

            if (!workspaceDetails.IsGroupChatEnabled)
                throw new MethodAccessException(ExceptionConstants.GroupchatNotEnabledExceptionMessage);

            var groupResponse = await orchestratorService.GetOrchestratorAgentResponseAsync(
                chatRequest,
                workspaceDetails,
                cancellationToken
            ).ConfigureAwait(false);
            return new GroupChatResponseDomain()
            {
                AgentResponse = groupResponse.FinalResponse,
                AgentsInvoked = [.. groupResponse.GroupChatAgentsResponses.Select(x => x.AgentName)]
            };
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(GetWorkspaceGroupChatResponseAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetWorkspaceGroupChatResponseAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, chatRequest }));
        }
    }

    /// <summary>
    /// Invoke the workspace agent with user message and get the response.
    /// </summary>
    /// <param name="chatRequest">The chat request domain model.</param>
    /// <param name="cancellationToken">The cancellation token used to cancel the asynchronous operation. Optional.</param>
    /// <returns>The string response from AI.</returns>
    public async Task<string> InvokeWorkspaceAgentAsync(WorkspaceAgentChatRequestDomain chatRequest, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(chatRequest);
        ArgumentException.ThrowIfNullOrWhiteSpace(chatRequest.WorkspaceId);
        ArgumentException.ThrowIfNullOrWhiteSpace(chatRequest.AgentId);
        ArgumentException.ThrowIfNullOrWhiteSpace(chatRequest.UserMessage);

        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(InvokeWorkspaceAgentAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, chatRequest }));

            if (string.IsNullOrWhiteSpace(chatRequest.ConversationId))
                chatRequest.ConversationId = Guid.NewGuid().ToString();

            var workspaceDetails = await this.GetWorkspaceByWorkspaceIdAsync(
                workspaceId: chatRequest.WorkspaceId,
                currentUserEmail: chatRequest.ApplicationName,
                cancellationToken
            ).ConfigureAwait(false) ?? throw new FileNotFoundException(ExceptionConstants.DataNotFoundExceptionMessage);

            var agentDetails = workspaceDetails.ActiveAgentsListInWorkspace.FirstOrDefault(agent => agent.AgentGuid == chatRequest.AgentId)
                ?? throw new FileNotFoundException(ExceptionConstants.DataNotFoundExceptionMessage);

            var agentChatRequestModel = new ChatRequestDomain()
            {
                AgentId = agentDetails.AgentGuid,
                AgentName = agentDetails.AgentName,
                ConversationId = chatRequest.ConversationId,
                UserMessage = chatRequest.UserMessage,
            };
            return await agentChatService.GetAgentChatResponseAsync(
                chatRequest: agentChatRequestModel,
                cancellationToken
            ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(InvokeWorkspaceAgentAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(InvokeWorkspaceAgentAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, chatRequest }));
        }
    }

    /// <summary>
    /// Updates the existing workspace data.
    /// </summary>
    /// <param name="agentsWorkspaceData">The agents workspace data domain model.</param>
    /// <param name="currentUserEmail">The current logged in user email.</param>
    /// <param name="cancellationToken">The cancellation token used to cancel the asynchronous operation. Optional.</param>
    /// <returns>A boolean for <c>success/failure.</c></returns>
    public async Task<bool> UpdateExistingWorkspaceDataAsync(AgentsWorkspaceDomain agentsWorkspaceData, string currentUserEmail, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(agentsWorkspaceData);
        ArgumentException.ThrowIfNullOrWhiteSpace(currentUserEmail);

        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(UpdateExistingWorkspaceDataAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, agentsWorkspaceData, currentUserEmail }));

            return await workspacesDataManager.UpdateExistingWorkspaceDataAsync(
                agentsWorkspaceData,
                currentUserEmail,
                cancellationToken
            ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(UpdateExistingWorkspaceDataAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(UpdateExistingWorkspaceDataAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, agentsWorkspaceData, currentUserEmail }));
        }
    }
}
