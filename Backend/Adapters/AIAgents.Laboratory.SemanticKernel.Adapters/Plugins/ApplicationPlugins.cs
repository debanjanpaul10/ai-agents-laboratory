using System.ComponentModel;
using AIAgents.Laboratory.SemanticKernel.Adapters.Helpers;
using Microsoft.SemanticKernel;
using static AIAgents.Laboratory.Domain.Helpers.ApplicationPluginsHelpers;

namespace AIAgents.Laboratory.SemanticKernel.Adapters.Plugins;

/// <summary>
/// The application plugins class containing plugins for individual applications.
/// </summary>
public class ApplicationPlugins
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
            { Constants.ArgumentsConstants.KernelArgumentsInputConstant, input }
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
            { Constants.ArgumentsConstants.KernelArgumentsInputConstant, input }
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
            { Constants.ArgumentsConstants.KernelArgumentsInputConstant, input }
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
            { Constants.ArgumentsConstants.KernelArgumentsInputConstant, input }
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
    public static async Task<string> GetChatMessageResponseAsync(Kernel kernel, [Description(GetChatMessageResponseFunction.InputDescription)] string input)
    {
        var arguments = new KernelArguments()
        {
            { Constants.ArgumentsConstants.KernelArgumentsInputConstant, input }
        };

        var result = await kernel.InvokePromptAsync(GetChatMessageResponseFunction.FunctionInstructions, arguments).ConfigureAwait(false);
        return result.GetValue<string>() ?? string.Empty;
    }
}
