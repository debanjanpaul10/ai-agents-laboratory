using AIAgents.Laboratory.Domain.Helpers;
using Microsoft.AspNetCore.Http;

namespace AIAgents.Laboratory.Domain.Ports.Out;

/// <summary>
/// The interface declaration for blob storage manager.
/// </summary>
/// <remarks>All blob storage related activities like uploading, downloading, deleting, etc will be handled for static files.</remarks>
public interface IBlobStorageManager
{
    /// <summary>
    /// Uploads documents to BLOB storage.
    /// </summary>
    /// <param name="documentFile">The user uploaded document file.</param>
    /// <param name="agentGuid">The agent guid id.</param>
    /// <param name="fileType">The type of uploaded file.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The public URL for the document.</returns>
    Task<string> UploadDocumentsToStorageAsync(IFormFile documentFile, string agentGuid, UploadedFileType fileType, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes the documents data and folder from blob storage.
    /// </summary>
    /// <param name="agentId">The agent id.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A boolean for success/failure.</returns>
    Task<bool> DeleteDocumentsFolderAndDataAsync(string agentId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Downloads a file from blob storage.
    /// </summary>
    /// <param name="agentGuid">The agent guid id.</param>
    /// <param name="fileName">The file name.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The download file link.</returns>
    Task<string> DownloadFileFromBlobStorageAsync(string agentGuid, string fileName, CancellationToken cancellationToken = default);
}
