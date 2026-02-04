using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.Domain.DomainEntities.AgentsEntities;
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
/// The Agents Service class.
/// </summary>
/// <param name="logger">The logger service.</param>
/// <param name="mongoDatabaseService">The mongo db database service.</param>
/// <param name="configuration">The configuration service.</param>
/// <param name="documentIntelligenceService">The document intelligence service.</param>
/// <param name="toolSkillsService">The tools skill service.</param>
/// <seealso cref="IAgentsService" />
public sealed class AgentsService(ILogger<AgentsService> logger, IConfiguration configuration, IMongoDatabaseService mongoDatabaseService,
    IDocumentIntelligenceService documentIntelligenceService, IToolSkillsService toolSkillsService) : IAgentsService
{
    /// <summary>
    /// The mongo database name configuration value.
    /// </summary>
    private readonly string MongoDatabaseName = configuration[MongoDbCollectionConstants.AiAgentsPrimaryDatabase] ?? throw new KeyNotFoundException(ExceptionConstants.ConfigurationKeyNotFoundExceptionMessage);

    /// <summary>
    /// The agents data collection name configuration value.
    /// </summary>
    private readonly string AgentsDataCollectionName = configuration[MongoDbCollectionConstants.AgentsCollectionName] ?? throw new KeyNotFoundException(ExceptionConstants.ConfigurationKeyNotFoundExceptionMessage);

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
    /// <returns>
    /// The boolean for success/failure.
    /// </returns>
    public async Task<bool> CreateNewAgentAsync(AgentDataDomain agentData, string userEmail)
    {
        ArgumentNullException.ThrowIfNull(agentData);
        ArgumentException.ThrowIfNullOrWhiteSpace(userEmail);

        try
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(CreateNewAgentAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { userEmail, agentData.AgentName }));

            agentData.AgentId = Guid.NewGuid().ToString();
            if (agentData.KnowledgeBaseDocument is not null && agentData.KnowledgeBaseDocument.Any() && this.IsKnowledgeBaseServiceAllowed)
                await documentIntelligenceService.CreateAndProcessKnowledgeBaseDocumentAsync(agentData).ConfigureAwait(false);

            if (agentData.VisionImages is not null && agentData.VisionImages.Any() && this.IsAiVisionServiceAllowed)
                await documentIntelligenceService.CreateAndProcessAiVisionImagesKeywordsAsync(agentData).ConfigureAwait(false);
            if (agentData.AssociatedSkillGuids.Any())
                await this.UpdateSkillsWithAssociatedAgentsDataAsync(agentData, userEmail).ConfigureAwait(false);

            agentData.PrepareAuditEntityData(userEmail);
            return await mongoDatabaseService.SaveDataAsync(
                data: agentData,
                databaseName: this.MongoDatabaseName,
                collectionName: this.AgentsDataCollectionName).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, LoggingConstants.LogHelperMethodFailed, nameof(CreateNewAgentAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message);
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(CreateNewAgentAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { userEmail, agentData.AgentName }));
        }
    }

    /// <summary>
    /// Gets the agent data by identifier asynchronous.
    /// </summary>
    /// <param name="agentId">The agent identifier.</param>
    /// <returns>
    /// The agent data dto.
    /// </returns>
    public async Task<AgentDataDomain> GetAgentDataByIdAsync(string agentId, string userEmail)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(agentId);

        try
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(GetAgentDataByIdAsync), DateTime.UtcNow, agentId);

            var filter = Builders<AgentDataDomain>.Filter.And(
                Builders<AgentDataDomain>.Filter.Eq(x => x.IsActive, true),
                Builders<AgentDataDomain>.Filter.Eq(x => x.AgentId, agentId));

            if (!string.IsNullOrEmpty(userEmail))
                filter = Builders<AgentDataDomain>.Filter.And(
                    filter,
                    Builders<AgentDataDomain>.Filter.Or(
                        Builders<AgentDataDomain>.Filter.Eq(x => x.IsPrivate, false),
                        Builders<AgentDataDomain>.Filter.And(
                            Builders<AgentDataDomain>.Filter.Eq(x => x.IsPrivate, true),
                            Builders<AgentDataDomain>.Filter.Eq(x => x.CreatedBy, userEmail)
                        )
                    )
                );

            var allData = await mongoDatabaseService.GetDataFromCollectionAsync(
                databaseName: this.MongoDatabaseName,
                collectionName: this.AgentsDataCollectionName,
                filter).ConfigureAwait(false);

            var agentData = allData.First() ?? throw new FileNotFoundException(ExceptionConstants.AgentNotFoundExceptionMessage);
            if (agentData.StoredKnowledgeBase is not null && agentData.StoredKnowledgeBase.Any())
                agentData.ConvertKnowledgebaseBinaryDataToFile();

            return agentData;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, LoggingConstants.LogHelperMethodFailed, nameof(GetAgentDataByIdAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message);
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetAgentDataByIdAsync), DateTime.UtcNow, agentId);
        }
    }

    /// <summary>
    /// Gets all agents data asynchronous.
    /// </summary>
    /// <param name="userEmail">The current logged in user email.</param>
    /// <returns>The list of <see cref="AgentDataDomain"/></returns>
    public async Task<IEnumerable<AgentDataDomain>> GetAllAgentsDataAsync(string userEmail)
    {
        try
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(GetAllAgentsDataAsync), DateTime.UtcNow, userEmail);

            var filter = Builders<AgentDataDomain>.Filter.And(
                Builders<AgentDataDomain>.Filter.Eq(x => x.IsActive, true),
                Builders<AgentDataDomain>.Filter.Eq(x => x.IsDefaultChatbot, false),
                Builders<AgentDataDomain>.Filter.Or(
                    Builders<AgentDataDomain>.Filter.Eq(x => x.IsPrivate, false),
                    Builders<AgentDataDomain>.Filter.And(
                        Builders<AgentDataDomain>.Filter.Eq(x => x.IsPrivate, true),
                        Builders<AgentDataDomain>.Filter.Eq(x => x.CreatedBy, userEmail)
                    )
                )
            );

            var agents = await mongoDatabaseService.GetDataFromCollectionAsync(
                databaseName: this.MongoDatabaseName,
                collectionName: this.AgentsDataCollectionName,
                filter).ConfigureAwait(false);
            // Process stored knowledge base data if available
            foreach (var agent in from agent in agents where agent.StoredKnowledgeBase is not null && agent.StoredKnowledgeBase.Any() select agent)
                agent.ConvertKnowledgebaseBinaryDataToFile();

            return agents;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, LoggingConstants.LogHelperMethodFailed, nameof(GetAllAgentsDataAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message);
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetAllAgentsDataAsync), DateTime.UtcNow, userEmail);
        }
    }

    /// <summary>
    /// Updates the existing agent data.
    /// </summary>
    /// <param name="updateDataDomain">The update agent data DTO model.</param>
    /// <param name="userEmail">The current logged in user email address.</param>
    /// <returns>The boolean for success/failure.</returns>
    public async Task<bool> UpdateExistingAgentDataAsync(AgentDataDomain updateDataDomain, string userEmail)
    {
        ArgumentNullException.ThrowIfNull(updateDataDomain);
        ArgumentException.ThrowIfNullOrWhiteSpace(updateDataDomain.AgentId);
        ArgumentException.ThrowIfNullOrWhiteSpace(userEmail);

        try
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(UpdateExistingAgentDataAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { updateDataDomain.AgentId, updateDataDomain.ModifiedBy }));

            var filter = Builders<AgentDataDomain>.Filter.And(Builders<AgentDataDomain>.Filter.Eq(x => x.IsActive, true), Builders<AgentDataDomain>.Filter.Eq(x => x.AgentId, updateDataDomain.AgentId));
            var agentsData = await mongoDatabaseService.GetDataFromCollectionAsync(this.MongoDatabaseName, this.AgentsDataCollectionName, filter).ConfigureAwait(false);
            var existingAgent = agentsData.FirstOrDefault() ?? throw new KeyNotFoundException(ExceptionConstants.AgentNotFoundExceptionMessage);

            var updates = new List<UpdateDefinition<AgentDataDomain>>
            {
                Builders<AgentDataDomain>.Update.Set(x => x.AgentMetaPrompt, updateDataDomain.AgentMetaPrompt),
                Builders<AgentDataDomain>.Update.Set(x => x.AgentName, updateDataDomain.AgentName),
                Builders<AgentDataDomain>.Update.Set(x => x.ApplicationName, updateDataDomain.ApplicationName),
                Builders<AgentDataDomain>.Update.Set(x => x.IsPrivate, updateDataDomain.IsPrivate),
                Builders<AgentDataDomain>.Update.Set(x => x.AgentDescription, updateDataDomain.AgentDescription),
                Builders<AgentDataDomain>.Update.Set(x => x.DateModified, DateTime.UtcNow),
                Builders<AgentDataDomain>.Update.Set(x => x.AssociatedSkillGuids, updateDataDomain.AssociatedSkillGuids),
                Builders<AgentDataDomain>.Update.Set(x => x.ModifiedBy, userEmail)
            };

            if (this.IsKnowledgeBaseServiceAllowed)
                await documentIntelligenceService.HandleKnowledgeBaseDataUpdateAsync(updateDataDomain, updates, existingAgent).ConfigureAwait(false);

            if (this.IsAiVisionServiceAllowed)
                await documentIntelligenceService.HandleAiVisionImagesDataUpdateAsync(updateDataDomain, updates, existingAgent).ConfigureAwait(false);

            if (updateDataDomain.AssociatedSkillGuids.Any())
                await this.UpdateSkillsWithAssociatedAgentsDataAsync(updateDataDomain, userEmail).ConfigureAwait(false);

            var update = Builders<AgentDataDomain>.Update.Combine(updates);
            return await mongoDatabaseService.UpdateDataInCollectionAsync(
                filter,
                update,
                databaseName: this.MongoDatabaseName,
                collectionName: this.AgentsDataCollectionName).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, LoggingConstants.LogHelperMethodFailed, nameof(UpdateExistingAgentDataAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message);
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(UpdateExistingAgentDataAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { updateDataDomain.AgentId, updateDataDomain.ModifiedBy }));
        }
    }

    /// <summary>
    /// Deletes an existing agent data.
    /// </summary>
    /// <param name="agentId">The agent id.</param>
    /// <param name="currentUserEmail">The current logged in user email</param>
    /// <returns>The boolean for success/failure.</returns>
    public async Task<bool> DeleteExistingAgentDataAsync(string agentId, string currentUserEmail)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(agentId);
        ArgumentException.ThrowIfNullOrWhiteSpace(currentUserEmail);

        try
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(DeleteExistingAgentDataAsync), DateTime.UtcNow, agentId);

            var filter = Builders<AgentDataDomain>.Filter.Where(x => x.IsActive && x.AgentId == agentId);
            var allAgents = await mongoDatabaseService.GetDataFromCollectionAsync(
                databaseName: this.MongoDatabaseName,
                collectionName: this.AgentsDataCollectionName,
                filter: filter).ConfigureAwait(false);
            var updateAgent = allAgents.FirstOrDefault() ?? throw new KeyNotFoundException(ExceptionConstants.AgentNotFoundExceptionMessage);

            if (updateAgent.CreatedBy != currentUserEmail)
                throw new UnauthorizedAccessException(ExceptionConstants.UnauthorizedUserExceptionMessage);

            var updates = new List<UpdateDefinition<AgentDataDomain>>
            {
                Builders<AgentDataDomain>.Update.Set(x => x.IsActive, false),
                Builders<AgentDataDomain>.Update.Set(x => x.DateModified, DateTime.UtcNow)
            };
            var update = Builders<AgentDataDomain>.Update.Combine(updates);
            await documentIntelligenceService.DeleteKnowledgebaseAndImagesDataAsync(agentId).ConfigureAwait(false);
            return await mongoDatabaseService.UpdateDataInCollectionAsync(
                filter,
                update,
                databaseName: this.MongoDatabaseName,
                collectionName: this.AgentsDataCollectionName).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, LoggingConstants.LogHelperMethodFailed, nameof(DeleteExistingAgentDataAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message);
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(DeleteExistingAgentDataAsync), DateTime.UtcNow, agentId);
        }
    }

    /// <summary>
    /// Downloads the knowledgebase file asynchronous.
    /// </summary>
    /// <param name="agentGuid">The agent guid id.</param>
    /// <param name="fileName">The file name.</param>
    /// <returns>The downloaded file url</returns>
    public async Task<string> DownloadKnowledgebaseFileAsync(string agentGuid, string fileName)
    {
        try
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(DeleteExistingAgentDataAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { agentGuid, fileName }));
            return await documentIntelligenceService.DownloadKnowledgebaseFileAsync(agentGuid, fileName).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, LoggingConstants.LogHelperMethodFailed, nameof(DownloadKnowledgebaseFileAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message);
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(DeleteExistingAgentDataAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { agentGuid, fileName }));
        }
    }

    #region PRIVATE METHODS

    /// <summary>
    /// Updates the skills with associated agents data asynchronous.
    /// </summary>
    /// <param name="agentData">The agent data domain model.</param>
    /// <param name="currentUserEmail">The current logged in user email.</param>
    /// <returns>A task to wait on.</returns>
    private async Task UpdateSkillsWithAssociatedAgentsDataAsync(AgentDataDomain agentData, string currentUserEmail)
    {
        var associatedAgentsData = new List<AssociatedAgentsSkillDataDomain>
        {
            new()
            {
                AgentGuid = agentData.AgentId,
                AgentName = agentData.AgentName
            }
        };
        await toolSkillsService.AssociateSkillAndAgentAsync(associatedAgentsData, agentData.AssociatedSkillGuids[0], currentUserEmail);
    }

    #endregion
}