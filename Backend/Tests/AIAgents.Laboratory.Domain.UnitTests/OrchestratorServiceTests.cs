using AIAgents.Laboratory.Domain.Contracts;
using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.Domain.DomainEntities.AgentsEntities;
using AIAgents.Laboratory.Domain.DomainEntities.Workspaces;
using AIAgents.Laboratory.Domain.Helpers;
using AIAgents.Laboratory.Domain.Ports.In;
using AIAgents.Laboratory.Domain.Ports.Out;
using AIAgents.Laboratory.Domain.UseCases;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;

namespace AIAgents.Laboratory.Domain.UnitTests;

/// <summary>
/// The orchestrator services tests class.
/// </summary>
public sealed class OrchestratorServiceTests
{
    /// <summary>
    /// The mock logger.
    /// </summary>
    private readonly Mock<ILogger<OrchestratorService>> _mockLogger = new();

    /// <summary>
    /// The mock correlation context.
    /// </summary>
    private readonly Mock<ICorrelationContext> _mockCorrelationContext = new();

    /// <summary>
    /// The mock agents service.
    /// </summary>
    private readonly Mock<IAgentsService> _mockAgentsService = new();

    /// <summary>
    /// The mock agent chat service.
    /// </summary>
    private readonly Mock<IAgentChatService> _mockAgentChatService = new();

    /// <summary>
    /// The mock ai services.
    /// </summary>
    private readonly Mock<IAiServices> _mockAiServices = new();

    /// <summary>
    /// The orchestrator service.
    /// </summary>
    private readonly OrchestratorService _orchestratorService;

    /// <summary>
    /// Initializes a new instance of the <see cref="OrchestratorServiceTests"/> class.
    /// </summary>
    public OrchestratorServiceTests()
    {
        _orchestratorService = new OrchestratorService(
            _mockLogger.Object,
            _mockCorrelationContext.Object,
            _mockAgentsService.Object,
            _mockAgentChatService.Object,
            _mockAiServices.Object
        );

        _mockCorrelationContext.Setup(c => c.CorrelationId).Returns(TestsHelpers.TestGuidId);
    }

    /// <summary>
    /// Gets the orchestrator agent response asynchronous service throws exception throws ai agents business exception.
    /// </summary>
    [Fact]
    public async Task GetOrchestratorAgentResponseAsync_ServiceThrowsException_ThrowsAIAgentsBusinessException()
    {
        // Arrange
        var chatRequest = TestsHelpers.GetWorkspaceAgentChatRequestDomain();
        var workspaceDetails = TestsHelpers.GetAgentsWorkspaceDomain();

        _mockAgentsService
            .Setup(s => s.GetAgentDataByIdAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("AgentsService Error"));

        // Act & Assert
        var ex = await Assert.ThrowsAsync<AIAgentsBusinessException>(() =>
            _orchestratorService.GetOrchestratorAgentResponseAsync(chatRequest, workspaceDetails));

        Assert.Contains("AgentsService Error", ex.Message);
    }

    /// <summary>
    /// Gets the orchestrator agent response asynchronous final response returns mapped final response.
    /// </summary>
    [Fact]
    public async Task GetOrchestratorAgentResponseAsync_FinalResponse_ReturnsMappedFinalResponse()
    {
        // Arrange
        var chatRequest = TestsHelpers.GetWorkspaceAgentChatRequestDomain();
        var workspaceDetails = TestsHelpers.GetAgentsWorkspaceDomain();
        var agentData = TestsHelpers.GetAgentDataDomain();

        _mockAgentsService
            .Setup(s => s.GetAgentDataByIdAsync(TestsHelpers.TestGuidId, string.Empty, It.IsAny<CancellationToken>()))
            .ReturnsAsync(agentData);

        var orchestratorResponseObj = new OrchestratorAgentResponseDomain
        {
            Type = "response",
            AgentName = string.Empty,
            Instruction = string.Empty,
            Content = "Final Answer"
        };
        var serializedResponse = JsonConvert.SerializeObject(orchestratorResponseObj);

        _mockAiServices
            .Setup(s => s.GetChatbotResponseAsync(It.IsAny<ConversationHistoryDomain>(), chatRequest.UserMessage, It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(serializedResponse);

        // Act
        var result = await _orchestratorService.GetOrchestratorAgentResponseAsync(chatRequest, workspaceDetails);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Final Answer", result.FinalResponse);
        Assert.Empty(result.GroupChatAgentsResponses);
    }

    /// <summary>
    /// Gets the orchestrator agent response asynchronous invalid format response returns invalid format error message.
    /// </summary>
    [Fact]
    public async Task GetOrchestratorAgentResponseAsync_InvalidFormatResponse_ReturnsInvalidFormatErrorMessage()
    {
        // Arrange
        var chatRequest = TestsHelpers.GetWorkspaceAgentChatRequestDomain();
        var workspaceDetails = TestsHelpers.GetAgentsWorkspaceDomain();
        var agentData = TestsHelpers.GetAgentDataDomain();

        _mockAgentsService
            .Setup(s => s.GetAgentDataByIdAsync(TestsHelpers.TestGuidId, string.Empty, It.IsAny<CancellationToken>()))
            .ReturnsAsync(agentData);

        var orchestratorResponseObj = new OrchestratorAgentResponseDomain
        {
            Type = "invalid_type",
        };
        var serializedResponse = JsonConvert.SerializeObject(orchestratorResponseObj);

        _mockAiServices
            .Setup(s => s.GetChatbotResponseAsync(It.IsAny<ConversationHistoryDomain>(), chatRequest.UserMessage, It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(serializedResponse);

        // Act
        var result = await _orchestratorService.GetOrchestratorAgentResponseAsync(chatRequest, workspaceDetails);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.FinalResponse);
        // It should contain the invalid format message
        Assert.Contains("invalid", result.FinalResponse.ToLower());
    }

    /// <summary>
    /// Gets the orchestrator agent response asynchronous loop limit reached returns loop limit reached message.
    /// </summary>
    [Fact]
    public async Task GetOrchestratorAgentResponseAsync_LoopLimitReached_ReturnsLoopLimitReachedMessage()
    {
        // Arrange
        var chatRequest = TestsHelpers.GetWorkspaceAgentChatRequestDomain();
        var workspaceDetails = TestsHelpers.GetAgentsWorkspaceDomain();
        var agentData = TestsHelpers.GetAgentDataDomain();

        _mockAgentsService
            .Setup(s => s.GetAgentDataByIdAsync(TestsHelpers.TestGuidId, string.Empty, It.IsAny<CancellationToken>()))
            .ReturnsAsync(agentData);

        var orchestratorResponseObj = new OrchestratorAgentResponseDomain
        {
            Type = "delegate",
            AgentName = "Test Agent",
            Instruction = "do task",
        };
        var serializedResponse = JsonConvert.SerializeObject(orchestratorResponseObj);

        _mockAiServices
            .Setup(s => s.GetChatbotResponseAsync(It.IsAny<ConversationHistoryDomain>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(serializedResponse);

        _mockAgentChatService
            .Setup(s => s.GetAgentChatResponseAsync(It.IsAny<ChatRequestDomain>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync("Agent output");

        // Act
        var result = await _orchestratorService.GetOrchestratorAgentResponseAsync(chatRequest, workspaceDetails);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.FinalResponse);
        Assert.NotEmpty(result.GroupChatAgentsResponses);
        Assert.Equal(10, result.GroupChatAgentsResponses.Count());
        Assert.Contains("limit", result.FinalResponse.ToLower());
    }

    /// <summary>
    /// Gets the orchestrator agent response asynchronous agent not found notifies orchestrator of error.
    /// </summary>
    [Fact]
    public async Task GetOrchestratorAgentResponseAsync_AgentNotFound_NotifiesOrchestratorOfError()
    {
        // Arrange
        var chatRequest = TestsHelpers.GetWorkspaceAgentChatRequestDomain();
        var workspaceDetails = TestsHelpers.GetAgentsWorkspaceDomain();
        var agentData = TestsHelpers.GetAgentDataDomain();

        _mockAgentsService
            .Setup(s => s.GetAgentDataByIdAsync(TestsHelpers.TestGuidId, string.Empty, It.IsAny<CancellationToken>()))
            .ReturnsAsync(agentData);

        var orchestratorResponseDelegate = new OrchestratorAgentResponseDomain
        {
            Type = "delegate",
            AgentName = "NonExistentAgent",
            Instruction = "do task",
        };
        var serializedDelegateResponse = JsonConvert.SerializeObject(orchestratorResponseDelegate);

        var orchestratorResponseFinal = new OrchestratorAgentResponseDomain
        {
            Type = "response",
            Content = "Done."
        };
        var serializedFinalResponse = JsonConvert.SerializeObject(orchestratorResponseFinal);

        // Return delegate on first loop, final on second
        _mockAiServices
            .SetupSequence(s => s.GetChatbotResponseAsync(It.IsAny<ConversationHistoryDomain>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(serializedDelegateResponse)
            .ReturnsAsync(serializedFinalResponse);

        // Act
        var result = await _orchestratorService.GetOrchestratorAgentResponseAsync(chatRequest, workspaceDetails);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Done.", result.FinalResponse);
        Assert.Single(result.GroupChatAgentsResponses);
        Assert.Equal("NonExistentAgent", result.GroupChatAgentsResponses.First().AgentName);
        Assert.Contains("not available", result.GroupChatAgentsResponses.First().AgentResponse, StringComparison.OrdinalIgnoreCase);

        _mockAgentChatService.Verify(s => s.GetAgentChatResponseAsync(It.IsAny<ChatRequestDomain>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
