using AIAgents.Laboratory.Domain.DomainEntities.AgentsEntities;
using AIAgents.Laboratory.Processor.Models;
using Microsoft.AspNetCore.Http;

namespace AIAgents.Laboratory.Domain.Helpers;

/// <summary>
/// The Document Handler Service.
/// </summary>
internal static class DocumentHandlerService
{
    /// <summary>
    /// Validates that the uploaded files in the specified agent data has an allowed file extension.
    /// </summary>
    /// <param name="uploadedFiles">The uploaded files</param>
    /// <param name="allowedFileFormats">The allowed file formats.</param>
    internal static void ValidateUploadedFiles(IList<IFormFile> uploadedFiles, string allowedFileFormats)
    {
        if (uploadedFiles is null || !uploadedFiles.Any() || string.IsNullOrEmpty(allowedFileFormats)) return;

        var allowedExtensions = allowedFileFormats.Split(",");
        foreach (var file in uploadedFiles)
        {
            if (file is null) continue;

            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(fileExtension))
                throw new FileFormatException($"Invalid file type. Allowed types are: {allowedFileFormats}");
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
}