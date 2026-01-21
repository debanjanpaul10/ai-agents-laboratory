using System.Globalization;
using System.Text.Json;
using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.Domain.DomainEntities.SkillsEntities;
using AIAgents.Laboratory.Domain.DrivenPorts;
using AIAgents.Laboratory.Domain.DrivingPorts;
using AIAgents.Laboratory.Domain.Helpers;
using Microsoft.Extensions.Logging;
using static AIAgents.Laboratory.Domain.Helpers.Constants;

namespace AIAgents.Laboratory.Domain.UseCases;

/// <summary>
/// The AI Chatbot Skills Service class.
/// </summary>
/// <param name="aiAgentServices">The AI agent services.</param>
/// <param name="logger">The logger service.</param>
/// <seealso cref="AIAgents.Laboratory.Domain.DrivingPorts.IChatbotSkillsService" />
public sealed class ChatbotSkillsService(ILogger<ChatbotSkillsService> logger, IAiServices aiAgentServices) : IChatbotSkillsService
{
    /// <summary>
    /// Detects the user intent asynchronous.
    /// </summary>
    /// <param name="userQueryRequest">The user query request.</param>
    /// <returns>
    /// The intent string.
    /// </returns>
    public async Task<string> DetectUserIntentAsync(UserRequestDomain userQueryRequest)
    {
        try
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodStart, nameof(DetectUserIntentAsync), DateTime.UtcNow, userQueryRequest.UserQuery));
            ArgumentException.ThrowIfNullOrWhiteSpace(userQueryRequest.UserQuery);
            return await aiAgentServices.GetAiFunctionResponseAsync(userQueryRequest.UserQuery, ChatbotPluginHelpers.PluginName, ChatbotPluginHelpers.DetermineUserIntentFunction.FunctionName).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogError(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(DetectUserIntentAsync), DateTime.UtcNow, ex.Message));
            throw;
        }
        finally
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodEnd, nameof(DetectUserIntentAsync), DateTime.UtcNow, userQueryRequest.UserQuery));
        }
    }

    /// <summary>
    /// Gets the list of followup questions.
    /// </summary>
    /// <param name="followupQuestionsRequest">The followup questions request.</param>
    /// <returns>The list of followup questions.</returns>
    public async Task<IEnumerable<string>> GetFollowupQuestionsResponseAsync(FollowupQuestionsRequestDomain followupQuestionsRequest)
    {
        try
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodStart, nameof(GetFollowupQuestionsResponseAsync), DateTime.UtcNow, followupQuestionsRequest.UserQuery));
            if (followupQuestionsRequest is null || string.IsNullOrEmpty(followupQuestionsRequest.UserQuery))
            {
                var exception = new Exception(ExceptionConstants.InputParametersCannotBeEmptyMessage);
                logger.LogError(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(GetFollowupQuestionsResponseAsync), DateTime.UtcNow, exception.Message));
                throw exception;
            }

            var response = await aiAgentServices.GetAiFunctionResponseAsync(followupQuestionsRequest, ChatbotPluginHelpers.PluginName, ChatbotPluginHelpers.GenerateFollowupQuestionsFunction.FunctionName).ConfigureAwait(false);
            var cleanedResponse = DomainUtilities.ExtractJsonFromMarkdown(response ?? string.Empty);
            return JsonSerializer.Deserialize<IEnumerable<string>>(cleanedResponse ?? "[]") ?? [];
        }
        catch (Exception ex)
        {
            logger.LogError(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(GetFollowupQuestionsResponseAsync), DateTime.UtcNow, ex.Message));
            throw;
        }
        finally
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodEnd, nameof(GetFollowupQuestionsResponseAsync), DateTime.UtcNow, followupQuestionsRequest.UserQuery));
        }
    }

    /// <summary>
    /// Gets the SQL query markdown response asynchronous.
    /// </summary>
    /// <param name="input">The input.</param>
    /// <returns>
    /// The formatted AI response.
    /// </returns>
    public async Task<string> GetSQLQueryMarkdownResponseAsync(string input)
    {
        try
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodStart, nameof(GetSQLQueryMarkdownResponseAsync), DateTime.UtcNow, string.Empty));
            ArgumentException.ThrowIfNullOrWhiteSpace(input);

            return await aiAgentServices.GetAiFunctionResponseAsync(input, ChatbotPluginHelpers.PluginName, ChatbotPluginHelpers.SQLQueryMarkdownResponseFunction.FunctionName).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogError(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(GetSQLQueryMarkdownResponseAsync), DateTime.UtcNow, ex.Message));
            throw;
        }
        finally
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodEnd, nameof(GetSQLQueryMarkdownResponseAsync), DateTime.UtcNow, string.Empty));
        }
    }

    /// <summary>
    /// Handles the rag text response asynchronous.
    /// </summary>
    /// <param name="skillsInput">The skills input.</param>
    /// <returns>The AI generated response.</returns>
    public async Task<string> HandleRAGTextResponseAsync(SkillsInputDomain skillsInput)
    {
        try
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodStart, nameof(HandleRAGTextResponseAsync), DateTime.UtcNow, string.Empty));

            ArgumentException.ThrowIfNullOrWhiteSpace(skillsInput.UserQuery);
            if (string.IsNullOrEmpty(skillsInput.KnowledgeBase))
            {
                return ExceptionConstants.CannotProcessUserQueryMessage;
            }

            return await aiAgentServices.GetAiFunctionResponseAsync(skillsInput, ChatbotPluginHelpers.PluginName, ChatbotPluginHelpers.RAGTextSkillFunction.FunctionName).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogError(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(HandleRAGTextResponseAsync), DateTime.UtcNow, ex.Message));
            throw;
        }
        finally
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodEnd, nameof(HandleRAGTextResponseAsync), DateTime.UtcNow, string.Empty));
        }
    }

    /// <summary>
    /// Handles the nl to SQL response asynchronous.
    /// </summary>
    /// <param name="nltosqlInput">The nltosql input.</param>
    /// <returns>
    /// The ai generated response.
    /// </returns>
    public async Task<string> HandleNLToSQLResponseAsync(NltosqlInputDomain nltosqlInput)
    {
        try
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodStart, nameof(HandleNLToSQLResponseAsync), DateTime.UtcNow, string.Empty));

            ArgumentException.ThrowIfNullOrWhiteSpace(nltosqlInput.UserQuery);
            if (string.IsNullOrEmpty(nltosqlInput.DatabaseSchema) || string.IsNullOrEmpty(nltosqlInput.KnowledgeBase))
            {
                return ExceptionConstants.CannotProcessUserQueryMessage;
            }

            return await aiAgentServices.GetAiFunctionResponseAsync(nltosqlInput, ChatbotPluginHelpers.PluginName, ChatbotPluginHelpers.NLToSqlSkillFunction.FunctionName).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogError(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(HandleNLToSQLResponseAsync), DateTime.UtcNow, ex.Message));
            throw;
        }
        finally
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodEnd, nameof(HandleNLToSQLResponseAsync), DateTime.UtcNow, string.Empty));
        }
    }

    /// <summary>
    /// Handles the user greeting intent asynchronous.
    /// </summary>
    /// <returns>
    /// The greeting from ai agent.
    /// </returns>
    public async Task<string> HandleUserGreetingIntentAsync()
    {
        try
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodStart, nameof(HandleUserGreetingIntentAsync), DateTime.UtcNow, string.Empty));
            return await aiAgentServices.GetAiFunctionResponseAsync(string.Empty, ChatbotPluginHelpers.PluginName, ChatbotPluginHelpers.GreetingFunction.FunctionName).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogError(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(HandleUserGreetingIntentAsync), DateTime.UtcNow, ex.Message));
            throw;
        }
        finally
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodEnd, nameof(HandleUserGreetingIntentAsync), DateTime.UtcNow, string.Empty));
        }
    }
}
