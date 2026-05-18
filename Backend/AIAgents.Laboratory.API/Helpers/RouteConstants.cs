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
    internal const string ApiBaseRoute = "aiagentsapi/[controller]";

    /// <summary>
    /// The Route constants for Agents Controller.
    /// </summary>
    internal static class AgentsRoutes
    {
        internal const string CreateNewAgent_Route = "createagent";
        internal const string GetAllAgents_Route = "getallagents";
        internal const string GetAgentById_Route = "getagentbyid";
        internal const string UpdateExistingAgent_Route = "updateagent";
        internal const string DeleteExistingAgent_Route = "deleteagent";
        internal const string DownloadAssociatedDocuments_Route = "downloadassociateddocuments";
    }

    /// <summary>
    /// The route constants for Chat Controller.
    /// </summary>
    internal static class ChatRoutes
    {
        internal const string InvokeAgent_Route = "invokeagent";
        internal const string GetDirectChatResponse_Route = "directchat";
        internal const string ClearConversationHistory_Route = "clearconversation";
        internal const string GetConversationHistoryUser_Route = "getconversationhistory";
    }

    /// <summary>
    /// The common routes constants class.
    /// </summary>
    internal static class CommonRoutes
    {
        internal const string GetConfigurations_Route = "getconfigurations";
        internal const string GetConfigurationByKey_Route = "getconfigurationbykey";
        internal const string AddBugReport_Route = "addbugreport";
        internal const string SubmitFeatureRequest_Route = "submitfeaturerequest";
        internal const string GetTopActiveAgents_Route = "topactiveagents";
    }

    /// <summary>
    /// The route constants class for Tool Skills Controller.
    /// </summary>
    internal static class ToolSkillsRoutes
    {
        internal const string AddNewToolSkill_Route = "addtoolskill";
        internal const string UpdateExistingToolSkillData_Route = "updatetoolskill";
        internal const string GetAllToolSkills_Route = "getalltoolskills";
        internal const string GetToolSkillBySkillId_Route = "gettoolskill";
        internal const string DeleteExistingToolSkillBySkillId_Route = "deletetoolskill";
        internal const string GetAllMcpToolsAvailable_Route = "getallmcptoolsavailable";
    }

    /// <summary>
    /// The route constants for Workspaces Controller.
    /// </summary>
    internal static class WorkspacesRoutes
    {
        internal const string GetAllWorkspaces_Route = "getallworkspaces";
        internal const string GetWorkspaceByWorkspaceId_Route = "getworkspace";
        internal const string AddNewWorkspace_Route = "createworkspace";
        internal const string UpdateExistingWorkspace_Route = "updateworkspace";
        internal const string DeleteExistingWorkspace_Route = "deleteworkspace";
        internal const string InvokeWorkspaceAgent_Route = "invokeworkspaceagent";
        internal const string GetWorkspaceGroupChatResponse_Route = "workspacegroupchatresponse";
    }

    /// <summary>
    /// The route constants for Application Admin Controller.
    /// </summary>
    internal static class ApplicationAdminRoutes
    {
        internal const string GetAllReportedBugs_Route = "getallreportedbugs";
        internal const string GetAllSubmittedFeatureRequests_Route = "getallsubmittedfeaturerequests";
        internal const string IsAdminAccessEnabled_Route = "isadminaccessenabled";
    }

    /// <summary>
    /// The route constants for Registered Application Controller.
    /// </summary>
    internal static class RegisteredApplicationRoutes
    {
        internal const string RegisterNewApplication_Route = "registernewapplication";
        internal const string GetAllRegisteredApplications_Route = "getallregisteredapplications";
        internal const string GetRegisteredApplicationById_Route = "getregisteredapplication";
        internal const string DeleteRegisteredApplicationById_Route = "deleteregisteredapplication";
        internal const string UpdateExistingRegisteredApplication_Route = "updateregisteredapplication";
    }

    /// <summary>
    /// The route constants for Notifications Controller.
    /// </summary>
    internal static class NotificationsRoutes
    {
        internal const string CreateNewNotification_Route = "createnewnotification";
        internal const string PollNotificationsForUser_Route = "pollnotifications";
        internal const string StreamNotificationsForUser_Route = "streamnotifications";
        internal const string MarkExistingNotificationAsRead_Route = "marknotificationasread";
        internal const string DeleteAllNotificationsForUser_Route = "deleteallnotifications";
    }
}
