namespace AIAgents.Laboratory.Domain.DomainEntities.SkillsEntities;

/// <summary>
/// The Skills input domain.
/// </summary>
public record SkillsInputDomain
{
    /// <summary>
    /// Gets or sets the user query.
    /// </summary>
    /// <value>
    /// The user query.
    /// </value>
    public string UserQuery { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the knowledge base.
    /// </summary>
    /// <value>
    /// The knowledge base.
    /// </value>
    public string KnowledgeBase { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the source.
    /// </summary>
    /// <value>
    /// The source.
    /// </value>
    public string Source { get; set; } = string.Empty;
}
