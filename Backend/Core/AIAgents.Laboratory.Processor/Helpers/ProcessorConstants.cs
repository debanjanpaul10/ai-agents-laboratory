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

        /// <summary>
        /// Represents the configuration key used to retrieve the Azure AI Vision service API key from application settings.
        /// </summary>
        internal const string AzureAiVisionKey = "AzureAiVision:Key";

        /// <summary>
        /// Represents the configuration key for the Azure AI Vision service endpoint.
        /// </summary>
        internal const string AzureAiVisionEndpoint = "AzureAiVision:Endpoint";
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
        internal const string LogHelperMethodStart = "{0} started at {1} for {2}";

        /// <summary>
        /// The log helper method failed.
        /// </summary>
        internal const string LogHelperMethodFailed = "{0} failed at {1} with {2}";

        /// <summary>
        /// The log helper method end.
        /// </summary>
        internal const string LogHelperMethodEnd = "{0} ended at {1} for {2}";
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

        /// <summary>
        /// The unsupported file type message.
        /// </summary>
        internal const string UnsupportedFileTypeMessage = "The uploaded file type is not supported for knowledge base processing.";
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

        /// <summary>
        /// The comma separator.
        /// </summary>
        internal const char CommaSeparator = ',';

        /// <summary>
        /// The file content types.
        /// </summary>
        internal static class FileContentTypes
        {
            /// <summary>
            /// The content type plain text.
            /// </summary>
            internal const string PlainTextFiles = ".txt";

            /// <summary>
            /// The pdf files.
            /// </summary>
            internal const string PdfFiles = ".pdf";

            /// <summary>
            /// The word files.
            /// </summary>
            internal const string WordFiles = ".docx, .doc, .docm";

            /// <summary>
            /// The excel files.
            /// </summary>
            internal const string ExcelFiles = ".xls, .xlsx, .xlsm";

            /// <summary>
            /// The JSON files.
            /// </summary>
            internal const string JsonFiles = ".json";
        }
    }

    /// <summary>
    /// Provides constant values used by the AI Vision components.
    /// </summary>
    internal static class AiVisionConstants
    {
        /// <summary>
        /// Represents the number of characters in a standard operation identifier.
        /// </summary>
        internal const int NUMBER_OF_CHARACTERS_IN_OPERATION_ID = 36;
    }
}
