namespace AIAgents.Laboratory.Infrastructure.AgentsFramework.Helpers;

/// <summary>
/// The AI Constants Class.
/// </summary>
internal static class Constants
{
    /// <summary>
    /// The Arguments Constants.
    /// </summary>
    internal static class ArgumentsConstants
    {
        /// <summary>
        /// The agent arguments input constant.
        /// </summary>
        internal const string AgentArgumentsInputConstant = "input";

        /// <summary>
        /// The available MCP tools argument
        /// </summary>
        internal const string AvailableMcpToolsArgument = "availableMcpTools";

        /// <summary>
        /// The tool result argument
        /// </summary>
        internal const string ToolResultArgument = "toolResult";

        /// <summary>
        /// The user role constant.
        /// </summary>
        internal const string UserRoleConstant = "user";

        /// <summary>
        /// The assistant role constant.
        /// </summary>
        internal const string AssistantRoleConstant = "assistant";
    }

    /// <summary>
    /// Logging constants.
    /// </summary>
    internal static class LoggingConstants
    {
        /// <summary>
        /// The log helper method start.
        /// </summary>
        internal const string LogHelperMethodStart = "{0} started at {1} for {2}";

        /// <summary>
        /// The log helper method failed.
        /// </summary>
        internal const string LogHelperMethodFailed = "{0} failed at {1} with {2}";

        /// <summary>
        /// The log helper method end.
        /// </summary>
        internal const string LogHelperMethodEnd = "{0} ended at {1} for {2}";

        /// <summary>
        /// The agent function invocation log message.
        /// </summary>
        internal const string AgentFunctionInvocation = "Invoking agent function {0}.{1} using provider {2}";

        /// <summary>
        /// The chat completion request log message.
        /// </summary>
        internal const string ChatCompletionRequest = "Requesting chat completion from provider {0}";
    }

    /// <summary>
    /// The Exception Constants Class.
    /// </summary>
    internal static class ExceptionConstants
    {
        /// <summary>
        /// Something went wrong message
        /// </summary>
        internal const string SomethingWentWrongMessage = "Something went wrong while processing the request!";

        /// <summary>
        /// The default ai exception message
        /// </summary>
        internal const string DefaultAIExceptionMessage = "I am currently unable to access the required information to process this request. Please try again later!";

        /// <summary>
        /// The tools not found exception constant
        /// </summary>
        internal const string ToolsNotFoundExceptionConstant = "Oops! No such tools exist in the given MCP server";

        /// <summary>
        /// The invalid service provider message.
        /// </summary>
        internal const string InvalidServiceProvider = "Unsupported AI service provider: {0}";

        /// <summary>
        /// The ai api key missing message.
        /// </summary>
        internal const string AiAPIKeyMissingMessage = "The AI Api Key is missing in configuration.";

        /// <summary>
        /// The ai service provider missing message.
        /// </summary>
        internal const string CurrentAiServiceProviderMissingMessage = "The Current AI Service Provider is missing in configuration.";

        /// <summary>
        /// The invalid agent configuration exception message.
        /// </summary>
        internal const string InvalidAgentConfigurationExceptionMessage = "Invalid agent configuration: {0}";

        /// <summary>
        /// The invalid json format exception message.
        /// </summary>
        internal const string InvalidJsonDeserializeExceptionMessage = "Failed to deserialize the JSON";
    }

    /// <summary>
    /// The Configuration Constants class.
    /// </summary>
    internal static class ConfigurationConstants
    {
        /// <summary>
        /// The token scope format.
        /// </summary>
        internal const string TokenScopeFormat = "{0}/.default";

        /// <summary>
        /// The ai agents ad client identifier
        /// </summary>
        internal const string AiAgentsAdClientId = "AiAgentsClientId";

        /// <summary>
        /// The ai agents ad client secret
        /// </summary>
        internal const string AiAgentsAdClientSecret = "ClientSecret";

        /// <summary>
        /// The ai agents lab tenant identifier
        /// </summary>
        internal const string AiAgentsLabTenantId = "TenantId";

        /// <summary>
        /// The authorization constant
        /// </summary>
        internal const string AuthorizationConstant = "Authorization";

        /// <summary>
        /// The bearer token constant
        /// </summary>
        internal const string BearerTokenConstant = "Bearer {0}";
    }

    /// <summary>
    /// The Azure App Configuration Constants.
    /// </summary>
    internal static class AzureAppConfigurationConstants
    {
        /// <summary>
        /// The current ai service provider
        /// </summary>
        internal const string CurrentAiServiceProvider = "CurrentAiServiceProvider";
    }

    /// <summary>
    /// The Google Gemini AI constants.
    /// </summary>
    internal static class GoogleGeminiAiConstants
    {
        /// <summary>
        /// The service provider name
        /// </summary>
        internal const string ServiceProviderName = "GoogleGemini";

        /// <summary>
        /// The gemini API key constant
        /// </summary>
        internal const string GeminiAPIKeyConstant = "GeminiAPIKey";

        /// <summary>
        /// The gemini flash model
        /// </summary>
        internal const string GeminiFlashModel = "GeminiFlashModel";

        /// <summary>
        /// The gemini pro model
        /// </summary>
        internal const string GeminiProModel = "GeminiProModel";

        /// <summary>
        /// The is pro model enabled flag
        /// </summary>
        internal const string IsProModelEnabledFlag = "IsProModelEnabled";
    }

    /// <summary>
    /// The Perplexity AI constants.
    /// </summary>
    internal static class PerplexityAiConstants
    {
        /// <summary>
        /// The service provider name
        /// </summary>
        internal const string ServiceProviderName = "PerplexityAi";

        /// <summary>
        /// The model identifier
        /// </summary>
        internal const string ModelId = "PerplexityAI:ModelId";

        /// <summary>
        /// The API key
        /// </summary>
        internal const string ApiKey = "PerplexityAI:ApiKey";

        /// <summary>
        /// The API endpoint
        /// </summary>
        internal const string ApiEndpoint = "PerplexityAI:Endpoint";
    }

    /// <summary>
    /// The ChatGpt AI constants.
    /// </summary>
    internal static class ChatGptAiConstants
    {
        /// <summary>
        /// The service provider name
        /// </summary>
        internal const string ServiceProviderName = "OpenAiGpt";

        /// <summary>
        /// The model identifier
        /// </summary>
        internal const string ModelId = "ChatGpt:ModelId";

        /// <summary>
        /// The API key
        /// </summary>
        internal const string ApiKey = "ChatGpt:ApiKey";

        /// <summary>
        /// The API endpoint
        /// </summary>
        internal const string ApiEndpoint = "ChatGpt:Endpoint";
    }
}