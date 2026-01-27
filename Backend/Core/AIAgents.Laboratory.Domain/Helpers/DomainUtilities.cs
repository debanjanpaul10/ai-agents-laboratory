using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.Domain.DomainEntities.AgentsEntities;
using AIAgents.Laboratory.Domain.DomainEntities.FeedbackEntities;

namespace AIAgents.Laboratory.Domain.Helpers;

/// <summary>
/// The domain utilities class.
/// </summary>
public static class DomainUtilities
{
    /// <summary>
    /// Prepares the bug report data domain entity before creation.
    /// </summary>
    /// <param name="bugReportDataDomain">The bug report data domain model.</param>
    internal static void PrepareBugReportDataDomain(this BugReportData bugReportDataDomain)
    {
        bugReportDataDomain.IsActive = true;
        bugReportDataDomain.DateCreated = DateTime.UtcNow;
        bugReportDataDomain.DateModified = DateTime.UtcNow;
        bugReportDataDomain.ModifiedBy = bugReportDataDomain.CreatedBy;
    }

    /// <summary>
    /// Prepares the new feature request data domain entity before creation.
    /// </summary>
    /// <param name="featureRequestDataDomain">The feature request data domain model.</param>
    internal static void PrepareNewFeatureRequestDataDomain(this NewFeatureRequestData featureRequestDataDomain)
    {
        featureRequestDataDomain.IsActive = true;
        featureRequestDataDomain.DateCreated = DateTime.UtcNow;
        featureRequestDataDomain.DateModified = DateTime.UtcNow;
        featureRequestDataDomain.ModifiedBy = featureRequestDataDomain.CreatedBy;
    }

    /// <summary>
    /// Prepares the audit entity data.
    /// </summary>
    /// <param name="entityModel">The entity data model.</param>
    /// <param name="currentUser">The current logged in user.</param>
    internal static void PrepareAuditEntityData(this BaseEntity entityModel, string currentUser)
    {
        entityModel.IsActive = true;
        entityModel.DateModified = DateTime.UtcNow;
        entityModel.ModifiedBy = currentUser;
        entityModel.DateCreated = DateTime.UtcNow;
        entityModel.CreatedBy = currentUser;
    }

    /// <summary>
    /// Processes and cleans the AI response to extract the JSON content.
    /// </summary>
    /// <param name="response">The raw AI response.</param>
    /// <returns>Cleaned JSON string.</returns>
    public static string ExtractJsonFromMarkdown(string response)
    {
        if (string.IsNullOrWhiteSpace(response)) return string.Empty;

        // Try to find the start of a JSON object or array
        var firstObject = response.IndexOf('{');
        var firstArray = response.IndexOf('[');

        var startIndex = -1;
        var endIndex = -1;

        if (firstObject != -1 && (firstArray == -1 || firstObject < firstArray))
        {
            startIndex = firstObject;
            endIndex = response.LastIndexOf('}');
        }
        else if (firstArray != -1)
        {
            startIndex = firstArray;
            endIndex = response.LastIndexOf(']');
        }

        if (startIndex != -1 && endIndex != -1 && endIndex > startIndex)
            return response.Substring(startIndex, endIndex - startIndex + 1);

        return response.Trim();
    }

    /// <summary>
    /// Checks if the agent data has any knowledge base content.
    /// </summary>
    /// <param name="agentData">The agent data domain model.</param>
    /// <returns>The boolean to indicate the KB content.</returns>
    internal static bool HasKnowledgeBaseContent(this AgentDataDomain agentData) => agentData?.KnowledgeBaseDocument?.Count > 0 || agentData?.StoredKnowledgeBase?.Count > 0;
}
