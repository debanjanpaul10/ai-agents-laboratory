using Microsoft.AspNetCore.Http;

namespace AIAgents.Laboratory.Domain.DrivenPorts;

/// <summary>
/// Defines the methods for cloudinary storage manager.
/// </summary>
public interface ICloudinaryStorageManager
{
    /// <summary>
    /// Uploads image to cloudinary storage asynchronously.
    /// </summary>
    /// <param name="imageFile">The image form file.</param>
    /// <param name="agentGuid">The agent guid id.</param>
    /// <returns>The uploaded image url.</returns>
    Task<string> UploadImageToStorageAsync(IFormFile imageFile, string agentGuid);
}
