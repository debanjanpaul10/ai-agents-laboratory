using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AIAgents.Laboratory.Domain.DomainEntities;

/// <summary>
/// The conversation history domain.
/// </summary>
[BsonIgnoreExtraElements]
public sealed record ConversationHistoryDomain
{
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>
    /// The identifier.
    /// </value>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// The conversation id guid.
    /// </summary>
    public string ConversationId { get; set; } = string.Empty;

    /// <summary>
    /// The user name.
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// The Chat history domain.
    /// </summary>
    public List<ChatHistoryDomain> ChatHistory { get; set; } = [];

    /// <summary>
    /// The is active boolean flag.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// The last modified on date.
    /// </summary>
    public DateTime LastModifiedOn { get; set; }
}
