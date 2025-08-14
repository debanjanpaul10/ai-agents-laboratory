// *********************************************************************************
//	<copyright file="IBulletinAIServices.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>Bulletin Board AI services interface.</summary>
// *********************************************************************************

using AIAgents.Laboratory.Domain.DomainEntities.IBBS;

namespace AIAgents.Laboratory.Domain.DrivingPorts;

/// <summary>
/// Bulletin Board AI services interface.
/// </summary>
public interface IBulletinAIServices
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
}
