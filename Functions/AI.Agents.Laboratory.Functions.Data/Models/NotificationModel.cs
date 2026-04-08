using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AI.Agents.Laboratory.Functions.Data.Models;

public sealed class NotificationModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public string RecipientUserName { get; set; } = string.Empty;

    public string NotificationType { get; set; } = string.Empty;

    public string CreatedBy { get; set; } = string.Empty;

    public bool IsGlobal { get; set; }

    public DateTime CreatedOnUtc { get; set; } = DateTime.UtcNow;
}

