using AIAgents.Laboratory.Domain.DrivenPorts;
using AIAgents.Laboratory.Domain.Helpers;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using static AIAgents.Laboratory.Storage.Blobs.Helpers.Constants;

namespace AIAgents.Laboratory.Storage.Blobs.DataManager;

/// <summary>
/// The Google Cloud Storage Manager implementation for handling file operations with Google Cloud Storage.
/// </summary>
/// <param name="logger">The logger instance.</param>
/// <param name="configuration">The configuration service.</param>
/// <param name="storageClient">The Google Cloud Storage client instance.</param>
/// <seealso cref="IBlobStorageManager"/>
public sealed class GcpCloudStorageManager(ILogger<GcpCloudStorageManager> logger, IConfiguration configuration, StorageClient storageClient) : IBlobStorageManager
{
    /// <summary>
    /// The Google Cloud Platform storage bucket name.
    /// </summary>
    private readonly string GCPStorageBucketName = configuration[AzureAppConfigurationConstants.GCPBucketNameConstant]
        ?? throw new KeyNotFoundException(ExceptionConstants.GCPBucketNotConfiguredExceptionMessage);

    /// <summary>
    /// The Google Cloud Platform knowledge base folder name.
    /// </summary>
    private readonly string GCPKnowledgeBaseFolderName = configuration[AzureAppConfigurationConstants.GCPKBFolderNameConstant]
        ?? throw new KeyNotFoundException(ExceptionConstants.ConfigurationKeyNotFoundExceptionMessage);

    /// <summary>
    /// The Google Cloud Platform vision images folder name.
    /// </summary>
    private readonly string GCPVisionImagesFolderName = configuration[AzureAppConfigurationConstants.GCPImageFolderNameConstant]
        ?? throw new KeyNotFoundException(ExceptionConstants.ConfigurationKeyNotFoundExceptionMessage);

    /// <summary>
    /// Deletes the documents data and folder from blob storage.
    /// </summary>
    /// <param name="agentId">The agent id.</param>
    /// <returns>A boolean for success/failure.</returns>
    public async Task<bool> DeleteDocumentsFolderAndDataAsync(string agentId)
    {
        try
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(DeleteDocumentsFolderAndDataAsync), DateTime.UtcNow, agentId);

            var folderNames = new List<string>();
            if (!string.IsNullOrEmpty(this.GCPKnowledgeBaseFolderName)) folderNames.Add(this.GCPKnowledgeBaseFolderName);
            if (!string.IsNullOrEmpty(this.GCPVisionImagesFolderName)) folderNames.Add(this.GCPVisionImagesFolderName);

            if (folderNames.Count == 0)
            {
                logger.LogWarning("No folder names configured for deletion. Skipping deletion for agent {AgentId}.", agentId);
                return false;
            }

            // Delete objects from each folder
            bool allDeletionsSuccessful = true;
            foreach (var folderName in folderNames)
            {
                try
                {
                    // Construct the prefix for objects to delete: {folderName}/{agentId}/
                    string prefix = string.Format(GcpCloudStorageConstants.AgentFolderStructureFormat, folderName, agentId) + "/";

                    // List all objects with the specified prefix
                    var objectsToDelete = storageClient.ListObjects(this.GCPStorageBucketName, prefix);

                    // Iterate through all objects with the prefix
                    var deleteTasks = new List<Task>();
                    foreach (var obj in objectsToDelete)
                        deleteTasks.Add(storageClient.DeleteObjectAsync(this.GCPStorageBucketName, obj.Name));

                    // Wait for all deletions to complete
                    if (deleteTasks.Count > 0)
                        await Task.WhenAll(deleteTasks).ConfigureAwait(false);
                }
                catch (Exception folderEx)
                {
                    logger.LogError(folderEx, "Failed to delete objects from folder {FolderName} for agent {AgentId}. Error: {ErrorMessage}", folderName, agentId, folderEx.Message);
                    allDeletionsSuccessful = false;
                }
            }

            return allDeletionsSuccessful;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, LoggingConstants.LogHelperMethodFailed, nameof(DeleteDocumentsFolderAndDataAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message);
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(DeleteDocumentsFolderAndDataAsync), DateTime.UtcNow, agentId);
        }
    }

    /// <summary>
    /// Downloads a file from blob storage.
    /// </summary>
    /// <param name="agentGuid">The agent guid id.</param>
    /// <param name="fileName">The file name.</param>
    /// <returns>The download file link.</returns>
    public async Task<string> DownloadFileFromBlobStorageAsync(string agentGuid, string fileName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(agentGuid);
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);

        try
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(DownloadFileFromBlobStorageAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { agentGuid, fileName }));

            var safeFileName = Path.GetFileName(fileName);
            string objectName = string.Format(GcpCloudStorageConstants.AgentFolderStructureFormat, this.GCPKnowledgeBaseFolderName, agentGuid) + "/" + safeFileName;

            var stream = new MemoryStream();
            var file = await storageClient.DownloadObjectAsync(
                bucket: this.GCPStorageBucketName,
                objectName: objectName,
                destination: stream).ConfigureAwait(false);

            if (file is not null)
                return string.Format(GcpCloudStorageConstants.PublicUrlConstant, this.GCPStorageBucketName, file.Name);
            else
                throw new FileNotFoundException(ExceptionConstants.FileNotFoundExceptionMessage);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, LoggingConstants.LogHelperMethodFailed, nameof(DownloadFileFromBlobStorageAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message);
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(DownloadFileFromBlobStorageAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { agentGuid, fileName }));
        }
    }

    /// <summary>
    /// Uploads documents to BLOB storage.
    /// </summary>
    /// <param name="documentFile">The user uploaded document file.</param>
    /// <param name="agentGuid">The agent guid id.</param>
    /// <param name="folderName">The storage folder name.</param>
    /// <returns>The public URL for the document.</returns>
    public async Task<string> UploadDocumentsToStorageAsync(IFormFile documentFile, string agentGuid, UploadedFileType fileType)
    {
        if (documentFile.Length == 0) return string.Empty;
        try
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(UploadDocumentsToStorageAsync), DateTime.UtcNow, documentFile.FileName);

            var folderName = fileType switch
            {
                UploadedFileType.AiVisionImageDocument => this.GCPVisionImagesFolderName,
                UploadedFileType.KnowledgeBaseDocument => this.GCPKnowledgeBaseFolderName,
                _ => string.Empty,
            };

            var safeFileName = Path.GetFileName(documentFile.FileName);
            var objectName = string.Format(GcpCloudStorageConstants.AgentFolderStructureFormat, folderName, agentGuid) + "/" + safeFileName;

            using var stream = documentFile.OpenReadStream();
            var uploadedObject = await storageClient.UploadObjectAsync(
                bucket: this.GCPStorageBucketName,
                objectName: objectName,
                contentType: documentFile.ContentType,
                source: stream).ConfigureAwait(false);

            // Construct the public URL for the uploaded object
            return string.Format(GcpCloudStorageConstants.PublicUrlConstant, this.GCPStorageBucketName, uploadedObject.Name);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, LoggingConstants.LogHelperMethodFailed, nameof(UploadDocumentsToStorageAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message);
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(UploadDocumentsToStorageAsync), DateTime.UtcNow, documentFile.FileName);
        }
    }

}
