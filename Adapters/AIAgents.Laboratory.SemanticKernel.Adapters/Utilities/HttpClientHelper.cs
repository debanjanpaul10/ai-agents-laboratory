// *********************************************************************************
//	<copyright file="HttpClientHelper.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>The Http Client Helper Services Class.</summary>
// *********************************************************************************

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using static AIAgents.Laboratory.SemanticKernel.Adapters.Helpers.Constants;

namespace AIAgents.Laboratory.SemanticKernel.Adapters.Utilities;

/// <summary>
/// Http client helper interface.
/// </summary>
public interface IHttpClientHelper
{
	/// <summary>
	/// Gets the api response asynchronous.
	/// </summary>
	/// <param name="apiUrl">The API URL.</param>
	/// <returns>The http response message.</returns>
	Task<HttpResponseMessage> GetFitGymToolApiResponseAsync(string apiUrl);
}

/// <summary>
/// The Http Client Helper.
/// </summary>
/// <param name="configuration">The configuration.</param>
/// <param name="httpClientFactory">The http client factory.</param>
/// <param name="logger">The logger.</param>
/// <seealso cref="IHttpClientHelper" />
public class HttpClientHelper(ILogger<HttpClientHelper> logger, IConfiguration configuration, IHttpClientFactory httpClientFactory) : IHttpClientHelper
{
	/// <summary>
	/// Gets the FG Tool api response asynchronous.
	/// </summary>
	/// <param name="apiUrl">The API URL.</param>
	/// <returns>
	/// The http response message.
	/// </returns>
	public async Task<HttpResponseMessage> GetFitGymToolApiResponseAsync(string apiUrl)
	{
		try
		{
			logger.LogInformation(string.Format(LoggingConstants.LogHelperMethodStart, nameof(GetFitGymToolApiResponseAsync), DateTime.UtcNow, apiUrl));
			
			var client = httpClientFactory.CreateClient(ConfigurationConstants.FGToolHttpClient);
			ArgumentException.ThrowIfNullOrWhiteSpace(apiUrl);
			await PrepareHttpClientFactoryAsync(client, TokenHelper.GetFitGymToolTokenAsync(configuration, logger)).ConfigureAwait(false);

			var response = await client.GetAsync(apiUrl).ConfigureAwait(false);
			if (!response.IsSuccessStatusCode)
			{
				return response.EnsureSuccessStatusCode();
			}

			return response;
		}
		catch (Exception ex)
		{
			logger.LogInformation(string.Format(LoggingConstants.LogHelperMethodFailed, nameof(GetFitGymToolApiResponseAsync), DateTime.UtcNow, ex.Message));
			throw;
		}
		finally
		{
			logger.LogInformation(string.Format(LoggingConstants.LogHelperMethodEnd, nameof(GetFitGymToolApiResponseAsync), DateTime.UtcNow, apiUrl));
		}
	}

	#region PRIVATE METHODS

	/// <summary>
	/// Prepares http client factory async.
	/// </summary>
	/// <param name="client">The client.</param>
	/// <param name="tokenTask">The task to get the token.</param>
	private static async Task PrepareHttpClientFactoryAsync(HttpClient client, Task<string> tokenTask)
	{
		var token = await tokenTask.ConfigureAwait(false);
		client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(ConfigurationConstants.BearerConstant, token);
	}

	#endregion
}
