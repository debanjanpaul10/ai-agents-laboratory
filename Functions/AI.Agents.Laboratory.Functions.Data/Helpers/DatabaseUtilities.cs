using AI.Agents.Laboratory.Functions.Data.Models;
using AI.Agents.Laboratory.Functions.Shared.Models;

namespace AI.Agents.Laboratory.Functions.Data.Helpers;

/// <summary>
/// Helper class that provides utility methods for preparing data models for database operations.
/// </summary>
internal static class DatabaseUtilities
{
    /// <summary>
    /// Prepares a <c>NotificationModel</c> from a given NotificationRequest, mapping the relevant properties and setting the creation and modification timestamps. 
    /// This method is used to transform the incoming request data into a format suitable for persistence in the MongoDB database. 
    /// </summary>
    /// <param name="request">The notification request containing the details to be transformed into a NotificationModel.</param>
    /// <returns>A <c>NotificationModel</c> object populated with data from the request and additional metadata such as timestamps.</returns>
    internal static NotificationModel PrepareNotificationDataModel(
        NotificationRequest request
    ) =>
        new()
        {
            Id = request.Id,
            Title = request.Title,
            Message = request.Message,
            RecipientUserName = request.RecipientUserName,
            NotificationType = request.NotificationType,
            CreatedBy = request.CreatedBy,
            IsGlobal = request.IsGlobal,
            DateCreated = DateTime.UtcNow,
            DateModified = DateTime.UtcNow,
            ModifiedBy = request.CreatedBy,
            IsActive = true,
        };
}
