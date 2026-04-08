namespace AI.Agents.Laboratory.Functions.Shared.Models;

/// <summary>
/// The <c>NotificationRequest</c> class represents a request to send a notification to a user or group of users.
/// </summary>
public sealed record NotificationRequest
{
    /// <summary>
    /// Gets or sets the notification title.
    /// </summary>
    /// <value>
    /// The notification title.
    /// </value>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the notification message content.
    /// </summary>
    /// <value>
    /// The notification message content.
    /// </value>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the recipient user identifier.
    /// </summary>
    /// <value>
    /// The recipient user identifier.
    /// </value>
    public string RecipientUserName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the notification type.
    /// </summary>
    /// <value>
    /// The notification type (e.g., email, in-app, push).
    /// </value>
    public string NotificationType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the created by.
    /// </summary>
    /// <value>
    /// The created by user identifier.
    /// </value>
    public string CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether this notification is global (visible to all users) or user-specific.
    /// </summary>
    public bool IsGlobal { get; set; }
}
