using AIAgents.Laboratory.Domain.DrivenPorts;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using static AIAgents.Laboratory.Storage.Blobs.Helpers.Constants;

namespace AIAgents.Laboratory.Storage.Blobs.DataManager;

/// <summary>
/// The Google Cloud Storage Manager implementation for handling file operations with Google Cloud Storage.
/// </summary>
/// <param name="logger">The logger instance.</param>
/// <param name="configuration">The configuration service.</param>
/// <param name="storageClient">The Google Cloud Storage client instance.</param>
/// <seealso cref="IBlobStorageManager"/>
public class GCPCloudStorageManager(ILogger<GCPCloudStorageManager> logger, IConfiguration configuration, StorageClient storageClient) : IBlobStorageManager
{
    /// <summary>
    /// Uploads image to Google Cloud Storage asynchronously.
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

            var bucketName = configuration[AzureAppConfigurationConstants.GCPBucketNameConstant];
            if (string.IsNullOrWhiteSpace(bucketName))
                throw new InvalidOperationException(ExceptionConstants.GCPBucketNotConfiguredExceptionMessage);

            var folderName = configuration[AzureAppConfigurationConstants.GCPFolderNameConstant];
            var safeFileName = Path.GetFileName(imageFile.FileName);
            var objectName = string.Format(GCPCloudStorageConstants.AgentImagesFolderStructureFormat, folderName, agentGuid) + "/" + safeFileName;

            using var stream = imageFile.OpenReadStream();
            var uploadedObject = await storageClient.UploadObjectAsync(
                bucket: bucketName,
                objectName: objectName,
                contentType: imageFile.ContentType,
                source: stream,
                options: new UploadObjectOptions { PredefinedAcl = PredefinedObjectAcl.PublicRead }).ConfigureAwait(false);

            // Construct the public URL for the uploaded object
            return string.Format(GCPCloudStorageConstants.PublicUrlConstant, bucketName, objectName);
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
