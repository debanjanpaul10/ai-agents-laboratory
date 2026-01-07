using System.Diagnostics.CodeAnalysis;
using AIAgents.Laboratory.Domain.DrivenPorts;
using AIAgents.Laboratory.Storage.Blobs.DataManager;
using CloudinaryDotNet;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static AIAgents.Laboratory.Storage.Blobs.Helpers.Constants;

namespace AIAgents.Laboratory.Storage.Blobs.IOC;

/// <summary>
/// The Dependency Injection Container.
/// </summary>
[ExcludeFromCodeCoverage]
public static class DIContainer
{
    /// <summary>
    /// Adds the blob storage dependencies.
    /// </summary>
    /// <param name="services">The services collection.</param>
    /// <param name="configuration">The configuration service.</param>
    /// <returns>The updated services collection.</returns>
    public static IServiceCollection AddBlobStorageDependencies(this IServiceCollection services, IConfiguration configuration) =>
        services.AddGoogleCloudStorageDependencies(configuration);

    /// <summary>
    /// Adds the cloudinary storage dependencies.
    /// </summary>
    /// <param name="services">The services collection.</param>
    /// <param name="configuration">The configuration service.</param>
    /// <returns>The updated services collection.</returns>
    private static IServiceCollection AddCloudinaryDependencies(this IServiceCollection services, IConfiguration configuration) =>
        services.AddScoped<ICloudinary>(provider =>
        {
            var account = new Account(
                configuration[AzureAppConfigurationConstants.CloudinaryCloudNameConstant],
                configuration[AzureAppConfigurationConstants.CloudinaryApiKeyConstant],
                configuration[AzureAppConfigurationConstants.CloudinaryApiSecretConstant]);
            return new Cloudinary(account);
        }).AddScoped<IBlobStorageManager, CloudinaryStorageManager>();

    /// <summary>
    /// Adds the Google Cloud Storage dependencies.
    /// </summary>
    /// <param name="services">The services collection.</param>
    /// <param name="configuration">The configuration service.</param>
    /// <returns>The updated services collection.</returns>
    private static IServiceCollection AddGoogleCloudStorageDependencies(this IServiceCollection services, IConfiguration configuration) =>
        services.AddSingleton(provider =>
        {
            var serviceAccountJson = configuration[AzureAppConfigurationConstants.GCPServiceAccountJsonConstant];
            ArgumentNullException.ThrowIfNull(serviceAccountJson);

            // Use service account JSON content from configuration
            var credential = GoogleCredential.FromJson(serviceAccountJson);
            var storageClient = StorageClient.Create(credential);
            return storageClient;
        }).AddScoped<IBlobStorageManager, GCPCloudStorageManager>();
}
