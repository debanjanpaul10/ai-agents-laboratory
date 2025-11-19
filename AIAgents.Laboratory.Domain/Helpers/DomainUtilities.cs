using AIAgents.Laboratory.Domain.DomainEntities.FeedbackEntities;

namespace AIAgents.Laboratory.Domain.Helpers;

/// <summary>
/// The domain utilities class.
/// </summary>
internal static class DomainUtilities
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
}
