using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.Domain.DomainEntities.AgentsEntities;
using AIAgents.Laboratory.Domain.DomainEntities.Workspaces;
using AIAgents.Laboratory.Domain.Helpers;
using Microsoft.AspNetCore.Http;

namespace AIAgents.Laboratory.Domain.UnitTests;

/// <summary>
/// The Tests Helper class.
/// </summary>
internal static class TestsHelpers
{
    /// <summary>
    /// The test user email
    /// </summary>
    internal static string TestUserEmail = "testuser@example.com";

    /// <summary>
    /// The test unique identifier identifier
    /// </summary>
    internal static string TestGuidId = Guid.NewGuid().ToString();

    /// <summary>
    /// The agent unique identifier identifier.
    /// </summary>
    internal static string AgentGuidId = Guid.NewGuid().ToString();

    /// <summary>
    /// The tool skill unique identifier.
    /// </summary>
    internal static string ToolSkillGuid = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets the conversation history domain.
    /// </summary>
    /// <param name="userName">Name of the user.</param>
    /// <returns>The conversation history domain model.</returns>
    internal static ConversationHistoryDomain GetConversationHistoryDomain(string userName) =>
        new()
        {
            Id = TestGuidId,
            ConversationId = TestGuidId,
            UserName = userName,
            ChatHistory =
            [
                new () { Role = "user", Content = "hello" },
            ],
            IsActive = true,
            LastModifiedOn = DateTime.UtcNow
        };

    /// <summary>
    /// Gets the agent data domain.
    /// </summary>
    /// <returns>The prepared agent data domain model.</returns>
    public static AgentDataDomain GetAgentDataDomain() =>
        new()
        {
            Id = TestGuidId,
            AgentId = TestGuidId,
            AgentName = "Test Agent",
            AgentMetaPrompt = "Test Meta Prompt"
        };

    /// <summary>
    /// Gets the agents workspace domain.
    /// </summary>
    /// <returns>The prepared agents workspace domain model.</returns>
    internal static AgentsWorkspaceDomain GetAgentsWorkspaceDomain() =>
        new()
        {
            Id = TestGuidId,
            AgentWorkspaceGuid = TestGuidId,
            AgentWorkspaceName = "Test Workspace",
            ActiveAgentsListInWorkspace =
        [
        new() { AgentName = "Test Agent", AgentGuid = TestGuidId }
        ],
            WorkspaceUsers = [TestUserEmail],
            IsGroupChatEnabled = true
        };

    /// <summary>
    /// Gets the workspace agent chat request domain.
    /// </summary>
    /// <returns>The prepared workspace agents chat request domain model.</returns>
    internal static WorkspaceAgentChatRequestDomain GetWorkspaceAgentChatRequestDomain() =>
        new()
        {
            ConversationId = TestGuidId,
            WorkspaceId = TestGuidId,
            AgentId = TestGuidId,
            UserMessage = "hello orchestrator",
            ApplicationName = "test-app"
        };

    /// <summary>
    /// Creates the mock form file.
    /// </summary>
    /// <param name="fileName">Name of the file.</param>
    /// <param name="contentType">Type of the content.</param>
    /// <returns>The form file.</returns>
    internal static IFormFile CreateMockFormFile(
        string fileName,
        string contentType = "application/octet-stream"
    )
    {
        var content = "test file content"u8.ToArray();
        var stream = new MemoryStream(content);
        return new FormFileImplementation(stream, content.Length, fileName, fileName) { ContentType = contentType };
    }

    /// <summary>
    /// Builds the chat request.
    /// </summary>
    /// <param name="agentId">The agent identifier.</param>
    /// <param name="userMessage">The user message.</param>
    /// <returns>The populated chat request domain model.</returns>
    internal static ChatRequestDomain BuildChatRequest(
        string agentId = "agent-1",
        string userMessage = "hello"
    ) =>
        new()
        {
            AgentId = agentId,
            UserMessage = userMessage,
            AgentName = "Test Agent"
        };

    /// <summary>
    /// Builds the agent data.
    /// </summary>
    /// <param name="agentId">The agent identifier.</param>
    /// <param name="withKnowledgeBase">if set to <c>true</c> [with knowledge base].</param>
    /// <param name="withVisionImages">if set to <c>true</c> [with vision images].</param>
    /// <param name="withSkills">if set to <c>true</c> [with skills].</param>
    /// <returns>The populated agent data domain model.</returns>
    internal static AgentDataDomain BuildAgentData(
        string agentId = "agent-1",
        bool withKnowledgeBase = false,
        bool withVisionImages = false,
        bool withSkills = false
    )
    {
        var agent = TestsHelpers.GetAgentDataDomain();
        agent.AgentId = agentId;
        agent.AgentMetaPrompt = "You are a helpful assistant.";

        if (withKnowledgeBase)
            agent.StoredKnowledgeBase = [new KnowledgeBaseDocumentDomain { FileName = "doc.pdf" }];

        if (withVisionImages)
            agent.AiVisionImagesData = [new AiVisionImagesDomain { ImageName = "img.jpg", ImageKeywords = ["cat"] }];

        if (withSkills)
            agent.AssociatedSkillGuids = ["skill-guid-1"];

        return agent;
    }

    /// <summary>
    /// Builds the tool skill.
    /// </summary>
    /// <param name="guid">The unique identifier.</param>
    /// <returns>The populated tool skill domain model.</returns>
    internal static ToolSkillDomain BuildToolSkill(string guid) =>
        new()
        {
            ToolSkillGuid = guid,
            ToolSkillDisplayName = "Test Skill",
            ToolSkillTechnicalName = "test_skill",
            ToolSkillMcpServerUrl = "https://mcp.example.com"
        };

}
