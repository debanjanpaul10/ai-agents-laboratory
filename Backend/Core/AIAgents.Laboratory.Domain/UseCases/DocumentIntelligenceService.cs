using System.Globalization;
using AIAgents.Laboratory.Domain.DomainEntities.AgentsEntities;
using AIAgents.Laboratory.Domain.DrivenPorts;
using AIAgents.Laboratory.Domain.DrivingPorts;
using AIAgents.Laboratory.Domain.Helpers;
using AIAgents.Laboratory.Processor.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using static AIAgents.Laboratory.Domain.Helpers.Constants;

namespace AIAgents.Laboratory.Domain.UseCases;

/// <summary>
/// Provides services for processing and managing agent knowledge base documents and AI Vision images, including uploading, analyzing, and preparing updates for persistence.
/// </summary>
/// <remarks>This service coordinates the processing of agent-related documents and images, including validation,
/// content extraction, and update preparation. It is intended to be used as part of the agent data management workflow, and relies on injected dependencies for logging, document processing, image storage, and vision analysis.</remarks>
/// <param name="logger">The logger used to record informational and error messages for operations performed by the service. Cannot be null.</param>
/// <param name="configuration">The configuration service.</param>
/// <param name="knowledgeBaseProcessor">The processor responsible for reading and analyzing knowledge base documents. Cannot be null.</param>
/// <param name="blobStorageManager">The storage manager used to upload and manage images in cloud storage. Cannot be null.</param>
/// <param name="visionProcessor">The processor used to analyze images and extract keywords using AI vision capabilities. Cannot be null.</param>
/// <seealso cref="IDocumentIntelligenceService"/>
public class DocumentIntelligenceService(ILogger<DocumentIntelligenceService> logger, IConfiguration configuration, IKnowledgeBaseProcessor knowledgeBaseProcessor, IBlobStorageManager blobStorageManager, IVisionProcessor visionProcessor) : IDocumentIntelligenceService
{
    /// <summary>
    /// The configuration value for allowed knowledge base file formats.
    /// </summary>
    private readonly string AllowedKnowledgebaseFileFormats = configuration[AzureAppConfigurationConstants.AllowedKbFileFormatsConstant]!;

    /// <summary>
    /// The configuration value for allowed ai vision images file formats.
    /// </summary>
    private readonly string AllowedAiVisionImagesFileFormats = configuration[AzureAppConfigurationConstants.AllowedVisionImageFileFormatsConstant]!;

    #region KNOWLEDGE BASE

    /// <summary>
    /// Creates and processes a knowledge base document for the specified agent asynchronously.
    /// </summary>
    /// <remarks>This method validates the uploaded files, processes the knowledge base document data, and
    /// then processes each stored knowledge base file for the agent. If no files are present, the method completes without processing any documents.</remarks>
    /// <param name="agentData">The agent data domain object containing information and files to be processed for the knowledge base. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task CreateAndProcessKnowledgeBaseDocumentAsync(AgentDataDomain agentData)
    {
        try
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(CreateAndProcessKnowledgeBaseDocumentAsync), DateTime.UtcNow, agentData.AgentId);

            if (agentData.KnowledgeBaseDocument is null || !agentData.KnowledgeBaseDocument.Any() || string.IsNullOrEmpty(AllowedKnowledgebaseFileFormats)) return;
            DocumentHandlerService.ValidateUploadedFiles(agentData.KnowledgeBaseDocument, AllowedKnowledgebaseFileFormats);

            await agentData.ProcessKnowledgebaseDocumentDataAsync().ConfigureAwait(false);
            if (agentData.StoredKnowledgeBase is not null && agentData.StoredKnowledgeBase.Any())
            {
                foreach (var file in agentData.StoredKnowledgeBase)
                {
                    var content = knowledgeBaseProcessor.DetectAndReadFileContent(file);
                    await knowledgeBaseProcessor.ProcessKnowledgeBaseDocumentAsync(content, agentData.AgentId).ConfigureAwait(false);
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, LoggingConstants.LogHelperMethodFailed, nameof(CreateAndProcessKnowledgeBaseDocumentAsync), DateTime.UtcNow, ex.Message);
            throw;
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(CreateAndProcessKnowledgeBaseDocumentAsync), DateTime.UtcNow, agentData.AgentId);
        }
    }

    /// <summary>
    /// Processes updates to an agent's knowledge base documents, including adding new documents and removing specified ones, and prepares the corresponding update definitions for persistence.
    /// </summary>
    /// <remarks>This method only adds update definitions to the provided list if changes to the knowledge base are detected. It validates and processes any newly uploaded documents and ensures that removed documents
    /// are excluded from the persisted knowledge base. The caller is responsible for applying the accumulated updates to the data store.</remarks>
    /// <param name="updateDataDomain">The domain object containing the knowledge base update information, including any new or removed documents. Cannot be null.</param>
    /// <param name="updates">A list to which update definitions for the agent's knowledge base will be added if changes are detected. Cannot be null.</param>
    /// <param name="existingAgent">The current state of the agent's data, used as the baseline for applying knowledge base updates. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task HandleKnowledgeBaseDataUpdateAsync(AgentDataDomain updateDataDomain, List<UpdateDefinition<AgentDataDomain>> updates, AgentDataDomain existingAgent)
    {
        try
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodStart, nameof(HandleKnowledgeBaseDataUpdateAsync), DateTime.UtcNow, updateDataDomain.AgentId));

            // Start from existing stored knowledge base
            var updatedStoredKnowledgeBase = existingAgent.StoredKnowledgeBase?.ToList() ?? [];
            var hasChanges = false;

            // 1. Remove any existing documents whose names are in RemovedKnowledgeBaseDocuments
            if (updateDataDomain.RemovedKnowledgeBaseDocuments is not null && updateDataDomain.RemovedKnowledgeBaseDocuments.Any())
            {
                var removedSet = new HashSet<string>(updateDataDomain.RemovedKnowledgeBaseDocuments, StringComparer.OrdinalIgnoreCase);
                updatedStoredKnowledgeBase = [.. updatedStoredKnowledgeBase.Where(doc => !removedSet.Contains(doc.FileName))];
                hasChanges = true;
            }

            // 2. Process any newly uploaded knowledge base documents
            if (updateDataDomain.KnowledgeBaseDocument is not null && updateDataDomain.KnowledgeBaseDocument.Any() && !string.IsNullOrEmpty(AllowedKnowledgebaseFileFormats))
            {
                DocumentHandlerService.ValidateUploadedFiles(updateDataDomain.KnowledgeBaseDocument, AllowedKnowledgebaseFileFormats);
                await updateDataDomain.ProcessKnowledgebaseDocumentDataAsync().ConfigureAwait(false);
                if (updateDataDomain.StoredKnowledgeBase is not null && updateDataDomain.StoredKnowledgeBase.Any())
                {
                    foreach (var file in updateDataDomain.StoredKnowledgeBase)
                    {
                        var content = knowledgeBaseProcessor.DetectAndReadFileContent(file);
                        await knowledgeBaseProcessor.ProcessKnowledgeBaseDocumentAsync(content, updateDataDomain.AgentId).ConfigureAwait(false);
                    }

                    updatedStoredKnowledgeBase.AddRange(updateDataDomain.StoredKnowledgeBase);
                    hasChanges = true;
                }
            }

            // 3. Only persist changes if something actually changed
            if (hasChanges)
                // If all documents were removed, set StoredKnowledgeBase to null, otherwise save the updated list
                updates.Add(Builders<AgentDataDomain>.Update.Set(
                    x => x.StoredKnowledgeBase,
                    updatedStoredKnowledgeBase.Count != 0 ? updatedStoredKnowledgeBase : null));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, LoggingConstants.LogHelperMethodFailed, nameof(HandleKnowledgeBaseDataUpdateAsync), DateTime.UtcNow, ex.Message);
            throw;
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(HandleKnowledgeBaseDataUpdateAsync), DateTime.UtcNow, updateDataDomain.AgentId);
        }
    }

    #endregion

    #region VISION IMAGES

    /// <summary>
    /// Processes the vision images associated with the specified agent by uploading them to cloud storage, extracting keywords using AI vision processing, and updating the agent's data with the results.
    /// </summary>
    /// <remarks>This method uploads each image in the agent's vision images collection to cloud storage, analyzes the image to extract keywords using computer vision, and adds the resulting keywords and image
    /// information to the agent's data. The method logs progress and errors for monitoring purposes. If any image in the collection is null, it is skipped.</remarks>
    /// <param name="agentData">The agent data containing the vision images to process. Must not be null and must contain valid uploaded images.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task CreateAndProcessAiVisionImagesKeywordsAsync(AgentDataDomain agentData)
    {
        try
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(CreateAndProcessAiVisionImagesKeywordsAsync), DateTime.UtcNow, agentData.AgentId);

            if (agentData.VisionImages is null || !agentData.VisionImages.Any() || string.IsNullOrEmpty(AllowedAiVisionImagesFileFormats)) return;
            DocumentHandlerService.ValidateUploadedFiles(agentData.VisionImages, AllowedAiVisionImagesFileFormats);
            foreach (var image in agentData.VisionImages)
            {
                if (image is null) continue;

                // Upload to BLOB STORAGE
                var imageUrl = await blobStorageManager.UploadImageToStorageAsync(image, agentData.AgentId).ConfigureAwait(false);

                // Process the image to generate keywords data
                var processedImageKeywords = await visionProcessor.ReadDataFromImageWithComputerVisionAsync(imageUrl).ConfigureAwait(false);

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
            logger.LogError(ex, LoggingConstants.LogHelperMethodFailed, nameof(CreateAndProcessAiVisionImagesKeywordsAsync), DateTime.UtcNow, ex.Message);
            throw;
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodFailed, nameof(CreateAndProcessAiVisionImagesKeywordsAsync), DateTime.UtcNow, agentData.AgentId);
        }
    }

    /// <summary>
    /// Processes updates to an agent's AI Vision images, including adding new images and removing specified ones, and prepares the corresponding update definitions for persistence.
    /// </summary>
    /// <remarks>This method only adds update definitions to the provided list if changes to the images are detected. It validates and processes any newly uploaded images and ensures that removed images
    /// are excluded from the persisted AI Vision images store. The caller is responsible for applying the accumulated updates to the data store.</remarks>
    /// <param name="updateDataDomain">The domain object containing the images update information, including any new or removed images. Cannot be null.</param>
    /// <param name="updates">A list to which update definitions for the agent's AI Vision images will be added if changes are detected. Cannot be null.</param>
    /// <param name="existingAgent">The current state of the agent's data, used as the baseline for applying AI Vision images updates. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task HandleAiVisionImagesDataUpdateAsync(AgentDataDomain updateDataDomain, List<UpdateDefinition<AgentDataDomain>> updates, AgentDataDomain existingAgent)
    {
        try
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodStart, nameof(HandleAiVisionImagesDataUpdateAsync), DateTime.UtcNow, updateDataDomain.AgentId));

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
            if (updateDataDomain.VisionImages is not null && updateDataDomain.VisionImages.Any() && !string.IsNullOrEmpty(AllowedAiVisionImagesFileFormats))
            {
                DocumentHandlerService.ValidateUploadedFiles(updateDataDomain.VisionImages, AllowedAiVisionImagesFileFormats);
                foreach (var image in updateDataDomain.VisionImages)
                {
                    if (image is null) continue;

                    // Upload to BLOB STORAGE
                    var imageUrl = await blobStorageManager.UploadImageToStorageAsync(image, updateDataDomain.AgentId).ConfigureAwait(false);

                    // Process the image to generate keywords data
                    var processedImageKeywords = await visionProcessor.ReadDataFromImageWithComputerVisionAsync(imageUrl).ConfigureAwait(false);

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
                // If all images were removed, set to AiVisionImagesData null, otherwise save the updated list
                updates.Add(Builders<AgentDataDomain>.Update.Set(
                    x => x.AiVisionImagesData,
                    updatedStoredImagesKeywords.Count != 0 ? updatedStoredImagesKeywords : null));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(HandleAiVisionImagesDataUpdateAsync), DateTime.UtcNow, ex.Message));
            throw;
        }
        finally
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodEnd, nameof(HandleAiVisionImagesDataUpdateAsync), DateTime.UtcNow, updateDataDomain.AgentId));
        }
    }

    #endregion
}
