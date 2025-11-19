using System.ComponentModel;
using System.Globalization;
using System.Text.Json;
using AIAgents.Laboratory.Domain.DomainEntities.SkillsEntities;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using static AIAgents.Laboratory.Domain.Helpers.ChatbotPluginHelpers;
using static AIAgents.Laboratory.SemanticKernel.Adapters.Helpers.Constants;

namespace AIAgents.Laboratory.SemanticKernel.Adapters.Plugins;

/// <summary>
/// The Chatbot Plugins.
/// </summary>
/// <param name="logger">The logger service.</param>
public class ChatbotPlugins(ILogger<ChatbotPlugins> logger)
{
    /// <summary>
    /// Detects the user intent function.
    /// </summary>
    /// <param name="kernel">The semantic kernel.</param>
    /// <param name="input">The input.</param>
    /// <returns>The AI response.</returns>
    [KernelFunction(DetermineUserIntentFunction.FunctionName)]
    [Description(DetermineUserIntentFunction.FunctionDescription)]
    public static async Task<string> DetectUserIntentFunctionAsync(Kernel kernel, [Description(DetermineUserIntentFunction.InputDescription)] string input)
    {
        var arguments = new KernelArguments()
        {
            { ArgumentsConstants.KernelArgumentsInputConstant, input }
        };

        var result = await kernel.InvokePromptAsync(DetermineUserIntentFunction.FunctionInstructions, arguments).ConfigureAwait(false);
        return result.GetValue<string>() ?? string.Empty;
    }

    /// <summary>
    /// Users the greeting function.
    /// </summary>
    /// <param name="kernel">The semantic kernel.</param>
    /// <param name="input">The input.</param>
    /// <returns>The AI response.</returns>
    [KernelFunction(GreetingFunction.FunctionName)]
    [Description(GreetingFunction.FunctionDescription)]
    public static async Task<string> UserGreetingFunctionAsync(Kernel kernel, [Description(GreetingFunction.InputDescription)] string input)
    {
        var arguments = new KernelArguments()
        {
            { ArgumentsConstants.KernelArgumentsInputConstant, input }
        };

        var result = await kernel.InvokePromptAsync(GreetingFunction.FunctionInstructions, arguments).ConfigureAwait(false);
        return result.GetValue<string>() ?? string.Empty;
    }

    /// <summary>
    /// Natural Language to SQL skill function.
    /// </summary>
    /// <param name="kernel">The semantic kernel.</param>
    /// <param name="input">The input.</param>
    /// <returns>The AI response.</returns>
    [KernelFunction(NLToSqlSkillFunction.FunctionName)]
    [Description(NLToSqlSkillFunction.FunctionDescription)]
    public async Task<string> NLToSQLSkillFunctionAsync(Kernel kernel, [Description(NLToSqlSkillFunction.InputDescription)] string input)
    {
        try
        {
            var nltosqlRequest = JsonSerializer.Deserialize<NltosqlInputDomain>(input) ?? throw new Exception(); ;
            var arguments = new KernelArguments()
            {
                { ArgumentsConstants.KernelArgumentsInputConstant, nltosqlRequest.UserQuery },
                { ArgumentsConstants.KnowledgeBaseInputConstant, nltosqlRequest.KnowledgeBase },
                { ArgumentsConstants.DatabaseSchemaInputConstant, nltosqlRequest.DatabaseSchema },
            };
            var aiResponse = await kernel.InvokePromptAsync(NLToSqlSkillFunction.FunctionInstructions, arguments).ConfigureAwait(false);
            return aiResponse.GetValue<string>() ?? string.Empty;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(NLToSQLSkillFunctionAsync), DateTime.UtcNow, ex.Message));
            return ExceptionConstants.DefaultAIExceptionMessage;
        }
    }

    /// <summary>
    /// The RAG text skill function.
    /// </summary>
    /// <param name="kernel">The semantic kernel.</param>
    /// <param name="input">The input.</param>
    /// <returns>The AI response.</returns>
    [KernelFunction(RAGTextSkillFunction.FunctionName)]
    [Description(RAGTextSkillFunction.FunctionDescription)]
    public async Task<string> RAGTextSkillFunctionAsync(Kernel kernel, [Description(RAGTextSkillFunction.InputDescription)] string input)
    {
        try
        {
            var ragTextSkillInput = JsonSerializer.Deserialize<SkillsInputDomain>(input) ?? throw new Exception();
            var arguments = new KernelArguments()
            {
                { ArgumentsConstants.KernelArgumentsInputConstant, ragTextSkillInput.UserQuery },
                { ArgumentsConstants.KnowledgeBaseInputConstant, ragTextSkillInput.KnowledgeBase },
            };

            var result = await kernel.InvokePromptAsync(RAGTextSkillFunction.FunctionInstructions, arguments).ConfigureAwait(false);
            return result.GetValue<string>() ?? string.Empty;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(RAGTextSkillFunctionAsync), DateTime.UtcNow, ex.Message));
            return ExceptionConstants.DefaultAIExceptionMessage;
        }
    }

    /// <summary>
    /// SQLs the query markdown response function asynchronous.
    /// </summary>
    /// <param name="kernel">The kernel.</param>
    /// <param name="input">The input.</param>
    /// <returns>The AI response.</returns>
    [KernelFunction(SQLQueryMarkdownResponseFunction.FunctionName)]
    [Description(SQLQueryMarkdownResponseFunction.FunctionDescription)]
    public static async Task<string> SQLQueryMarkdownResponseFunctionAsync(Kernel kernel, [Description(SQLQueryMarkdownResponseFunction.InputDescription)] string input)
    {
        var arguments = new KernelArguments()
        {
            { ArgumentsConstants.SQLJsonInputConstant, input },
        };

        var result = await kernel.InvokePromptAsync(SQLQueryMarkdownResponseFunction.FunctionInstructions, arguments).ConfigureAwait(false);
        return result.GetValue<string>() ?? string.Empty;
    }

    /// <summary>
    /// Generates the followup questions function asynchronous.
    /// </summary>
    /// <param name="kernel">The kernel.</param>
    /// <param name="input">The input data as JSON string.</param>
    /// <returns>The ai response.</returns>
    [KernelFunction(GenerateFollowupQuestionsFunction.FunctionName)]
    [Description(GenerateFollowupQuestionsFunction.FunctionDescription)]
    public async Task<string> GenerateFollowupQuestionsFunctionAsync(Kernel kernel, [Description(GenerateFollowupQuestionsFunction.InputDescription)] string input)
    {
        try
        {
            var followupQuestionsRequest = JsonSerializer.Deserialize<FollowupQuestionsRequestDomain>(input) ?? throw new Exception();
            var arguments = new KernelArguments()
            {
                { ArgumentsConstants.UserQueryInputConstant, followupQuestionsRequest.UserQuery },
                { ArgumentsConstants.UserIntentInputConstant, followupQuestionsRequest.UserIntent },
                { ArgumentsConstants.AIResponseInputConstant, followupQuestionsRequest.AiResponseData }
            };

            var result = await kernel.InvokePromptAsync(GenerateFollowupQuestionsFunction.FunctionInstructions, arguments).ConfigureAwait(false);
            return result.GetValue<string>() ?? string.Empty;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(GenerateFollowupQuestionsFunctionAsync), DateTime.UtcNow, ex.Message));
            return ExceptionConstants.DefaultAIExceptionMessage;
        }
    }
}
