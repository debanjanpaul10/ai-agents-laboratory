﻿using AIAgents.Laboratory.API.Adapters.Models.Request;

namespace AIAgents.Laboratory.API.Adapters.Contracts;

/// <summary>
/// The Chat API adapter handler interface.
/// </summary>
public interface IChatHandler
{
	/// <summary>
	/// Invokes the chat agent asynchronous.
	/// </summary>
	/// <param name="chatRequestDTO">The chat request dto.</param>
	/// <returns>The chatbot response.</returns>
	Task<string> InvokeChatAgentAsync(ChatRequestDTO chatRequestDTO);
}
