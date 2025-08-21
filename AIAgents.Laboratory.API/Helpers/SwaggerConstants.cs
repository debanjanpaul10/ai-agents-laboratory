namespace AIAgents.Laboratory.API.Helpers;

/// <summary>
/// The Swagger Constants class containing swagger documentation.
/// </summary>
internal static class SwaggerConstants
{
    /// <summary>
    /// The FitGym Tool AI Controller.
    /// </summary>
    internal static class FitGymToolAIController
    {
        /// <summary>
        /// Swagger documentation for GetBugSeverityAsync
        /// </summary>
        internal static class GetBugSeverityAction
        {
            internal const string Summary = "Gets the bug severity string data asynchronous.";
            internal const string Description = "Gets the bug severity for the given user bug based on the bug title and bug description.";
            internal const string OperationId = nameof(GetBugSeverityAction);
        }

        /// <summary>
        /// Swagger documentation for GetChatbotResponseAsync
        /// </summary>
        internal static class GetChatbotResponseAction
        {
            internal const string Summary = "Gets the chatbot response asynchronous.";
            internal const string Description = "Gets the chatbot response from AI based on user query and the existing prompts and metadata.";
            internal const string OperationId = nameof(GetChatbotResponseAction);
        }
    }

    /// <summary>
    /// The Internet Bulletin Board System AI Controller.
    /// </summary>
    internal static class IBBSAIController
    {
        /// <summary>
        /// Swagger documentation for RewriteTextAsync
        /// </summary>
        internal static class RewriteTextAction
        {
            internal const string Summary = "Rewrites the text asynchronous.";
            internal const string Description = "Rewrites and improves the provided user story text using AI to enhance readability, grammar, and overall quality while maintaining the original meaning and context.";
            internal const string OperationId = nameof(RewriteTextAction);
        }

        /// <summary>
        /// Swagger documentation for GenerateTagForStoryAsync
        /// </summary>
        internal static class GenerateTagForStoryAction
        {
            internal const string Summary = "Generates the tag for story asynchronous.";
            internal const string Description = "Analyzes the provided user story content and generates an appropriate genre tag or category classification using AI to help with content organization and discovery.";
            internal const string OperationId = nameof(GenerateTagForStoryAction);
        }

        /// <summary>
        /// Swagger documentation for ModerateContentDataAsync
        /// </summary>
        internal static class ModerateContentDataAction
        {
            internal const string Summary = "Moderates the content data asynchronous.";
            internal const string Description = "Analyzes the provided user story content for inappropriate material and assigns a content rating to ensure compliance with community guidelines and content policies.";
            internal const string OperationId = nameof(ModerateContentDataAction);
        }
    }

    /// <summary>
    /// The Health Check Controller.
    /// </summary>
    internal static class HealthCheckController
    {
        /// <summary>
        /// Swagger documentation for GetAgentStatus
        /// </summary>
        internal static class GetAgentStatusAction
        {
            internal const string Summary = "Gets the agent status.";
            internal const string Description = "Gets the agent status from the app configuration via SignalR.";
            internal const string OperationId = nameof(GetAgentStatusAction);
        }
    }

}
