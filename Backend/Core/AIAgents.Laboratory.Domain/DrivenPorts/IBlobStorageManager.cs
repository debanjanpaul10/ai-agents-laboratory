using AIAgents.Laboratory.Domain.Helpers;
using Microsoft.AspNetCore.Http;

namespace AIAgents.Laboratory.Domain.DrivenPorts;

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
    /// <returns>The public URL for the document.</returns>
    Task<string> UploadDocumentsToStorageAsync(IFormFile documentFile, string agentGuid, UploadedFileType fileType);

    /// <summary>
    /// Deletes the documents data and folder from blob storage.
    /// </summary>
    /// <param name="agentId">The agent id.</param>
    /// <returns>A boolean for success/failure.</returns>
    Task<bool> DeleteDocumentsFolderAndDataAsync(string agentId);
}
