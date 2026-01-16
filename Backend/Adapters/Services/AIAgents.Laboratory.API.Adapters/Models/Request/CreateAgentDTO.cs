using Microsoft.AspNetCore.Http;

namespace AIAgents.Laboratory.API.Adapters.Models.Request;

/// <summary>
/// The Create Agent Data DTO model.
/// </summary>
public record CreateAgentDTO
{
    /// <summary>
    /// Gets or sets the name of the agent.
    /// </summary>
    /// <value>
    /// The name of the agent.
    /// </value>
    public string AgentName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the agent description.
    /// </summary>
    /// <value>
    /// The value of agent description.
    /// </value>
    public string AgentDescription { get; set; } = string.Empty;

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
    /// Gets or sets the knowledge base document.
    /// </summary>
    /// <value>
    /// The knowledge base document.
    /// </value>
    public IEnumerable<IFormFile>? KnowledgeBaseDocument { get; set; } = [];

    /// <summary>
    /// Gets or sets the list of knowledge base documents to remove by file name (used for updates).
    /// </summary>
    public IList<string> RemovedKnowledgeBaseDocuments { get; set; } = [];

    /// <summary>
    /// Gets or sets the is private boolean flag.
    /// </summary>
    /// <value>
    /// The boolean flag for private agent.
    /// </value>
    public bool IsPrivate { get; set; } = false;

    /// <summary>
    /// Gets or sets the AI Vision images files.
    /// </summary>
    /// <value>
    ///     The AI Vision images files value.
    /// </value>
    public IEnumerable<IFormFile?> VisionImages { get; set; } = [];

    /// <summary>
    /// Gets or sets the removed AI vision images (To be used for updates only).
    /// </summary>
    /// <value>
    ///     The removed AI vision images value.
    /// </value>
    public IList<string> RemovedAiVisionImages { get; set; } = [];

    /// <summary>
    /// Gets or sets the associated skill guids.
    /// </summary>
    /// <value>The list of associated skill guids.</value>
    public IList<string> AssociatedSkillGuids { get; set; } = [];
}
