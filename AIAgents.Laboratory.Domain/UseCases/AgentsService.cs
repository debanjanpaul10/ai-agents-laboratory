using System.Globalization;
using AIAgents.Laboratory.Domain.DomainEntities.AgentsEntities;
using AIAgents.Laboratory.Domain.DrivenPorts;
using AIAgents.Laboratory.Domain.DrivingPorts;
using AIAgents.Laboratory.Domain.Helpers;
using AIAgents.Laboratory.Processor.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using static AIAgents.Laboratory.Domain.Helpers.Constants;

namespace AIAgents.Laboratory.Domain.UseCases;

/// <summary>
/// The Agents Service class.
/// </summary>
/// <param name="knowledgeBaseProcessor">The knowledge base processor service.</param>
/// <param name="logger">The logger service.</param>
/// <param name="mongoDatabaseService">The mongo db database service.</param>
/// <param name="configuration">The configuration service.</param>
/// <seealso cref="AIAgents.Laboratory.Domain.DrivingPorts.IAgentsService" />
public class AgentsService(ILogger<AgentsService> logger, IMongoDatabaseService mongoDatabaseService, IKnowledgeBaseProcessor knowledgeBaseProcessor, IConfiguration configuration) : IAgentsService
{
    /// <summary>
    /// The is knowledge base service allowed
    /// </summary>
    private readonly bool IsKnowledgeBaseServiceAllowed = bool.TryParse(configuration[AzureAppConfigurationConstants.IsKnowledgeBaseServiceEnabledConstant], out var value) && value;

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
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodStart, nameof(CreateNewAgentAsync), DateTime.UtcNow, agentData.AgentName));

            agentData.AgentId = Guid.NewGuid().ToString();
            if (agentData.KnowledgeBaseDocument is not null && agentData.KnowledgeBaseDocument.Length > 0 && IsKnowledgeBaseServiceAllowed)
            {
                agentData.ValidateUploadedFile();
                await agentData.ProcessKnowledgebaseDocumentDataAsync().ConfigureAwait(false);
                if (agentData.StoredKnowledgeBase?.FileContent is not null)
                {
                    var content = System.Text.Encoding.UTF8.GetString(agentData.StoredKnowledgeBase.FileContent);
                    await knowledgeBaseProcessor.ProcessKnowledgeBaseDocumentAsync(content, agentData.AgentId).ConfigureAwait(false);
                }
            }

            agentData.IsActive = true;
            agentData.DateCreated = DateTime.UtcNow;
            agentData.CreatedBy = userEmail;
            return await mongoDatabaseService.SaveDataAsync(agentData, MongoDbCollectionConstants.AiAgentsPrimaryDatabase, MongoDbCollectionConstants.AgentsCollectionName).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(CreateNewAgentAsync), DateTime.UtcNow, ex.Message));
            throw;
        }
        finally
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodEnd, nameof(CreateNewAgentAsync), DateTime.UtcNow, agentData.AgentName));
        }
    }

    /// <summary>
    /// Gets the agent data by identifier asynchronous.
    /// </summary>
    /// <param name="agentId">The agent identifier.</param>
    /// <returns>
    /// The agent data dto.
    /// </returns>
    public async Task<AgentDataDomain> GetAgentDataByIdAsync(string agentId)
    {
        try
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodStart, nameof(GetAgentDataByIdAsync), DateTime.UtcNow, agentId));

            var allData = await mongoDatabaseService.GetDataFromCollectionAsync(
                MongoDbCollectionConstants.AiAgentsPrimaryDatabase, MongoDbCollectionConstants.AgentsCollectionName,
                Builders<AgentDataDomain>.Filter.Where(x => x.AgentId == agentId && x.IsActive)).ConfigureAwait(false);

            var agentData = allData.First() ?? throw new Exception(ExceptionConstants.AgentNotFoundExceptionMessage);
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
    /// <returns>
    /// The list of <see cref="AgentDataDomain" />
    /// </returns>
    public async Task<IEnumerable<AgentDataDomain>> GetAllAgentsDataAsync()
    {
        try
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodStart, nameof(GetAllAgentsDataAsync), DateTime.UtcNow, string.Empty));

            // Get all active agents from the database
            var agents = await mongoDatabaseService.GetDataFromCollectionAsync(
                MongoDbCollectionConstants.AiAgentsPrimaryDatabase,
                MongoDbCollectionConstants.AgentsCollectionName,
                Builders<AgentDataDomain>.Filter.Where(x => x.IsActive)).ConfigureAwait(false);

            // Process stored knowledge base data if available
            foreach (var agent in agents) agent.ConvertKnowledgebaseBinaryDataToFile();
            return agents;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(GetAllAgentsDataAsync), DateTime.UtcNow, ex.Message));
            throw;
        }
        finally
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodEnd, nameof(GetAllAgentsDataAsync), DateTime.UtcNow, string.Empty));
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
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodStart, nameof(UpdateExistingAgentDataAsync), DateTime.UtcNow, updateDataDomain.AgentId));

            var filter = Builders<AgentDataDomain>.Filter.And(
                Builders<AgentDataDomain>.Filter.Eq(x => x.IsActive, true),
                Builders<AgentDataDomain>.Filter.Eq(x => x.AgentId, updateDataDomain.AgentId));

            var agentsData = await mongoDatabaseService.GetDataFromCollectionAsync(
                MongoDbCollectionConstants.AiAgentsPrimaryDatabase, MongoDbCollectionConstants.AgentsCollectionName, filter).ConfigureAwait(false);
            var existingAgent = agentsData.FirstOrDefault() ?? throw new Exception(ExceptionConstants.AgentNotFoundExceptionMessage);

            var updates = new List<UpdateDefinition<AgentDataDomain>>
            {
                Builders<AgentDataDomain>.Update.Set(x => x.AgentMetaPrompt, updateDataDomain.AgentMetaPrompt),
                Builders<AgentDataDomain>.Update.Set(x => x.AgentName, updateDataDomain.AgentName)
            };

            if (IsKnowledgeBaseServiceAllowed)
            {
                if (updateDataDomain.KnowledgeBaseDocument is not null && updateDataDomain.KnowledgeBaseDocument.Length > 0)
                {
                    updateDataDomain.ValidateUploadedFile();
                    await updateDataDomain.ProcessKnowledgebaseDocumentDataAsync().ConfigureAwait(false);
                    if (updateDataDomain.StoredKnowledgeBase?.FileContent is not null)
                    {
                        var content = System.Text.Encoding.UTF8.GetString(updateDataDomain.StoredKnowledgeBase.FileContent);
                        await knowledgeBaseProcessor.ProcessKnowledgeBaseDocumentAsync(content, updateDataDomain.AgentId).ConfigureAwait(false);
                    }

                    updates.Add(Builders<AgentDataDomain>.Update.Set(x => x.StoredKnowledgeBase, updateDataDomain.StoredKnowledgeBase));
                }
                else if (existingAgent.StoredKnowledgeBase is not null)
                {
                    updates.Add(Builders<AgentDataDomain>.Update.Set(x => x.StoredKnowledgeBase, null));
                }
            }

            var update = Builders<AgentDataDomain>.Update.Combine(updates);
            return await mongoDatabaseService.UpdateDataInCollectionAsync(filter, update, MongoDbCollectionConstants.AiAgentsPrimaryDatabase, MongoDbCollectionConstants.AgentsCollectionName).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(UpdateExistingAgentDataAsync), DateTime.UtcNow, ex.Message));
            throw;
        }
        finally
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodEnd, nameof(UpdateExistingAgentDataAsync), DateTime.UtcNow, updateDataDomain.AgentId));
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
            var allAgents = await mongoDatabaseService.GetDataFromCollectionAsync(MongoDbCollectionConstants.AiAgentsPrimaryDatabase, MongoDbCollectionConstants.AgentsCollectionName, filter).ConfigureAwait(false);
            var updateAgent = allAgents.FirstOrDefault() ?? throw new Exception(ExceptionConstants.AgentNotFoundExceptionMessage);

            var update = Builders<AgentDataDomain>.Update.Set(x => x.IsActive, false);
            return await mongoDatabaseService.UpdateDataInCollectionAsync(filter, update, MongoDbCollectionConstants.AiAgentsPrimaryDatabase, MongoDbCollectionConstants.AgentsCollectionName).ConfigureAwait(false);
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