using System.Globalization;
using System.Text.Json;
using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.Domain.DrivenPorts;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using static AIAgents.Laboratory.SemanticKernel.Adapters.Helpers.Constants;

namespace AIAgents.Laboratory.SemanticKernel.Adapters.AIServices;

/// <summary>
/// The AI services Class.
/// </summary>
/// <param name="kernel">The Semantic Kernel.</param>
/// <param name="logger">The Logger service.</param>
/// <seealso cref="IAiServices" />
public class AiServices(ILogger<AiServices> logger, Kernel kernel) : IAiServices
{
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
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodStart, nameof(GetAiFunctionResponseAsync), DateTime.UtcNow));
            return await InvokePluginFunctionAsync(input, pluginName, functionName).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogError(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(GetAiFunctionResponseAsync), DateTime.UtcNow, ex.Message));
            throw;
        }
        finally
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodEnd, nameof(GetAiFunctionResponseAsync), DateTime.UtcNow));
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
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodStart, nameof(GetChatbotResponseAsync), DateTime.UtcNow));

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
            logger.LogError(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(GetChatbotResponseAsync), DateTime.UtcNow, ex.Message));
            throw;
        }
        finally
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodEnd, nameof(GetChatbotResponseAsync), DateTime.UtcNow));
        }
    }

    #region PRIVATE METHODS

    /// <summary>
    /// Invokes the plugin function asynchronous.
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    /// <param name="input">The input.</param>
    /// <param name="pluginName">Name of the plugin.</param>
    /// <param name="functionName">Name of the function.</param>
    /// <returns>The AI string response.</returns>
    private async Task<string> InvokePluginFunctionAsync<TInput>(TInput input, string pluginName, string functionName)
    {
        try
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodStart, nameof(InvokePluginFunctionAsync), DateTime.UtcNow));
            var kernelArguments = new KernelArguments()
            {
                [ArgumentsConstants.KernelArgumentsInputConstant] = JsonSerializer.Serialize(input)
            };

            var responseFromAI = await kernel.InvokeAsync(pluginName, functionName, kernelArguments).ConfigureAwait(false);
            return responseFromAI.GetValue<string>()!;
        }
        catch (Exception ex)
        {
            logger.LogError(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(InvokePluginFunctionAsync), DateTime.UtcNow, ex.Message));
            throw;
        }
        finally
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodEnd, nameof(InvokePluginFunctionAsync), DateTime.UtcNow));
        }
    }

    #endregion
}
