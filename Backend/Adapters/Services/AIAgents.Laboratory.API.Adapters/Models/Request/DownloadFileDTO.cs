namespace AIAgents.Laboratory.API.Adapters.Models.Request;

/// <summary>
/// The Download File Data Transfer Object.
/// </summary>
public sealed record DownloadFileDTO
{
    /// <summary>
    /// The agent unique identifier.
    /// </summary>
    public string AgentGuid { get; init; } = string.Empty;

    /// <summary>
    /// The file name to download.
    /// </summary>
    public string FileName { get; init; } = string.Empty;
}
