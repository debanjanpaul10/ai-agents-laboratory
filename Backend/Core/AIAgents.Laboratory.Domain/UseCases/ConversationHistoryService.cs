using AIAgents.Laboratory.Domain.Contracts;
using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.Domain.Helpers;
using AIAgents.Laboratory.Domain.Ports.Out;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using static AIAgents.Laboratory.Domain.Helpers.Constants;

namespace AIAgents.Laboratory.Domain.UseCases;

/// <summary>
/// Provides services for managing and persisting conversation history for chat users.
/// </summary>
/// <remarks>This service enables retrieval, storage, and clearing of conversation history for individual users.
/// It relies on MongoDB for data persistence and supports correlation tracking for distributed operations. All operations are asynchronous and log relevant events for monitoring and troubleshooting.</remarks>
/// <param name="logger">The logger used for recording diagnostic and operational information.</param>
/// <param name="correlationContext">The correlation context used for tracking request and operation correlation across services.</param>
/// <param name="conversationHistoryDataManager">The conversation history data manager used for data access and persistence operations.</param>
/// <seealso cref="IConversationHistoryService"/>
public sealed class ConversationHistoryService(
    ILogger<ConversationHistoryService> logger,
    ICorrelationContext correlationContext,
    IConversationHistoryDataManager conversationHistoryDataManager) : IConversationHistoryService
{
    /// <summary>
    /// Gets the conversation history data for current chat.
    /// </summary>
    /// <param name="userName">The user name.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The conversation history domain.</returns>
    public async Task<ConversationHistoryDomain> GetConversationHistoryAsync(
        string userName,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userName);
        ConversationHistoryDomain? result = null;
        try
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodStart,
                nameof(GetConversationHistoryAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { correlationContext.CorrelationId, userName })
            );

            result = await conversationHistoryDataManager.GetConversationHistoryAsync(
                userName,
                cancellationToken
            ).ConfigureAwait(false);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex,
                LoggingConstants.LogHelperMethodFailed,
                nameof(GetConversationHistoryAsync), DateTime.UtcNow, ex.Message
            );
            throw new AIAgentsBusinessException(
                message: ex.Message,
                correlationId: correlationContext.CorrelationId
            );
        }
        finally
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodEnd,
                nameof(GetConversationHistoryAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { correlationContext.CorrelationId, userName, result })
            );
        }
    }

    /// <summary>
    /// Saves the current message data to conversation history.
    /// </summary>
    /// <param name="conversationHistory">The conversation history.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The boolean for success/failure.</returns>
    public async Task<bool> SaveMessageToConversationHistoryAsync(
        ConversationHistoryDomain conversationHistory,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(conversationHistory);
        bool result = false;
        try
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodStart,
                nameof(SaveMessageToConversationHistoryAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { correlationContext.CorrelationId, conversationHistory.ConversationId })
            );

            result = await conversationHistoryDataManager.SaveMessageToConversationHistoryAsync(
                conversationHistory,
                cancellationToken
            ).ConfigureAwait(false);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex,
                LoggingConstants.LogHelperMethodFailed,
                nameof(SaveMessageToConversationHistoryAsync), DateTime.UtcNow, ex.Message
            );
            throw new AIAgentsBusinessException(
                message: ex.Message,
                correlationId: correlationContext.CorrelationId
            );
        }
        finally
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodEnd,
                nameof(SaveMessageToConversationHistoryAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { correlationContext.CorrelationId, conversationHistory.ConversationId, result })
            );
        }
    }

    /// <summary>
    /// Clears the conversation history data for the user.
    /// </summary>
    /// <param name="userName">The user name for user.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The boolean for success/failure.</returns>
    public async Task<bool> ClearConversationHistoryForUserAsync(
        string userName,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userName);
        bool result = false;
        try
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodStart,
                nameof(ClearConversationHistoryForUserAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { correlationContext.CorrelationId, userName })
            );

            result = await conversationHistoryDataManager.ClearConversationHistoryForUserAsync(
                userName,
                cancellationToken
            ).ConfigureAwait(false);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex,
                LoggingConstants.LogHelperMethodFailed,
                nameof(ClearConversationHistoryForUserAsync), DateTime.UtcNow, ex.Message
            );
            throw new AIAgentsBusinessException(
                message: ex.Message,
                correlationId: correlationContext.CorrelationId
            );
        }
        finally
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodEnd,
                nameof(ClearConversationHistoryForUserAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { correlationContext.CorrelationId, userName, result })
            );
        }
    }
}
