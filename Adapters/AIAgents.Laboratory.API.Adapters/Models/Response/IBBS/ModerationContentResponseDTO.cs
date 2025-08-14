// *********************************************************************************
//	<copyright file="ModerationContentResponseDTO.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>The content moderation response DTO.</summary>
// *********************************************************************************

namespace AIAgents.Laboratory.API.Adapters.Models.Response.IBBS;

/// <summary>
/// The moderation content response DTO.
/// </summary>
public class ModerationContentResponseDTO : BaseResponseDTO
{
    /// <summary>
    /// The content rating for the user story.
    /// </summary>
    public string ContentRating { get; set; } = string.Empty;
}
