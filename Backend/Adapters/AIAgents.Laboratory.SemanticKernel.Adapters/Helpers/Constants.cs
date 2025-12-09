namespace AIAgents.Laboratory.SemanticKernel.Adapters.Helpers;

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
        /// The kernel arguments input constant.
        /// </summary>
        internal const string KernelArgumentsInputConstant = "input";

        /// <summary>
        /// The knowledge base input constant
        /// </summary>
        internal const string KnowledgeBaseInputConstant = "knowledge_base";

        /// <summary>
        /// The database schema input constant
        /// </summary>
        internal const string DatabaseSchemaInputConstant = "database_schema";

        /// <summary>
        /// The SQL query result input constant
        /// </summary>
        internal const string SQLQueryResultInputConstant = "sql_result";

        /// <summary>
        /// The SQL json input constant
        /// </summary>
        internal const string SQLJsonInputConstant = "sql_json";

        /// <summary>
        /// The user intent input constant
        /// </summary>
        internal const string UserIntentInputConstant = "user_intent";

        /// <summary>
        /// The user query input constant
        /// </summary>
        internal const string UserQueryInputConstant = "user_query";

        /// <summary>
        /// The ai response input constant
        /// </summary>
        internal const string AIResponseInputConstant = "ai_response";

        /// <summary>
        /// The user message input constant.
        /// </summary>
        internal const string UserMessageInputConstant = "user_message";

        /// <summary>
        /// The user role constant.
        /// </summary>
        internal const string UserRoleConstant = "user";

        /// <summary>
        /// The assistant role constant.
        /// </summary>
        internal const string AssistantRoleConstant = "assistant";

        /// <summary>
        /// The available MCP tools argument
        /// </summary>
        internal const string AvailableMcpToolsArgument = "availableMcpTools";

        /// <summary>
        /// The tool result argument
        /// </summary>
        internal const string ToolResultArgument = "toolResult";
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
    /// Logging constants.
    /// </summary>
    internal static class LoggingConstants
    {
        /// <summary>
        /// The log helper method start.
        /// </summary>
        internal const string LogHelperMethodStart = "{0} started at {1}";

        /// <summary>
        /// The log helper method failed.
        /// </summary>
        internal const string LogHelperMethodFailed = "{0} failed at {1} with {2}";

        /// <summary>
        /// The log helper method end.
        /// </summary>
        internal const string LogHelperMethodEnd = "{0} ended at {1}";
    }

    /// <summary>
    /// The Exception Constants Class.
    /// </summary>
    internal static class ExceptionConstants
    {
        /// <summary>
        /// The ai api key missing message.
        /// </summary>
        public const string AiAPIKeyMissingMessage = "The AI Api Key is missing in configuration.";

        /// <summary>
        /// Something went wrong message
        /// </summary>
        internal const string SomethingWentWrongMessage = "Something went wrong while processing the request!";

        /// <summary>
        /// The default ai exception message
        /// </summary>
        internal const string DefaultAIExceptionMessage = "I am currently unable to access the required information to process this request. Please try again later!";

        /// <summary>
        /// The invalid service provider message.
        /// </summary>
        internal const string InvalidServiceProvider = "Unsupported AI service provider: {0}";

        /// <summary>
        /// The no valid chunks generated
        /// </summary>
        internal const string NoValidChunksGenerated = "No valid chunks were generated from the content.";

        /// <summary>
        /// The number of embeddings mismatch
        /// </summary>
        internal const string NumberOfEmbeddingsMismatch = "The number of embeddings generated does not match the number of text chunks.";

        /// <summary>
        /// The tools not found exception constant
        /// </summary>
        internal const string ToolsNotFoundExceptionConstant = "Oops! No such tools exist in the given MCP server";
    }

    /// <summary>
    /// The intent constants class.
    /// </summary>
    internal static class IntentConstants
    {
        /// <summary>
        /// The greeting intent
        /// </summary>
        internal const string GreetingIntent = "GREETING";

        /// <summary>
        /// The SQL intent
        /// </summary>
        internal const string SQLIntent = "SQL";

        /// <summary>
        /// The rag intent
        /// </summary>
        internal const string RAGIntent = "RAG";

        /// <summary>
        /// The unclear intent
        /// </summary>
        internal const string UnclearIntent = "UNCLEAR";
    }

    /// <summary>
    /// The Configuration Constants class.
    /// </summary>
    internal static class ConfigurationConstants
    {
        /// <summary>
        /// The bearer constant
        /// </summary>
        internal const string BearerConstant = "Bearer";

        /// <summary>
        /// The application json constant
        /// </summary>
        internal const string ApplicationJsonConstant = "application/json";

        /// <summary>
        /// The fg tool HTTP client
        /// </summary>
        internal const string FGToolHttpClient = nameof(FGToolHttpClient);

        /// <summary>
        /// The token scope format.
        /// </summary>
        internal const string TokenScopeFormat = "{0}/.default";

        /// <summary>
        /// The is development mode constant
        /// </summary>
        internal const string IsDevelopmentModeConstant = "IsDevelopmentMode";

        /// <summary>
        /// The managed identity client identifier constant
        /// </summary>
        internal const string ManagedIdentityClientIdConstant = "ManagedIdentityClientId";

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

}
