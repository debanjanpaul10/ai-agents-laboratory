namespace AIAgents.Laboratory.Domain.DomainEntities.SkillsEntities;

/// <summary>
/// The followup question request domain model.
/// </summary>
public sealed record FollowupQuestionsRequestDomain
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
