using AIAgents.Laboratory.Domain.DomainEntities.AgentsEntities;
using MongoDB.Driver;

namespace AIAgents.Laboratory.Domain.DrivingPorts;

/// <summary>
/// Defines methods for processing and updating an agent's knowledge base documents and AI Vision images, including handling uploads, removals, and preparing update definitions for persistence.
/// </summary>
/// <remarks>Implementations of this interface are responsible for validating, processing, and preparing updates
/// to an agent's knowledge base and AI Vision images. The interface supports asynchronous operations for handling document and image data, and is intended to be used in scenarios where agent data must be updated and persisted in
/// response to user actions or system events. Callers are responsible for applying the generated update definitions to the underlying data store.</remarks>
public interface IDocumentIntelligenceService
{
    /// <summary>
    /// Processes updates to an agent's knowledge base documents, including adding new documents and removing specified ones, and prepares the corresponding update definitions for persistence.
    /// </summary>
    /// <remarks>This method only adds update definitions to the provided list if changes to the knowledge base are detected. It validates and processes any newly uploaded documents and ensures that removed documents
    /// are excluded from the persisted knowledge base. The caller is responsible for applying the accumulated updates to the data store.</remarks>
    /// <param name="updateDataDomain">The domain object containing the knowledge base update information, including any new or removed documents. Cannot be null.</param>
    /// <param name="updates">A list to which update definitions for the agent's knowledge base will be added if changes are detected. Cannot be null.</param>
    /// <param name="existingAgent">The current state of the agent's data, used as the baseline for applying knowledge base updates. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task HandleKnowledgeBaseDataUpdateAsync(AgentDataDomain updateDataDomain, List<UpdateDefinition<AgentDataDomain>> updates, AgentDataDomain existingAgent);

    /// <summary>
    /// Processes updates to an agent's AI Vision images, including adding new images and removing specified ones, and prepares the corresponding update definitions for persistence.
    /// </summary>
    /// <remarks>This method only adds update definitions to the provided list if changes to the images are detected. It validates and processes any newly uploaded images and ensures that removed images
    /// are excluded from the persisted AI Vision images store. The caller is responsible for applying the accumulated updates to the data store.</remarks>
    /// <param name="updateDataDomain">The domain object containing the images update information, including any new or removed images. Cannot be null.</param>
    /// <param name="updates">A list to which update definitions for the agent's AI Vision images will be added if changes are detected. Cannot be null.</param>
    /// <param name="existingAgent">The current state of the agent's data, used as the baseline for applying AI Vision images updates. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task HandleAiVisionImagesDataUpdateAsync(AgentDataDomain updateDataDomain, List<UpdateDefinition<AgentDataDomain>> updates, AgentDataDomain existingAgent);

    /// <summary>
    /// Creates and processes a knowledge base document for the specified agent asynchronously.
    /// </summary>
    /// <remarks>This method validates the uploaded files, processes the knowledge base document data, and
    /// then processes each stored knowledge base file for the agent. If no files are present, the method completes without processing any documents.</remarks>
    /// <param name="agentData">The agent data domain object containing information and files to be processed for the knowledge base. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task CreateAndProcessKnowledgeBaseDocumentAsync(AgentDataDomain agentData);

    /// <summary>
    /// Processes the vision images associated with the specified agent by uploading them to cloud storage, extracting keywords using AI vision processing, and updating the agent's data with the results.
    /// </summary>
    /// <remarks>This method uploads each image in the agent's vision images collection to cloud storage, analyzes the image to extract keywords using computer vision, and adds the resulting keywords and image
    /// information to the agent's data. The method logs progress and errors for monitoring purposes. If any image in the collection is null, it is skipped.</remarks>
    /// <param name="agentData">The agent data containing the vision images to process. Must not be null and must contain valid uploaded images.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task CreateAndProcessAiVisionImagesKeywordsAsync(AgentDataDomain agentData);

    /// <summary>
    /// Deletes the knowledge base documents and AI Vision images data associated with a specific agent from the storage.
    /// </summary>
    /// <remarks>
    /// This method invokes the blob storage manager to remove all folders and data related to the agent's documents.
    /// It logs the start and end of the operation, as well as any errors that occur during the process.
    /// </remarks>
    /// <param name="agentId">The unique identifier of the agent whose data is to be deleted. Cannot be null or empty.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    Task DeleteKnowledgebaseAndImagesDataAsync(string agentId);

    /// <summary>
    /// Downloads the knowledgebase file asynchronous.
    /// </summary>
    /// <param name="agentGuid">The agent guid id.</param>
    /// <param name="fileName">The file name.</param>
    /// <returns>The downloaded file url</returns>
    Task<string> DownloadKnowledgebaseFileAsync(string agentGuid, string fileName);
}
