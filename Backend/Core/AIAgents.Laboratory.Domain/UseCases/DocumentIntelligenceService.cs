using AIAgents.Laboratory.Domain.Contracts;
using AIAgents.Laboratory.Domain.DomainEntities.AgentsEntities;
using AIAgents.Laboratory.Domain.Helpers;
using AIAgents.Laboratory.Domain.Ports.Out;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using static AIAgents.Laboratory.Domain.Helpers.Constants;

namespace AIAgents.Laboratory.Domain.UseCases;

/// <summary>
/// Provides services for processing and managing agent knowledge base documents and AI Vision images, including uploading, analyzing, and preparing updates for persistence.
/// </summary>
/// <remarks>This service coordinates the processing of agent-related documents and images, including validation,
/// content extraction, and update preparation. It is intended to be used as part of the agent data management workflow, and relies on injected dependencies for logging, document processing, image storage, and vision analysis.</remarks>
/// <param name="logger">The logger used to record informational and error messages for operations performed by the service. Cannot be null.</param>
/// <param name="configuration">The configuration service.</param>
/// <param name="correlationContext">The correlation context for logging.</param>
/// <param name="knowledgeBaseProcessor">The processor responsible for reading and analyzing knowledge base documents. Cannot be null.</param>
/// <param name="blobStorageManager">The storage manager used to upload and manage images in cloud storage. Cannot be null.</param>
/// <param name="visionProcessor">The processor used to analyze images and extract keywords using AI vision capabilities. Cannot be null.</param>
/// <seealso cref="IDocumentIntelligenceService"/>
public sealed class DocumentIntelligenceService(
    ILogger<DocumentIntelligenceService> logger,
    IConfiguration configuration,
    ICorrelationContext correlationContext,
    IKnowledgeBaseProcessor knowledgeBaseProcessor,
    IBlobStorageManager blobStorageManager,
    IVisionProcessor visionProcessor) : IDocumentIntelligenceService
{
    /// <summary>
    /// The configuration value for allowed knowledge base file formats.
    /// </summary>
    private readonly string AllowedKnowledgebaseFileFormats = configuration[AzureAppConfigurationConstants.AllowedKbFileFormatsConstant]
        ?? throw new KeyNotFoundException(ExceptionConstants.ConfigurationKeyNotFoundExceptionMessage);

    /// <summary>
    /// The configuration value for allowed ai vision images file formats.
    /// </summary>
    private readonly string AllowedAiVisionImagesFileFormats = configuration[AzureAppConfigurationConstants.AllowedVisionImageFileFormatsConstant]
        ?? throw new KeyNotFoundException(ExceptionConstants.ConfigurationKeyNotFoundExceptionMessage);

    /// <inheritdoc />
    public async Task DeleteKnowledgebaseAndImagesDataAsync(
        string agentId,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodStart,
                nameof(DeleteKnowledgebaseAndImagesDataAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { correlationContext.CorrelationId, agentId })
            );

            await blobStorageManager.DeleteDocumentsFolderAndDataAsync(
                agentId,
                cancellationToken
            ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex,
                LoggingConstants.LogHelperMethodFailed,
                nameof(DeleteKnowledgebaseAndImagesDataAsync), DateTime.UtcNow, ex.Message
            );
            throw new AIAgentsBusinessException(
                message: ex.Message,
                correlationId: correlationContext.CorrelationId
            );
        }
        finally
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodEnd,
                nameof(DeleteKnowledgebaseAndImagesDataAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { correlationContext.CorrelationId, agentId })
            );
        }
    }

    #region KNOWLEDGE BASE

    /// <inheritdoc />
    public async Task CreateAndProcessKnowledgeBaseDocumentAsync(
        AgentDataDomain agentData,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodStart,
                nameof(CreateAndProcessKnowledgeBaseDocumentAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { correlationContext.CorrelationId, agentData.AgentId })
            );

            if (agentData.KnowledgeBaseDocument is null || !agentData.KnowledgeBaseDocument.Any() || string.IsNullOrEmpty(this.AllowedKnowledgebaseFileFormats))
                return;

            DocumentHandlerService.ValidateUploadedFiles(
                uploadedFiles: agentData.KnowledgeBaseDocument,
                allowedFileFormats: this.AllowedKnowledgebaseFileFormats
            );

            foreach (var uploadedDocument in agentData.KnowledgeBaseDocument)
                await blobStorageManager.UploadDocumentsToStorageAsync(
                    documentFile: uploadedDocument,
                    agentGuid: agentData.AgentId,
                    fileType: UploadedFileType.KnowledgeBaseDocument,
                    cancellationToken
                ).ConfigureAwait(false);

            await agentData.ProcessKnowledgebaseDocumentDataAsync(
                cancellationToken
            ).ConfigureAwait(false);
            if (agentData.StoredKnowledgeBase is not null && agentData.StoredKnowledgeBase.Any())
            {
                foreach (var file in agentData.StoredKnowledgeBase)
                {
                    var content = knowledgeBaseProcessor.DetectAndReadFileContent(
                        knowledgeBaseDocumentDomain: file
                    );
                    await knowledgeBaseProcessor.ProcessKnowledgeBaseDocumentAsync(
                        content,
                        agentId: agentData.AgentId,
                        cancellationToken
                    ).ConfigureAwait(false);
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex,
                LoggingConstants.LogHelperMethodFailed,
                nameof(CreateAndProcessKnowledgeBaseDocumentAsync), DateTime.UtcNow, ex.Message
            );
            throw new AIAgentsBusinessException(
                message: ex.Message,
                correlationId: correlationContext.CorrelationId
            );
        }
        finally
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodEnd,
                nameof(CreateAndProcessKnowledgeBaseDocumentAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { correlationContext.CorrelationId, agentData.AgentId })
            );
        }
    }

    /// <inheritdoc />
    public async Task HandleKnowledgeBaseDataUpdateAsync(
        AgentDataDomain updateDataDomain,
        AgentDataDomain existingAgent,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodStart,
                nameof(HandleKnowledgeBaseDataUpdateAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { correlationContext.CorrelationId, updateDataDomain.AgentId })
            );

            // Start from existing stored knowledge base
            var updatedStoredKnowledgeBase = existingAgent.StoredKnowledgeBase?.ToList() ?? [];
            var hasChanges = false;

            // 1. Remove any existing documents whose names are in RemovedKnowledgeBaseDocuments
            if (updateDataDomain.RemovedKnowledgeBaseDocuments?.Count > 0)
            {
                var removedSet = new HashSet<string>(updateDataDomain.RemovedKnowledgeBaseDocuments, StringComparer.OrdinalIgnoreCase);
                updatedStoredKnowledgeBase = [.. updatedStoredKnowledgeBase.Where(doc => !removedSet.Contains(doc.FileName))];
                hasChanges = true;
            }

            // 2. Process any newly uploaded knowledge base documents
            if (updateDataDomain.KnowledgeBaseDocument?.Count > 0 && !string.IsNullOrEmpty(this.AllowedKnowledgebaseFileFormats))
            {
                DocumentHandlerService.ValidateUploadedFiles(
                    uploadedFiles: updateDataDomain.KnowledgeBaseDocument,
                    allowedFileFormats: this.AllowedKnowledgebaseFileFormats
                );
                foreach (var uploadedFile in updateDataDomain.KnowledgeBaseDocument)
                    await blobStorageManager.UploadDocumentsToStorageAsync(
                        documentFile: uploadedFile,
                        agentGuid: updateDataDomain.AgentId,
                        fileType: UploadedFileType.KnowledgeBaseDocument,
                        cancellationToken
                    ).ConfigureAwait(false);

                await updateDataDomain.ProcessKnowledgebaseDocumentDataAsync(
                    cancellationToken
                ).ConfigureAwait(false);
                if (updateDataDomain.StoredKnowledgeBase is not null && updateDataDomain.StoredKnowledgeBase.Any())
                {
                    foreach (var file in updateDataDomain.StoredKnowledgeBase)
                    {
                        var content = knowledgeBaseProcessor.DetectAndReadFileContent(
                            knowledgeBaseDocumentDomain: file
                        );
                        await knowledgeBaseProcessor.ProcessKnowledgeBaseDocumentAsync(
                            content, agentId: updateDataDomain.AgentId,
                            cancellationToken
                        ).ConfigureAwait(false);
                    }

                    updatedStoredKnowledgeBase.AddRange(updateDataDomain.StoredKnowledgeBase);
                    hasChanges = true;
                }
            }

            // 3. Only persist changes if something actually changed
            if (hasChanges)
                // If all documents were removed, set StoredKnowledgeBase to null, otherwise save the updated list
                updateDataDomain.StoredKnowledgeBase = updatedStoredKnowledgeBase.Count != 0 ? updatedStoredKnowledgeBase : [];
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex,
                LoggingConstants.LogHelperMethodFailed,
                nameof(HandleKnowledgeBaseDataUpdateAsync), DateTime.UtcNow, ex.Message
            );
            throw new AIAgentsBusinessException(
                message: ex.Message,
                correlationId: correlationContext.CorrelationId
            );
        }
        finally
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodEnd,
                nameof(HandleKnowledgeBaseDataUpdateAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { correlationContext.CorrelationId, updateDataDomain.AgentId })
            );
        }
    }

    /// <inheritdoc />
    public async Task<string> DownloadKnowledgebaseFileAsync(
        string agentGuid,
        string fileName,
        CancellationToken cancellationToken = default
    )
    {
        string response = string.Empty;
        try
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodStart,
                nameof(DownloadKnowledgebaseFileAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { correlationContext.CorrelationId, agentGuid, fileName })
            );

            response = await blobStorageManager.DownloadFileFromBlobStorageAsync(
                agentGuid,
                fileName,
                cancellationToken
            ).ConfigureAwait(false);
            return response;
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex,
                LoggingConstants.LogHelperMethodFailed,
                nameof(DownloadKnowledgebaseFileAsync), DateTime.UtcNow, ex.Message
            );
            throw new AIAgentsBusinessException(
                message: ex.Message,
                correlationId: correlationContext.CorrelationId
            );
        }
        finally
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodEnd,
                nameof(DownloadKnowledgebaseFileAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { correlationContext.CorrelationId, agentGuid, fileName, response })
            );
        }
    }

    #endregion

    #region VISION IMAGES

    /// <inheritdoc />
    public async Task CreateAndProcessAiVisionImagesKeywordsAsync(
        AgentDataDomain agentData,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodStart,
                nameof(CreateAndProcessAiVisionImagesKeywordsAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { correlationContext.CorrelationId, agentData.AgentId })
            );

            if (agentData.VisionImages is null || !agentData.VisionImages.Any() || string.IsNullOrEmpty(this.AllowedAiVisionImagesFileFormats))
                return;

            DocumentHandlerService.ValidateUploadedFiles(
                uploadedFiles: agentData.VisionImages,
                allowedFileFormats: this.AllowedAiVisionImagesFileFormats
            );

            foreach (var image in agentData.VisionImages)
            {
                if (image is null)
                    continue;

                // Upload to BLOB STORAGE
                var imageUrl = await blobStorageManager.UploadDocumentsToStorageAsync(
                    documentFile: image,
                    agentGuid: agentData.AgentId,
                    fileType: UploadedFileType.AiVisionImageDocument,
                    cancellationToken
                ).ConfigureAwait(false);

                if (string.IsNullOrEmpty(imageUrl))
                    continue;

                // Process the image to generate keywords data
                var processedImageKeywords = await visionProcessor.ReadDataFromImageWithComputerVisionAsync(
                    imageUrl,
                    cancellationToken
                ).ConfigureAwait(false);

                // Save the keywords to data object
                agentData.AiVisionImagesData.Add(new()
                {
                    ImageKeywords = processedImageKeywords,
                    ImageName = image.FileName,
                    ImageUrl = imageUrl,
                });
            }
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex,
                LoggingConstants.LogHelperMethodFailed,
                nameof(CreateAndProcessAiVisionImagesKeywordsAsync), DateTime.UtcNow, ex.Message
            );
            throw new AIAgentsBusinessException(
                message: ex.Message,
                correlationId: correlationContext.CorrelationId
            );
        }
        finally
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodEnd,
                nameof(CreateAndProcessAiVisionImagesKeywordsAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { correlationContext.CorrelationId, agentData.AgentId })
            );
        }
    }

    /// <inheritdoc />
    public async Task HandleAiVisionImagesDataUpdateAsync(
        AgentDataDomain updateDataDomain,
        AgentDataDomain existingAgent,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodStart,
                nameof(HandleAiVisionImagesDataUpdateAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { correlationContext.CorrelationId, updateDataDomain.AgentId })
            );

            // Start from existing stored image keywords
            var updatedStoredImagesKeywords = existingAgent.AiVisionImagesData?.ToList() ?? [];
            var hasChanges = false;

            // Step 1: Remove any existing documents whose names are in RemovedAiVisionImages
            if (updateDataDomain.RemovedAiVisionImages is not null && updateDataDomain.RemovedAiVisionImages.Any())
            {
                var removedSet = new HashSet<string>(updateDataDomain.RemovedAiVisionImages, StringComparer.OrdinalIgnoreCase);
                updatedStoredImagesKeywords = [.. updatedStoredImagesKeywords.Where(img => !removedSet.Contains(img!.ImageName))];
                hasChanges = true;
            }

            // Step 2: Process newly updated ai vision images
            if (updateDataDomain.VisionImages is not null && updateDataDomain.VisionImages.Any() && !string.IsNullOrEmpty(this.AllowedAiVisionImagesFileFormats))
            {
                DocumentHandlerService.ValidateUploadedFiles(
                    uploadedFiles: updateDataDomain.VisionImages,
                    allowedFileFormats: this.AllowedAiVisionImagesFileFormats
                );
                foreach (var image in updateDataDomain.VisionImages)
                {
                    if (image is null)
                        continue;

                    // Upload to BLOB STORAGE
                    var imageUrl = await blobStorageManager.UploadDocumentsToStorageAsync(
                        documentFile: image,
                        agentGuid: updateDataDomain.AgentId,
                        fileType: UploadedFileType.AiVisionImageDocument,
                        cancellationToken
                    ).ConfigureAwait(false);

                    // Process the image to generate keywords data
                    var processedImageKeywords = await visionProcessor.ReadDataFromImageWithComputerVisionAsync(
                        imageUrl,
                        cancellationToken
                    ).ConfigureAwait(false);

                    // Save the keywords to data object
                    updatedStoredImagesKeywords.Add(new()
                    {
                        ImageKeywords = processedImageKeywords,
                        ImageName = image.FileName,
                        ImageUrl = imageUrl,
                    });
                    hasChanges = true;
                }
            }

            // Step 3: Only persist changes if something actually changed
            if (hasChanges)
                // If all images were removed, set to AiVisionImagesData empty list, otherwise save the updated list
                updateDataDomain.AiVisionImagesData = updatedStoredImagesKeywords.Count != 0 ? updatedStoredImagesKeywords : [];
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex,
                LoggingConstants.LogHelperMethodFailed,
                nameof(HandleAiVisionImagesDataUpdateAsync), DateTime.UtcNow, ex.Message
            );
            throw new AIAgentsBusinessException(
                message: ex.Message,
                correlationId: correlationContext.CorrelationId
            );
        }
        finally
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodEnd,
                nameof(HandleAiVisionImagesDataUpdateAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { correlationContext.CorrelationId, updateDataDomain.AgentId })
            );
        }
    }

    #endregion
}
