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
    /// Gets or sets the unique identifier used to correlate related operations or requests.
    /// </summary>
    /// <remarks>Use this property to track and associate actions across distributed systems or components.
    /// The value should be unique for each logical operation to ensure accurate correlation.</remarks>
    public string CorrelationId { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AIAgentsBusinessException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="correlationId">The correlation id.</param>
    public AIAgentsBusinessException(string? message, string correlationId) : base(message)
    {
        this.ExceptionMessage = message;
        this.CorrelationId = correlationId;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AIAgentsBusinessException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="statusCode">The status code.</param>
    /// <param name="details">The details.</param>
    /// <param name="correlationId">The correlation id.</param>
    public AIAgentsBusinessException(string? message, int statusCode, string? details, string correlationId) : base(message)
    {
        this.ExceptionMessage = message;
        this.StatusCode = statusCode;
        this.Details = details;
        this.CorrelationId = correlationId;
    }
}
