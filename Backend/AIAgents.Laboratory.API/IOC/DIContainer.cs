using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Security.Claims;
using AIAgents.Laboratory.API.Adapters.IOC;
using AIAgents.Laboratory.API.Controllers;
using AIAgents.Laboratory.Domain.IOC;
using AIAgents.Laboratory.Infrastructure.AgentsFramework.IOC;
using AIAgents.Laboratory.Messaging.Adapters.IOC;
using AIAgents.Laboratory.Persistence.Caching.IOC;
using AIAgents.Laboratory.Persistence.MongoDatabase.IOC;
using AIAgents.Laboratory.Persistence.SQLDatabase.IOC;
using AIAgents.Laboratory.Processor.IOC;
using AIAgents.Laboratory.Storage.Blobs.IOC;
using Azure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using static AIAgents.Laboratory.API.Helpers.Constants;

namespace AIAgents.Laboratory.API.IOC;

/// <summary>
/// The Dependency Injection Container Class.
/// </summary>
[ExcludeFromCodeCoverage]
public static class DIContainer
{
    /// <summary>
    /// Adds azure services.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="credentials">The credentials.</param>
    /// <exception cref="InvalidOperationException">InvalidOperationException error.</exception>
    internal static void ConfigureAzureAppConfiguration(this WebApplicationBuilder builder, DefaultAzureCredential credentials)
    {
        var configuration = builder.Configuration;
        var appConfigurationEndpoint = configuration[EnvironmentConfigurationConstants.AppConfigurationEndpointKeyConstant];
        if (string.IsNullOrEmpty(appConfigurationEndpoint))
            throw new InvalidOperationException(ExceptionConstants.MissingConfigurationMessage);

        builder.Configuration.AddAzureAppConfiguration(options =>
        {
            options.Connect(new Uri(appConfigurationEndpoint), credentials)
                .Select(KeyFilter.Any)
                .Select(KeyFilter.Any, AzureAppConfigurationConstants.BaseConfigurationAppConfigKeyConstant)
                .Select(KeyFilter.Any, AzureAppConfigurationConstants.FeatureFlagAppConfigKeyConstant)
                .Select(KeyFilter.Any, AzureAppConfigurationConstants.AiConfigurationAppConfigKeyConstant)
                .Select(KeyFilter.Any, AzureAppConfigurationConstants.MongoDbAppConfigKeyConstant)
            .ConfigureKeyVault(configure => configure.SetCredential(credentials));
        });
    }

    /// <summary>
    /// Configures the application dependencies.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    /// <param name="isDevelopmentMode">The development mode.</param>
    internal static void ConfigureApplicationDependencies(this IServiceCollection services, IConfiguration configuration, bool isDevelopmentMode)
    {
        services.ConfigureAuthenticationServices(configuration);

        services.AddAPIAdapterDependencies().AddMessagingDependencies(configuration).AddMemoryCache().AddCacheDependencies();
        services.AddAgentsFrameworkDependencies(configuration).AddMongoDbAdapterDependencies(configuration)
            .AddRelationalSqlDependencies(configuration, isDevelopmentMode).AddBlobStorageDependencies(configuration);

        services.AddDomainDependencies().AddProcessorDependencies(configuration);
        services.AddSignalR().AddAzureSignalR(configuration[AzureAppConfigurationConstants.AzureSignalRConnection]);
    }

    /// <summary>
    /// Adds API versioning to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    internal static void AddApiVersions(this IServiceCollection services)
    {
        services.AddApiVersioning(configuration =>
        {
            configuration.AssumeDefaultVersionWhenUnspecified = true;
            configuration.DefaultApiVersion = new ApiVersion(2, 0);
            configuration.ReportApiVersions = true;
        });
    }

    #region PRIVATE METHODS

    /// <summary>
    /// Configures the authentication services.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    private static void ConfigureAuthenticationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidateLifetime = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                RequireExpirationTime = true,
                RequireSignedTokens = true,
                ValidAudience = configuration[AzureAppConfigurationConstants.AIAgentsClientIdConstant],
                ValidIssuer = string.Format(CultureInfo.CurrentCulture, AzureAppConfigurationConstants.TokenFormatUrl, configuration[AzureAppConfigurationConstants.AzureAdTenantIdConstant]),
                SignatureValidator = (token, _) => new Microsoft.IdentityModel.JsonWebTokens.JsonWebToken(token)
            };
            options.Events = new JwtBearerEvents
            {
                OnTokenValidated = HandleAuthTokenValidationSuccessAsync,
                OnAuthenticationFailed = HandleAuthTokenValidationFailedAsync
            };
        });

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
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<BaseController>>();
            logger.LogError(ExceptionConstants.InvalidTokenExceptionConstant);
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
        await context.HttpContext.Response.WriteAsync(authenticationFailedException.Message);
    }

    #endregion
}
