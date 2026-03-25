using AIAgents.Laboratory.Domain.Contracts;
using AIAgents.Laboratory.Domain.Helpers;
using AIAgents.Laboratory.Domain.Ports.Out;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using static AIAgents.Laboratory.Storage.Blobs.Helpers.Constants;

namespace AIAgents.Laboratory.Storage.Blobs.StorageManager;

/// <summary>
/// The Cloudinary Storage Manager implementation for handling file operations with Cloudinary.
/// </summary>
/// <param name="logger">The logger instance.</param>
/// <param name="configuration">The configuration service.</param>
/// <param name="correlationContext">The correlation context for logging.</param>
/// <param name="cloudinary">The Cloudinary client instance.</param>
/// <seealso cref="IBlobStorageManager"/>
public sealed class CloudinaryStorageManager(ILogger<CloudinaryStorageManager> logger, IConfiguration configuration, ICorrelationContext correlationContext, ICloudinary cloudinary) : IBlobStorageManager
{
    /// <summary>
    /// The Cloudinary knowledge base folder name.
    /// </summary>
    private readonly string CloudinaryKnowledgeBaseFolderName = configuration[AzureAppConfigurationConstants.CloudinaryKnowledgebaseFolderNameConstant]
        ?? throw new KeyNotFoundException(ExceptionConstants.ConfigurationKeyNotFoundExceptionMessage);

    /// <summary>
    /// The Cloudinary vision images folder name.
    /// </summary>
    private readonly string CloudinaryVisionImagesFolderName = configuration[AzureAppConfigurationConstants.CloudinaryImageFolderNameConstant]
        ?? throw new KeyNotFoundException(ExceptionConstants.ConfigurationKeyNotFoundExceptionMessage);

    /// <summary>
    /// Deletes the documents data and folder from blob storage.
    /// </summary>
    /// <param name="agentId">The agent id.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the asynchronous process. Optional.</param>
    /// <returns>A boolean for success/failure.</returns>
    public async Task<bool> DeleteDocumentsFolderAndDataAsync(string agentId, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(DeleteDocumentsFolderAndDataAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, agentId }));

            var folderNames = new List<string>();
            if (!string.IsNullOrEmpty(this.CloudinaryKnowledgeBaseFolderName))
                folderNames.Add(this.CloudinaryKnowledgeBaseFolderName);

            if (!string.IsNullOrEmpty(this.CloudinaryVisionImagesFolderName))
                folderNames.Add(this.CloudinaryVisionImagesFolderName);

            if (folderNames.Count == 0)
                return false;

            // Delete objects from each folder
            bool allDeletionsSuccessful = true;
            foreach (var folderName in folderNames)
            {
                try
                {
                    // Construct the folder path: {folderName}/{agentId}
                    string folderPath = string.Format(CloudinaryConstants.AgentImagesFolderStructureFormat, folderName, agentId);

                    // Delete resources of type 'image'
                    await cloudinary.DeleteResourcesAsync(
                        parameters: new DelResParams
                        {
                            Prefix = folderPath,
                            ResourceType = ResourceType.Image
                        },
                        cancellationToken
                    ).ConfigureAwait(false);

                    // Delete resources of type 'raw' (for non-image docs)
                    await cloudinary.DeleteResourcesAsync(
                        parameters: new DelResParams
                        {
                            Prefix = folderPath,
                            ResourceType = ResourceType.Raw
                        },
                        cancellationToken
                    ).ConfigureAwait(false);

                    // Delete the folder itself
                    await cloudinary.DeleteFolderAsync(
                        folder: folderPath,
                        cancellationToken: cancellationToken
                    ).ConfigureAwait(false);
                }
                catch (Exception folderEx)
                {
                    logger.LogAppError(folderEx, "Failed to delete objects from folder {FolderName} for agent {AgentId}. Error: {ErrorMessage}", folderName, agentId, folderEx.Message);
                    allDeletionsSuccessful = false;
                }
            }

            return allDeletionsSuccessful;
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(DeleteDocumentsFolderAndDataAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(DeleteDocumentsFolderAndDataAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, agentId }));
        }
    }

    /// <summary>
    /// Downloads a file from blob storage.
    /// </summary>
    /// <param name="agentGuid">The agent guid id.</param>
    /// <param name="fileName">The file name.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the asynchronous process. Optional.</param>
    /// <returns>The download file link.</returns>
    public async Task<string> DownloadFileFromBlobStorageAsync(string agentGuid, string fileName, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(agentGuid);
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);

        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(DownloadFileFromBlobStorageAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { agentGuid, fileName }));

            var safeFileName = Path.GetFileName(fileName);
            // Try to get resource as Raw first (most documents are Raw)
            string folderPath = string.Format(
                CloudinaryConstants.AgentImagesFolderStructureFormat,
                this.CloudinaryKnowledgeBaseFolderName, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, agentGuid }));
            string publicId = folderPath + "/" + safeFileName;

            var getResourceParams = new GetResourceParams(publicId)
            {
                ResourceType = ResourceType.Raw
            };

            var resource = await cloudinary.GetResourceAsync(
                parameters: getResourceParams,
                cancellationToken
            ).ConfigureAwait(false);

            if (resource.Error is not null)
            {
                // Try as Image if Raw fails (Cloudinary sometimes treats files as images or publicId might not have extension)
                getResourceParams.ResourceType = ResourceType.Image;
                getResourceParams.PublicId = folderPath + "/" + Path.GetFileNameWithoutExtension(safeFileName);

                resource = await cloudinary.GetResourceAsync(
                    parameters: getResourceParams,
                    cancellationToken
                ).ConfigureAwait(false);
            }

            if (resource.Error is not null)
            {
                var ex = new FileNotFoundException(string.Format(ExceptionConstants.FileNotFoundExceptionMessage, fileName, agentGuid));
                logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(DownloadFileFromBlobStorageAsync), DateTime.UtcNow, ex.Message);
                throw ex;
            }

            return resource.Url;
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(DownloadFileFromBlobStorageAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(DownloadFileFromBlobStorageAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, agentGuid, fileName }));
        }
    }

    /// <summary>
    /// Uploads documents to BLOB storage.
    /// </summary>
    /// <param name="documentFile">The user uploaded document file.</param>
    /// <param name="agentGuid">The agent guid id.</param>
    /// <param name="fileType">The uploaded file type.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the asynchronous process. Optional.</param>
    /// <returns>The public URL for the document.</returns>
    public async Task<string> UploadDocumentsToStorageAsync(IFormFile documentFile, string agentGuid, UploadedFileType fileType, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(UploadDocumentsToStorageAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, agentGuid, documentFile.FileName }));

            if (documentFile.Length == 0) return string.Empty;
            var folderName = fileType switch
            {
                UploadedFileType.AiVisionImageDocument => this.CloudinaryVisionImagesFolderName,
                UploadedFileType.KnowledgeBaseDocument => this.CloudinaryKnowledgeBaseFolderName,
                _ => string.Empty,
            };

            using var stream = documentFile.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(documentFile.FileName, stream),
                Folder = string.Format(CloudinaryConstants.AgentImagesFolderStructureFormat, folderName, agentGuid)
            };
            var uploadResult = await cloudinary.UploadAsync(
                parameters: uploadParams,
                cancellationToken
            ).ConfigureAwait(false);

            if (uploadResult.Error is not null)
            {
                logger.LogError("Cloudinary error: {Error}", uploadResult.Error.Message);
                return string.Empty;
            }

            return uploadResult.StatusCode == System.Net.HttpStatusCode.OK && uploadResult.Url is not null ? Convert.ToString(uploadResult.Url)! : string.Empty;
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(UploadDocumentsToStorageAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(UploadDocumentsToStorageAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, agentGuid, documentFile.FileName }));
        }
    }
}
