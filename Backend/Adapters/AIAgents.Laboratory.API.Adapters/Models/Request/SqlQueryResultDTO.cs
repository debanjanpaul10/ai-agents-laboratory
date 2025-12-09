namespace AIAgents.Laboratory.API.Adapters.Models.Request;

/// <summary>
/// The SQL Query Result DTO.
/// </summary>
public sealed record SqlQueryResultDTO
{
    /// <summary>
    /// Gets or sets the json query.
    /// </summary>
    /// <value>
    /// The json query.
    /// </value>
    public string JsonQuery { get; set; } = string.Empty;
}
