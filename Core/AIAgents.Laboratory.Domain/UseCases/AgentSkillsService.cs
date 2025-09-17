using System.Globalization;
using AIAgents.Laboratory.Domain.DomainEntities.AgentsEntities;
using AIAgents.Laboratory.Domain.DrivenPorts;
using AIAgents.Laboratory.Domain.DrivingPorts;
using Microsoft.Extensions.Logging;
using static AIAgents.Laboratory.Domain.Helpers.Constants;

namespace AIAgents.Laboratory.Domain.UseCases;

/// <summary>
/// The Agent Skills Business service interface.
/// </summary>
/// <param name="logger">The logger service.</param>
/// <param name="mongoDbService">The mongo db service.</param>
/// <seealso cref="IAgentSkillsService"/>
public class AgentSkillsService(ILogger<AgentSkillsService> logger, IMongoDatabaseService mongoDbService) : IAgentSkillsService
{
	/// <summary>
	/// Creates the new skill asynchronous.
	/// </summary>
	/// <param name="agentSkillRequest">The agent skill request.</param>
	/// <returns>
	/// The boolean for success/failure.
	/// </returns>
	public async Task<bool> CreateNewSkillAsync(AgentSkillDomain agentSkillRequest)
	{
		try
		{
			logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodStart, nameof(CreateNewSkillAsync), DateTime.UtcNow, agentSkillRequest.SkillName));
			return await mongoDbService.SaveDataAsync(agentSkillRequest, MongoDbCollectionConstants.AiAgentsPrimaryDatabase, MongoDbCollectionConstants.SkillsCollectionName).ConfigureAwait(false);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(CreateNewSkillAsync), DateTime.UtcNow, ex.Message));
			throw;
		}
		finally
		{
			logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodEnd, nameof(CreateNewSkillAsync), DateTime.UtcNow, agentSkillRequest.SkillName));
		}
	}

	/// <summary>
	/// Gets all skills asynchronous.
	/// </summary>
	/// <returns>
	/// The list of <see cref="AgentSkillDomain" />
	/// </returns>
	public async Task<IEnumerable<AgentSkillDomain>> GetAllSkillsAsync()
	{
		try
		{
			logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodStart, nameof(CreateNewSkillAsync), DateTime.UtcNow, string.Empty));
			return await mongoDbService.GetDataFromCollectionAsync<AgentSkillDomain>(MongoDbCollectionConstants.AiAgentsPrimaryDatabase, MongoDbCollectionConstants.SkillsCollectionName).ConfigureAwait(false);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(CreateNewSkillAsync), DateTime.UtcNow, ex.Message));
			throw;
		}
		finally
		{
			logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodEnd, nameof(CreateNewSkillAsync), DateTime.UtcNow, string.Empty));
		}
	}

	/// <summary>
	/// Gets the skill by identifier asynchronous.
	/// </summary>
	/// <param name="skillId">The skill identifier.</param>
	/// <returns>
	/// The agent skill DTO.
	/// </returns>
	public async Task<AgentSkillDomain> GetSkillByIdAsync(string skillId)
	{
		try
		{
			logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodStart, nameof(CreateNewSkillAsync), DateTime.UtcNow, skillId));
			return await mongoDbService.GetDataFromCollectionAsync<string, AgentSkillDomain>(skillId, MongoDbCollectionConstants.AiAgentsPrimaryDatabase, MongoDbCollectionConstants.SkillsCollectionName).ConfigureAwait(false);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(CreateNewSkillAsync), DateTime.UtcNow, ex.Message));
			throw;
		}
		finally
		{
			logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodEnd, nameof(CreateNewSkillAsync), DateTime.UtcNow, skillId));
		}
	}
}
