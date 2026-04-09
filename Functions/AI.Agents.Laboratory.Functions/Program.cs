using AI.Agents.Laboratory.Functions.IOC;
using AI.Agents.Laboratory.Functions.Middleware;
using AI.Agents.Laboratory.Functions.Shared.Constants;
using AI.Agents.Laboratory.Functions.Shared.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using var startupLoggerFactory = LoggerFactory.Create(
    builder => builder.AddConsole().SetMinimumLevel(LogLevel.Information));
var startupLogger = startupLoggerFactory.CreateLogger<Program>();

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication(app =>
    {
        app.UseMiddleware<GlobalExceptionMiddleware>();
        app.UseMiddleware<CorrelationIdMiddleware>();
    })
    .ConfigureAppConfiguration((context, config) => context.ConfigureAzureAppConfiguration(config))
    .ConfigureServices((context, services) =>
    {
        var configuration = context.Configuration;
        var serviceBusConnection = configuration[AzureAppConfigurationConstants.AzureServiceBusConnectionString];
        ArgumentException.ThrowIfNullOrEmpty(serviceBusConnection);

        services.AddHttpContextAccessor();
        services.AddScoped<ICorrelationContext, CorrelationContext>();
        services.AddBusinessDependencies().AddDataDependencies(configuration).AddRepositories();
    })
    .ConfigureLogging(logging => logging.SetMinimumLevel(LogLevel.Information))
    .Build();

startupLogger.LogInformation("Host built successfully. Starting Azure Functions runtime.");
await host.RunAsync().ConfigureAwait(false);
