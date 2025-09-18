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
