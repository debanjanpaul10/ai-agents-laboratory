// *********************************************************************************
//	<copyright file="ChatbotPlugins.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>The Chatbot Plugins.</summary>
// *********************************************************************************

using AIAgents.Laboratory.SemanticKernel.Adapters.Utilities;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using System.ComponentModel;
using static AIAgents.Laboratory.Domain.Helpers.PluginHelpers.ChatBotPlugins;
using static AIAgents.Laboratory.SemanticKernel.Adapters.Helpers.Constants;

namespace AIAgents.Laboratory.SemanticKernel.Adapters.Plugins.FitGymTool;

/// <summary>
/// The Chatbot Plugins.
/// </summary>
/// <param name="httpClient">The http client.</param>
/// <param name="logger">The logger service.</param>
public class ChatbotPlugins(IHttpClientHelper httpClient, ILogger<ChatbotPlugins> logger)
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
		var sqlKnowledgebaseTask = GetFitGymToolMetadataAsync(ExternalApiRouteConstants.FitGymToolAPI.SqlKnowledgeBaseSql_ApiRoute);
		var databaseSchemaTask = GetFitGymToolMetadataAsync(ExternalApiRouteConstants.FitGymToolAPI.DatabaseSchemaSql_ApiRoute);
		await Task.WhenAll(sqlKnowledgebaseTask, databaseSchemaTask).ConfigureAwait(false);

		var arguments = new KernelArguments()
		{
			{ ArgumentsConstants.KernelArgumentsInputConstant, input },
			{ ArgumentsConstants.KnowledgeBaseInputConstant, sqlKnowledgebaseTask.Result },
			{ ArgumentsConstants.DatabaseSchemaInputConstant, databaseSchemaTask.Result },
		};
		var aiResponse = await kernel.InvokePromptAsync(NLToSqlSkillFunction.FunctionInstructions, arguments).ConfigureAwait(false);
		return aiResponse.GetValue<string>() ?? string.Empty;
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
		return "I am unable to process this response right now, please try something else";
		string knowledgeBase = await GetFitGymToolMetadataAsync(ExternalApiRouteConstants.FitGymToolAPI.RagKnowledgeBase_ApiRoute).ConfigureAwait(false);
		var arguments = new KernelArguments()
		{
			{ ArgumentsConstants.KernelArgumentsInputConstant, input },
			{ ArgumentsConstants.KnowledgeBaseInputConstant, knowledgeBase },
		};

		var result = await kernel.InvokePromptAsync(RAGTextSkillFunction.FunctionInstructions, arguments).ConfigureAwait(false);
		return result.GetValue<string>() ?? string.Empty;
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

	#region PRIVATE METHODS

	/// <summary>
	/// Gets the fit gym tool metadata asynchronous.
	/// </summary>
	/// <param name="apiUrl">The API URL.</param>
	/// <returns>The response.</returns>
	private async Task<string> GetFitGymToolMetadataAsync(string apiUrl)
	{
		try
		{
			logger.LogInformation(string.Format(LoggingConstants.LogHelperMethodStart, nameof(GetFitGymToolMetadataAsync), DateTime.UtcNow, apiUrl));
			var response = await httpClient.GetFitGymToolApiResponseAsync(apiUrl).ConfigureAwait(false);
			return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
		}
		catch (HttpRequestException ex)
		{
			logger.LogError(ex, string.Format(LoggingConstants.LogHelperMethodFailed, nameof(GetFitGymToolMetadataAsync), DateTime.UtcNow, apiUrl));
			throw;
		}
		finally
		{
			logger.LogInformation(string.Format(LoggingConstants.LogHelperMethodEnd, nameof(GetFitGymToolMetadataAsync), DateTime.UtcNow, apiUrl));
		}
	}

	#endregion
}
