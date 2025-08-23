// *********************************************************************************
//	<copyright file="AgentHelpers.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>The Agent Helpers Utility class.</summary>
// *********************************************************************************

using AIAgents.Laboratory.Domain.DomainEntities.FitGymTool;

namespace AIAgents.Laboratory.SemanticKernel.Adapters.Utilities;

/// <summary>
/// The Agent Helpers Utility class.
/// </summary>
public static class AgentHelpers
{
	/// <summary>
	/// Prepares the agent chatbot reponse.
	/// </summary>
	/// <param name="aiAgentResponse">The ai agent response.</param>
	/// <param name="userIntent">The user intent.</param>
	/// <param name="input">The input.</param>
	/// <param name="aiResponse">The ai response.</param>
	/// <returns></returns>
	public static AIAgentResponseDomain PrepareAgentChatbotReponse(this AIAgentResponseDomain aiAgentResponse, string userIntent, string input, string aiResponse)
	{
		aiAgentResponse.UserIntent = userIntent.Trim();
		aiAgentResponse.UserQuery = input.Trim();
		aiAgentResponse.AIResponseData = aiResponse.Trim();

		return aiAgentResponse;
	}
}
