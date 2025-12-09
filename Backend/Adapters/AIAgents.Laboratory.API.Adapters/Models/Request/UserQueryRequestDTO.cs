namespace AIAgents.Laboratory.API.Adapters.Models.Request;

/// <summary>
/// The User Query Request DTO.
/// </summary>
public sealed record UserQueryRequestDTO
{
    /// <summary>
    /// Gets or sets the user query.
    /// </summary>
    /// <value>
    /// The user query.
    /// </value>
    public string UserQuery { get; set; } = string.Empty;
}
