using AIAgents.Laboratory.API.Adapters.Contracts;
using AIAgents.Laboratory.API.Adapters.Models.Request;
using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.Domain.DrivingPorts;
using AutoMapper;

namespace AIAgents.Laboratory.API.Adapters.Handlers;

/// <summary>
/// The Plugins Handler.
/// </summary>
/// <param name="mapper">The auto mapper service.</param>
/// <param name="pluginsAiServices">The plugins ai services.</param>
/// <seealso cref="IPluginsHandler"/>
public class PluginsHandler(IMapper mapper, IPluginsAiService pluginsAiServices) : IPluginsHandler
{
	/// <summary>
	/// Generates the tag for story asynchronous.
	/// </summary>
	/// <param name="story">The story.</param>
	/// <returns>
	/// The genre tag response dto.
	/// </returns>
	public async Task<string> GenerateTagForStoryAsync(string story)
	{
		return await pluginsAiServices.GenerateTagForStoryAsync(story).ConfigureAwait(false);
	}

	/// <summary>
	/// Gets the bug severity asynchronous.
	/// </summary>
	/// <param name="bugSeverityInput">The bug severity input.</param>
	/// <returns>
	/// The bug severity response.
	/// </returns>
	public async Task<string> GetBugSeverityAsync(BugSeverityInputDTO bugSeverityInput)
	{
		var domainInput = mapper.Map<BugSeverityInput>(bugSeverityInput);
		return await pluginsAiServices.GetBugSeverityAsync(domainInput).ConfigureAwait(false);
	}

	/// <summary>
	/// Moderates the content data asynchronous.
	/// </summary>
	/// <param name="story">The story.</param>
	/// <returns>
	/// The moderation content response dto.
	/// </returns>
	public async Task<string> ModerateContentDataAsync(string story)
	{
		return await pluginsAiServices.ModerateContentDataAsync(story).ConfigureAwait(false);
	}

	/// <summary>
	/// Rewrites text async.
	/// </summary>
	/// <param name="story">The story.</param>
	/// <returns>
	/// The rewrite response dto.
	/// </returns>
	public async Task<string> RewriteTextAsync(string story)
	{
		return await pluginsAiServices.RewriteTextAsync(story).ConfigureAwait(false);
	}
}
