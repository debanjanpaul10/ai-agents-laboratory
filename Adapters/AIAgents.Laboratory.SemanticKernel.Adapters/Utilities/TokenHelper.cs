// *********************************************************************************
//	<copyright file="TokenHelper.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>Token helper class.</summary>
// *********************************************************************************

using Azure.Core;
using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using static AIAgents.Laboratory.SemanticKernel.Adapters.Helpers.Constants;

namespace AIAgents.Laboratory.SemanticKernel.Adapters.Utilities;

/// <summary>
/// The Token Helper Class.
/// </summary>
[ExcludeFromCodeCoverage]
internal static class TokenHelper
{
	/// <summary>
	/// Gets the fit gym tool token asynchronous.
	/// </summary>
	/// <param name="configuration">The configuration.</param>
	/// <param name="logger">The logger.</param>
	/// <returns>The token value.</returns>
	/// <exception cref="System.Exception"></exception>
	public static async Task<string> GetFitGymToolTokenAsync(IConfiguration configuration, ILogger logger)
	{
		try
		{
			logger.LogInformation(string.Format(LoggingConstants.LogHelperMethodStart, nameof(GetFitGymToolTokenAsync), DateTime.UtcNow));

			var tenantId = configuration[ConfigurationConstants.FGToolTenantIdConstant];
			var clientId = configuration[ConfigurationConstants.FGToolClientIdConstant];
			var clientSecret = configuration[ConfigurationConstants.FGToolClientSecretConstant];
			var scopes = new[] { string.Format(CultureInfo.CurrentCulture, ConfigurationConstants.TokenScopeFormat, clientId) };

			var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);
			var accessToken = await credential.GetTokenAsync(new TokenRequestContext(scopes), CancellationToken.None).ConfigureAwait(false);

			if (string.IsNullOrEmpty(accessToken.Token))
			{
				throw new Exception(ExceptionConstants.SomethingWentWrongMessage);
			}

			return accessToken.Token;
		}
		catch (Exception ex)
		{
			logger.LogInformation(string.Format(LoggingConstants.LogHelperMethodFailed, nameof(GetFitGymToolTokenAsync), DateTime.UtcNow, ex.Message));
			throw;
		}
		finally
		{
			logger.LogInformation(string.Format(LoggingConstants.LogHelperMethodEnd, nameof(GetFitGymToolTokenAsync), DateTime.UtcNow));
		}
	}

}