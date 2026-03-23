using AIAgents.Laboratory.Domain.Contracts;
using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.Domain.DomainEntities.AgentsEntities;
using AIAgents.Laboratory.Domain.DrivenPorts;
using AIAgents.Laboratory.Domain.Helpers;
using AIAgents.Laboratory.Infrastructure.AgentsFramework.Helpers;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using static AIAgents.Laboratory.Domain.Helpers.ApplicationPluginsHelpers;
using static AIAgents.Laboratory.Infrastructure.AgentsFramework.Helpers.Constants;

namespace AIAgents.Laboratory.Infrastructure.AgentsFramework.AgentServices;

/// <summary>
/// Provides AI-related services using Microsoft Agents Framework, including invoking AI agent functions and generating chatbot responses.
/// </summary>
/// <remarks>This class serves as a central point for interacting with AI capabilities using the Agents Framework, such as executing agent
/// functions and handling chatbot conversations. It is intended to be used as a service within applications that require AI integration. All public methods are asynchronous and thread-safe.</remarks>
/// <param name="logger">The logger used to record diagnostic and operational information for the service.</param>
/// <param name="correlationContext">The correlation context used to track and correlate logs and operations across different components and services during AI interactions.</param>
/// <param name="mcpClientServices">The mcp client services.</param>
/// <param name="chatClient">The chat client.</param>
/// <seealso cref="IAiServices"/>
public sealed class AgentFrameworkServices(ILogger<AgentFrameworkServices> logger, ICorrelationContext correlationContext, IMcpClientServices mcpClientServices, IChatClient chatClient) : IAiServices
{
    /// <summary>
    /// Gets the ai function aiResponse asynchronous.
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    /// <param name="input">The input.</param>
    /// <param name="pluginName">Name of the plugin.</param>
    /// <param name="functionName">Name of the function.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// The AI aiResponse.
    /// </returns>
    public async Task<string> GetAiFunctionResponseAsync<TInput>(TInput input, string pluginName, string functionName, CancellationToken cancellationToken = default)
    {
        AgentServiceHelpers.ValidateInputParameters(input, pluginName, functionName);

        string aiResponse = string.Empty;
        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(GetAiFunctionResponseAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, pluginName, functionName }));

            var chatMessages = new List<ChatMessage>
            {
                new(ChatRole.User, JsonConvert.SerializeObject(input)),
                new(ChatRole.System, GetChatMessageResponseFunction.FunctionInstructions)
            };

            var response = await chatClient.GetResponseAsync(
                messages: chatMessages, cancellationToken: cancellationToken).ConfigureAwait(false);
            aiResponse = response?.Text ?? ExceptionConstants.DefaultAIExceptionMessage;
            return aiResponse;
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(GetAiFunctionResponseAsync), DateTime.UtcNow, AgentServiceHelpers.SanitizeErrorMessage(ex.Message));
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetAiFunctionResponseAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, pluginName, functionName, aiResponse }));
        }
    }

    /// <summary>
    /// Gets the ai function aiResponse with MCP integration asynchronous.
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    /// <param name="input">The input.</param>
    /// <param name="mcpServerUrl">The MCP server URL.</param>
    /// <param name="pluginName">Name of the plugin.</param>
    /// <param name="functionName">Name of the function.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The AI aiResponse.</returns>
    public async Task<string> GetAiFunctionResponseAsync<TInput>(TInput input, string mcpServerUrl, string pluginName, string functionName, CancellationToken cancellationToken = default)
    {
        AgentServiceHelpers.ValidateInputParameters(input, pluginName, functionName);
        ArgumentException.ThrowIfNullOrWhiteSpace(mcpServerUrl);

        string aiResponse = string.Empty;
        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(GetAiFunctionResponseAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, pluginName, functionName }));

            var jsonInput = JsonConvert.SerializeObject(input);

            // STEP 1: Get all available MCP Tools
            var availableMcpTools = await mcpClientServices.GetAllMcpToolsAsync(
                mcpServerUrl, cancellationToken).ConfigureAwait(false);

            // STEP 2: Ask LLM to determine which tool to call (if any)
            var toolDescriptions = string.Join("\n", availableMcpTools.Select(t => $"- {t.Name}: {t.Description}"));
            var toolSelectionResultResponse = await this.GetChatMessageAiResponseAsync(
                prompt: DetermineToolToCallFunction.GetFunctionInstructions(toolDescriptions, jsonInput),
                input: jsonInput,
                cancellationToken).ConfigureAwait(false);

            var toolSelectionResult = JsonConvert.DeserializeObject<ToolSelectionResultDomain>(DomainUtilities.ExtractJsonFromMarkdown(toolSelectionResultResponse))
                ?? throw new JsonSerializationException(ExceptionConstants.InvalidJsonDeserializeExceptionMessage);

            // STEP 3: If a tool is selected, invoke the MCP tool and get the result
            var toolResult = string.Empty;
            if (!string.IsNullOrEmpty(toolSelectionResult.ToolName))
                toolResult = await mcpClientServices.GetMcpToolResponseAsync(
                    mcpServerUrl, toolName: toolSelectionResult.ToolName, toolArguments: toolSelectionResult.ToolArguments).ConfigureAwait(false);

            // STEP 4: Finally, call the AI function with the original input and the tool result (if any)
            aiResponse = await this.GetChatMessageAiResponseAsync(
                prompt: GenerateFinalResponseWithToolResultFunction.GetFunctionInstructions(jsonInput, toolResult),
                input: jsonInput,
                cancellationToken).ConfigureAwait(false);
            return aiResponse;
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(GetAiFunctionResponseAsync), DateTime.UtcNow, AgentServiceHelpers.SanitizeErrorMessage(ex.Message));
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetAiFunctionResponseAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, pluginName, functionName, aiResponse }));
        }
    }

    /// <summary>
    /// Gets the chatbot aiResponse.
    /// </summary>
    /// <param name="userMessage">The user message data.</param>
    /// <param name="conversationDataDomain">The conversation history data domain.</param>
    /// <param name="agentMetaPrompt">The agent meta prompt.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The AI chatbot aiResponse.</returns>
    public async Task<string> GetChatbotResponseAsync(ConversationHistoryDomain conversationDataDomain, string userMessage, string agentMetaPrompt, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userMessage);
        ArgumentException.ThrowIfNullOrWhiteSpace(agentMetaPrompt);

        string aiResponse = string.Empty;
        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(GetChatbotResponseAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, userMessage }));

            var chatMessages = new List<ChatMessage>
            {
                new(ChatRole.System, agentMetaPrompt),
                new(ChatRole.User, userMessage)
            };

            if (conversationDataDomain.ChatHistory is not null)
            {
                foreach (var message in conversationDataDomain.ChatHistory)
                {
                    if (message.Role.Equals(ArgumentsConstants.UserRoleConstant, StringComparison.OrdinalIgnoreCase))
                        chatMessages.Add(new(ChatRole.User, message.Content));
                    else if (message.Role.Equals(ArgumentsConstants.AssistantRoleConstant, StringComparison.OrdinalIgnoreCase))
                        chatMessages.Add(new(ChatRole.Assistant, message.Content));
                }
            }

            // Get aiResponse from AI
            var response = await chatClient.GetResponseAsync(
                messages: chatMessages, cancellationToken: cancellationToken).ConfigureAwait(false);
            aiResponse = response?.Text ?? string.Empty;
            return aiResponse;
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(GetChatbotResponseAsync), DateTime.UtcNow, AgentServiceHelpers.SanitizeErrorMessage(ex.Message));
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetChatbotResponseAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, userMessage, aiResponse }));
        }
    }

    #region PRIVATE METHODS

    /// <summary>
    /// Gets the chat message aiResponse asynchronous.
    /// </summary>
    /// <param name="prompt">The prompt to be sent to the LLM.</param>
    /// <param name="input">The input from user.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The string AI response.</returns>
    private async Task<string> GetChatMessageAiResponseAsync(string prompt, string input, CancellationToken cancellationToken = default)
    {
        string response = string.Empty;
        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(GetChatMessageAiResponseAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, input }));

            var chatMessages = new List<ChatMessage>
            {
                new(ChatRole.System, prompt),
                new(ChatRole.User, input)
            };

            var aiResponse = await chatClient.GetResponseAsync(
                messages: chatMessages, cancellationToken: cancellationToken).ConfigureAwait(false);
            response = aiResponse?.Text ?? ExceptionConstants.DefaultAIExceptionMessage;
            return response;
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(GetChatMessageAiResponseAsync), DateTime.UtcNow, AgentServiceHelpers.SanitizeErrorMessage(ex.Message));
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetChatMessageAiResponseAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, input, response }));
        }
    }

    #endregion
}

