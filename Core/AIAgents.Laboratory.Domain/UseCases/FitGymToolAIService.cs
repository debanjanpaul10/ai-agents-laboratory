// *********************************************************************************
//	<copyright file="FitGymToolAIService.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>The FitGym Tool AI Service class.</summary>
// *********************************************************************************

using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.Domain.DomainEntities.FitGymTool;
using AIAgents.Laboratory.Domain.DrivenPorts;
using AIAgents.Laboratory.Domain.DrivingPorts;
using Microsoft.Extensions.Logging;
using System.Globalization;
using static AIAgents.Laboratory.Domain.Helpers.Constants;
using static AIAgents.Laboratory.Domain.Helpers.PluginHelpers;

namespace AIAgents.Laboratory.Domain.UseCases;

/// <summary>
/// The FitGym Tool AI Service class.
/// </summary>
/// <param name="aiAgentServices">The ai agent services.</param>
/// <param name="commonAiService">The common ai service.</param>
/// <param name="logger">The logger service.</param>
/// <seealso cref="AIAgents.Laboratory.Domain.DrivingPorts.IFitGymToolAIService" />
public class FitGymToolAIService(ILogger<FitGymToolAIService> logger, IAIAgentServices aiAgentServices, ICommonAiService commonAiService) : IFitGymToolAIService
{
    /// <summary>
    /// Gets the bug severity asynchronous.
    /// </summary>
    /// <param name="bugSeverityInput">The bug severity input.</param>
    /// <returns>
    /// The bug severity response.
    /// </returns>
    public async Task<BugSeverityResponse> GetBugSeverityAsync(BugSeverityInput bugSeverityInput)
    {
        try
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodStart, nameof(GetBugSeverityAsync), DateTime.UtcNow, string.Empty));
            if (bugSeverityInput is null)
            {
                var exception = new Exception(ExceptionConstants.InputParametersCannotBeEmptyMessage);
                logger.LogError(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(GetBugSeverityAsync), DateTime.UtcNow, exception.Message));
                throw exception;
            }

            var response = await aiAgentServices.GetAiFunctionResponseAsync<BugSeverityInput, BugSeverityResponse>(bugSeverityInput, UtilityPlugins.PluginName, UtilityPlugins.DetermineBugSeverityFunction.FunctionName).ConfigureAwait(false);
            if (response is not null)
            {
                response.ModelUsed = commonAiService.GetCurrentModelId();
            }

            return response ?? new BugSeverityResponse { ModelUsed = commonAiService.GetCurrentModelId() };
        }
        catch (Exception ex)
        {
            logger.LogError(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(GetBugSeverityAsync), DateTime.UtcNow, ex.Message));
            throw;
        }
        finally
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodEnd, nameof(GetBugSeverityAsync), DateTime.UtcNow, string.Empty));
        }
    }

    /// <summary>
    /// Gets the orchestrator response asynchronous.
    /// </summary>
    /// <param name="userQueryRequest">The user query request.</param>
    /// <returns>
    /// The AI response.
    /// </returns>
    public async Task<string> GetOrchestratorResponseAsync(UserRequestDomain userQueryRequest)
    {
        try
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodStart, nameof(GetOrchestratorResponseAsync), DateTime.UtcNow, string.Empty));
            if (userQueryRequest is null || string.IsNullOrEmpty(userQueryRequest.UserQuery))
            {
                var exception = new Exception(ExceptionConstants.InputParametersCannotBeEmptyMessage);
                logger.LogError(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(GetOrchestratorResponseAsync), DateTime.UtcNow, exception.Message));
                throw exception;
            }

            return await aiAgentServices.GetOrchestratorFunctionResponseAsync(userQueryRequest.UserQuery).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogError(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(GetOrchestratorResponseAsync), DateTime.UtcNow, ex.Message));
            throw;
        }
        finally
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodEnd, nameof(GetOrchestratorResponseAsync), DateTime.UtcNow, string.Empty));
        }
    }
}
