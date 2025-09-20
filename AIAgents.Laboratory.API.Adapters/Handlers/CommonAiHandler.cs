// *********************************************************************************
//	<copyright file="CommonAiHandler.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>Common AI handler.</summary>
// *********************************************************************************

using AIAgents.Laboratory.API.Adapters.Contracts;
using AIAgents.Laboratory.API.Adapters.Models.Response;
using AIAgents.Laboratory.Domain.DrivingPorts;
using AutoMapper;

namespace AIAgents.Laboratory.API.Adapters.Handlers;

/// <summary>
/// The Common AI Handler.
/// </summary>
/// <param name="commonAiService">The common ai service.</param>
/// <param name="mapper">The mapper.</param>
/// <seealso cref="AIAgents.Laboratory.API.Adapters.Contracts.ICommonAiHandler" />
public class CommonAiHandler(ICommonAiService commonAiService, IMapper mapper) : ICommonAiHandler
{
	/// <summary>
	/// Gets the agent current status.
	/// </summary>
	/// <returns>
	/// The agent status data.
	/// </returns>
	public AgentStatusDTO GetAgentCurrentStatus()
	{
		var domainStatusResponse = commonAiService.GetAgentCurrentStatus();
		return mapper.Map<AgentStatusDTO>(domainStatusResponse);
	}
}
