using AIAgents.Laboratory.Domain.Contracts;
using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.Domain.DomainEntities.AgentsEntities;
using AIAgents.Laboratory.Domain.Helpers;
using AIAgents.Laboratory.Domain.Ports.In;
using AIAgents.Laboratory.Domain.Ports.Out;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using static AIAgents.Laboratory.Domain.Helpers.Constants;

namespace AIAgents.Laboratory.Domain.UseCases;

/// <summary>
/// The Agents Service class.
/// </summary>
/// <param name="logger">The logger service.</param>
/// <param name="configuration">The configuration service.</param>
/// <param name="correlationContext">The correlation context for logging.</param>
/// <param name="mongoDatabaseService">The mongo db database service.</param>
/// <param name="documentIntelligenceService">The document intelligence service.</param>
/// <param name="toolSkillsService">The tools skill service.</param>
/// <seealso cref="IAgentsService" />
public sealed class AgentsService(ILogger<AgentsService> logger, IConfiguration configuration, ICorrelationContext correlationContext, IAgentsDataManager agentsDataService,
    IDocumentIntelligenceService documentIntelligenceService, IToolSkillsService toolSkillsService) : IAgentsService
{
    /// <summary>
    /// The is knowledge base service allowed.
    /// </summary>
    private readonly bool IsKnowledgeBaseServiceAllowed = bool.TryParse(configuration[AzureAppConfigurationConstants.IsKnowledgeBaseServiceEnabledConstant], out var value) && value;

    /// <summary>
    /// The feature flag for AI vision service.
    /// </summary>
    private readonly bool IsAiVisionServiceAllowed = bool.TryParse(configuration[AzureAppConfigurationConstants.IsAiVisionServiceEnabledConstant], out var aivisionAllowed) && aivisionAllowed;

    /// <summary>
    /// Creates the new agent asynchronous.
    /// </summary>
    /// <param name="agentData">The agent data.</param>
    /// <param name="userEmail">The user email address.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// The boolean for success/failure.
    /// </returns>
    public async Task<bool> CreateNewAgentAsync(AgentDataDomain agentData, string userEmail, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(agentData);
        ArgumentException.ThrowIfNullOrWhiteSpace(userEmail);

        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(CreateNewAgentAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, userEmail, agentData.AgentName }));

            agentData.AgentId = Guid.NewGuid().ToString();
            if (agentData.KnowledgeBaseDocument is not null && agentData.KnowledgeBaseDocument.Any() && this.IsKnowledgeBaseServiceAllowed)
                await documentIntelligenceService.CreateAndProcessKnowledgeBaseDocumentAsync(
                    agentData,
                    cancellationToken
                ).ConfigureAwait(false);

            if (agentData.VisionImages is not null && agentData.VisionImages.Any() && this.IsAiVisionServiceAllowed)
                await documentIntelligenceService.CreateAndProcessAiVisionImagesKeywordsAsync(
                    agentData,
                    cancellationToken
                ).ConfigureAwait(false);

            if (agentData.AssociatedSkillGuids.Any())
                await this.UpdateSkillsWithAssociatedAgentsDataAsync(
                    agentData,
                    currentUserEmail: userEmail,
                    cancellationToken
                ).ConfigureAwait(false);

            agentData.PrepareAuditEntityData(userEmail);
            return await agentsDataService.CreateNewAgentAsync(
                agentData,
                userEmail,
                cancellationToken
            ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(CreateNewAgentAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(CreateNewAgentAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, userEmail, agentData.AgentName }));
        }
    }

    /// <summary>
    /// Gets the agent data by identifier asynchronous.
    /// </summary>
    /// <param name="agentId">The agent identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// The agent data dto.
    /// </returns>
    public async Task<AgentDataDomain> GetAgentDataByIdAsync(string agentId, string userEmail, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(agentId);

        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(GetAgentDataByIdAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, agentId, userEmail }));

            var agentData = await agentsDataService.GetAgentDataByIdAsync(
                agentId,
                userEmail,
                cancellationToken
            ).ConfigureAwait(false);
            if (agentData.StoredKnowledgeBase is not null && agentData.StoredKnowledgeBase.Any())
                agentData.ConvertKnowledgebaseBinaryDataToFile();

            return agentData;
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(GetAgentDataByIdAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetAgentDataByIdAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, agentId, userEmail }));
        }
    }

    /// <summary>
    /// Gets all agents data asynchronous.
    /// </summary>
    /// <param name="userEmail">The current logged in user email.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The list of <see cref="AgentDataDomain"/></returns>
    public async Task<IEnumerable<AgentDataDomain>> GetAllAgentsDataAsync(string userEmail, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(GetAllAgentsDataAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, userEmail }));

            var agents = await agentsDataService.GetAllAgentsDataAsync(
                userEmail,
                cancellationToken
            ).ConfigureAwait(false);

            // Process stored knowledge base data if available
            foreach (var agent in from agent in agents where agent.StoredKnowledgeBase is not null && agent.StoredKnowledgeBase.Any() select agent)
                agent.ConvertKnowledgebaseBinaryDataToFile();

            return agents;
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(GetAllAgentsDataAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetAllAgentsDataAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, userEmail }));
        }
    }

    /// <summary>
    /// Updates the existing agent data.
    /// </summary>
    /// <param name="updateDataDomain">The update agent data DTO model.</param>
    /// <param name="userEmail">The current logged in user email address.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The boolean for success/failure.</returns>
    public async Task<bool> UpdateExistingAgentDataAsync(AgentDataDomain updateDataDomain, string userEmail, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(updateDataDomain);
        ArgumentException.ThrowIfNullOrWhiteSpace(updateDataDomain.AgentId);
        ArgumentException.ThrowIfNullOrWhiteSpace(userEmail);

        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(UpdateExistingAgentDataAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, updateDataDomain.AgentId, updateDataDomain.ModifiedBy }));

            var existingAgent = await agentsDataService.GetAgentDataByIdAsync(
               updateDataDomain.AgentId,
                userEmail,
                cancellationToken
            ).ConfigureAwait(false);

            if (this.IsKnowledgeBaseServiceAllowed)
                await documentIntelligenceService.HandleKnowledgeBaseDataUpdateAsync(
                    updateDataDomain,
                    existingAgent,
                    cancellationToken
                ).ConfigureAwait(false);

            if (this.IsAiVisionServiceAllowed)
                await documentIntelligenceService.HandleAiVisionImagesDataUpdateAsync(
                    updateDataDomain,
                    existingAgent,
                    cancellationToken
                ).ConfigureAwait(false);

            if (updateDataDomain.AssociatedSkillGuids.Any())
                await this.UpdateSkillsWithAssociatedAgentsDataAsync(
                    agentData: updateDataDomain,
                    currentUserEmail: userEmail,
                    cancellationToken
                ).ConfigureAwait(false);

            return await agentsDataService.UpdateExistingAgentDataAsync(
                updateDataDomain,
                userEmail,
                cancellationToken
            ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(UpdateExistingAgentDataAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(UpdateExistingAgentDataAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, updateDataDomain.AgentId, updateDataDomain.ModifiedBy }));
        }
    }

    /// <summary>
    /// Deletes an existing agent data.
    /// </summary>
    /// <param name="agentId">The agent id.</param>
    /// <param name="currentUserEmail">The current logged in user email</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The boolean for success/failure.</returns>
    public async Task<bool> DeleteExistingAgentDataAsync(string agentId, string currentUserEmail, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(agentId);
        ArgumentException.ThrowIfNullOrWhiteSpace(currentUserEmail);

        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(DeleteExistingAgentDataAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentUserEmail, agentId }));

            await documentIntelligenceService.DeleteKnowledgebaseAndImagesDataAsync(
                agentId,
                cancellationToken
            ).ConfigureAwait(false);

            return await agentsDataService.DeleteExistingAgentDataAsync(
                agentId,
                currentUserEmail,
                cancellationToken
            ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(DeleteExistingAgentDataAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(DeleteExistingAgentDataAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentUserEmail, agentId }));
        }
    }

    /// <summary>
    /// Downloads the knowledgebase file asynchronous.
    /// </summary>
    /// <param name="agentGuid">The agent guid id.</param>
    /// <param name="fileName">The file name.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The downloaded file url</returns>
    public async Task<string> DownloadKnowledgebaseFileAsync(string agentGuid, string fileName, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(DownloadKnowledgebaseFileAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, agentGuid, fileName }));
            return await documentIntelligenceService.DownloadKnowledgebaseFileAsync(
                agentGuid,
                fileName,
                cancellationToken
            ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(DownloadKnowledgebaseFileAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(DeleteExistingAgentDataAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, agentGuid, fileName }));
        }
    }

    #region PRIVATE METHODS

    /// <summary>
    /// Updates the skills with associated agents data asynchronous.
    /// </summary>
    /// <param name="agentData">The agent data domain model.</param>
    /// <param name="currentUserEmail">The current logged in user email.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task to wait on.</returns>
    private async Task UpdateSkillsWithAssociatedAgentsDataAsync(AgentDataDomain agentData, string currentUserEmail, CancellationToken cancellationToken = default)
    {
        var associatedAgentsData = new List<AssociatedAgentsSkillDataDomain>
        {
            new()
            {
                AgentGuid = agentData.AgentId,
                AgentName = agentData.AgentName
            }
        };
        await toolSkillsService.AssociateSkillAndAgentAsync(
            agentData: associatedAgentsData,
            toolSkillId: agentData.AssociatedSkillGuids[0],
            currentUserEmail,
            cancellationToken
        ).ConfigureAwait(false);
    }

    #endregion
}