namespace AIAgents.Laboratory.Processor.Helpers;

/// <summary>
/// The processor constants class.
/// </summary>
internal static class ProcessorConstants
{
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
    /// The OpenAI GPT constants.
    /// </summary>
    internal static class OpenAiGptConstants
    {
        /// <summary>
        /// The service provider name.
        /// </summary>
        internal const string ServiceProviderName = "OpenAiGPT";

        /// <summary>
        /// The Model ID.
        /// </summary>
        internal const string ModelId = "OpenAiGPT:ModelId";

        /// <summary>
        /// The API Key.
        /// </summary>
        internal const string ApiKey = "OpenAiGPT:ApiKey";

        /// <summary>
        /// The API endpoint
        /// </summary>
        internal const string ApiEndpoint = "OpenAiGPT:Endpoint";
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
    /// The Exception ProcessorConstants Class.
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
    }

    /// <summary>
    /// The knowledge base constants.
    /// </summary>
    internal static class KnowledgeBaseConstants
    {
        /// <summary>
        /// The chunk description template
        /// </summary>
        internal const string ChunkDescriptionTemplate = "Knowledge base chunk {0} of {1}";
    }
}
