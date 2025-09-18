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
	/// <returns>
	/// The boolean for success/failure.
	/// </returns>
	public async Task<bool> CreateNewAgentAsync(AgentDataDomain agentData)
	{
		try
		{
			logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodStart, nameof(CreateNewAgentAsync), DateTime.UtcNow, agentData.AgentName));

			agentData.AgentId = Guid.NewGuid().ToString();
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
			return await mongoDatabaseService.GetDataFromCollectionAsync<string, AgentDataDomain>(agentId, MongoDbCollectionConstants.AiAgentsPrimaryDatabase, MongoDbCollectionConstants.AgentsCollectionName).ConfigureAwait(false);
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
}
