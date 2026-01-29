namespace AIAgents.Laboratory.API.Helpers;

/// <summary>
/// The Swagger Constants class containing swagger documentation.
/// </summary>
internal static class SwaggerConstants
{
    /// <summary>
    /// The Skills Controller.
    /// </summary>
    internal static class ChatbotSkillsController
    {
        /// <summary>
        /// Swagger documentation for GetSQLQueryMarkdownResponseAsync
        /// </summary>
        internal static class GetSqlQueryMarkdownResponseAction
        {
            internal const string Summary = "Gets the markdown response for sql query json.";
            internal const string Description = "Gets a properly formatted markdown table and response format from a json sql request.";
            internal const string OperationId = nameof(GetSqlQueryMarkdownResponseAction);
        }

        /// <summary>
        /// Swagger documentation for GetFollowupQuestionsResponseAsync.
        /// </summary>
        internal static class GetFollowupQuestionsResponseAction
        {
            internal const string Summary = "Gets the list of followup questions.";
            internal const string Description = "Gets a list of strings that contain followup questions based on last user query and ai responses.";
            internal const string OperationId = nameof(GetFollowupQuestionsResponseAction);
        }

        /// <summary>
        /// Swagger documentation for DetectUserIntentAsync.
        /// </summary>
        internal static class DetectUserIntentAction
        {
            internal const string Summary = "Detects the user intent based on the user's query.";
            internal const string Description = "Detects the user intent and classifies them into SQL, RAG, GREETING, etc based on the user's asked question and query.";
            internal const string OperationId = nameof(DetectUserIntentAction);
        }

        /// <summary>
        /// Swagger documentation for GetUserGreetingResponseAsync.
        /// </summary>
        internal static class GetUserGreetingResponseAction
        {
            internal const string Summary = "Gives a greeting message for the user's reply.";
            internal const string Description = "Based on the user's greeting message, returns an AI generated greeting response.";
            internal const string OperationId = nameof(GetUserGreetingResponseAction);
        }

        /// <summary>
        /// Swagger documentation for GetRAGTextResponseAsync.
        /// </summary>
        internal static class GetRagTextResponseAction
        {
            internal const string Summary = "Gives the RAG text response based on user query.";
            internal const string Description = "Gives the RAG text response based on user query and knowledge base passed on.";
            internal const string OperationId = nameof(GetRagTextResponseAction);
        }

        /// <summary>
        /// Swagger documentation for GetNLToSQLResponseAsync.
        /// </summary>
        internal static class GetNlToSqlResponseAction
        {
            internal const string Summary = "Gives the Nl to sql response based on user query.";
            internal const string Description = "Creates a SQL query based on user's query, knowledge base and database schema.";
            internal const string OperationId = nameof(GetNlToSqlResponseAction);
        }
    }

    /// <summary>
    /// The PluginsRoutes Controller.
    /// </summary>
    internal static class PluginsController
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

    /// <summary>
    /// The Agent Skills Controller.
    /// </summary>
    internal static class AgentSkillsController
    {
        /// <summary>
        /// Swagger documentation for CreateNewSkillAsync.
        /// </summary>
        internal static class CreateNewSkillAction
        {
            internal const string Summary = "Creates a new skill.";
            internal const string Description = "Creates a new skill for the plugins.";
            internal const string OperationId = nameof(CreateNewSkillAction);
        }

        /// <summary>
        /// Swagger documentation for GetAllSkillsAsync.
        /// </summary>
        internal static class GetAllSkillsAction
        {
            internal const string Summary = "Gets all the skills.";
            internal const string Description = "Get all the skills available.";
            internal const string OperationId = nameof(GetAllSkillsAction);
        }

        /// <summary>
        /// Swagger documentation for GetSkillByIdAsync.
        /// </summary>
        internal static class GetSkillByIdAction
        {
            internal const string Summary = "Gets a skill by id.";
            internal const string Description = "Gets a unique skill by the skill id guid.";
            internal const string OperationId = nameof(GetSkillByIdAction);
        }
    }

    /// <summary>
    /// Swagger documentation for AgentsController.
    /// </summary>
    internal static class AgentsController
    {
        /// <summary>
        /// Swagger documentation for CreateNewAgentAsync.
        /// </summary>
        internal static class CreateNewAgentAction
        {
            internal const string Summary = "Creates a new agent.";
            internal const string Description = "Creates a new agent for the plugins.";
            internal const string OperationId = nameof(CreateNewAgentAction);
        }

        /// <summary>
        /// Swagger documentation for GetAllAgentsDataAsync.
        /// </summary>
        internal static class GetAllAgentsDataAction
        {
            internal const string Summary = "Gets all the agents data.";
            internal const string Description = "Get all the agents data available.";
            internal const string OperationId = nameof(GetAllAgentsDataAction);
        }

        /// <summary>
        /// Swagger documentation for GetAgentDataByIdAsync.
        /// </summary>
        internal static class GetAgentDataByIdAction
        {
            internal const string Summary = "Gets a agent data by id.";
            internal const string Description = "Gets a unique agent data by the agent id guid.";
            internal const string OperationId = nameof(GetAgentDataByIdAction);
        }

        /// <summary>
        /// Swagger documentation for UpdateExistingAgentDataAsync.
        /// </summary>
        internal static class UpdateExistingAgentDataAction
        {
            internal const string Summary = "Updates an existing agent data.";
            internal const string Description = "Updates an existing agent data in db.";
            internal const string OperationId = nameof(UpdateExistingAgentDataAction);
        }

        /// <summary>
        /// Swagger documentation for DeleteExistingAgentDataAsync.
        /// </summary>
        internal static class DeleteExistingAgentDataAction
        {
            internal const string Summary = "Deletes an existing agent data.";
            internal const string Description = "Deletes an existing agent data from database.";
            internal const string OperationId = nameof(DeleteExistingAgentDataAction);
        }

        /// <summary>
        /// Swagger documentation for DownloadKnowledgebaseFileAsync.
        /// </summary>
        internal static class DownloadKnowledgebaseFileAction
        {
            internal const string Summary = "Downloads a knowledge base file.";
            internal const string Description = "Downloads a knowledge base file from an agent.";
            internal const string OperationId = nameof(DownloadKnowledgebaseFileAction);
        }
    }

    /// <summary>
    /// Swagger documentation for ChatController.
    /// </summary>
    internal static class ChatController
    {
        /// <summary>
        /// Swagger documentation for InvokeAgentAsync.
        /// </summary>
        internal static class InvokeAgentAction
        {
            internal const string Summary = "Invokes an agent via chat.";
            internal const string Description = "Invokes an agent via chat with the user prompt and agent metadata.";
            internal const string OperationId = nameof(InvokeAgentAction);
        }

        /// <summary>
        /// Swagger documentation for GetDirectChatResponseAsync.
        /// </summary>
        internal static class GetDirectChatResponseAction
        {
            internal const string Summary = "Gets the direct chat response data.";
            internal const string Description = "Calls the LLM directly with user message.";
            internal const string OperationId = nameof(GetDirectChatResponseAction);
        }

        /// <summary>
        /// Swagger documentation for ClearConversationHistoryForUserAsync.
        /// </summary>
        internal static class ClearConversationHistoryForUserAction
        {
            internal const string Summary = "Clears the conversation history data.";
            internal const string Description = "Clears the conversation history data for user.";
            internal const string OperationId = nameof(ClearConversationHistoryForUserAction);
        }

        /// <summary>
        /// Swagger documentation for GetConversationHistoryDataForUserAsync.
        /// </summary>
        internal static class GetConversationHistoryDataForUserAction
        {
            internal const string Summary = "Gets the conversation history data.";
            internal const string Description = "Gets the conversation history data for user.";
            internal const string OperationId = nameof(GetConversationHistoryDataForUserAction);
        }
    }

    /// <summary>
    /// Swagger documentation for ConfigurationController.
    /// </summary>
    internal static class ConfigurationController
    {
        /// <summary>
        /// Swagger documentation for GetConfigurationsData.
        /// </summary>
        internal static class GetConfigurationsDataAction
        {
            internal const string Summary = "Gets all the application configurations data at startup.";
            internal const string Description = "Gets all the application configurations data at startup";
            internal const string OperationId = nameof(GetConfigurationsDataAction);
        }

        /// <summary>
        /// Swagger documentation for GetConfigurationByKeyName.
        /// </summary>
        internal static class GetConfigurationByKeyNameAction
        {
            internal const string Summary = "Gets the configuration data by key name.";
            internal const string Description = "Gets the configuration data by key name.";
            internal const string OperationId = nameof(GetConfigurationByKeyNameAction);
        }

        /// <summary>
        /// Swagger documentation for AddBugReportDataAsync
        /// </summary>
        internal static class AddBugReportDataAction
        {
            internal const string Summary = "Adds the bug report data asynchronous.";
            internal const string Description = "Creates a new bug report documentation by user/member to be reviewed by devs and fixed.";
            internal const string OperationId = nameof(AddBugReportDataAction);
        }

        /// <summary>
        /// Swagger documentation for SubmitFeatureRequestDataAsync.
        /// </summary>
        internal static class SubmitFeatureRequestDataAction
        {
            internal const string Summary = "Submits the feature request data asynchronous.";
            internal const string Description = "Creates a new feature request documentation by user/member to be reviewed by devs and implemented.";
            internal const string OperationId = nameof(SubmitFeatureRequestDataAction);
        }
    }

    /// <summary>
    /// Swagger documentation for ToolSkillsController.
    /// </summary>
    internal static class ToolSkillsController
    {
        /// <summary>
        /// Swagger documentation for GetAllToolSkillsAsync.
        /// </summary>
        internal static class GetAllToolSkillsAction
        {
            internal const string Summary = "Gets all the tool skills.";
            internal const string Description = "Gets the list of all the existing skills for all tools present in the system.";
            internal const string OperationId = nameof(GetAllToolSkillsAction);
        }

        /// <summary>
        /// Swagger documentation for GetToolSkillBySkillIdAsync.
        /// </summary>
        internal static class GetToolSkillBySkillIdAction
        {
            internal const string Summary = "Gets a single tool skill by skill id.";
            internal const string Description = "Gets the single tool skill by its corresponding skill id.";
            internal const string OperationId = nameof(GetToolSkillBySkillIdAction);
        }

        /// <summary>
        /// Swagger documentation for AddNewToolSkillAsync.
        /// </summary>
        internal static class AddNewToolSkillAction
        {
            internal const string Summary = "Creates a new tool skill.";
            internal const string Description = "Creates a new tool skill that will be used by the agents via the marketplace.";
            internal const string OperationId = nameof(AddNewToolSkillAction);
        }

        /// <summary>
        /// Swagger documentation for UpdateExistingToolSkillDataAsync.
        /// </summary>
        internal static class UpdateExistingToolSkillDataAction
        {
            internal const string Summary = "Updates an existing tool.";
            internal const string Description = "Updates an existing tool data present in the marketplace.";
            internal const string OperationId = nameof(UpdateExistingToolSkillDataAction);
        }

        /// <summary>
        /// Swagger documentation for DeleteExistingToolSkillBySkillIdAsync.
        /// </summary>
        internal static class DeleteExistingToolSkillBySkillIdAction
        {
            internal const string Summary = "Deletes an existing tool data.";
            internal const string Description = "Deletes an existing tool data based on the skill id from the marketplace.";
            internal const string OperationId = nameof(DeleteExistingToolSkillBySkillIdAction);
        }

        /// <summary>
        /// Swagger documentation for GetAllMcpToolsAvailableAsync.
        /// </summary>
        internal static class GetAllMcpToolsAvailableAction
        {
            internal const string Summary = "Gets all the MCP tools available from the given MCP server url.";
            internal const string Description = "Gets the list of all the available MCP tools present in given MCP server.";
            internal const string OperationId = nameof(GetAllMcpToolsAvailableAction);
        }

    }

    /// <summary>
    /// Swagger documentation for WorkspacesController.
    /// </summary>
    internal static class WorkspacesController
    {
        /// <summary>
        /// Swagger documentation for GetAllWorkspacesAsync.
        /// </summary>
        internal static class GetAllWorkspacesAction
        {
            internal const string Summary = "Gets all the workspaces list data.";
            internal const string Description = "Gets the list of all the available workspaces in the system.";
            internal const string OperationId = nameof(GetAllWorkspacesAction);
        }

        /// <summary>
        /// Swagger documentation for GetWorkspaceByWorkspaceIdAsync.
        /// </summary>
        internal static class GetWorkspaceByWorkspaceIdAction
        {
            internal const string Summary = "Gets the workspace data by workspace id.";
            internal const string Description = "Gets a single workspace data by the workspace id passed.";
            internal const string OperationId = nameof(GetWorkspaceByWorkspaceIdAction);
        }

        /// <summary>
        /// Swagger documentation for CreateNewWorkspaceAsync.
        /// </summary>
        internal static class CreateNewWorkspaceAction
        {
            internal const string Summary = "Creates a new workspace.";
            internal const string Description = "Creates a new workspace in the system for agents to be created under.";
            internal const string OperationId = nameof(CreateNewWorkspaceAction);
        }

        /// <summary>
        /// Swagger documentation for DeleteExistingWorkspaceAsync.
        /// </summary>
        internal static class DeleteExistingWorkspaceAction
        {
            internal const string Summary = "Deletes an existing workspace.";
            internal const string Description = "Deletes an existing workspace from the system based on the workspace guid id passed.";
            internal const string OperationId = nameof(DeleteExistingWorkspaceAction);
        }

        /// <summary>
        /// Swagger documentation for UpdateExistingWorkspaceDataAsync.
        /// </summary>
        internal static class UpdateExistingWorkspaceDataAction
        {
            internal const string Summary = "Updates an existing workspace data.";
            internal const string Description = "Updates an existing workspace data in the system.";
            internal const string OperationId = nameof(UpdateExistingWorkspaceDataAction);
        }

        /// <summary>
        /// Swagger documentation for InvokeWorkspaceAgentAsync.
        /// </summary>
        internal static class InvokeWorkspaceAgentAction
        {
            internal const string Summary = "Invokes the workspace agent via chat.";
            internal const string Description = "Invokes the workspace agent via chat with the user prompt and agent metadata.";
            internal const string OperationId = nameof(InvokeWorkspaceAgentAction);
        }

        /// <summary>
        /// Swagger documentation for GetWorkspaceGroupChatResponseAsync.
        /// </summary>
        internal static class GetWorkspaceGroupChatResponseAction
        {
            internal const string Summary = "Gets the workspace group chat response data.";
            internal const string Description = "Calls the LLM directly with user message for workspace group chat.";
            internal const string OperationId = nameof(GetWorkspaceGroupChatResponseAction);
        }
    }
}

