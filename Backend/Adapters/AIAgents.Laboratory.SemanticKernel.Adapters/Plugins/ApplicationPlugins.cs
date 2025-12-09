using System.ComponentModel;
using System.Globalization;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using ModelContextProtocol.Client;
using static AIAgents.Laboratory.Domain.Helpers.ApplicationPluginsHelpers;
using static AIAgents.Laboratory.SemanticKernel.Adapters.Helpers.Constants;

namespace AIAgents.Laboratory.SemanticKernel.Adapters.Plugins;

/// <summary>
/// The application plugins class containing plugins for individual applications.
/// </summary>
/// <param name="logger">The logger service.</param>
public class ApplicationPlugins(ILogger<ApplicationPlugins> logger)
{
    #region IBBS

    /// <summary>
    /// Executes the generate tag for story plugin asynchronous.
    /// </summary>
    /// <param name="kernel">The kernel.</param>
    /// <param name="input">The input.</param>
    /// <returns>The AI response.</returns>
    [KernelFunction(name: GenerateGenreTagForStoryFunction.FunctionName)]
    [Description(description: GenerateGenreTagForStoryFunction.FunctionDescription)]
    public static async Task<string> ExecuteGenerateTagForStoryPluginAsync(Kernel kernel, [Description(GenerateGenreTagForStoryFunction.InputDescription)] string input)
    {
        var arguments = new KernelArguments
        {
            { ArgumentsConstants.KernelArgumentsInputConstant, input }
        };

        var result = await kernel.InvokePromptAsync(GenerateGenreTagForStoryFunction.FunctionInstructions, arguments).ConfigureAwait(false);
        return result.GetValue<string>() ?? string.Empty;
    }

    /// <summary>
    /// Executes the content moderation plugin.
    /// </summary>
    /// <param name="kernel">The kernel.</param>
    /// <param name="input">The input.</param>
    /// <returns>The AI response.</returns>
    [KernelFunction(name: ContentModerationFunction.FunctionName)]
    [Description(description: ContentModerationFunction.FunctionDescription)]
    public static async Task<string> ExecuteContentModerationPlugin(Kernel kernel, [Description(ContentModerationFunction.InputDescription)] string input)
    {
        var arguments = new KernelArguments
        {
            { ArgumentsConstants.KernelArgumentsInputConstant, input }
        };

        var result = await kernel.InvokePromptAsync(ContentModerationFunction.FunctionInstructions, arguments).ConfigureAwait(false);
        return result.GetValue<string>() ?? string.Empty;
    }

    /// <summary>
    /// Executes rewrite user story async.
    /// </summary>
    /// <param name="kernel">The kernel.</param>
    /// <param name="input">The input.</param>
    [KernelFunction(RewriteUserStoryFunction.FunctionName)]
    [Description(RewriteUserStoryFunction.FunctionDescription)]
    public static async Task<string> ExecuteRewriteUserStoryAsync(Kernel kernel, [Description(RewriteUserStoryFunction.InputDescription)] string input)
    {
        var arguments = new KernelArguments
        {
            { ArgumentsConstants.KernelArgumentsInputConstant, input }
        };

        var result = await kernel.InvokePromptAsync(RewriteUserStoryFunction.FunctionInstructions, arguments).ConfigureAwait(false);
        return result.GetValue<string>() ?? string.Empty;
    }

    #endregion

    /// <summary>
    /// Determines the bug severity asynchronous.
    /// </summary>
    /// <param name="kernel">The kernel.</param>
    /// <param name="input">The input.</param>
    /// <returns>The ai prompt response.</returns>
    [KernelFunction(DetermineBugSeverityFunction.FunctionName)]
    [Description(DetermineBugSeverityFunction.FunctionDescription)]
    public static async Task<string> DetermineBugSeverityAsync(Kernel kernel, [Description(DetermineBugSeverityFunction.InputDescription)] string input)
    {
        var arguments = new KernelArguments()
        {
            { ArgumentsConstants.KernelArgumentsInputConstant, input }
        };

        var result = await kernel.InvokePromptAsync(DetermineBugSeverityFunction.FunctionInstructions, arguments).ConfigureAwait(false);
        return result.GetValue<string>() ?? string.Empty;
    }

    /// <summary>
    /// Gets the chat message response asynchronous.
    /// </summary>
    /// <param name="kernel">The kernel.</param>
    /// <param name="input">The input.</param>
    /// <returns>The AI response.</returns>
    [KernelFunction(GetChatMessageResponseFunction.FunctionName)]
    [Description(GetChatMessageResponseFunction.FunctionDescription)]
    public async Task<string> GetChatMessageResponseAsync(Kernel kernel, [Description(GetChatMessageResponseFunction.InputDescription)] string input)
    {
        try
        {
            var arguments = new KernelArguments()
            {
                { ArgumentsConstants.KernelArgumentsInputConstant, input },
            };

            var result = await kernel.InvokePromptAsync(GetChatMessageResponseFunction.FunctionInstructions, arguments).ConfigureAwait(false);
            return result.GetValue<string>() ?? string.Empty;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(GetChatMessageResponseAsync), DateTime.UtcNow, ex.Message));
            return ExceptionConstants.DefaultAIExceptionMessage;
        }
    }

    /// <summary>
    /// Determines the tool to call asynchronous.
    /// </summary>
    /// <param name="kernel">The kernel.</param>
    /// <param name="input">The input.</param>
    /// <param name="availableMcpTools">The available tools.</param>
    /// <returns>The AI response.</returns>
    [KernelFunction(DetermineToolToCallFunction.FunctionName)]
    [Description(DetermineToolToCallFunction.FunctionDescription)]
    public async Task<string> DetermineToolToCallAsync(
        Kernel kernel,
        [Description(DetermineToolToCallFunction.InputDescriptions.UserInput)] string input,
        [Description(DetermineToolToCallFunction.InputDescriptions.ListOfTools)] IEnumerable<McpClientTool> availableMcpTools)
    {
        try
        {
            var toolDescriptions = string.Join("\n", availableMcpTools.Select(t => $"- {t.Name}: {t.Description}"));
            var prompt = DetermineToolToCallFunction.GetFunctionInstructions(toolDescriptions, input);

            var kernelArguments = new KernelArguments
            {
                [ArgumentsConstants.KernelArgumentsInputConstant] = prompt
            };

            var result = await kernel.InvokePromptAsync(prompt, kernelArguments).ConfigureAwait(false);
            return result.GetValue<string>() ?? string.Empty;
        }
        catch (JsonException ex)
        {
            logger.LogError(ex, string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(DetermineToolToCallAsync), DateTime.UtcNow, ex.Message));
            return ExceptionConstants.DefaultAIExceptionMessage;
        }
    }

    /// <summary>
    /// Generates the final response using the tool result.
    /// </summary>
    /// <param name="input">The user input.</param>
    /// <param name="toolResult">The tool result (if any).</param>
    /// <returns>The final AI response.</returns>
    [KernelFunction(GenerateFinalResponseWithToolResultFunction.FunctionName)]
    [Description(GenerateFinalResponseWithToolResultFunction.FunctionDescription)]
    public async Task<string> GenerateFinalResponseWithToolResultAsync(
        Kernel kernel,
        [Description(GenerateFinalResponseWithToolResultFunction.InputDescriptions.UserInput)] string input,
        [Description(GenerateFinalResponseWithToolResultFunction.InputDescriptions.ToolResult)] string? toolResult)
    {
        try
        {
            var prompt = GenerateFinalResponseWithToolResultFunction.GetFunctionInstructions(input, toolResult);
            var kernelArguments = new KernelArguments
            {
                [ArgumentsConstants.KernelArgumentsInputConstant] = prompt
            };

            var result = await kernel.InvokePromptAsync(prompt, kernelArguments).ConfigureAwait(false);
            return result.GetValue<string>() ?? string.Empty;
        }
        catch (JsonException ex)
        {
            logger.LogError(ex, LoggingConstants.LogHelperMethodFailed, nameof(GenerateFinalResponseWithToolResultAsync), DateTime.UtcNow, ex.Message);
            return ExceptionConstants.DefaultAIExceptionMessage;
        }
    }
}
