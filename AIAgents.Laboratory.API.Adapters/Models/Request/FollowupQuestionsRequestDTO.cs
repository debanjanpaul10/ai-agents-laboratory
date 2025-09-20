// *********************************************************************************
//	<copyright file="FollowupQuestionsRequestDTO.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>The followup question request DTO model.</summary>
// *********************************************************************************

namespace AIAgents.Laboratory.API.Adapters.Models.Request;

/// <summary>
/// The followup questions request DTO model.
/// </summary>
public class FollowupQuestionsRequestDTO
{
    /// <summary>
    /// The user query.
    /// </summary>
    public string UserQuery { get; set; } = string.Empty;

    /// <summary>
    /// The user intent.
    /// </summary>
    public string UserIntent { get; set; } = string.Empty;

    /// <summary>
    /// The AI Response object.
    /// </summary>
    public string AiResponseData { get; set; } = string.Empty;
}
