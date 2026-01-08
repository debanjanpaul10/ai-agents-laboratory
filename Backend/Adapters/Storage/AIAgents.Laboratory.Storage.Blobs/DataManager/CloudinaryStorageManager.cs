using AIAgents.Laboratory.Domain.DrivenPorts;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using static AIAgents.Laboratory.Storage.Blobs.Helpers.Constants;

namespace AIAgents.Laboratory.Storage.Blobs.DataManager;

/// <summary>
/// The Cloudinary Storage Manager implementation for handling file operations with Cloudinary.
/// </summary>
/// <param name="logger">The logger instance.</param>
/// <param name="configuration">The configuration service.</param>
/// <param name="cloudinary">The Cloudinary client instance.</param>
/// <seealso cref="ICloudinaryStorageManager"/>
public class CloudinaryStorageManager(ILogger<CloudinaryStorageManager> logger, IConfiguration configuration, ICloudinary cloudinary) : IBlobStorageManager
{
    /// <summary>
    /// Uploads image to cloudinary storage asynchronously.
    /// </summary>
    /// <param name="imageFile">The image form file.</param>
    /// <param name="agentGuid">The agent guid id.</param>
    /// <returns>The uploaded image url.</returns>
    public async Task<string> UploadImageToStorageAsync(IFormFile imageFile, string agentGuid)
    {
        try
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(UploadImageToStorageAsync), DateTime.UtcNow, imageFile.FileName);

            if (imageFile.Length == 0) return string.Empty;

            using var stream = imageFile.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(imageFile.FileName, stream),
                Folder = string.Format(CloudinaryConstants.AgentImagesFolderStructureFormat, configuration[AzureAppConfigurationConstants.CloudinaryFolderNameConstant], agentGuid)
            };

            var uploadResult = await cloudinary.UploadAsync(uploadParams).ConfigureAwait(false);
            if (uploadResult.Error is not null)
            {
                logger.LogError("Cloudinary error: {Error}", uploadResult.Error.Message);
                return string.Empty;
            }

            return uploadResult.StatusCode == System.Net.HttpStatusCode.OK && uploadResult.Url is not null ? Convert.ToString(uploadResult.Url)! : string.Empty;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, LoggingConstants.LogHelperMethodFailed, nameof(UploadImageToStorageAsync), DateTime.UtcNow, ex.Message);
            throw;
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(UploadImageToStorageAsync), DateTime.UtcNow, imageFile.FileName);
        }
    }
}
