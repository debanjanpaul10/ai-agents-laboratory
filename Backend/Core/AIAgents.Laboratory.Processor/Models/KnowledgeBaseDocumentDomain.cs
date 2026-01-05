namespace AIAgents.Laboratory.Processor.Models;

/// <summary>
/// The Knowledge Base Document Domain model.
/// </summary>

public sealed record KnowledgeBaseDocumentDomain
{
    /// <summary>
    /// Gets or sets the original filename
    /// </summary>
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the file content type
    /// </summary>
    public string ContentType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the file content as binary data
    /// </summary>
    public byte[] FileContent { get; set; } = [];

    /// <summary>
    /// Gets or sets the file size in bytes
    /// </summary>
    public long FileSize { get; set; }

    /// <summary>
    /// Gets or sets the upload date
    /// </summary>
    public DateTime UploadDate { get; set; }
}