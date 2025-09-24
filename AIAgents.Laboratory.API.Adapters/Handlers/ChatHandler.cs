using AIAgents.Laboratory.API.Adapters.Contracts;
using AIAgents.Laboratory.API.Adapters.Models.Request;
using AIAgents.Laboratory.Domain.DomainEntities.AgentsEntities;
using AIAgents.Laboratory.Domain.DrivingPorts;
using AutoMapper;

namespace AIAgents.Laboratory.API.Adapters.Handlers;

/// <summary>
/// The Chat API adapter handler class.
/// </summary>
/// <param name="chatService">The chat service.</param>
/// <param name="mapper">The auto mapper service.</param>
/// <seealso cref="AIAgents.Laboratory.API.Adapters.Contracts.IChatHandler" />
public class ChatHandler(IMapper mapper, IChatService chatService) : IChatHandler
{
	/// <summary>
	/// Gets the chatbot response.
	/// </summary>
	/// <param name="userQuery">The user query.</param>
	/// <returns>The AI response.</returns>
	public async Task<string> GetDirectChatResponseAsync(string userQuery)
	{
		return await chatService.GetDirectChatResponseAsync(userQuery).ConfigureAwait(false);
	}

	/// <summary>
	/// Invokes the chat agent asynchronous.
	/// </summary>
	/// <param name="chatRequestDTO">The chat request dto.</param>
	/// <returns>
	/// The chatbot response.
	/// </returns>
	public async Task<string> InvokeChatAgentAsync(ChatRequestDTO chatRequestDTO)
	{
		var domainInput = mapper.Map<ChatRequestDomain>(chatRequestDTO);
		return await chatService.GetAgentChatResponseAsync(domainInput).ConfigureAwait(false);
	}
}
