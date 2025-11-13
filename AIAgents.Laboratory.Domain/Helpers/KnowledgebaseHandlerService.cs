using AIAgents.Laboratory.Domain.DomainEntities.AgentsEntities;

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
        if (agentData.KnowledgeBaseDocument is null)
            return;

        var allowedExtensions = new[] { ".docx", ".doc", ".pdf", ".xlsx", ".xls", ".txt", ".json" };
        var fileExtensions = Path.GetExtension(agentData.KnowledgeBaseDocument.FileName).ToLowerInvariant();
        if (!allowedExtensions.Contains(fileExtensions))
            throw new Exception("Invalid file type. Allowed types are: .docx, .doc, .pdf, .xlsx, .xls, .txt, .json");
    }

    /// <summary>
    /// Process the knowledge base document data async.
    /// </summary>
    /// <param name="agentData">The agent data.</param>
    /// <returns>A task to wait on.</returns>
    internal static async Task ProcessKnowledgebaseDocumentDataAsync(this AgentDataDomain agentData)
    {
        if (agentData.KnowledgeBaseDocument is null)
            return;

        using var memoryStream = new MemoryStream();
        await agentData.KnowledgeBaseDocument.CopyToAsync(memoryStream).ConfigureAwait(false);
        agentData.StoredKnowledgeBase = new KnowledgeBaseDocumentDomain
        {
            FileName = agentData.KnowledgeBaseDocument.FileName,
            ContentType = agentData.KnowledgeBaseDocument.ContentType,
            FileContent = memoryStream.ToArray(),
            FileSize = agentData.KnowledgeBaseDocument.Length,
            UploadDate = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Converts the stored knowledge base binary data to IFormFile.
    /// </summary>
    internal static void ConvertKnowledgebaseBinaryDataToFile(this AgentDataDomain agentData)
    {
        if (agentData.StoredKnowledgeBase?.FileContent is null)
            return;

        var stream = new MemoryStream(agentData.StoredKnowledgeBase.FileContent);
        agentData.KnowledgeBaseDocument = new FormFileImplementation(stream, agentData.StoredKnowledgeBase.FileContent.Length, agentData.StoredKnowledgeBase.FileName, agentData.StoredKnowledgeBase.FileName);
    }
}