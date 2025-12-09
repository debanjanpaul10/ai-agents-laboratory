namespace AIAgents.Laboratory.Domain.DomainEntities;

/// <summary>
/// The user request domain.
/// </summary>
public sealed record UserRequestDomain
{
    /// <summary>
    /// Gets or sets the user query.
    /// </summary>
    /// <value>
    /// The user query.
    /// </value>
    public string UserQuery { get; set; } = string.Empty;
}
