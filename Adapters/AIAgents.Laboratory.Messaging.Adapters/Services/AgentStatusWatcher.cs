// *********************************************************************************
//	<copyright file="AgentStatusWatcher.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>The Agent Status Watcher Class.</summary>
// *********************************************************************************

using AIAgents.Laboratory.Domain.DrivenPorts;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Azure.SignalR.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Globalization;
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
public class AgentStatusWatcher(ILogger<AgentStatusWatcher> logger, IConfiguration configuration, IAgentStatusStore agentStatusStore, IHubContext<AgentStatusHub> agentHub) : BackgroundService
{
	/// <summary>
	/// This method is called when the <see cref="T:Microsoft.Extensions.Hosting.IHostedService" /> starts. The implementation should return a task that represents
	/// the lifetime of the long running operation(s) being performed.
	/// </summary>
	/// <param name="stoppingToken">Triggered when <see cref="M:Microsoft.Extensions.Hosting.IHostedService.StopAsync(System.Threading.CancellationToken)" /> is called.</param>
	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodStart, nameof(AgentStatusWatcher), DateTime.UtcNow, string.Empty));
		while(!stoppingToken.IsCancellationRequested)
		{
			try
			{
				var isAiServiceEnabled = bool.TryParse(configuration[AzureAppConfigurationConstants.IsAIServiceEnabledConstant], out bool parsedValue) && parsedValue;
				logger.LogInformation("Current AI service status from configuration: {Status}", isAiServiceEnabled);
				
				var previousStatus = agentStatusStore.Current;
				if (agentStatusStore.TryUpdate(isAiServiceEnabled, out var updated))
				{
					logger.LogInformation("Agent status changed from {PreviousStatus} to {CurrentStatus} at {Timestamp}", previousStatus.IsAvailable, updated.IsAvailable, updated.UpdatedAt);
					try
					{
						await agentHub.Clients.All.SendAsync(MessagingConstants.AgentStatusChanged, new
						{
							isAvailable = updated.IsAvailable,
							updatedAt = updated.UpdatedAt
						}, cancellationToken: stoppingToken);
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
				else
				{
					logger.LogDebug(LoggingConstants.NoStatusChangeDetected, updated.IsAvailable);
				}
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Error in AgentStatusWatcher: {Message}", ex.Message);
				await Task.Delay(5000, stoppingToken);
			}
			
			await Task.Delay(MessagingConstants.DelayBetweenIterationsMs, stoppingToken);
		}
	}
}
