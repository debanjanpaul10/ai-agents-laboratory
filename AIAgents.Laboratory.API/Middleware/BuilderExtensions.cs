// *********************************************************************************
//	<copyright file="BuilderExtensions.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>Builder extensions.</summary>
// *********************************************************************************

using AIAgents.Laboratory.API.Controllers;
using AIAgents.Laboratory.API.IOC;
using AIAgents.Laboratory.Shared.Constants;
using Azure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using System.Globalization;
using System.Security.Claims;
using static AIAgents.Laboratory.Shared.Constants.ConfigurationConstants;

namespace AIAgents.Laboratory.API.Middleware;

/// <summary>
/// The Builder Extensions Class.
/// </summary>
public static class BuilderExtensions
{
	/// <summary>
	/// Adds azure services.
	/// </summary>
	/// <param name="builder">The builder.</param>
	/// <param name="credentials">The credentials.</param>
	/// <exception cref="InvalidOperationException">InvalidOperationException error.</exception>
	public static void ConfigureAzureAppConfiguration(this WebApplicationBuilder builder, DefaultAzureCredential credentials)
	{
		var configuration = builder.Configuration;
		var appConfigurationEndpoint = configuration[EnvironmentConfigurationConstants.AppConfigurationEndpointKeyConstant];
		if (string.IsNullOrEmpty(appConfigurationEndpoint))
		{
			throw new InvalidOperationException(ExceptionConstants.MissingConfigurationMessage);
		}

		builder.Configuration.AddAzureAppConfiguration(options =>
		{
			options.Connect(new Uri(appConfigurationEndpoint), credentials)
			.Select(KeyFilter.Any).Select(KeyFilter.Any, AzureAppConfigurationConstants.BaseConfigurationAppConfigKeyConstant)
			.ConfigureKeyVault(configure =>
			{
				configure.SetCredential(credentials);
			});
		});
	}

	/// <summary>
	/// Configures api services.
	/// </summary>
	/// <param name="builder">The builder.</param>
	public static void ConfigureAiBusinessServices(this WebApplicationBuilder builder)
	{
		builder.ConfigureAuthenticationServices();
		builder.ConfigureBusinessDependencies();
	}

	#region PRIVATE METHODS

	/// <summary>
	/// Configures authentication services.
	/// </summary>
	/// <param name="builder">The builder.</param>
	private static void ConfigureAuthenticationServices(this WebApplicationBuilder builder)
	{
		var configuration = builder.Configuration;
		builder.Services.AddAuthentication(options =>
		{
			options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
			options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
		}).AddJwtBearer(options =>
		{
			options.Authority = string.Format(
				CultureInfo.CurrentCulture, AzureAppConfigurationConstants.TokenFormatUrl, configuration[AzureAppConfigurationConstants.AzureAdTenantIdConstant]);
			options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
			{
				ValidAudience = configuration[AzureAppConfigurationConstants.AIAgentsClientIdConstant],
				ValidateLifetime = true,
			};
			options.Events = new JwtBearerEvents
			{
				OnTokenValidated = HandleAuthTokenValidationSuccessAsync,
				OnAuthenticationFailed = HandleAuthTokenValidationFailedAsync
			};
		});

	}

	/// <summary>
	/// Configures business dependencies.
	/// </summary>
	/// <param name="builder">The builder.</param>
	private static void ConfigureBusinessDependencies(this WebApplicationBuilder builder)
	{
		builder.Services.AddSingleton(KernelFactory.CreateKernel(builder.Configuration));
		builder.Services.AddSingleton(KernelFactory.CreateMemory());
	}

	/// <summary>
	/// Handles auth token validation success async.
	/// </summary>
	/// <param name="context">The token validation context.</param>
	private static async Task HandleAuthTokenValidationSuccessAsync(this TokenValidatedContext context)
	{
		var claimsPrincipal = context.Principal;
		if (claimsPrincipal?.Identity is not ClaimsIdentity claimsIdentity || !claimsIdentity.IsAuthenticated)
		{
			context.Fail(ExceptionConstants.InvalidTokenExceptionConstant);
			return;
		}

		context.HttpContext.User = new ClaimsPrincipal(claimsIdentity);
		await Task.CompletedTask;
	}

	/// <summary>
	/// Handles auth token validation failed async.
	/// </summary>
	/// <param name="context">The auth failed context.</param>
	private static async Task HandleAuthTokenValidationFailedAsync(this AuthenticationFailedContext context)
	{
		var authenticationFailedException = new UnauthorizedAccessException(ExceptionConstants.InvalidTokenExceptionConstant);
		var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<BaseController>>();
		logger.LogError(authenticationFailedException, authenticationFailedException.Message);
		await Task.CompletedTask;
	}

	#endregion
}
