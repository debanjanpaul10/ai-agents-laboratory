namespace AIAgents.Laboratory.API.Helpers;

/// <summary>
/// The Route Constants Class.
/// </summary>
public static class RouteConstants
{
    /// <summary>
    /// The agent status hub route
    /// </summary>
    internal const string AgentStatusHub_Route = "/hubs/agent-status";

    /// <summary>
    /// The base route template for all API endpoints, which includes the API version as a route parameter and the controller name.
    /// </summary>
    internal const string ApiBaseRoute = "aiagentsapi/v{version:apiVersion}/[controller]";

    /// <summary>
    /// Provides constant values for supported API version identifiers.
    /// </summary>
    internal static class ApiVersionsConstants
    {
        /// <summary>
        /// Represents the string value for version 1 of the API.
        /// </summary>
        internal const string ApiVersionV1 = "1";

        /// <summary>
        /// Represents the API version string for version 2.
        /// </summary>
        internal const string ApiVersionV2 = "2";
    }

    /// <summary>
    /// The Route constants for Agents Controller.
    /// </summary>
    internal static class AgentsRoutes
    {
        /// <summary>
        /// The create new agent route
        /// </summary>
        internal const string CreateNewAgent_Route = "createagent";

        /// <summary>
        /// The get all agents route
        /// </summary>
        internal const string GetAllAgents_Route = "getallagents";

        /// <summary>
        /// The get agent by identifier route
        /// </summary>
        internal const string GetAgentById_Route = "getagentbyid";

        /// <summary>
        /// The update existing agent data route.
        /// </summary>
        internal const string UpdateExistingAgent_Route = "updateagent";

        /// <summary>
        /// The delete agent data route.
        /// </summary>
        internal const string DeleteExistingAgent_Route = "deleteagent";

        /// <summary>
        /// The download associated documents route.
        /// </summary>
        internal const string DownloadAssociatedDocuments_Route = "downloadassociateddocuments";
    }

    /// <summary>
    /// The route constants for Chat Controller.
    /// </summary>
    internal static class ChatRoutes
    {
        /// <summary>
        /// The invoke agent route
        /// </summary>
        internal const string InvokeAgent_Route = "invokeagent";

        /// <summary>
        /// The get direct chat response route.
        /// </summary>
        internal const string GetDirectChatResponse_Route = "directchat";

        /// <summary>
        /// The clear conversation history route.
        /// </summary>
        internal const string ClearConversationHistory_Route = "clearconversation";

        /// <summary>
        /// The get conversation history for user route.
        /// </summary>
        internal const string GetConversationHistoryUser_Route = "getconversationhistory";
    }

    /// <summary>
    /// The common routes constants class.
    /// </summary>
    internal static class CommonRoutes
    {
        /// <summary>
        /// The get configurations route.
        /// </summary>
        internal const string GetConfigurations_Route = "getconfigurations";

        /// <summary>
        /// The get configuration by key route.
        /// </summary>
        internal const string GetConfigurationByKey_Route = "getconfigurationbykey";

        /// <summary>
        /// The add bug report route.
        /// </summary>
        internal const string AddBugReport_Route = "addbugreport";

        /// <summary>
        /// The submit feature request route.
        /// </summary>
        internal const string SubmitFeatureRequest_Route = "submitfeaturerequest";

        /// <summary>
        /// The route constant for getting top 3 active ai agents.
        /// </summary>
        internal const string GetTopActiveAgents_Route = "topactiveagents";
    }

    /// <summary>
    /// The route constants class for Tool Skills Controller.
    /// </summary>
    internal static class ToolSkillsRoutes
    {
        /// <summary>
        /// The add new tool skill route.
        /// </summary>
        internal const string AddNewToolSkill_Route = "addtoolskill";

        /// <summary>
        /// The update existing tool skill route.
        /// </summary>
        internal const string UpdateExistingToolSkillData_Route = "updatetoolskill";

        /// <summary>
        /// The get all tool skills data route.
        /// </summary>
        internal const string GetAllToolSkills_Route = "getalltoolskills";

        /// <summary>
        /// The get tool skill by skill id route.
        /// </summary>
        internal const string GetToolSkillBySkillId_Route = "gettoolskill";

        /// <summary>
        /// The delete existing tool skill by skill id route.
        /// </summary>
        internal const string DeleteExistingToolSkillBySkillId_Route = "deletetoolskill";

        /// <summary>
        /// The get all MCP tools available route.
        /// </summary>
        internal const string GetAllMcpToolsAvailable_Route = "getallmcptoolsavailable";
    }

    /// <summary>
    /// The route constants for Workspaces Controller.
    /// </summary>
    internal static class WorkspacesRoutes
    {
        /// <summary>
        /// The get all workspaces route.
        /// </summary>
        internal const string GetAllWorkspaces_Route = "getallworkspaces";

        /// <summary>
        /// The get workspace by workspace id route.
        /// </summary>
        internal const string GetWorkspaceByWorkspaceId_Route = "getworkspace";

        /// <summary>
        /// The add new workspace route.
        /// </summary>
        internal const string AddNewWorkspace_Route = "createworkspace";

        /// <summary>
        /// The update existing workspace route.
        /// </summary>
        internal const string UpdateExistingWorkspace_Route = "updateworkspace";

        /// <summary>
        /// The delete existing workspace route.
        /// </summary>
        internal const string DeleteExistingWorkspace_Route = "deleteworkspace";

        /// <summary>
        /// The invoke workspace agent route.
        /// </summary>
        internal const string InvokeWorkspaceAgent_Route = "invokeworkspaceagent";

        /// <summary>
        /// The get workspace group chat response route.
        /// </summary>
        internal const string GetWorkspaceGroupChatResponse_Route = "workspacegroupchatresponse";
    }

    /// <summary>
    /// The route constants for Application Admin Controller.
    /// </summary>
    internal static class ApplicationAdminRoutes
    {
        /// <summary>
        /// The get all reported bugs route.
        /// </summary>
        internal const string GetAllReportedBugs_Route = "getallreportedbugs";

        /// <summary>
        /// The get all submitted feature requests route.
        /// </summary>
        internal const string GetAllSubmittedFeatureRequests_Route = "getallsubmittedfeaturerequests";

        /// <summary>
        /// The route constant for checking if admin access is enabled.
        /// </summary>
        internal const string IsAdminAccessEnabled_Route = "isadminaccessenabled";
    }

    /// <summary>
    /// The route constants for Registered Application Controller.
    /// </summary>
    internal static class RegisteredApplicationRoutes
    {
        /// <summary>
        /// The route constant for registering an application.
        /// </summary>
        internal const string RegisterNewApplication_Route = "registernewapplication";

        /// <summary>
        /// The route constant for getting all registered applications.
        /// </summary>
        internal const string GetAllRegisteredApplications_Route = "getallregisteredapplications";

        /// <summary>
        /// The route constant for getting a registered application by its identifier.
        /// </summary>
        internal const string GetRegisteredApplicationById_Route = "getregisteredapplication";

        /// <summary>
        /// The route constant for deleting a registered application by its identifier.
        /// </summary>
        internal const string DeleteRegisteredApplicationById_Route = "deleteregisteredapplication";

        /// <summary>
        /// The route constant for updating an existing registered application.
        /// </summary>
        internal const string UpdateExistingRegisteredApplication_Route = "updateregisteredapplication";
    }
}
