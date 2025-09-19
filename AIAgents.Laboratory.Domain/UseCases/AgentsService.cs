using System.Globalization;
using AIAgents.Laboratory.Domain.DomainEntities.AgentsEntities;
using AIAgents.Laboratory.Domain.DrivenPorts;
using AIAgents.Laboratory.Domain.DrivingPorts;
using Microsoft.Extensions.Logging;
using static AIAgents.Laboratory.Domain.Helpers.Constants;

namespace AIAgents.Laboratory.Domain.UseCases;

/// <summary>
/// The Agents Service class.
/// </summary>
/// <param name="logger">The logger service.</param>
/// <param name="mongoDatabaseService">The mongo database service.</param>
/// <seealso cref="IAgentsService" />
public class AgentsService(ILogger<AgentsService> logger, IMongoDatabaseService mongoDatabaseService) : IAgentsService
{
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
            var allData = await mongoDatabaseService.GetDataFromCollectionAsync<AgentDataDomain>(MongoDbCollectionConstants.AiAgentsPrimaryDatabase, MongoDbCollectionConstants.AgentsCollectionName).ConfigureAwait(false);
            return allData.FirstOrDefault(x => x.AgentId == agentId) ?? throw new Exception(ExceptionConstants.AgentNotFoundExceptionMessage);
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
            return await mongoDatabaseService.GetDataFromCollectionAsync<AgentDataDomain>(MongoDbCollectionConstants.AiAgentsPrimaryDatabase, MongoDbCollectionConstants.AgentsCollectionName).ConfigureAwait(false);
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
    /// <returns>The updated agent data dto.</returns>
    public async Task<AgentDataDomain> UpdateExistingAgentDataAsync(AgentDataDomain updateDataDomain)
    {
        try
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodStart, nameof(UpdateExistingAgentDataAsync), DateTime.UtcNow, updateDataDomain.AgentId));

            var allAgents = await mongoDatabaseService.GetDataFromCollectionAsync<AgentDataDomain>(MongoDbCollectionConstants.AiAgentsPrimaryDatabase, MongoDbCollectionConstants.AgentsCollectionName).ConfigureAwait(false);
            var updateAgent = allAgents.FirstOrDefault(x => x.AgentId == updateDataDomain.AgentId) ?? throw new Exception(ExceptionConstants.AgentNotFoundExceptionMessage);

            updateAgent.AgentMetaPrompt = updateDataDomain.AgentMetaPrompt;
            updateAgent.AgentName = updateDataDomain.AgentName;

            return await mongoDatabaseService.UpdateDataFromCollectionAsync<AgentDataDomain, AgentDataDomain>(
                updateDataDomain, MongoDbCollectionConstants.AiAgentsPrimaryDatabase, MongoDbCollectionConstants.AgentsCollectionName).ConfigureAwait(false);
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

            var allAgents = await mongoDatabaseService.GetDataFromCollectionAsync<AgentDataDomain>(MongoDbCollectionConstants.AiAgentsPrimaryDatabase, MongoDbCollectionConstants.AgentsCollectionName).ConfigureAwait(false);
            var updateAgent = allAgents.FirstOrDefault(x => x.AgentId == agentId) ?? throw new Exception(ExceptionConstants.AgentNotFoundExceptionMessage);

            updateAgent.IsActive = false;
            await mongoDatabaseService.UpdateDataFromCollectionAsync<AgentDataDomain, AgentDataDomain>(
                updateAgent, MongoDbCollectionConstants.AiAgentsPrimaryDatabase, MongoDbCollectionConstants.AgentsCollectionName).ConfigureAwait(false);

            return true;
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
