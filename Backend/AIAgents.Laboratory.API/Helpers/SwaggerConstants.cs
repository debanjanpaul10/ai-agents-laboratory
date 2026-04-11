namespace AIAgents.Laboratory.API.Helpers;

/// <summary>
/// The Swagger Constants class containing swagger documentation.
/// </summary>
internal static class SwaggerConstants
{
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

        /// <summary>
        /// Swagger documentation for GetTopActiveAgentsDataAsync.
        /// </summary>
        internal static class GetTopActiveAgentsDataAction
        {
            internal const string Summary = "Gets the top active agents.";
            internal const string Description = "Gets the list of top active agents being used by users in the current application.";
            internal const string OperationId = nameof(GetTopActiveAgentsDataAction);
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

    /// <summary>
    /// Swagger documentation for ApplicationAdminController.
    /// </summary>
    internal static class ApplicationAdminController
    {
        /// <summary>
        /// Swagger documentation for GetAllSubmittedFeatureRequestsAsync.
        /// </summary>
        internal static class GetAllSubmittedFeatureRequestsAction
        {
            internal const string Summary = "Gets all the submitted feature requests.";
            internal const string Description = "Gets the list of all the submitted feature requests by users to be reviewed by devs and implemented.";
            internal const string OperationId = nameof(GetAllSubmittedFeatureRequestsAction);
        }
        /// <summary>
        /// Swagger documentation for GetAllBugReportsDataAsync.
        /// </summary>
        internal static class GetAllBugReportsDataAction
        {
            internal const string Summary = "Gets all the bug reports data.";
            internal const string Description = "Gets the list of all the bug reports data submitted by users to be reviewed by devs and fixed.";
            internal const string OperationId = nameof(GetAllBugReportsDataAction);
        }

        /// <summary>
        /// Swagger documentation for IsAdminAccessEnabledAsync.
        /// </summary>
        internal static class IsAdminAccessEnabledAction
        {
            internal const string Summary = "Checks if the admin access is enabled.";
            internal const string Description = "Checks if the admin access is enabled for the current logged in user.";
            internal const string OperationId = nameof(IsAdminAccessEnabledAction);
        }
    }

    /// <summary>
    /// Swagger documentation for RegisteredApplicationController.
    /// </summary>
    internal static class RegisteredApplicationController
    {
        /// <summary>
        /// Swagger documentation for GetAllRegisteredApplicationsAsync.
        /// </summary>
        internal static class GetAllRegisteredApplicationsAction
        {
            internal const string Summary = "Gets all the registered applications list data.";
            internal const string Description = "Gets the list of all the available registered applications in the system.";
            internal const string OperationId = nameof(GetAllRegisteredApplicationsAction);
        }

        /// <summary>
        /// Swagger documentation for GetRegisteredApplicationByIdAsync.
        /// </summary>
        internal static class GetRegisteredApplicationByIdAction
        {
            internal const string Summary = "Gets the registered application data by application id.";
            internal const string Description = "Gets a single registered application data by the application id passed.";
            internal const string OperationId = nameof(GetRegisteredApplicationByIdAction);
        }

        /// <summary>
        /// Swagger documentation for CreateNewRegisteredApplicationAsync.
        /// </summary>
        internal static class RegisterNewApplicationAction
        {
            internal const string Summary = "Creates a new registered application.";
            internal const string Description = "Creates a new registered application in the system for agents to use as tools.";
            internal const string OperationId = nameof(RegisterNewApplicationAction);
        }

        /// <summary>
        /// Swagger documentation for DeleteExistingRegisteredApplicationAsync.
        /// </summary>
        internal static class DeleteExistingRegisteredApplicationAction
        {
            internal const string Summary = "Deletes an existing registered application.";
            internal const string Description = "Deletes an existing registered application from the system based on the application guid id passed.";
            internal const string OperationId = nameof(DeleteExistingRegisteredApplicationAction);
        }

        /// <summary>
        /// Swagger documentation for UpdateExistingRegisteredApplicationDataAsync.
        /// </summary>
        internal static class UpdateExistingRegisteredApplicationDataAction
        {
            internal const string Summary = "Updates an existing registered application data.";
            internal const string Description = "Updates an existing registered application data in the system.";
            internal const string OperationId = nameof(UpdateExistingRegisteredApplicationDataAction);
        }
    }

    /// <summary>
    /// Swagger documentation for NotificationsController.
    /// </summary>
    internal static class NotificationsController
    {
        /// <summary>
        /// Swagger documentation for CreateNewNotificationAsync.
        /// </summary>
        internal static class CreateNewNotificationAction
        {
            internal const string Summary = "Creates a new notification.";
            internal const string Description = "Creates a new notification for the users.";
            internal const string OperationId = nameof(CreateNewNotificationAction);
        }

        /// <summary>
        /// Swagger documentation for PollNotificationsForUserAsync.
        /// </summary>
        internal static class PollNotificationsForUserAction
        {
            internal const string Summary = "Polls the notifications for user.";
            internal const string Description = "Polls the notifications for user based on the user id passed.";
            internal const string OperationId = nameof(PollNotificationsForUserAction);
        }

        /// <summary>
        /// Swagger documentation for MarkExistingNotificationAsReadAsync.
        /// </summary>
        internal static class MarkExistingNotificationAsReadAction
        {
            internal const string Summary = "Marks an existing notification as read.";
            internal const string Description = "Marks an existing notification as read in the system based on the notification guid id passed.";
            internal const string OperationId = nameof(MarkExistingNotificationAsReadAction);
        }

        /// <summary>
        /// Swagger documentation for StreamNotificationsForUserAsync.
        /// </summary>
        internal static class StreamNotificationsForUserAction
        {
            internal const string Summary = "Streams the notifications for user.";
            internal const string Description = "Streams the notifications for user based on the user id passed.";
            internal const string OperationId = nameof(StreamNotificationsForUserAction);
        }

        /// <summary>
        /// Swagger documentation for DeleteAllNotificationsForUserAsync.
        /// </summary>
        internal static class DeleteAllNotificationsForUserAction
        {
            internal const string Summary = "Deletes all notifications for a user.";
            internal const string Description = "Deletes all the read and unread notifications for a user based on the user email.";
            internal const string OperationId = nameof(DeleteAllNotificationsForUserAction);
        }
    }
}
