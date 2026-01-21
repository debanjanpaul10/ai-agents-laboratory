using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.Domain.DrivenPorts;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Azure.SignalR.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using static AIAgents.Laboratory.Messaging.Adapters.Helpers.Constants;

namespace AIAgents.Laboratory.Messaging.Adapters.Services;

/// <summary>
/// The Agent Status Watcher Class.
/// </summary>
/// <param name="agentHub">The agent hub service.</param>
/// <param name="agentStatusStore">The agent status store.</param>
/// <param name="configuration">The configuration service.</param>
/// <param name="logger">The logger service.</param>
/// <seealso cref="Microsoft.Extensions.Hosting.BackgroundService" />
public sealed class AgentStatusWatcher(ILogger<AgentStatusWatcher> logger, IConfiguration configuration, IAgentStatusStore agentStatusStore, IHubContext<AgentStatusHub> agentHub) : BackgroundService
{
    /// <summary>
    /// This method is called when the <see cref="T:Microsoft.Extensions.Hosting.IHostedService" /> starts. The implementation should return a task that represents
    /// the lifetime of the long running operation(s) being performed.
    /// </summary>
    /// <param name="stoppingToken">Triggered when <see cref="M:Microsoft.Extensions.Hosting.IHostedService.StopAsync(System.Threading.CancellationToken)" /> is called.</param>
    /// <remarks>
    /// See <see href="https://learn.microsoft.com/dotnet/core/extensions/workers">Worker Services in .NET</see> for implementation guidelines.
    /// </remarks>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(AgentStatusWatcher), DateTime.UtcNow, string.Empty);
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Check AI service status
                var isAiServiceEnabled = bool.TryParse(configuration[AzureAppConfigurationConstants.IsAIServiceEnabledConstant], out bool parsedValue) && parsedValue;
                logger.LogInformation("Current AI service status from configuration: {Status}", isAiServiceEnabled);

                // Get current status and attempt update
                var previousStatus = agentStatusStore.Current;
                if (agentStatusStore.TryUpdate(isAiServiceEnabled, out var updated))
                {
                    logger.LogInformation("Agent status changed from {PreviousStatus} to {CurrentStatus} at {Timestamp}", previousStatus.IsAvailable, updated.IsAvailable, updated.UpdatedAt);

                    // Only attempt to broadcast if there was an actual status change
                    await BroadcastStatusChangeAsync(updated, stoppingToken);
                }
                else
                {
                    logger.LogDebug(LoggingConstants.NoStatusChangeDetected, updated.IsAvailable);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in AgentStatusWatcher: {Message}", ex.Message);
            }
            finally
            {
                await Task.Delay(MessagingConstants.DelayBetweenIterationsMs, stoppingToken);
            }
        }
    }

    #region PRIVATE METHODS

    /// <summary>
    /// Broadcasts status change to all connected clients using SignalR
    /// </summary>
    /// <param name="status">The updated agent status</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A task representing the asynchronous operation</returns>
    private async Task BroadcastStatusChangeAsync(AgentStatus status, CancellationToken cancellationToken)
    {
        try
        {
            await _retryPolicy.ExecuteAsync(async () =>
            {
                await agentHub.Clients.All.SendAsync(MessagingConstants.AgentStatusChanged, new
                {
                    isAvailable = status.IsAvailable,
                    updatedAt = status.UpdatedAt
                }, cancellationToken: cancellationToken);

                logger.LogDebug("Successfully broadcast status change to all clients");
            });
        }
        catch (AzureSignalRNotConnectedException ex)
        {
            logger.LogWarning(LoggingConstants.UnableToRelayMessage, ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, LoggingConstants.ErrorBroadcastingStatusChange, ex.Message);
        }
    }

    /// <summary>
    /// The retry policy
    /// </summary>
    private static readonly AsyncRetryPolicy _retryPolicy = Policy
        .Handle<AzureSignalRNotConnectedException>().WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

    #endregion
}
