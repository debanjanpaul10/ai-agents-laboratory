// *********************************************************************************
//	<copyright file="TagResponseDTO.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>The user story tag response DTO.</summary>
// *********************************************************************************

namespace AIAgents.Laboratory.Domain.DomainEntities.IBBS;

/// <summary>
/// The user story tag response DTO.
/// </summary>
public class TagResponse : BaseResponse
{
    /// <summary>
    /// The user story genre.
    /// </summary>
    public string UserStoryTag { get; set; } = string.Empty;
}
