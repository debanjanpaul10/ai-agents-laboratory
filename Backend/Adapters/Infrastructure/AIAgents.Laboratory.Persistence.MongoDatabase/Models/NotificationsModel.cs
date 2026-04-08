using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AIAgents.Laboratory.Persistence.MongoDatabase.Models;

/// <summary>
/// The NotificationsModel class represents the structure of a notification document stored in the MongoDB database for the AIAgents Laboratory application.
/// </summary>
[BsonIgnoreExtraElements]
public sealed record NotificationsModel : BaseDataModel
{
    /// <summary>
    /// The unique identifier for the registered application, represented as an ObjectId in MongoDB.
    /// </summary>
    [BsonId]
    public ObjectId _id { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the registered application.
    /// </summary>
    /// <summary>
    /// The Tool skill id.
    /// </summary>
    [BsonElement("Id")]
    public int Id { get; set; }

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
    /// Gets or sets a value indicating whether this notification is global (visible to all users) or user-specific.
    /// </summary>
    public bool IsGlobal { get; set; }
}
