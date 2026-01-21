using AIAgents.Laboratory.API.Adapters.Models.Request;

namespace AIAgents.Laboratory.API.Adapters.Models.Response;

/// <summary>
/// The Agent Data DTO model.
/// </summary>
/// <seealso cref="CreateAgentDTO" />
public sealed record AgentDataDTO : CreateAgentDTO
{
    /// <summary>
    /// Gets or sets the agent identifier.
    /// </summary>
    /// <value>
    /// The agent identifier.
    /// </value>
    public string AgentId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the created by.
    /// </summary>
    /// <value>
    /// The created by.
    /// </value>
    public string CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date created on.
    /// </summary>
    /// <value>
    /// The date created.
    /// </value>
    public DateTime DateCreated { get; set; }

    /// <summary>
    /// Gets or sets the date modified.
    /// </summary>
    /// <value>The date modified.</value>
    public DateTime DateModified { get; set; }

    /// <summary>
    /// Gets or sets the is default chatbot boolean flag.
    /// </summary>
    /// <value>
    /// The boolean flag for is default chat bot.
    /// </value>
    public bool IsDefaultChatbot { get; set; } = false;

    /// <summary>
    /// Gets or sets the AI Vision images data.
    /// </summary>
    /// <value>
    ///     The AI Vision images data value.
    /// </value>
    public IList<AiVisionImagesDataDTO?> AiVisionImagesData { get; set; } = [];
}
