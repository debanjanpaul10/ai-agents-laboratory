using System.Globalization;
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
/// <seealso cref="IAgentsService" />
public class AgentsService(ILogger<AgentsService> logger, IConfiguration configuration, IMongoDatabaseService mongoDatabaseService, IDocumentIntelligenceService documentIntelligenceService) : IAgentsService
{
    /// <summary>
    /// The mongo database name configuration value.
    /// </summary>
    private readonly string MongoDatabaseName = configuration[MongoDbCollectionConstants.AiAgentsPrimaryDatabase] ?? throw new Exception(ExceptionConstants.ConfigurationKeyNotFoundExceptionMessage);

    /// <summary>
    /// The agents data collection name configuration value.
    /// </summary>
    private readonly string AgentsDataCollectionName = configuration[MongoDbCollectionConstants.AgentsCollectionName] ?? throw new Exception(ExceptionConstants.ConfigurationKeyNotFoundExceptionMessage);

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
        try
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(CreateNewAgentAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { userEmail, agentData.AgentName }));

            agentData.AgentId = Guid.NewGuid().ToString();
            if (agentData.KnowledgeBaseDocument is not null && agentData.KnowledgeBaseDocument.Any() && IsKnowledgeBaseServiceAllowed)
                await documentIntelligenceService.CreateAndProcessKnowledgeBaseDocumentAsync(agentData).ConfigureAwait(false);

            if (agentData.VisionImages is not null && agentData.VisionImages.Any() && IsAiVisionServiceAllowed)
                await documentIntelligenceService.CreateAndProcessAiVisionImagesKeywordsAsync(agentData).ConfigureAwait(false);

            agentData.PrepareAuditEntityData(userEmail);
            return await mongoDatabaseService.SaveDataAsync(agentData, MongoDatabaseName, AgentsDataCollectionName).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, LoggingConstants.LogHelperMethodFailed, nameof(CreateNewAgentAsync), DateTime.UtcNow, ex.Message);
            throw;
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
        try
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodStart, nameof(GetAgentDataByIdAsync), DateTime.UtcNow, agentId));

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

            var allData = await mongoDatabaseService.GetDataFromCollectionAsync(MongoDatabaseName, AgentsDataCollectionName, filter).ConfigureAwait(false);

            var agentData = allData.First() ?? throw new Exception(ExceptionConstants.AgentNotFoundExceptionMessage);
            if (agentData.StoredKnowledgeBase is not null && agentData.StoredKnowledgeBase.Any())
                agentData.ConvertKnowledgebaseBinaryDataToFile();

            return agentData;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(GetAgentDataByIdAsync), DateTime.UtcNow, ex.Message));
            throw;
        }
        finally
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodEnd, nameof(GetAgentDataByIdAsync), DateTime.UtcNow, agentId));
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
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodStart, nameof(GetAllAgentsDataAsync), DateTime.UtcNow, userEmail));

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

            var agents = await mongoDatabaseService.GetDataFromCollectionAsync(MongoDatabaseName, AgentsDataCollectionName, filter).ConfigureAwait(false);

            // Process stored knowledge base data if available
            foreach (var agent in agents)
                if (agent.StoredKnowledgeBase is not null && agent.StoredKnowledgeBase.Any())
                    agent.ConvertKnowledgebaseBinaryDataToFile();

            return agents;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(GetAllAgentsDataAsync), DateTime.UtcNow, ex.Message));
            throw;
        }
        finally
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodEnd, nameof(GetAllAgentsDataAsync), DateTime.UtcNow, userEmail));
        }
    }

    /// <summary>
    /// Updates the existing agent data.
    /// </summary>
    /// <param name="updateDataDomain">The update agent data DTO model.</param>
    /// <returns>The boolean for success/failure.</returns>
    public async Task<bool> UpdateExistingAgentDataAsync(AgentDataDomain updateDataDomain)
    {
        ArgumentNullException.ThrowIfNull(updateDataDomain);
        ArgumentException.ThrowIfNullOrWhiteSpace(updateDataDomain.AgentId);

        try
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(UpdateExistingAgentDataAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { updateDataDomain.AgentId, updateDataDomain.ModifiedBy }));

            var filter = Builders<AgentDataDomain>.Filter.And(Builders<AgentDataDomain>.Filter.Eq(x => x.IsActive, true), Builders<AgentDataDomain>.Filter.Eq(x => x.AgentId, updateDataDomain.AgentId));
            var agentsData = await mongoDatabaseService.GetDataFromCollectionAsync(MongoDatabaseName, AgentsDataCollectionName, filter).ConfigureAwait(false);
            var existingAgent = agentsData.FirstOrDefault() ?? throw new Exception(ExceptionConstants.AgentNotFoundExceptionMessage);

            var updates = new List<UpdateDefinition<AgentDataDomain>>
            {
                Builders<AgentDataDomain>.Update.Set(x => x.AgentMetaPrompt, updateDataDomain.AgentMetaPrompt),
                Builders<AgentDataDomain>.Update.Set(x => x.AgentName, updateDataDomain.AgentName),
                Builders<AgentDataDomain>.Update.Set(x => x.McpServerUrl, updateDataDomain.McpServerUrl),
                Builders<AgentDataDomain>.Update.Set(x => x.IsPrivate, updateDataDomain.IsPrivate),
                Builders<AgentDataDomain>.Update.Set(x => x.AgentDescription, updateDataDomain.AgentDescription),
                Builders<AgentDataDomain>.Update.Set(x => x.DateModified, DateTime.UtcNow)
            };

            if (IsKnowledgeBaseServiceAllowed)
                await documentIntelligenceService.HandleKnowledgeBaseDataUpdateAsync(updateDataDomain, updates, existingAgent).ConfigureAwait(false);

            if (IsAiVisionServiceAllowed)
                await documentIntelligenceService.HandleAiVisionImagesDataUpdateAsync(updateDataDomain, updates, existingAgent).ConfigureAwait(false);

            var update = Builders<AgentDataDomain>.Update.Combine(updates);
            return await mongoDatabaseService.UpdateDataInCollectionAsync(filter, update, MongoDatabaseName, AgentsDataCollectionName).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(UpdateExistingAgentDataAsync), DateTime.UtcNow, ex.Message));
            throw;
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
    /// <returns>The boolean for success/failure.</returns>
    public async Task<bool> DeleteExistingAgentDataAsync(string agentId)
    {
        try
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodStart, nameof(DeleteExistingAgentDataAsync), DateTime.UtcNow, agentId));

            var filter = Builders<AgentDataDomain>.Filter.Where(x => x.IsActive && x.AgentId == agentId);
            var allAgents = await mongoDatabaseService.GetDataFromCollectionAsync(MongoDatabaseName, AgentsDataCollectionName, filter).ConfigureAwait(false);
            var updateAgent = allAgents.FirstOrDefault() ?? throw new Exception(ExceptionConstants.AgentNotFoundExceptionMessage);

            var updates = new List<UpdateDefinition<AgentDataDomain>>
            {
                Builders<AgentDataDomain>.Update.Set(x => x.IsActive, false),
                Builders<AgentDataDomain>.Update.Set(x => x.DateModified, DateTime.UtcNow)
            };
            var update = Builders<AgentDataDomain>.Update.Combine(updates);
            await documentIntelligenceService.DeleteKnowledgebaseAndImagesDataAsync(agentId).ConfigureAwait(false);
            return await mongoDatabaseService.UpdateDataInCollectionAsync(filter, update, MongoDatabaseName, AgentsDataCollectionName).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(DeleteExistingAgentDataAsync), DateTime.UtcNow, ex.Message));
            throw;
        }
        finally
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodEnd, nameof(DeleteExistingAgentDataAsync), DateTime.UtcNow, agentId));
        }
    }
}