// *********************************************************************************
//	<copyright file="AgentStatusWatcher.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>The Agent Status Watcher Class.</summary>
// *********************************************************************************

using AIAgents.Laboratory.Domain.DrivenPorts;
using Microsoft.AspNetCore.SignalR;
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
		while(!stoppingToken.IsCancellationRequested)
		{
			try
			{
				var isAiServiceEnabled = bool.TryParse(configuration[AzureAppConfigurationConstants.IsAIServiceEnabledConstant], out bool parsedValue) && parsedValue;
				if (agentStatusStore.TryUpdate(isAiServiceEnabled, out var updated))
				{
					await agentHub.Clients.All.SendAsync("agentStatusChanged", new
					{
						isAvailable = updated.IsAvailable,
						updatedAt = updated.UpdatedAt
					}, cancellationToken: stoppingToken);
				}
			}
			catch (Exception ex)
			{
				logger.LogError(ex, string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(AgentStatusWatcher), DateTime.UtcNow, string.Empty));
				throw;
			}
		}
		
	}
}
