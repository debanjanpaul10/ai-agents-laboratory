namespace AIAgents.Laboratory.API.Adapters.Models.Request;

/// <summary>
/// The followup questions request DTO model.
/// </summary>
public sealed record FollowupQuestionsRequestDTO
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
