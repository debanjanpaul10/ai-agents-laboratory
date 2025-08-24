// *********************************************************************************
//	<copyright file="FollowupQuestionsRequestDomain.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>The followup question request domain model.</summary>
// *********************************************************************************

namespace AIAgents.Laboratory.Domain.DomainEntities.FitGymTool;

/// <summary>
/// The followup question request domain model.
/// </summary>
public class FollowupQuestionsRequestDomain
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
