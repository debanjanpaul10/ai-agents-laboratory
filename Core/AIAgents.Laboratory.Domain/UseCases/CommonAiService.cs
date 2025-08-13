// *********************************************************************************
//	<copyright file="CommonAiService.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>Common AI Service.</summary>
// *********************************************************************************

using AIAgents.Laboratory.Domain.DrivingPorts;
using Microsoft.Extensions.Configuration;
using static AIAgents.Laboratory.Domain.Helpers.Constants;

namespace AIAgents.Laboratory.Domain.UseCases;

/// <summary>
/// Common AI Service.
/// </summary>
/// <param name="configuration">The configuration.</param>
/// <seealso cref="ICommonAiService"/>
public class CommonAiService(IConfiguration configuration) : ICommonAiService
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
}