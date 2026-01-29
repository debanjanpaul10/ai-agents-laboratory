namespace AIAgents.Laboratory.API.Helpers;

/// <summary>
/// The AI Agents Exception Class.
/// </summary>
/// <seealso cref="Exception" />
public class AIAgentsException : Exception
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
	/// Initializes a new instance of the <see cref="AIAgentsException"/> class.
	/// </summary>
	/// <param name="message">The message.</param>
	public AIAgentsException(string? message) : base(message)
	{
		ExceptionMessage = message;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="AIAgentsException"/> class.
	/// </summary>
	/// <param name="message">The message.</param>
	/// <param name="statusCode">The status code.</param>
	/// <param name="details">The details.</param>
	public AIAgentsException(string? message, int statusCode, string? details) : base(message)
	{
		ExceptionMessage = message;
		StatusCode = statusCode;
		Details = details;
	}
}
