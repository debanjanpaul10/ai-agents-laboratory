// *********************************************************************************
//	<copyright file="CommonAiService.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>Common AI Service.</summary>
// *********************************************************************************

using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.Domain.DrivenPorts;
using AIAgents.Laboratory.Domain.DrivingPorts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Globalization;
using static AIAgents.Laboratory.Domain.Helpers.Constants;

namespace AIAgents.Laboratory.Domain.UseCases;

/// <summary>
/// Common AI Service.
/// </summary>
/// <param name="configuration">The configuration.</param>
/// <param name="agentStatusStore">THe agent status store.</param>
/// <param name="logger">The logger service.</param>
/// <seealso cref="ICommonAiService"/>
public class CommonAiService(IConfiguration configuration, ILogger<CommonAiService> logger, IAgentStatusStore agentStatusStore) : ICommonAiService
{
	/// <summary>
	/// Gets the current model identifier.
	/// </summary>
	/// <returns>The current model identifier.</returns>
	public string GetCurrentModelId()
	{
		var isProModelEnabled = bool.TryParse(configuration[AzureAppConfigurationConstants.IsProModelEnabledFlag], out bool parsedValue) && parsedValue;
		var geminiAiModel = isProModelEnabled ? AzureAppConfigurationConstants.GeminiProModel : AzureAppConfigurationConstants.GeminiFlashModel;
		return configuration[geminiAiModel] ?? ExceptionConstants.ModelNameNotFoundExceptionConstant;
	}

	/// <summary>
	/// Gets the agent current status.
	/// </summary>
	/// <returns>
	/// The agent status data.
	/// </returns>
	public AgentStatus GetAgentCurrentStatus()
	{
		try
		{
			logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodStart, nameof(GetAgentCurrentStatus), DateTime.UtcNow, string.Empty));
			return agentStatusStore.Current;
		}
		catch (Exception ex)
		{
			logger.LogError(ex, string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(GetAgentCurrentStatus), DateTime.UtcNow, ex.Message));
			throw;
		}
		finally
		{
			logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodEnd, nameof(GetAgentCurrentStatus), DateTime.UtcNow, string.Empty));
		}
	}
}