using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Azure.Core;
using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using static AIAgents.Laboratory.Infrastructure.AgentsFramework.Helpers.Constants;

namespace AIAgents.Laboratory.Infrastructure.AgentsFramework.Helpers;

/// <summary>
/// The Token Helper.
/// </summary>
[ExcludeFromCodeCoverage]
internal static class TokenHelper
{
    /// <summary>
    /// Gets ai agents lab token async.
    /// </summary>
    /// <param name="configuration">The configuration.</param>
    /// <param name="logger">The logger.</param>
    /// <returns>The access token.</returns>
    /// <exception cref="Exception">Exception error.</exception>
    internal static async Task<string> GetAiAgentsLabTokenAsync(IConfiguration configuration, ILogger logger)
    {
        try
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(GetAiAgentsLabTokenAsync), DateTime.UtcNow, string.Empty);

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
            logger.LogError(ex, LoggingConstants.LogHelperMethodFailed, nameof(GetAiAgentsLabTokenAsync), DateTime.UtcNow, ex.Message);
            return string.Empty;
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetAiAgentsLabTokenAsync), DateTime.UtcNow, string.Empty);
        }
    }
}