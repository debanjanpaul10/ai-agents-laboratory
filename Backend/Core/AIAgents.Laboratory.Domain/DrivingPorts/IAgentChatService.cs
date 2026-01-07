using AIAgents.Laboratory.Domain.DomainEntities.AgentsEntities;

namespace AIAgents.Laboratory.Domain.DrivingPorts;

/// <summary>
/// Defines methods for managing agent chat interaction services.
/// </summary>
public interface IAgentChatService
{
    /// <summary>
    /// Gets the agent chat response asynchronous.
    /// </summary>
    /// <param name="chatRequest">The chat request.</param>
    /// <returns>The AI response.</returns>
    Task<string> GetAgentChatResponseAsync(ChatRequestDomain chatRequest);
}
