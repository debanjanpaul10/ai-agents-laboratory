using AIAgents.Laboratory.Domain.DomainEntities.AgentsEntities;
using AIAgents.Laboratory.Domain.DomainEntities.Workspaces;
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
/// The Workspace Service implementation.
/// </summary>
/// <remarks>This service provides functionalities to manage workspaces, including creating, updating, deleting, and retrieving workspaces.</remarks>
/// <param name="logger">The logger service.</param>
/// <param name="configuration">The configuration service.</param>
/// <param name="mongoDatabaseService">The mongo db service.</param>
/// <param name="agentChatService">The agent chat service.</param>
/// <seealso cref="IWorkspacesService"/>
public sealed class WorkspacesService(ILogger<WorkspacesService> logger, IConfiguration configuration, IMongoDatabaseService mongoDatabaseService,
    IAgentChatService agentChatService, IOrchestratorService orchestratorService) : IWorkspacesService
{
    /// <summary>
    /// The mongo database name configuration value.
    /// </summary>
    private readonly string MongoDatabaseName = configuration[MongoDbCollectionConstants.AiAgentsPrimaryDatabase] ?? throw new Exception(ExceptionConstants.ConfigurationKeyNotFoundExceptionMessage);

    /// <summary>
    /// The workspaces collection name configuration value.
    /// </summary>
    private readonly string WorkspacesCollectionName = configuration[MongoDbCollectionConstants.WorkspaceCollectionName] ?? throw new Exception(ExceptionConstants.ConfigurationKeyNotFoundExceptionMessage);

    /// <summary>
    /// Creates a new workspace.
    /// </summary>
    /// <param name="agentsWorkspaceData">The agents workspace data.</param>
    /// <param name="currentUserEmail">The current user email address.</param>
    /// <returns>A boolean for <c>success/failure.</c></returns>
    public async Task<bool> CreateNewWorkspaceAsync(AgentsWorkspaceDomain agentsWorkspaceData, string currentUserEmail)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(currentUserEmail);
        ArgumentNullException.ThrowIfNull(agentsWorkspaceData);

        try
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(CreateNewWorkspaceAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { agentsWorkspaceData, currentUserEmail }));

            agentsWorkspaceData.AgentWorkspaceGuid = Guid.NewGuid().ToString();
            agentsWorkspaceData.PrepareAuditEntityData(currentUserEmail);
            return await mongoDatabaseService.SaveDataAsync(agentsWorkspaceData, this.MongoDatabaseName, this.WorkspacesCollectionName).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodFailed, nameof(CreateNewWorkspaceAsync), DateTime.UtcNow, ex.Message);
            throw;
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(CreateNewWorkspaceAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { agentsWorkspaceData, currentUserEmail }));
        }
    }

    /// <summary>
    /// Deletes the existing workspace by workspace guid id.
    /// </summary>
    /// <param name="workspaceGuidId">The workspace guid id.</param>
    /// <param name="currentUserEmail">The current logged in user email address.</param>
    /// <returns>A boolean for <c>success/failure.</c></returns>
    public async Task<bool> DeleteExistingWorkspaceAsync(string workspaceGuidId, string currentUserEmail)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(workspaceGuidId);
        ArgumentException.ThrowIfNullOrWhiteSpace(currentUserEmail);

        try
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(DeleteExistingWorkspaceAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { workspaceGuidId, currentUserEmail }));

            var filter = Builders<AgentsWorkspaceDomain>.Filter.Where(ws => ws.IsActive && ws.AgentWorkspaceGuid == workspaceGuidId);
            var allWorkspaces = await mongoDatabaseService.GetDataFromCollectionAsync(MongoDatabaseName, WorkspacesCollectionName, filter);
            var updateWorkspace = allWorkspaces.FirstOrDefault() ?? throw new Exception(ExceptionConstants.DataNotFoundExceptionMessage);

            if (updateWorkspace.CreatedBy != currentUserEmail)
                throw new UnauthorizedAccessException(ExceptionConstants.UnauthorizedUserExceptionMessage);

            var updates = new List<UpdateDefinition<AgentsWorkspaceDomain>>
            {
                Builders<AgentsWorkspaceDomain>.Update.Set(x => x.IsActive, false),
                Builders<AgentsWorkspaceDomain>.Update.Set(x => x.DateModified, DateTime.UtcNow),
                Builders<AgentsWorkspaceDomain>.Update.Set(x => x.ModifiedBy, currentUserEmail)
            };
            var update = Builders<AgentsWorkspaceDomain>.Update.Combine(updates);
            return await mongoDatabaseService.UpdateDataInCollectionAsync(filter, update, this.MongoDatabaseName, this.WorkspacesCollectionName).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodFailed, nameof(DeleteExistingWorkspaceAsync), DateTime.UtcNow, ex.Message);
            throw;
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(DeleteExistingWorkspaceAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { workspaceGuidId, currentUserEmail }));
        }
    }

    /// <summary>
    /// Gets the collection of all available workspaces.
    /// </summary>
    /// <param name="userName">The current logged in user name.</param>
    /// <returns>The list of <see cref="AgentsWorkspaceDomain"/></returns>
    public async Task<IEnumerable<AgentsWorkspaceDomain>> GetAllWorkspacesAsync(string userName)
    {
        try
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(GetAllWorkspacesAsync), DateTime.UtcNow, userName);

            var filter = Builders<AgentsWorkspaceDomain>.Filter.And(Builders<AgentsWorkspaceDomain>.Filter.Eq(x => x.IsActive, true));
            return await mongoDatabaseService.GetDataFromCollectionAsync(this.MongoDatabaseName, this.WorkspacesCollectionName, filter).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodFailed, nameof(GetAllWorkspacesAsync), DateTime.UtcNow, ex.Message);
            throw;
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetAllWorkspacesAsync), DateTime.UtcNow, userName);
        }
    }

    /// <summary>
    /// Gets the workspace by workspace id.
    /// </summary>
    /// <param name="workspaceId">The workspace id.</param>
    /// <param name="currentUserEmail">The current logged in user email</param>
    /// <returns>The agent workspace domain model.</returns>
    public async Task<AgentsWorkspaceDomain> GetWorkspaceByWorkspaceIdAsync(string workspaceId, string currentUserEmail)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(workspaceId);

        try
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(GetWorkspaceByWorkspaceIdAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { workspaceId, currentUserEmail }));

            var filter = Builders<AgentsWorkspaceDomain>.Filter.And(
                Builders<AgentsWorkspaceDomain>.Filter.Eq(x => x.IsActive, true), Builders<AgentsWorkspaceDomain>.Filter.Eq(x => x.AgentWorkspaceGuid, workspaceId));

            var allData = await mongoDatabaseService.GetDataFromCollectionAsync(this.MongoDatabaseName, this.WorkspacesCollectionName, filter).ConfigureAwait(false);
            return allData?.First() ?? throw new Exception(ExceptionConstants.DataNotFoundExceptionMessage);
        }
        catch (Exception ex)
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodFailed, nameof(GetWorkspaceByWorkspaceIdAsync), DateTime.UtcNow, ex.Message);
            throw;
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetWorkspaceByWorkspaceIdAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { workspaceId, currentUserEmail }));
        }
    }

    /// <summary>
    /// Gets the workspace group chat response.
    /// </summary>
    /// <param name="chatRequest">The workspace agent chat request dto model.</param>
    /// <returns>The group chat response.</returns>
    public async Task<string> GetWorkspaceGroupChatResponseAsync(WorkspaceAgentChatRequestDomain chatRequest)
    {
        try
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(GetWorkspaceGroupChatResponseAsync), DateTime.UtcNow, JsonConvert.SerializeObject(chatRequest));
            if (string.IsNullOrWhiteSpace(chatRequest.ConversationId))
                chatRequest.ConversationId = Guid.NewGuid().ToString();

            var workspaceDetails = await this.GetWorkspaceByWorkspaceIdAsync(chatRequest.WorkspaceId, chatRequest.ApplicationName).ConfigureAwait(false)
                ?? throw new Exception(ExceptionConstants.DataNotFoundExceptionMessage);

            if (!workspaceDetails.IsGroupChatEnabled)
                throw new Exception(ExceptionConstants.GroupchatNotEnabledExceptionMessage);

            return await orchestratorService.GetOrchestratorAgentResponseAsync(chatRequest, workspaceDetails).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodFailed, nameof(GetWorkspaceGroupChatResponseAsync), DateTime.UtcNow, ex.Message);
            throw;
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetWorkspaceGroupChatResponseAsync), DateTime.UtcNow, JsonConvert.SerializeObject(chatRequest));
        }
    }

    /// <summary>
    /// Invoke the workspace agent with user message and get the response.
    /// </summary>
    /// <param name="chatRequest">The chat request domain model.</param>
    /// <returns>The string response from AI.</returns>
    public async Task<string> InvokeWorkspaceAgentAsync(WorkspaceAgentChatRequestDomain chatRequest)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(chatRequest.WorkspaceId);
        ArgumentException.ThrowIfNullOrWhiteSpace(chatRequest.AgentId);
        ArgumentException.ThrowIfNullOrWhiteSpace(chatRequest.UserMessage);

        try
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(InvokeWorkspaceAgentAsync), DateTime.UtcNow, JsonConvert.SerializeObject(chatRequest));

            if (string.IsNullOrWhiteSpace(chatRequest.ConversationId))
                chatRequest.ConversationId = Guid.NewGuid().ToString();

            var workspaceDetails = await this.GetWorkspaceByWorkspaceIdAsync(chatRequest.WorkspaceId, chatRequest.ApplicationName).ConfigureAwait(false)
                ?? throw new Exception(ExceptionConstants.DataNotFoundExceptionMessage);
            var agentDetails = workspaceDetails.ActiveAgentsListInWorkspace.FirstOrDefault(agent => agent.AgentGuid == chatRequest.AgentId)
                ?? throw new Exception(ExceptionConstants.DataNotFoundExceptionMessage);

            var agentChatRequestModel = new ChatRequestDomain()
            {
                AgentId = agentDetails.AgentGuid,
                AgentName = agentDetails.AgentName,
                ConversationId = chatRequest.ConversationId,
                UserMessage = chatRequest.UserMessage,
            };
            return await agentChatService.GetAgentChatResponseAsync(agentChatRequestModel).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodFailed, nameof(InvokeWorkspaceAgentAsync), DateTime.UtcNow, ex.Message);
            throw;
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(InvokeWorkspaceAgentAsync), DateTime.UtcNow, JsonConvert.SerializeObject(chatRequest));
        }
    }

    /// <summary>
    /// Updates the existing workspace data.
    /// </summary>
    /// <param name="agentsWorkspaceData">The agents workspace data domain model.</param>
    /// <param name="currentUserEmail">The current logged in user email.</param>
    /// <returns>A boolean for <c>success/failure.</c></returns>
    public async Task<bool> UpdateExistingWorkspaceDataAsync(AgentsWorkspaceDomain agentsWorkspaceData, string currentUserEmail)
    {
        ArgumentNullException.ThrowIfNull(agentsWorkspaceData);
        ArgumentException.ThrowIfNullOrWhiteSpace(currentUserEmail);

        try
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(UpdateExistingWorkspaceDataAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { agentsWorkspaceData, currentUserEmail }));

            var filter = Builders<AgentsWorkspaceDomain>.Filter.And(
                   Builders<AgentsWorkspaceDomain>.Filter.Eq(x => x.IsActive, true), Builders<AgentsWorkspaceDomain>.Filter.Eq(x => x.AgentWorkspaceGuid, agentsWorkspaceData.AgentWorkspaceGuid));
            var allWorkspacesData = await mongoDatabaseService.GetDataFromCollectionAsync(this.MongoDatabaseName, this.WorkspacesCollectionName, filter).ConfigureAwait(false);
            var existingWorkspaceData = allWorkspacesData.FirstOrDefault() ?? throw new Exception(ExceptionConstants.DataNotFoundExceptionMessage);

            if (!existingWorkspaceData.WorkspaceUsers.Contains(currentUserEmail))
                throw new UnauthorizedAccessException(ExceptionConstants.UnauthorizedUserExceptionMessage);

            var updates = new List<UpdateDefinition<AgentsWorkspaceDomain>>
            {
                Builders<AgentsWorkspaceDomain>.Update.Set(x => x.AgentWorkspaceName, agentsWorkspaceData.AgentWorkspaceName),
                Builders<AgentsWorkspaceDomain>.Update.Set(x => x.ActiveAgentsListInWorkspace, agentsWorkspaceData.ActiveAgentsListInWorkspace),
                Builders<AgentsWorkspaceDomain>.Update.Set(x => x.WorkspaceUsers, agentsWorkspaceData.WorkspaceUsers),
                Builders<AgentsWorkspaceDomain>.Update.Set(x => x.DateModified, DateTime.UtcNow),
                Builders<AgentsWorkspaceDomain>.Update.Set(x => x.ModifiedBy, currentUserEmail)
            };

            var update = Builders<AgentsWorkspaceDomain>.Update.Combine(updates);
            return await mongoDatabaseService.UpdateDataInCollectionAsync(filter, update, this.MongoDatabaseName, this.WorkspacesCollectionName).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodFailed, nameof(UpdateExistingWorkspaceDataAsync), DateTime.UtcNow, ex.Message);
            throw;
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(UpdateExistingWorkspaceDataAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { agentsWorkspaceData, currentUserEmail }));
        }
    }
}
