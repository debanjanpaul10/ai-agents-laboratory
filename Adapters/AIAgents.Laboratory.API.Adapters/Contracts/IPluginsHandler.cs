using AIAgents.Laboratory.API.Adapters.Models.Request;
using AIAgents.Laboratory.API.Adapters.Models.Response;

namespace AIAgents.Laboratory.API.Adapters.Contracts;

/// <summary>
/// The plugins handler interface.
/// </summary>
public interface IPluginsHandler
{
	/// <summary>
	/// Rewrites text async.
	/// </summary>
	/// <param name="story">The story.</param>
	/// <returns>The rewrite response dto.</returns>
	Task<RewriteResponseDTO> RewriteTextAsync(string story);

	/// <summary>
	/// Generates the tag for story asynchronous.
	/// </summary>
	/// <param name="story">The story.</param>
	/// <returns>The genre tag response dto.</returns>
	Task<TagResponseDTO> GenerateTagForStoryAsync(string story);

	/// <summary>
	/// Moderates the content data asynchronous.
	/// </summary>
	/// <param name="story">The story.</param>
	/// <returns>The moderation content response dto.</returns>
	Task<ModerationContentResponseDTO> ModerateContentDataAsync(string story);

	/// <summary>
	/// Gets the bug severity asynchronous.
	/// </summary>
	/// <param name="bugSeverityInput">The bug severity input.</param>
	/// <returns>The bug severity response.</returns>
	Task<string> GetBugSeverityAsync(BugSeverityInputDTO bugSeverityInput);
}
