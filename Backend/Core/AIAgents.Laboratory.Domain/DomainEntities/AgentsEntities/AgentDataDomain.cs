using AIAgents.Laboratory.Processor.Models;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AIAgents.Laboratory.Domain.DomainEntities.AgentsEntities;

/// <summary>
/// The Agent Data Domain model.
/// </summary>
[BsonIgnoreExtraElements]
public sealed record AgentDataDomain
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
    /// Gets or sets the agent identifier.
    /// </summary>
    /// <value>
    /// The agent identifier.
    /// </value>
    public string AgentId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name of the agent.
    /// </summary>
    /// <value>
    /// The name of the agent.
    /// </value>
    public string AgentName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the agent meta prompt.
    /// </summary>
    /// <value>
    /// The agent meta prompt.
    /// </value>
    public string AgentMetaPrompt { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name of the application.
    /// </summary>
    /// <value>
    /// The name of the application.
    /// </value>
    public string ApplicationName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the isactive boolean flag.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Gets or sets the created by identifier.
    /// </summary>
    public string CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date created.
    /// </summary>
    public DateTime DateCreated { get; set; }

    /// <summary>
    /// Gets or sets the knowledge base document.
    /// </summary>
    /// <value>
    /// The knowledge base document.
    /// </value>
    [BsonIgnore]
    public IList<IFormFile>? KnowledgeBaseDocument { get; set; } = [];

    /// <summary>
    /// Gets or sets the list of knowledge base documents to remove by file name (used for updates only).
    /// </summary>
    [BsonIgnore]
    public IList<string> RemovedKnowledgeBaseDocuments { get; set; } = [];

    /// <summary>
    /// Gets or sets the stored knowledge base document.
    /// </summary>
    /// <value>
    /// The knowledge base document domain.
    /// </value>
    public IList<KnowledgeBaseDocumentDomain> StoredKnowledgeBase { get; set; } = [];

    /// <summary>
    /// Gets or sets the URL of the MCP server used for network communication.
    /// </summary>
    /// <value>
    /// The MCP server URL.
    /// </value>
    public string? McpServerUrl { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the is private boolean flag.
    /// </summary>
    /// <value>
    /// The boolean flag for private agent.
    /// </value>
    public bool IsPrivate { get; set; } = false;

    /// <summary>
    /// Gets or sets the is default chatbot boolean flag.
    /// </summary>
    /// <value>
    /// The boolean flag for is default chat bot.
    /// </value>
    public bool IsDefaultChatbot { get; set; } = false;
}
