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
            if (agentData.KnowledgeBaseDocument is not null && agentData.KnowledgeBaseDocument.Any() && IsKnowledgeBaseServiceAllowed)
            {
                agentData.ValidateUploadedFile();
                await agentData.ProcessKnowledgebaseDocumentDataAsync().ConfigureAwait(false);
                if (agentData.StoredKnowledgeBase is not null && agentData.StoredKnowledgeBase.Any())
                {
                    foreach (var file in agentData.StoredKnowledgeBase)
                    {
                        var content = knowledgeBaseProcessor.DetectAndReadFileContent(file);
                        await knowledgeBaseProcessor.ProcessKnowledgeBaseDocumentAsync(content, agentData.AgentId).ConfigureAwait(false);
                    }
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

            var allData = await mongoDatabaseService.GetDataFromCollectionAsync(
                MongoDbCollectionConstants.AiAgentsPrimaryDatabase, MongoDbCollectionConstants.AgentsCollectionName, filter).ConfigureAwait(false);

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

            var agents = await mongoDatabaseService.GetDataFromCollectionAsync(
                MongoDbCollectionConstants.AiAgentsPrimaryDatabase, MongoDbCollectionConstants.AgentsCollectionName, filter).ConfigureAwait(false);

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
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodStart, nameof(UpdateExistingAgentDataAsync), DateTime.UtcNow, updateDataDomain.AgentId));

            var filter = Builders<AgentDataDomain>.Filter.And(Builders<AgentDataDomain>.Filter.Eq(x => x.IsActive, true), Builders<AgentDataDomain>.Filter.Eq(x => x.AgentId, updateDataDomain.AgentId));
            var agentsData = await mongoDatabaseService.GetDataFromCollectionAsync(MongoDbCollectionConstants.AiAgentsPrimaryDatabase, MongoDbCollectionConstants.AgentsCollectionName, filter).ConfigureAwait(false);
            var existingAgent = agentsData.FirstOrDefault() ?? throw new Exception(ExceptionConstants.AgentNotFoundExceptionMessage);

            var updates = new List<UpdateDefinition<AgentDataDomain>>
            {
                Builders<AgentDataDomain>.Update.Set(x => x.AgentMetaPrompt, updateDataDomain.AgentMetaPrompt),
                Builders<AgentDataDomain>.Update.Set(x => x.AgentName, updateDataDomain.AgentName),
                Builders<AgentDataDomain>.Update.Set(x => x.McpServerUrl, updateDataDomain.McpServerUrl),
                Builders<AgentDataDomain>.Update.Set(x => x.IsPrivate, updateDataDomain.IsPrivate),
            };

            if (IsKnowledgeBaseServiceAllowed)
                await this.HandleKnowledgeBaseDataUpdateAsync(updateDataDomain, updates, existingAgent).ConfigureAwait(false);

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

    /// <summary>
    /// Processes updates to an agent's knowledge base documents, including adding new documents and removing specified ones, and prepares the corresponding update definitions for persistence.
    /// </summary>
    /// <remarks>This method only adds update definitions to the provided list if changes to the knowledge base are detected. It validates and processes any newly uploaded documents and ensures that removed documents
    /// are excluded from the persisted knowledge base. The caller is responsible for applying the accumulated updates to the data store.</remarks>
    /// <param name="updateDataDomain">The domain object containing the knowledge base update information, including any new or removed documents. Cannot be null.</param>
    /// <param name="updates">A list to which update definitions for the agent's knowledge base will be added if changes are detected. Cannot be null.</param>
    /// <param name="existingAgent">The current state of the agent's data, used as the baseline for applying knowledge base updates. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task HandleKnowledgeBaseDataUpdateAsync(AgentDataDomain updateDataDomain, List<UpdateDefinition<AgentDataDomain>> updates, AgentDataDomain existingAgent)
    {
        try
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodStart, nameof(HandleKnowledgeBaseDataUpdateAsync), DateTime.UtcNow, updateDataDomain.AgentId));

            // Start from existing stored knowledge base
            var updatedStoredKnowledgeBase = existingAgent.StoredKnowledgeBase?.ToList() ?? [];
            var hasChanges = false;

            // 1. Remove any existing documents whose names are in RemovedKnowledgeBaseDocuments
            if (updateDataDomain.RemovedKnowledgeBaseDocuments is not null && updateDataDomain.RemovedKnowledgeBaseDocuments.Any())
            {
                var removedSet = new HashSet<string>(updateDataDomain.RemovedKnowledgeBaseDocuments, StringComparer.OrdinalIgnoreCase);
                updatedStoredKnowledgeBase = [.. updatedStoredKnowledgeBase.Where(doc => !removedSet.Contains(doc.FileName))];
                hasChanges = true;
            }

            // 2. Process any newly uploaded knowledge base documents
            if (updateDataDomain.KnowledgeBaseDocument is not null && updateDataDomain.KnowledgeBaseDocument.Any())
            {
                updateDataDomain.ValidateUploadedFile();
                await updateDataDomain.ProcessKnowledgebaseDocumentDataAsync().ConfigureAwait(false);

                if (updateDataDomain.StoredKnowledgeBase is not null && updateDataDomain.StoredKnowledgeBase.Any())
                {
                    foreach (var file in updateDataDomain.StoredKnowledgeBase)
                    {
                        var content = knowledgeBaseProcessor.DetectAndReadFileContent(file);
                        await knowledgeBaseProcessor.ProcessKnowledgeBaseDocumentAsync(content, updateDataDomain.AgentId).ConfigureAwait(false);
                    }

                    updatedStoredKnowledgeBase.AddRange(updateDataDomain.StoredKnowledgeBase);
                    hasChanges = true;
                }
            }

            // 3. Only persist changes if something actually changed
            if (hasChanges)
                // If all documents were removed, set StoredKnowledgeBase to null, otherwise save the updated list
                updates.Add(Builders<AgentDataDomain>.Update.Set(
                    x => x.StoredKnowledgeBase,
                    updatedStoredKnowledgeBase.Count != 0 ? updatedStoredKnowledgeBase : null));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(HandleKnowledgeBaseDataUpdateAsync), DateTime.UtcNow, ex.Message));
            throw;
        }
        finally
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodEnd, nameof(HandleKnowledgeBaseDataUpdateAsync), DateTime.UtcNow, updateDataDomain.AgentId));
        }
    }
}