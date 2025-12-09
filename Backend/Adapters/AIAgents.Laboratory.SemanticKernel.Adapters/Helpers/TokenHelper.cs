using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Azure.Core;
using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using static AIAgents.Laboratory.SemanticKernel.Adapters.Helpers.Constants;

namespace AIAgents.Laboratory.SemanticKernel.Adapters.Helpers;

/// <summary>
/// The Token Helper.
/// </summary>
[ExcludeFromCodeCoverage]
internal static class TokenHelper
{
    /// <summary>
    /// Gets ibbs ai token async.
    /// </summary>
    /// <param name="configuration">The configuration.</param>
    /// <param name="logger">The logger.</param>
    /// <exception cref="Exception">Exception error.</exception>
    public static async Task<string> GetAiAgentsLabTokenAsync(IConfiguration configuration, ILogger logger)
    {
        try
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(GetAiAgentsLabTokenAsync), DateTime.UtcNow);

            var tenantId = configuration[ConfigurationConstants.AiAgentsLabTenantId];
            var clientId = configuration[ConfigurationConstants.AiAgentsAdClientId];
            var clientSecret = configuration[ConfigurationConstants.AiAgentsAdClientSecret];
            var scopes = new[] { string.Format(CultureInfo.CurrentCulture, ConfigurationConstants.TokenScopeFormat, clientId) };

            var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);
            var accessToken = await credential.GetTokenAsync(new TokenRequestContext(scopes!), CancellationToken.None);

            ArgumentException.ThrowIfNullOrWhiteSpace(accessToken.Token);
            return accessToken.Token;
        }
        catch (Exception ex)
        {
            logger.LogInformation(string.Format(LoggingConstants.LogHelperMethodFailed, nameof(GetAiAgentsLabTokenAsync), DateTime.UtcNow, ex.Message));
            throw;
        }
        finally
        {
            logger.LogInformation(string.Format(LoggingConstants.LogHelperMethodEnd, nameof(GetAiAgentsLabTokenAsync), DateTime.UtcNow));
        }
    }
}
