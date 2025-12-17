using AIAgents.Laboratory.Domain.DomainEntities.AgentsEntities;
using Microsoft.AspNetCore.Http;

namespace AIAgents.Laboratory.Domain.Helpers;

/// <summary>
/// The Knowledge base Handler Service.
/// </summary>
internal static class KnowledgebaseHandlerService
{
    /// <summary>
    /// Validates that the uploaded knowledge base document in the specified agent data has an allowed file extension.
    /// </summary>
    /// <param name="agentData">The agent data containing the knowledge base document to validate. If the document is null, no validation is performed.</param>
    internal static void ValidateUploadedFile(this AgentDataDomain agentData)
    {
        if (agentData.KnowledgeBaseDocument is null || !agentData.KnowledgeBaseDocument.Any()) return;

        var allowedExtensions = new[] { ".docx", ".doc", ".pdf", ".xlsx", ".xls", ".txt", ".json" };
        foreach (var file in agentData.KnowledgeBaseDocument)
        {
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(fileExtension))
                throw new Exception("Invalid file type. Allowed types are: .docx, .doc, .pdf, .xlsx, .xls, .txt, .json");
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

        using var memoryStream = new MemoryStream();
        var knowledgeBaseFiles = new List<KnowledgeBaseDocumentDomain>();
        foreach (var file in agentData.KnowledgeBaseDocument)
        {
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
}