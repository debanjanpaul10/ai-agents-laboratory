namespace AIAgents.Laboratory.Domain.Helpers;

/// <summary>
/// The AI Agents Business Exception Class.
/// </summary>
/// <seealso cref="Exception" />
public sealed class AIAgentsBusinessException : Exception
{
    /// <summary>
    /// Gets or sets the status code.
    /// </summary>
    /// <value>
    /// The status code.
    /// </value>
    public int StatusCode { get; set; }

    /// <summary>
    /// Gets or sets the message.
    /// </summary>
    /// <value>
    /// The message.
    /// </value>
    public string? ExceptionMessage { get; set; }

    /// <summary>
    /// Gets or sets the details.
    /// </summary>
    /// <value>
    /// The details.
    /// </value>
    public string? Details { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AIAgentsBusinessException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    public AIAgentsBusinessException(string? message) : base(message)
    {
        this.ExceptionMessage = message;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AIAgentsBusinessException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="statusCode">The status code.</param>
    /// <param name="details">The details.</param>
    public AIAgentsBusinessException(string? message, int statusCode, string? details) : base(message)
    {
        this.ExceptionMessage = message;
        this.StatusCode = statusCode;
        this.Details = details;
    }
}
