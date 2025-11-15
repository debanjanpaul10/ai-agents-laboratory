using AIAgents.Laboratory.Domain.DomainEntities;

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
    /// <returns>The populated agent response domain.</returns>
    public static AIAgentResponseDomain PrepareAgentChatbotReponse(this AIAgentResponseDomain aiAgentResponse, string userIntent, string input, string aiResponse)
    {
        aiAgentResponse.UserIntent = userIntent.Trim();
        aiAgentResponse.UserQuery = input.Trim();
        aiAgentResponse.AIResponseData = aiResponse.Trim();

        return aiAgentResponse;
    }
}
