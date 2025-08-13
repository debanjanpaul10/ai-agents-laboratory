// *********************************************************************************
//	<copyright file="CommonAiService.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>Common AI Service.</summary>
// *********************************************************************************

using AIAgents.Laboratory.Core.Contracts;
using AIAgents.Laboratory.Shared.Constants;
using Microsoft.Extensions.Configuration;
using static AIAgents.Laboratory.Shared.Constants.ConfigurationConstants;

namespace AIAgents.Laboratory.Core.Services;

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