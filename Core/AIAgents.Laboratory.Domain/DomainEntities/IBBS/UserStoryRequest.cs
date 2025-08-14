// *********************************************************************************
//	<copyright file="UserStoryRequestDTO.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>The Common User Story Request dto.</summary>
// *********************************************************************************

namespace AIAgents.Laboratory.Domain.DomainEntities.IBBS;

/// <summary>
/// The Common User Story Request dto.
/// </summary>
public class UserStoryRequest
{
	/// <summary>
	/// Gets or sets the story.
	/// </summary>
	/// <value>
	/// The story.
	/// </value>
	public string Story { get; set; } = string.Empty;
}