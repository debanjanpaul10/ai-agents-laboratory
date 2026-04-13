using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.Domain.DomainEntities.AgentsEntities;
using AIAgents.Laboratory.Domain.DomainEntities.Workspaces;

namespace AIAgents.Laboratory.Domain.UnitTests;

/// <summary>
/// The Tests Helper class.
/// </summary>
internal static class TestsHelpers
{
    /// <summary>
    /// The test user email
    /// </summary>
    public static string TestUserEmail = "testuser@example.com";

    /// <summary>
    /// The test unique identifier identifier
    /// </summary>
    public static string TestGuidId = Guid.NewGuid().ToString();

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
    public static AgentsWorkspaceDomain GetAgentsWorkspaceDomain() =>
             new()
             {
                 Id = TestGuidId,
                 AgentWorkspaceGuid = TestGuidId,
                 AgentWorkspaceName = "Test Workspace",
                 ActiveAgentsListInWorkspace = new List<WorkspaceAgentsDataDomain>
                {
                new() { AgentName = "Test Agent", AgentGuid = TestGuidId }
                },
                 WorkspaceUsers = new List<string> { TestUserEmail },
                 IsGroupChatEnabled = true
             };

    /// <summary>
    /// Gets the workspace agent chat request domain.
    /// </summary>
    /// <returns>The prepared workspace agents chat request domain model.</returns>
    public static WorkspaceAgentChatRequestDomain GetWorkspaceAgentChatRequestDomain() =>
             new()
             {
                 ConversationId = TestGuidId,
                 WorkspaceId = TestGuidId,
                 AgentId = TestGuidId,
                 UserMessage = "hello orchestrator",
                 ApplicationName = "test-app"
             };

}
