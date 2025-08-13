// *********************************************************************************
//	<copyright file="BulletinAiHandler.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>The Bulletin AI Handler Class.</summary>
// *********************************************************************************

using AIAgents.Laboratory.API.Adapters.Contracts;
using AIAgents.Laboratory.API.Adapters.Models.Response.IBBS;
using AIAgents.Laboratory.Domain.DrivingPorts;
using AutoMapper;

namespace AIAgents.Laboratory.API.Adapters.Handlers;

/// <summary>
/// The Bulletin AI Handler Class.
/// </summary>
/// <seealso cref="AIAgents.Laboratory.API.Adapters.Contracts.IBulletinAiHandler" />
public class BulletinAiHandler(IBulletinAIServices bulletinAIServices, IMapper mapper) : IBulletinAiHandler
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
		var response = await bulletinAIServices.GenerateTagForStoryAsync(story);
		return mapper.Map<TagResponseDTO>(response);
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
		var response = await bulletinAIServices.ModerateContentDataAsync(story);
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
		var response = await bulletinAIServices.RewriteTextAsync(story);
		return mapper.Map<RewriteResponseDTO>(response);
	}
}
