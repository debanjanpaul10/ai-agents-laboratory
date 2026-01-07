using Microsoft.AspNetCore.Http;

namespace AIAgents.Laboratory.Domain.DrivenPorts;

/// <summary>
/// The interface declaration for blob storage manager.
/// </summary>
/// <remarks>All blob storage related activities like uploading, downloading, deleting, etc will be handled for static files.</remarks>
public interface IBlobStorageManager
{
    /// <summary>
    /// Uploads image to blob storage asynchronously.
    /// </summary>
    /// <param name="imageFile">The image form file.</param>
    /// <param name="agentGuid">The agent guid id.</param>
    /// <returns>The uploaded image url.</returns>
    Task<string> UploadImageToStorageAsync(IFormFile imageFile, string agentGuid);
}
