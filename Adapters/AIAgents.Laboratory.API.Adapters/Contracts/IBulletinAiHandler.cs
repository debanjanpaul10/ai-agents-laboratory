// *********************************************************************************
//	<copyright file="IBulletinAiHandler.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>The Bulletin AI Handler Interface.</summary>
// *********************************************************************************

using AIAgents.Laboratory.API.Adapters.Models.Response.IBBS;

namespace AIAgents.Laboratory.API.Adapters.Contracts;

/// <summary>
/// The Bulletin AI Handler Interface.
/// </summary>
public interface IBulletinAiHandler
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
}
