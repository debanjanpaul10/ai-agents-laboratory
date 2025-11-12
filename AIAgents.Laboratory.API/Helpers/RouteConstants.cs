// *********************************************************************************
//	<copyright file="RouteConstants.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>The Route Constants Class.</summary>
// *********************************************************************************

namespace AIAgents.Laboratory.API.Helpers;

/// <summary>
/// The Route Constants Class.
/// </summary>
public static class RouteConstants
{
    /// <summary>
    /// The ai base route prefix
    /// </summary>
    internal const string AiBase_RoutePrefix = "aiagentsapi";

    /// <summary>
    /// The agent status hub route
    /// </summary>
    internal const string AgentStatusHub_Route = "/hubs/agent-status";

    /// <summary>
    /// The route constants for plugins.
    /// </summary>
    internal static class PluginsRoutes
    {
        /// <summary>
        /// The rewrite text route
        /// </summary>
        internal const string RewriteText_Route = "rewritetext";

        /// <summary>
        /// The generate tag route
        /// </summary>
        internal const string GenerateTag_Route = "generatetag";

        /// <summary>
        /// The moderate content route
        /// </summary>
        internal const string ModerateContent_Route = "moderatecontent";

        /// <summary>
        /// The get bug severity route
        /// </summary>
        internal const string GetBugSeverity_Route = "getbugseverity";
    }

    /// <summary>
    /// The AI skills routes.
    /// </summary>
    internal static class AISkillsRoutes
    {

        /// <summary>
        /// The get SQL query markdown response route
        /// </summary>
        internal const string GetSQLQueryMarkdownResponse_Route = "getsqlquerymarkdownresponse";

        /// <summary>
        /// The get followup questions api route.
        /// </summary>
        internal const string GetFollowupQuestionsResponse_Route = "getfollowupquestionsresponse";

        /// <summary>
        /// The detect user intent route
        /// </summary>
        internal const string DetectUserIntent_Route = "intentdetectionskill";

        /// <summary>
        /// The get user greeting response route
        /// </summary>
        internal const string GetUserGreetingResponse_Route = "usergreetingskill";

        /// <summary>
        /// The get rag text response route
        /// </summary>
        internal const string GetRAGTextResponse_Route = "ragtextresponseskill";

        /// <summary>
        /// The get nl to SQL response route
        /// </summary>
        internal const string GetNlToSqlResponse_Route = "nltosqlskill";
    }

    /// <summary>
    /// The Route constants for Health Check.
    /// </summary>
    internal static class HealthCheckRoutes
    {
        /// <summary>
        /// The get agent status route
        /// </summary>
        internal const string GetAgentStatus_Route = "agentstatus";
    }

    /// <summary>
    /// The Route constants for Agent Skills Controller.
    /// </summary>
    internal static class AgentSkillsRoutes
    {
        /// <summary>
        /// The create new skill route
        /// </summary>
        internal const string CreateNewSkill_Route = "createskill";

        /// <summary>
        /// The get all skills route
        /// </summary>
        internal const string GetAllSkills_Route = "getallskills";

        /// <summary>
        /// The get skill by identifier route
        /// </summary>
        internal const string GetSkillById_Route = "getskillbyid/{skillId}";
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
        internal const string GetAgentById_Route = "getagentbyid/{agentid}";

        /// <summary>
        /// The update existing agent data route.
        /// </summary>
        internal const string UpdateExistingAgent_Route = "updateagent";

        /// <summary>
        /// The delete agent data route.
        /// </summary>
        internal const string DeleteExistingAgent_Route = "deleteagent/{agentId}";
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
        internal const string GetConfigurationByKey_Route = "getconfigurationbykey/{configKey}";
    }
}
