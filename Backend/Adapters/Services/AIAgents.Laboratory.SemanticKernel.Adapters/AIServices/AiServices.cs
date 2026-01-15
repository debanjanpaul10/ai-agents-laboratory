using System.Text.Json;
using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.Domain.DomainEntities.AgentsEntities;
using AIAgents.Laboratory.Domain.DrivenPorts;
using AIAgents.Laboratory.Domain.DrivingPorts;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using static AIAgents.Laboratory.Domain.Helpers.ApplicationPluginsHelpers;
using static AIAgents.Laboratory.SemanticKernel.Adapters.Helpers.Constants;

namespace AIAgents.Laboratory.SemanticKernel.Adapters.AIServices;

/// <summary>
/// Provides AI-related services, including invoking AI plugin functions and generating chatbot responses.
/// </summary>
/// <remarks>This class serves as a central point for interacting with AI capabilities, such as executing plugin
/// functions and handling chatbot conversations. It is intended to be used as a service within applications that require AI integration. All public methods are asynchronous and thread-safe.</remarks>
/// <param name="logger">The logger used to record diagnostic and operational information for the service.</param>
/// <param name="kernel">The kernel instance used to access AI plugins and services.</param>
/// <param name="mcpClientServices">The mcp client services.</param>
/// <seealso cref="IAiServices"/>
public class AiServices(ILogger<AiServices> logger, Kernel kernel, IMcpClientServices mcpClientServices) : IAiServices
{
    /// <summary>
    /// The json options
    /// </summary>
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    /// <summary>
    /// Gets the ai function response asynchronous.
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    /// <param name="input">The input.</param>
    /// <param name="pluginName">Name of the plugin.</param>
    /// <param name="functionName">Name of the function.</param>
    /// <returns>
    /// The AI response.
    /// </returns>
    public async Task<string> GetAiFunctionResponseAsync<TInput>(TInput input, string pluginName, string functionName)
    {
        try
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(GetAiFunctionResponseAsync), DateTime.UtcNow);
            var kernelArguments = new KernelArguments()
            {
                [ArgumentsConstants.KernelArgumentsInputConstant] = JsonSerializer.Serialize(input)
            };

            var responseFromAI = await kernel.InvokeAsync(pluginName, functionName, kernelArguments).ConfigureAwait(false);
            return responseFromAI.GetValue<string>() ?? string.Empty;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, LoggingConstants.LogHelperMethodFailed, nameof(GetAiFunctionResponseAsync), DateTime.UtcNow, ex.Message);
            throw;
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetAiFunctionResponseAsync), DateTime.UtcNow);
        }
    }

    /// <summary>
    /// Gets the ai function response with MCP integration asynchronous.
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    /// <param name="input">The input.</param>
    /// <param name="mcpServerUrl">The MCP server URL.</param>
    /// <param name="pluginName">Name of the plugin.</param>
    /// <param name="functionName">Name of the function.</param>
    /// <returns>The AI response.</returns>
    public async Task<string> GetAiFunctionResponseWithMcpIntegrationAsync<TInput>(TInput input, string mcpServerUrl, string pluginName, string functionName)
    {
        try
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(GetAiFunctionResponseWithMcpIntegrationAsync), DateTime.UtcNow);

            // Step 1: Get available MCP tools
            var availableMcpTools = await mcpClientServices.GetAllMcpToolsAsync(mcpServerUrl).ConfigureAwait(false);

            // Step 2: Ask LLM to determine which tool to call (if any)
            var kernelArguments = new KernelArguments()
            {
                [ArgumentsConstants.KernelArgumentsInputConstant] = JsonSerializer.Serialize(input),
                [ArgumentsConstants.AvailableMcpToolsArgument] = availableMcpTools
            };

            var responseFromAI = await kernel.InvokeAsync(PluginName, DetermineToolToCallFunction.FunctionName, kernelArguments).ConfigureAwait(false);
            var toolSelectionResultResponse = responseFromAI.GetValue<string>() ?? throw new Exception(ExceptionConstants.ToolsNotFoundExceptionConstant);
            var toolSelectionResult = JsonSerializer.Deserialize<ToolSelectionResultDomain>(toolSelectionResultResponse, JsonOptions) ?? throw new Exception();

            // Step 3: If a tool was selected, call it
            var toolResult = string.Empty;
            if (!string.IsNullOrEmpty(toolSelectionResult.ToolName))
                toolResult = await mcpClientServices.GetMcpToolResponseAsync(mcpServerUrl, toolSelectionResult.ToolName, toolSelectionResult.ToolArguments).ConfigureAwait(false);

            // Step 4: Generate final response with tool result (if available)
            var finalKernelArguments = new KernelArguments()
            {
                [ArgumentsConstants.KernelArgumentsInputConstant] = JsonSerializer.Serialize(input),
                [ArgumentsConstants.ToolResultArgument] = toolResult
            };

            var finalResponseFromAI = await kernel.InvokeAsync(PluginName, GenerateFinalResponseWithToolResultFunction.FunctionName, finalKernelArguments).ConfigureAwait(false);
            return finalResponseFromAI.GetValue<string>() ?? string.Empty;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, LoggingConstants.LogHelperMethodFailed, nameof(GetAiFunctionResponseWithMcpIntegrationAsync), DateTime.UtcNow, ex.Message);
            throw;
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetAiFunctionResponseWithMcpIntegrationAsync), DateTime.UtcNow);
        }
    }

    /// <summary>
    /// Gets the chatbot response.
    /// </summary>
    /// <param name="userMessage">The user message data.</param>
    /// <param name="conversationDataDomain">The conversation history data domain.</param>
    /// <param name="agentPrompt">The agent prompt.</param>
    /// <returns>The AI chatbot response.</returns>
    public async Task<string> GetChatbotResponseAsync(ConversationHistoryDomain conversationDataDomain, string userMessage, string agentPrompt)
    {
        try
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(GetChatbotResponseAsync), DateTime.UtcNow);

            var chatHistory = new ChatHistory();
            chatHistory.AddSystemMessage(agentPrompt);
            if (conversationDataDomain.ChatHistory is not null)
            {
                foreach (var message in conversationDataDomain.ChatHistory)
                {
                    if (message.Role.Equals(ArgumentsConstants.UserRoleConstant, StringComparison.OrdinalIgnoreCase))
                        chatHistory.AddUserMessage(message.Content);
                    else if (message.Role.Equals(ArgumentsConstants.AssistantRoleConstant, StringComparison.OrdinalIgnoreCase))
                        chatHistory.AddAssistantMessage(message.Content);
                }
            }

            var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
            var response = await chatCompletionService.GetChatMessageContentAsync(chatHistory).ConfigureAwait(false);
            return response.Content ?? string.Empty;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, LoggingConstants.LogHelperMethodFailed, nameof(GetChatbotResponseAsync), DateTime.UtcNow, ex.Message);
            throw;
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetChatbotResponseAsync), DateTime.UtcNow);
        }
    }
}
