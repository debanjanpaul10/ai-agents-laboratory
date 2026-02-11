namespace AIAgents.Laboratory.Domain.DrivenPorts;

/// <summary>
/// The Email Notification Service interface.
/// </summary>
public interface IEmailNotificationService
{
    /// <summary>
    /// Sends an email notification asynchronously.
    /// </summary>
    /// <param name="subject">The email subject.</param>
    /// <param name="content">The email content.</param>
    /// <param name="recipient">The email recipient.</param>
    /// <returns>The boolean for success/failure.</returns>
    Task<bool> SendEmailNotificationAsync(string subject, string content, string recipient);
}
