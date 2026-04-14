using AIAgents.Laboratory.Domain.Contracts;
using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.Domain.DomainEntities.AgentsEntities;
using AIAgents.Laboratory.Domain.Helpers;
using AIAgents.Laboratory.Domain.Ports.In;
using AIAgents.Laboratory.Domain.Ports.Out;
using AIAgents.Laboratory.Domain.UseCases;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using static AIAgents.Laboratory.Domain.Helpers.Constants;

namespace AIAgents.Laboratory.Domain.UnitTests;

/// <summary>
/// The agent chat services tests class.
/// </summary>
public sealed class AgentChatServiceTests
{
    /// <summary>
    /// The mock logger.
    /// </summary>
    private readonly Mock<ILogger<AgentChatService>> _mockLogger = new();

    /// <summary>
    /// The mock configuration.
    /// </summary>
    private readonly Mock<IConfiguration> _mockConfiguration = new();

    /// <summary>
    /// The mock correlation context.
    /// </summary>
    private readonly Mock<ICorrelationContext> _mockCorrelationContext = new();

    /// <summary>
    /// The mock agents service.
    /// </summary>
    private readonly Mock<IAgentsService> _mockAgentsService = new();

    /// <summary>
    /// The mock knowledge base processor.
    /// </summary>
    private readonly Mock<IKnowledgeBaseProcessor> _mockKnowledgeBaseProcessor = new();

    /// <summary>
    /// The mock ai services.
    /// </summary>
    private readonly Mock<IAiServices> _mockAiServices = new();

    /// <summary>
    /// The mock tool skill service.
    /// </summary>
    private readonly Mock<IToolSkillsService> _mockToolSkillService = new();

    /// <summary>
    /// The agent chat service.
    /// </summary>
    private readonly AgentChatService _agentChatService;

    /// <summary>
    /// Initializes a new instance of the <see cref="AgentChatServiceTests"/> class.
    /// </summary>
    public AgentChatServiceTests()
    {
        _mockConfiguration
            .Setup(c => c[AzureAppConfigurationConstants.IsKnowledgeBaseServiceEnabledConstant])
            .Returns("true");
        _mockConfiguration
            .Setup(c => c[AzureAppConfigurationConstants.IsAiVisionServiceEnabledConstant])
            .Returns("true");
        _mockCorrelationContext
            .Setup(c => c.CorrelationId)
            .Returns(Guid.NewGuid().ToString());

        _agentChatService = new(
            _mockConfiguration.Object,
            _mockLogger.Object,
            _mockCorrelationContext.Object,
            _mockAgentsService.Object,
            _mockKnowledgeBaseProcessor.Object,
            _mockAiServices.Object,
            _mockToolSkillService.Object
        );
    }


    /// <summary>
    /// Constructor missing knowledge base configuration key throws key not found exception.
    /// </summary>
    [Fact]
    public void Constructor_MissingKnowledgeBaseConfigKey_ThrowsKeyNotFoundException()
    {
        // Arrange
        var config = new Mock<IConfiguration>();
        config.Setup(c => c[AzureAppConfigurationConstants.IsKnowledgeBaseServiceEnabledConstant]).Returns((string)null!);
        config.Setup(c => c[AzureAppConfigurationConstants.IsAiVisionServiceEnabledConstant]).Returns("false");

        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() => new AgentChatService(
            config.Object, _mockLogger.Object, _mockCorrelationContext.Object,
            _mockAgentsService.Object, _mockKnowledgeBaseProcessor.Object,
            _mockAiServices.Object, _mockToolSkillService.Object));
    }

    /// <summary>
    /// Constructor missing ai vision configuration key throws key not found exception.
    /// </summary>
    [Fact]
    public void Constructor_MissingAiVisionConfigKey_ThrowsKeyNotFoundException()
    {
        // Arrange
        var config = new Mock<IConfiguration>();
        config.Setup(c => c[AzureAppConfigurationConstants.IsKnowledgeBaseServiceEnabledConstant]).Returns("false");
        config.Setup(c => c[AzureAppConfigurationConstants.IsAiVisionServiceEnabledConstant]).Returns((string)null!);

        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() => new AgentChatService(
            config.Object, _mockLogger.Object, _mockCorrelationContext.Object,
            _mockAgentsService.Object, _mockKnowledgeBaseProcessor.Object,
            _mockAiServices.Object, _mockToolSkillService.Object));
    }

    /// <summary>
    /// Gets the agent chat response asynchronous agent not found throws ai agents business exception.
    /// </summary>
    [Fact]
    public async Task GetAgentChatResponseAsync_AgentNotFound_ThrowsAIAgentsBusinessException()
    {
        // Arrange
        _mockAgentsService
            .Setup(s => s.GetAgentDataByIdAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((AgentDataDomain)null!);

        // Act & Assert
        await Assert.ThrowsAsync<AIAgentsBusinessException>(() =>
            _agentChatService.GetAgentChatResponseAsync(TestsHelpers.BuildChatRequest()));
    }

    /// <summary>
    /// Gets the agent chat response asynchronous agent has empty meta prompt throws ai agents business exception.
    /// </summary>
    [Fact]
    public async Task GetAgentChatResponseAsync_AgentHasEmptyMetaPrompt_ThrowsAIAgentsBusinessException()
    {
        // Arrange
        var agent = TestsHelpers.BuildAgentData();
        agent = agent with { AgentMetaPrompt = string.Empty };
        _mockAgentsService
            .Setup(s => s.GetAgentDataByIdAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(agent);

        // Act & Assert
        await Assert.ThrowsAsync<AIAgentsBusinessException>(() =>
            _agentChatService.GetAgentChatResponseAsync(TestsHelpers.BuildChatRequest()));
    }

    /// <summary>
    /// Gets the agent chat response asynchronous agents service throws exception throws ai agents business exception.
    /// </summary>
    [Fact]
    public async Task GetAgentChatResponseAsync_AgentsServiceThrowsException_ThrowsAIAgentsBusinessException()
    {
        // Arrange
        _mockAgentsService
            .Setup(s => s.GetAgentDataByIdAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Agent fetch failed"));

        // Act & Assert
        var ex = await Assert.ThrowsAsync<AIAgentsBusinessException>(() =>
            _agentChatService.GetAgentChatResponseAsync(TestsHelpers.BuildChatRequest()));

        Assert.Contains("Agent fetch failed", ex.Message);
    }


    /// <summary>
    /// Gets the agent chat response asynchronous no skills returns ai response.
    /// </summary>
    [Fact]
    public async Task GetAgentChatResponseAsync_NoSkills_ReturnsAiResponse()
    {
        // Arrange
        var agent = TestsHelpers.BuildAgentData();
        _mockAgentsService
            .Setup(s => s.GetAgentDataByIdAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(agent);
        _mockAiServices
            .Setup(s => s.GetAiFunctionResponseAsync(It.IsAny<ChatMessageDomain>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync("AI response");

        // Act
        var result = await _agentChatService.GetAgentChatResponseAsync(TestsHelpers.BuildChatRequest());

        // Assert
        Assert.Equal("AI response", result);
        _mockAiServices.Verify(
            s => s.GetAiFunctionResponseAsync(It.IsAny<ChatMessageDomain>(), ApplicationPluginsHelpers.PluginName, ApplicationPluginsHelpers.GetChatMessageResponseFunction.FunctionName, It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    /// <summary>
    /// Gets the agent chat response asynchronous no skills ai service throws exception throws ai agents business exception.
    /// </summary>
    [Fact]
    public async Task GetAgentChatResponseAsync_NoSkills_AiServiceThrowsException_ThrowsAIAgentsBusinessException()
    {
        // Arrange
        var agent = TestsHelpers.BuildAgentData();
        _mockAgentsService
            .Setup(s => s.GetAgentDataByIdAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(agent);
        _mockAiServices
            .Setup(s => s.GetAiFunctionResponseAsync(It.IsAny<ChatMessageDomain>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("AI error"));

        // Act & Assert
        var ex = await Assert.ThrowsAsync<AIAgentsBusinessException>(() =>
            _agentChatService.GetAgentChatResponseAsync(TestsHelpers.BuildChatRequest()));

        Assert.Contains("AI error", ex.Message);
    }

    /// <summary>
    /// Gets the agent chat response asynchronous no skills passes correct agent id to agents service.
    /// </summary>
    [Fact]
    public async Task GetAgentChatResponseAsync_NoSkills_PassesCorrectAgentIdToAgentsService()
    {
        // Arrange
        var agentId = TestsHelpers.AgentGuidId;
        var agent = TestsHelpers.BuildAgentData(agentId);
        _mockAgentsService
            .Setup(s => s.GetAgentDataByIdAsync(agentId, string.Empty, It.IsAny<CancellationToken>()))
            .ReturnsAsync(agent);
        _mockAiServices
            .Setup(s => s.GetAiFunctionResponseAsync(It.IsAny<ChatMessageDomain>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync("response");

        // Act
        await _agentChatService.GetAgentChatResponseAsync(TestsHelpers.BuildChatRequest(agentId: agentId));

        // Assert
        _mockAgentsService.Verify(
            s => s.GetAgentDataByIdAsync(agentId, string.Empty, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    /// <summary>
    /// Gets the agent chat response asynchronous knowledge base enabled and agent has kb content enriches chat message.
    /// </summary>
    [Fact]
    public async Task GetAgentChatResponseAsync_KnowledgeBaseEnabledAndAgentHasKbContent_EnrichesChatMessage()
    {
        // Arrange
        var service = new AgentChatService(
            _mockConfiguration.Object, _mockLogger.Object, _mockCorrelationContext.Object,
            _mockAgentsService.Object, _mockKnowledgeBaseProcessor.Object, _mockAiServices.Object, _mockToolSkillService.Object);

        var agent = TestsHelpers.BuildAgentData(withKnowledgeBase: true);
        _mockAgentsService
            .Setup(s => s.GetAgentDataByIdAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(agent);
        _mockKnowledgeBaseProcessor
            .Setup(k => k.GetRelevantKnowledgeAsync(It.IsAny<string>(), agent.AgentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync("relevant knowledge");
        _mockAiServices
            .Setup(s => s.GetAiFunctionResponseAsync(It.IsAny<ChatMessageDomain>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync("response");

        // Act
        await service.GetAgentChatResponseAsync(TestsHelpers.BuildChatRequest());

        // Assert
        _mockKnowledgeBaseProcessor.Verify(
            k => k.GetRelevantKnowledgeAsync(It.IsAny<string>(), agent.AgentId, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    /// <summary>
    /// Gets the agent chat response asynchronous knowledge base disabled does not query knowledge base.
    /// </summary>
    [Fact]
    public async Task GetAgentChatResponseAsync_KnowledgeBaseDisabled_DoesNotQueryKnowledgeBase()
    {
        _mockConfiguration
            .Setup(c => c[AzureAppConfigurationConstants.IsKnowledgeBaseServiceEnabledConstant])
            .Returns("false");
        var agent = TestsHelpers.BuildAgentData(withKnowledgeBase: true);
        _mockAgentsService
            .Setup(s => s.GetAgentDataByIdAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(agent);
        _mockAiServices
            .Setup(s => s.GetAiFunctionResponseAsync(It.IsAny<ChatMessageDomain>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync("response");

        // Act
        await _agentChatService.GetAgentChatResponseAsync(TestsHelpers.BuildChatRequest());

        // Assert
        _mockKnowledgeBaseProcessor.Verify(
            k => k.GetRelevantKnowledgeAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    /// <summary>
    /// Gets the agent chat response asynchronous knowledge base enabled but agent has no kb content does not query knowledge base.
    /// </summary>
    [Fact]
    public async Task GetAgentChatResponseAsync_KnowledgeBaseEnabledButAgentHasNoKbContent_DoesNotQueryKnowledgeBase()
    {
        // Arrange
        var service = new AgentChatService(
            _mockConfiguration.Object, _mockLogger.Object, _mockCorrelationContext.Object,
            _mockAgentsService.Object, _mockKnowledgeBaseProcessor.Object, _mockAiServices.Object, _mockToolSkillService.Object);

        var agent = TestsHelpers.BuildAgentData(withKnowledgeBase: false);
        _mockAgentsService
            .Setup(s => s.GetAgentDataByIdAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(agent);
        _mockAiServices
            .Setup(s => s.GetAiFunctionResponseAsync(It.IsAny<ChatMessageDomain>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync("response");

        // Act
        await service.GetAgentChatResponseAsync(TestsHelpers.BuildChatRequest());

        // Assert
        _mockKnowledgeBaseProcessor.Verify(
            k => k.GetRelevantKnowledgeAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    /// <summary>
    /// Gets the agent chat response asynchronous vision enabled and agent has images includes image keywords in chat message.
    /// </summary>
    [Fact]
    public async Task GetAgentChatResponseAsync_VisionEnabledAndAgentHasImages_IncludesImageKeywordsInChatMessage()
    {
        // Arrange
        var service = new AgentChatService(
            _mockConfiguration.Object, _mockLogger.Object, _mockCorrelationContext.Object,
            _mockAgentsService.Object, _mockKnowledgeBaseProcessor.Object, _mockAiServices.Object, _mockToolSkillService.Object);

        var agent = TestsHelpers.BuildAgentData(withVisionImages: true);
        _mockAgentsService
            .Setup(s => s.GetAgentDataByIdAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(agent);

        ChatMessageDomain? capturedMessage = null;
        _mockAiServices
            .Setup(s => s.GetAiFunctionResponseAsync(It.IsAny<ChatMessageDomain>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback<ChatMessageDomain, string, string, CancellationToken>((msg, _, _, _) => capturedMessage = msg)
            .ReturnsAsync("response");

        // Act
        await service.GetAgentChatResponseAsync(TestsHelpers.BuildChatRequest());

        // Assert
        Assert.NotNull(capturedMessage);
        Assert.NotEmpty(capturedMessage.ImageKeyWords);
    }

    /// <summary>
    /// Gets the agent chat response asynchronous vision disabled does not include image keywords.
    /// </summary>
    [Fact]
    public async Task GetAgentChatResponseAsync_VisionDisabled_DoesNotIncludeImageKeywords()
    {
        _mockConfiguration
            .Setup(c => c[AzureAppConfigurationConstants.IsAiVisionServiceEnabledConstant])
            .Returns("false");
        var agent = TestsHelpers.BuildAgentData(withVisionImages: true);
        _mockAgentsService
            .Setup(s => s.GetAgentDataByIdAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(agent);

        ChatMessageDomain? capturedMessage = null;
        _mockAiServices
            .Setup(s => s.GetAiFunctionResponseAsync(It.IsAny<ChatMessageDomain>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback<ChatMessageDomain, string, string, CancellationToken>((msg, _, _, _) => capturedMessage = msg)
            .ReturnsAsync("response");

        // Act
        await _agentChatService.GetAgentChatResponseAsync(TestsHelpers.BuildChatRequest());

        // Assert
        Assert.NotNull(capturedMessage);
        Assert.Empty(capturedMessage.ImageKeyWords);
    }

    /// <summary>
    /// Gets the agent chat response asynchronous vision enabled but agent has no images does not include image keywords.
    /// </summary>
    [Fact]
    public async Task GetAgentChatResponseAsync_VisionEnabledButAgentHasNoImages_DoesNotIncludeImageKeywords()
    {
        // Arrange
        var service = new AgentChatService(
            _mockConfiguration.Object, _mockLogger.Object, _mockCorrelationContext.Object,
            _mockAgentsService.Object, _mockKnowledgeBaseProcessor.Object, _mockAiServices.Object, _mockToolSkillService.Object);

        var agent = TestsHelpers.BuildAgentData(withVisionImages: false);
        _mockAgentsService
            .Setup(s => s.GetAgentDataByIdAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(agent);

        ChatMessageDomain? capturedMessage = null;
        _mockAiServices
            .Setup(s => s.GetAiFunctionResponseAsync(It.IsAny<ChatMessageDomain>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback<ChatMessageDomain, string, string, CancellationToken>((msg, _, _, _) => capturedMessage = msg)
            .ReturnsAsync("response");

        // Act
        await service.GetAgentChatResponseAsync(TestsHelpers.BuildChatRequest());

        // Assert
        Assert.NotNull(capturedMessage);
        Assert.Empty(capturedMessage.ImageKeyWords);
    }

    /// <summary>
    /// Gets the agent chat response asynchronous with skills calls tool skill service and mcp ai service.
    /// </summary>
    [Fact]
    public async Task GetAgentChatResponseAsync_WithSkills_CallsToolSkillServiceAndMcpAiService()
    {
        // Arrange
        var agent = TestsHelpers.BuildAgentData(withSkills: true);
        var skill = new ToolSkillDomain { ToolSkillGuid = "skill-guid-1", ToolSkillMcpServerUrl = "https://mcp.example.com" };

        _mockAgentsService
            .Setup(s => s.GetAgentDataByIdAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(agent);
        _mockToolSkillService
            .Setup(t => t.GetToolSkillBySkillIdAsync("skill-guid-1", string.Empty, It.IsAny<CancellationToken>()))
            .ReturnsAsync(skill);
        _mockAiServices
            .Setup(s => s.GetAiFunctionResponseAsync(It.IsAny<ChatMessageDomain>(), skill.ToolSkillMcpServerUrl, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync("skill response");

        // Act
        var result = await _agentChatService.GetAgentChatResponseAsync(TestsHelpers.BuildChatRequest());

        // Assert
        Assert.Equal("skill response", result);
        _mockToolSkillService.Verify(
            t => t.GetToolSkillBySkillIdAsync("skill-guid-1", string.Empty, It.IsAny<CancellationToken>()),
            Times.Once);
        _mockAiServices.Verify(
            s => s.GetAiFunctionResponseAsync(
                It.IsAny<ChatMessageDomain>(),
                skill.ToolSkillMcpServerUrl,
                ApplicationPluginsHelpers.PluginName,
                ApplicationPluginsHelpers.GetChatMessageResponseFunction.FunctionName,
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );
    }

    /// <summary>
    /// Gets the agent chat response asynchronous with skills does not call direct ai service.
    /// </summary>
    [Fact]
    public async Task GetAgentChatResponseAsync_WithSkills_DoesNotCallDirectAiService()
    {
        // Arrange
        var agent = TestsHelpers.BuildAgentData(withSkills: true);
        var skill = new ToolSkillDomain { ToolSkillMcpServerUrl = "https://mcp.example.com" };

        _mockAgentsService
            .Setup(s => s.GetAgentDataByIdAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(agent);
        _mockToolSkillService
            .Setup(t => t.GetToolSkillBySkillIdAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(skill);
        _mockAiServices
            .Setup(s => s.GetAiFunctionResponseAsync(It.IsAny<ChatMessageDomain>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync("skill response");

        // Act
        await _agentChatService.GetAgentChatResponseAsync(TestsHelpers.BuildChatRequest());

        // Assert — 3-param overload (no MCP URL) must never be called
        _mockAiServices.Verify(
            s => s.GetAiFunctionResponseAsync(It.IsAny<ChatMessageDomain>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    /// <summary>
    /// Gets the agent chat response asynchronous with skills tool skill service returns null throws ai agents business exception.
    /// </summary>
    [Fact]
    public async Task GetAgentChatResponseAsync_WithSkills_ToolSkillServiceReturnsNull_ThrowsAIAgentsBusinessException()
    {
        // Arrange
        var agent = TestsHelpers.BuildAgentData(withSkills: true);
        _mockAgentsService
            .Setup(s => s.GetAgentDataByIdAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(agent);
        _mockToolSkillService
            .Setup(t => t.GetToolSkillBySkillIdAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((ToolSkillDomain)null!);

        // Act & Assert
        await Assert.ThrowsAsync<AIAgentsBusinessException>(() =>
            _agentChatService.GetAgentChatResponseAsync(TestsHelpers.BuildChatRequest()));
    }

    /// <summary>
    /// Gets the agent chat response asynchronous with skills tool skill has empty mcp url throws ai agents business exception.
    /// </summary>
    [Fact]
    public async Task GetAgentChatResponseAsync_WithSkills_ToolSkillHasEmptyMcpUrl_ThrowsAIAgentsBusinessException()
    {
        // Arrange
        var agent = TestsHelpers.BuildAgentData(withSkills: true);
        var skill = new ToolSkillDomain { ToolSkillMcpServerUrl = string.Empty };

        _mockAgentsService
            .Setup(s => s.GetAgentDataByIdAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(agent);
        _mockToolSkillService
            .Setup(t => t.GetToolSkillBySkillIdAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(skill);

        // Act & Assert
        await Assert.ThrowsAsync<AIAgentsBusinessException>(() =>
            _agentChatService.GetAgentChatResponseAsync(TestsHelpers.BuildChatRequest()));
    }

    /// <summary>
    /// Gets the agent chat response asynchronous with skills tool skill service throws exception throws ai agents business exception.
    /// </summary>
    [Fact]
    public async Task GetAgentChatResponseAsync_WithSkills_ToolSkillServiceThrowsException_ThrowsAIAgentsBusinessException()
    {
        // Arrange
        var agent = TestsHelpers.BuildAgentData(withSkills: true);
        _mockAgentsService
            .Setup(s => s.GetAgentDataByIdAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(agent);
        _mockToolSkillService
            .Setup(t => t.GetToolSkillBySkillIdAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Skill fetch failed"));

        // Act & Assert
        var ex = await Assert.ThrowsAsync<AIAgentsBusinessException>(() =>
            _agentChatService.GetAgentChatResponseAsync(TestsHelpers.BuildChatRequest()));

        Assert.Contains("Skill fetch failed", ex.Message);
    }

    /// <summary>
    /// Gets the agent chat response asynchronous with skills mcp ai service throws exception throws ai agents business exception.
    /// </summary>
    [Fact]
    public async Task GetAgentChatResponseAsync_WithSkills_McpAiServiceThrowsException_ThrowsAIAgentsBusinessException()
    {
        // Arrange
        var agent = TestsHelpers.BuildAgentData(withSkills: true);
        var skill = new ToolSkillDomain { ToolSkillMcpServerUrl = "https://mcp.example.com" };

        _mockAgentsService
            .Setup(s => s.GetAgentDataByIdAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(agent);
        _mockToolSkillService
            .Setup(t => t.GetToolSkillBySkillIdAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(skill);
        _mockAiServices
            .Setup(s => s.GetAiFunctionResponseAsync(It.IsAny<ChatMessageDomain>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("MCP AI error"));

        // Act & Assert
        var ex = await Assert.ThrowsAsync<AIAgentsBusinessException>(() =>
            _agentChatService.GetAgentChatResponseAsync(TestsHelpers.BuildChatRequest()));

        Assert.Contains("MCP AI error", ex.Message);
    }


    /// <summary>
    /// Gets the agent chat response asynchronous builds chat message with correct agent meta prompt and user message.
    /// </summary>
    [Fact]
    public async Task GetAgentChatResponseAsync_BuildsChatMessageWithCorrectAgentMetaPromptAndUserMessage()
    {
        // Arrange
        var userMessage = "What is the weather today?";
        var agent = TestsHelpers.BuildAgentData();
        _mockAgentsService
            .Setup(s => s.GetAgentDataByIdAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(agent);

        ChatMessageDomain? capturedMessage = null;
        _mockAiServices
            .Setup(s => s.GetAiFunctionResponseAsync(It.IsAny<ChatMessageDomain>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback<ChatMessageDomain, string, string, CancellationToken>((msg, _, _, _) => capturedMessage = msg)
            .ReturnsAsync("response");

        // Act
        await _agentChatService.GetAgentChatResponseAsync(TestsHelpers.BuildChatRequest(userMessage: userMessage));

        // Assert
        Assert.NotNull(capturedMessage);
        Assert.Equal(agent.AgentMetaPrompt, capturedMessage.AgentMetaPrompt);
        Assert.Equal(agent.AgentName, capturedMessage.AgentName);
        Assert.Equal(userMessage, capturedMessage.UserMessage);
    }

    /// <summary>
    /// Gets the agent chat response asynchronous with cancellation token passes token through pipeline.
    /// </summary>
    [Fact]
    public async Task GetAgentChatResponseAsync_WithCancellationToken_PassesTokenThroughPipeline()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        var agent = TestsHelpers.BuildAgentData();
        _mockAgentsService
            .Setup(s => s.GetAgentDataByIdAsync(It.IsAny<string>(), It.IsAny<string>(), cts.Token))
            .ReturnsAsync(agent);
        _mockAiServices
            .Setup(s => s.GetAiFunctionResponseAsync(It.IsAny<ChatMessageDomain>(), It.IsAny<string>(), It.IsAny<string>(), cts.Token))
            .ReturnsAsync("response");

        // Act
        await _agentChatService.GetAgentChatResponseAsync(TestsHelpers.BuildChatRequest(), cts.Token);

        // Assert
        _mockAgentsService.Verify(
            s => s.GetAgentDataByIdAsync(It.IsAny<string>(), It.IsAny<string>(), cts.Token),
            Times.Once);
        _mockAiServices.Verify(
            s => s.GetAiFunctionResponseAsync(It.IsAny<ChatMessageDomain>(), It.IsAny<string>(), It.IsAny<string>(), cts.Token),
            Times.Once);
    }

}
