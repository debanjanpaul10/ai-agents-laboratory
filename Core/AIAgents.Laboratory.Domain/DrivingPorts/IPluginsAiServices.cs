using AIAgents.Laboratory.Domain.DomainEntities;

namespace AIAgents.Laboratory.Domain.DrivingPorts;

/// <summary>
/// The plugins service interface.
/// </summary>
public interface IPluginsAiServices
{
    /// <summary>
    /// Rewrites text async.
    /// </summary>
    /// <param name="story">The story.</param>
    /// <returns>The rewrite response dto.</returns>
    Task<RewriteResponse> RewriteTextAsync(string story);

    /// <summary>
    /// Generates the tag for story asynchronous.
    /// </summary>
    /// <param name="story">The story.</param>
    /// <returns>The genre tag response dto.</returns>
    Task<TagResponse> GenerateTagForStoryAsync(string story);

    /// <summary>
    /// Moderates the content data asynchronous.
    /// </summary>
    /// <param name="story">The story.</param>
    /// <returns>The moderation content response dto.</returns>
    Task<ModerationContentResponse> ModerateContentDataAsync(string story);

    /// <summary>
    /// Gets the bug severity asynchronous.
    /// </summary>
    /// <param name="bugSeverityInput">The bug severity input.</param>
    /// <returns>The bug severity response.</returns>
    Task<BugSeverityResponse> GetBugSeverityAsync(BugSeverityInput bugSeverityInput);
}
