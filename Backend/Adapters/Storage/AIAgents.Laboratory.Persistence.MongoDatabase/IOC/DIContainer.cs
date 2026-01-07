using AIAgents.Laboratory.Domain.DrivenPorts;
using AIAgents.Laboratory.Persistence.MongoDatabase.DataManager;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using static AIAgents.Laboratory.Persistence.MongoDatabase.Helpers.Constants;

namespace AIAgents.Laboratory.Persistence.MongoDatabase.IOC;

/// <summary>
/// The Dependency Injection Container class.
/// </summary>
public static class DIContainer
{
	/// <summary>
	/// Adds the mongo database adapter dependencies.
	/// </summary>
	/// <param name="services">The services.</param>
	/// <param name="configuration">The configuration.</param>
	/// <returns>The service collection.</returns>
	public static IServiceCollection AddMongoDbAdapterDependencies(this IServiceCollection services, IConfiguration configuration) =>
		services.ConfigureMongoDbServer(configuration).AddScoped<IMongoDatabaseService, MongoDatabaseManager>();

	/// <summary>
	/// Configures the mongo database server.
	/// </summary>
	/// <param name="services">The services.</param>
	/// <param name="configuration">The configuration.</param>
	/// <returns>The service collection.</returns>
	private static IServiceCollection ConfigureMongoDbServer(this IServiceCollection services, IConfiguration configuration)
	{
		var mongoDbConnectionString = configuration[ConfigurationConstants.AiAgentsLabMongoConnectionString];
		ArgumentException.ThrowIfNullOrWhiteSpace(mongoDbConnectionString, nameof(mongoDbConnectionString));

		try
		{
			var settings = MongoClientSettings.FromConnectionString(mongoDbConnectionString);
			settings.UseTls = true;
			settings.SslSettings = new SslSettings()
			{
				EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12 | System.Security.Authentication.SslProtocols.Tls13,
				CheckCertificateRevocation = false
			};

			services.AddSingleton<IMongoClient>(new MongoClient(settings));
		}
		catch (Exception ex)
		{
			throw new InvalidOperationException($"Failed to configure MongoDB client. Connection string format may be incorrect. Error: {ex.Message}", ex);
		}

		return services;
	}
}
