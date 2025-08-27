using AIAgents.Laboratory.API.Adapters.Contracts;
using AIAgents.Laboratory.API.Adapters.Models.Request;
using AIAgents.Laboratory.API.Adapters.Models.Response;
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
public class PluginsHandler(IMapper mapper, IPluginsAiServices pluginsAiServices) : IPluginsHandler
{
    /// <summary>
	/// Generates the tag for story asynchronous.
	/// </summary>
	/// <param name="story">The story.</param>
	/// <returns>
	/// The genre tag response dto.
	/// </returns>
	public async Task<TagResponseDTO> GenerateTagForStoryAsync(string story)
    {
        var response = await pluginsAiServices.GenerateTagForStoryAsync(story).ConfigureAwait(false);
        return mapper.Map<TagResponseDTO>(response);
    }

    /// <summary>
	/// Gets the bug severity asynchronous.
	/// </summary>
	/// <param name="bugSeverityInput">The bug severity input.</param>
	/// <returns>
	/// The bug severity response.
	/// </returns>
	public async Task<BugSeverityResponseDTO> GetBugSeverityAsync(BugSeverityInputDTO bugSeverityInput)
    {
        var domainInput = mapper.Map<BugSeverityInput>(bugSeverityInput);
        var domainResult = await pluginsAiServices.GetBugSeverityAsync(domainInput).ConfigureAwait(false);
        return mapper.Map<BugSeverityResponseDTO>(domainResult);
    }

    /// <summary>
    /// Moderates the content data asynchronous.
    /// </summary>
    /// <param name="story">The story.</param>
    /// <returns>
    /// The moderation content response dto.
    /// </returns>
    public async Task<ModerationContentResponseDTO> ModerateContentDataAsync(string story)
    {
        var response = await pluginsAiServices.ModerateContentDataAsync(story).ConfigureAwait(false);
        return mapper.Map<ModerationContentResponseDTO>(response);
    }

    /// <summary>
    /// Rewrites text async.
    /// </summary>
    /// <param name="story">The story.</param>
    /// <returns>
    /// The rewrite response dto.
    /// </returns>
    public async Task<RewriteResponseDTO> RewriteTextAsync(string story)
    {
        var response = await pluginsAiServices.RewriteTextAsync(story).ConfigureAwait(false);
        return mapper.Map<RewriteResponseDTO>(response);
    }
}
