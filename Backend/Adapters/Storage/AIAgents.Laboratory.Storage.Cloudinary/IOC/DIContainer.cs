using System.Diagnostics.CodeAnalysis;
using AIAgents.Laboratory.Domain.DrivenPorts;
using AIAgents.Laboratory.Storage.Cloudinary.DataManager;
using CloudinaryDotNet;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static AIAgents.Laboratory.Storage.Cloudinary.Helpers.Constants;

namespace AIAgents.Laboratory.Storage.Cloudinary.IOC;

/// <summary>
/// The Dependency Injection Container.
/// </summary>
[ExcludeFromCodeCoverage]
public static class DIContainer
{
    /// <summary>
    /// Adds the cloudinary storage dependencies.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns>The service collection.</returns>
    public static IServiceCollection AddCloudinaryDependencies(this IServiceCollection services, IConfiguration configuration) =>
         services.AddScoped<ICloudinary>(provider =>
         {
             var account = new Account(
                 configuration[AzureAppConfigurationConstants.CloudinaryCloudNameConstant],
                 configuration[AzureAppConfigurationConstants.CloudinaryApiKeyConstant],
                 configuration[AzureAppConfigurationConstants.CloudinaryApiSecretConstant]);
             return new CloudinaryDotNet.Cloudinary(account);
         }).AddScoped<ICloudinaryStorageManager, CloudinaryStorageManager>();

}
