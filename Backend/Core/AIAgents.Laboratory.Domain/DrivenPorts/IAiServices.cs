using AIAgents.Laboratory.Domain.DomainEntities;

namespace AIAgents.Laboratory.Domain.DrivenPorts;

/// <summary>
/// The AI Services interface.
/// </summary>
public interface IAiServices
{
    /// <summary>
    /// Gets the ai function response asynchronous.
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    /// <param name="input">The input.</param>
    /// <param name="pluginName">Name of the plugin.</param>
    /// <param name="functionName">Name of the function.</param>
    /// <returns>The AI response.</returns>
    Task<string> GetAiFunctionResponseAsync<TInput>(TInput input, string pluginName, string functionName);

    /// <summary>
    /// Gets the chatbot response.
    /// </summary>
    /// <param name="userMessage">The user message data.</param>
    /// <param name="conversationDataDomain">The conversation history data domain.</param>
    /// <param name="agentPrompt">The agent prompt message.</param>
    /// <returns>The AI chatbot response.</returns>
    Task<string> GetChatbotResponseAsync(ConversationHistoryDomain conversationDataDomain, string userMessage, string agentPrompt);

    /// <summary>
    /// Gets the ai function response with MCP integration asynchronous.
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    /// <param name="input">The input.</param>
    /// <param name="mcpServerUrl">The MCP server URL.</param>
    /// <param name="pluginName">Name of the plugin.</param>
    /// <param name="functionName">Name of the function.</param>
    /// <returns>The AI response.</returns>
    Task<string> GetAiFunctionResponseAsync<TInput>(TInput input, string mcpServerUrl, string pluginName, string functionName);
}