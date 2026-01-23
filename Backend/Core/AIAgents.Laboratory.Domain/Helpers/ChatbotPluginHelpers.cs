namespace AIAgents.Laboratory.Domain.Helpers;

/// <summary>
/// The Chatbot Plugin Helpers class.
/// </summary>
public static class ChatbotPluginHelpers
{
    /// <summary>
    /// The plugin name
    /// </summary>
    public const string PluginName = "ChatbotPlugin";

    /// <summary>
    /// The Determine user intent function.
    /// </summary>
    public static class DetermineUserIntentFunction
    {
        /// <summary>
        /// The function name
        /// </summary>
        public const string FunctionName = nameof(DetermineUserIntentFunction);
    }

    /// <summary>
    /// The greeting function.
    /// </summary>
    public static class GreetingFunction
    {
        /// <summary>
        /// The function name
        /// </summary>
        public const string FunctionName = nameof(GreetingFunction);
    }

    /// <summary>
    /// The NL to SQL function.
    /// </summary>
    public static class NLToSqlSkillFunction
    {
        /// <summary>
        /// The function name
        /// </summary>
        public const string FunctionName = nameof(NLToSqlSkillFunction);
    }

    /// <summary>
    /// The RAG Text Skill Function.
    /// </summary>
    public static class RAGTextSkillFunction
    {
        /// <summary>
        /// The function name
        /// </summary>
        public const string FunctionName = nameof(RAGTextSkillFunction);
    }

    /// <summary>
    /// The SQL query markdown response function.
    /// </summary>
    public static class SQLQueryMarkdownResponseFunction
    {
        /// <summary>
        /// The function name
        /// </summary>
        public const string FunctionName = nameof(SQLQueryMarkdownResponseFunction);
    }

    /// <summary>
    /// The generate followup questions function.
    /// </summary>
    public static class GenerateFollowupQuestionsFunction
    {
        /// <summary>
        /// The function name
        /// </summary>
        public const string FunctionName = nameof(GenerateFollowupQuestionsFunction);
    }
}
