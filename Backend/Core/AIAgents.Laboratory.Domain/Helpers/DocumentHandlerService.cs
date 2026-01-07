using AIAgents.Laboratory.Domain.DomainEntities.AgentsEntities;
using AIAgents.Laboratory.Processor.Models;
using Microsoft.AspNetCore.Http;

namespace AIAgents.Laboratory.Domain.Helpers;

/// <summary>
/// The Document Handler Service.
/// </summary>
internal static class DocumentHandlerService
{
    #region KNOWLEDGE BASE

    /// <summary>
    /// Validates that the uploaded knowledge base document in the specified agent data has an allowed file extension.
    /// </summary>
    /// <param name="agentData">The agent data containing the knowledge base document to validate. If the document is null, no validation is performed.</param>
    internal static void ValidateUploadedFiles(this AgentDataDomain agentData)
    {
        if (agentData.KnowledgeBaseDocument is null || !agentData.KnowledgeBaseDocument.Any()) return;

        var allowedExtensions = new[] { ".docx", ".doc", ".pdf", ".xlsx", ".xls", ".txt", ".json" };
        foreach (var file in agentData.KnowledgeBaseDocument)
        {
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(fileExtension))
                throw new FileFormatException("Invalid file type. Allowed types are: .docx, .doc, .pdf, .xlsx, .xls, .txt, .json");
        }
    }

    /// <summary>
    /// Process the knowledge base document data async.
    /// </summary>
    /// <param name="agentData">The agent data.</param>
    /// <returns>A task to wait on.</returns>
    internal static async Task ProcessKnowledgebaseDocumentDataAsync(this AgentDataDomain agentData)
    {
        if (agentData.KnowledgeBaseDocument is null || !agentData.KnowledgeBaseDocument.Any()) return;

        var knowledgeBaseFiles = new List<KnowledgeBaseDocumentDomain>();
        foreach (var file in agentData.KnowledgeBaseDocument)
        {
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream).ConfigureAwait(false);
            knowledgeBaseFiles.Add(new KnowledgeBaseDocumentDomain
            {
                FileName = file.FileName,
                ContentType = file.ContentType,
                FileContent = memoryStream.ToArray(),
                FileSize = file.Length,
                UploadDate = DateTime.UtcNow
            });
        }

        agentData.StoredKnowledgeBase = knowledgeBaseFiles;
    }

    /// <summary>
    /// Converts the stored knowledge base binary data to IFormFile.
    /// </summary>
    internal static void ConvertKnowledgebaseBinaryDataToFile(this AgentDataDomain agentData)
    {
        if (agentData.StoredKnowledgeBase is null || !agentData.StoredKnowledgeBase.Any()) return;

        var knowledgeBaseFiles = new List<IFormFile>();
        foreach (var file in agentData.StoredKnowledgeBase)
        {
            var stream = new MemoryStream(file.FileContent);
            knowledgeBaseFiles.Add(new FormFileImplementation(stream, file.FileContent.Length, file.FileName, file.FileName));
        }

        agentData.KnowledgeBaseDocument = knowledgeBaseFiles;
    }

    #endregion

    #region VISION IMAGES

    /// <summary>
    /// Validates that the uploaded ai vision images in the specified agent data has an allowed file extension.
    /// </summary>
    /// <param name="agentData">The agent data containing the ai vision images to validate. If the images are null, no validation is performed.</param>
    internal static void ValidateUploadedImages(this AgentDataDomain agentData)
    {
        if (agentData.VisionImages is null || !agentData.VisionImages.Any()) return;

        var allowedExtensions = new[] { ".png", ".jpg", ".jpeg", ".svg" };
        foreach (var image in agentData.VisionImages)
        {
            if (image is null) continue;

            var fileExtension = Path.GetExtension(image.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(fileExtension))
                throw new FileFormatException("Invalid file type. Allowed types are: .png, .jpeg, .jpg, .svg");
        }
    }

    #endregion
}