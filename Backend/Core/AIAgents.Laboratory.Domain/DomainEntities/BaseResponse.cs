namespace AIAgents.Laboratory.Domain.DomainEntities;

/// <summary>
/// The Base Response Class.
/// </summary>
public record BaseResponse
{
    /// <summary>
    /// The total tokens consumed.
    /// </summary>
    public int TotalTokensConsumed { get; set; }

    /// <summary>
    /// The candidates token count.
    /// </summary>
    public int CandidatesTokenCount { get; set; }

    /// <summary>
    /// The prompt token count.
    /// </summary>
    public int PromptTokenCount { get; set; }

    /// <summary>
    /// The AI model used for this request.
    /// </summary>
    public string ModelUsed { get; set; } = string.Empty;
}
