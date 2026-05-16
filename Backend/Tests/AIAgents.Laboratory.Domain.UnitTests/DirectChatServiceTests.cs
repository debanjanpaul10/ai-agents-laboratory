using AIAgents.Laboratory.Domain.Contracts;
using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.Domain.Helpers;
using AIAgents.Laboratory.Domain.Ports.In;
using AIAgents.Laboratory.Domain.Ports.Out;
using AIAgents.Laboratory.Domain.UseCases;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace AIAgents.Laboratory.Domain.UnitTests;

/// <summary>
/// The Direct Chat Services Tests class.
/// </summary>
public sealed class DirectChatServiceTests
{
    /// <summary>
    /// The test user email.
    /// </summary>
    private readonly string TestUserEmail = TestsHelpers.TestUserEmail;

    /// <summary>
    /// The mock logger.
    /// </summary>
    private readonly Mock<ILogger<DirectChatService>> _mockLogger = new();

    /// <summary>
    /// The mock configuration.
    /// </summary>
    private readonly Mock<IConfiguration> _mockConfiguration = new();

    /// <summary>
    /// The mock agents service.
    /// </summary>
    private readonly Mock<IAgentsService> _mockAgentsService = new();

    /// <summary>
    /// The mock correlation context.
    /// </summary>
    private readonly Mock<ICorrelationContext> _mockCorrelationContext = new();

    /// <summary>
    /// The mock ai services.
    /// </summary>
    private readonly Mock<IAiServices> _mockAiServices = new();

    /// <summary>
    /// The mock conversation history service.
    /// </summary>
    private readonly Mock<IConversationHistoryService> _mockConversationHistoryService = new();

    /// <summary>
    /// The direct chat service.
    /// </summary>
    private readonly DirectChatService _directChatService;

    /// <summary>
    /// Initializes a new instance of the <see cref="DirectChatServiceTests"/> class.
    /// </summary>
    public DirectChatServiceTests()
    {
        _directChatService = new DirectChatService(
            _mockLogger.Object,
            _mockConfiguration.Object,
            _mockAgentsService.Object,
            _mockCorrelationContext.Object,
            _mockAiServices.Object,
            _mockConversationHistoryService.Object);

        _mockCorrelationContext.Setup(c => c.CorrelationId).Returns(Guid.NewGuid().ToString());
    }

    /// <summary>
    /// Gets the direct chat response asynchronous null or white space user query throws argument exception.
    /// </summary>
    /// <param name="userQuery">The user query.</param>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task GetDirectChatResponseAsync_NullOrWhiteSpaceUserQuery_ThrowsArgumentException(string? userQuery)
    {
        // Act & Assert
        await Assert.ThrowsAnyAsync<ArgumentException>(() =>
            _directChatService.GetDirectChatResponseAsync(userQuery!, TestUserEmail)
        );
    }

    /// <summary>
    /// Gets the direct chat response asynchronous null or white space user email throws argument exception.
    /// </summary>
    /// <param name="userEmail">The user email.</param>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task GetDirectChatResponseAsync_NullOrWhiteSpaceUserEmail_ThrowsArgumentException(string? userEmail)
    {
        // Act & Assert
        await Assert.ThrowsAnyAsync<ArgumentException>(() =>
           _directChatService.GetDirectChatResponseAsync("hello", userEmail!)
        );
    }

    /// <summary>
    /// Gets the direct chat response asynchronous missing configuration throws ai agents business exception.
    /// </summary>
    [Fact]
    public async Task GetDirectChatResponseAsync_MissingConfiguration_ThrowsAIAgentsBusinessException()
    {
        // Arrange
        _mockConfiguration
            .Setup(c => c[It.IsAny<string>()])
            .Returns((string)null!);

        // Act & Assert
        await Assert.ThrowsAsync<AIAgentsBusinessException>(() =>
            _directChatService.GetDirectChatResponseAsync("hello", TestUserEmail)
        );
    }

    /// <summary>
    /// Gets the direct chat response asynchronous service throws exception throws ai agents business exception.
    /// </summary>
    [Fact]
    public async Task GetDirectChatResponseAsync_ServiceThrowsException_ThrowsAIAgentsBusinessException()
    {
        // Arrange
        _mockConfiguration
            .Setup(c => c[It.IsAny<string>()])
            .Returns("chatbot-agent-id");
        _mockAgentsService
            .Setup(s => s.GetAgentDataByIdAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Mocked exception"));

        // Act & Assert
        var ex = await Assert.ThrowsAsync<AIAgentsBusinessException>(() =>
            _directChatService.GetDirectChatResponseAsync("hello", TestUserEmail)
        );

        Assert.Contains("Mocked exception", ex.Message);
    }

    /// <summary>
    /// Gets the direct chat response asynchronous success returns ai response and saves history.
    /// </summary>
    [Fact]
    public async Task GetDirectChatResponseAsync_Success_ReturnsAiResponseAndSavesHistory()
    {
        // Arrange
        var userQuery = "hello ai";
        var userEmail = "testuser@example.com";
        var aiResponse = "hello human";
        var agentData = TestsHelpers.GetAgentDataDomain();
        var conversationData = TestsHelpers.GetConversationHistoryDomain(userEmail);

        _mockConfiguration
            .Setup(c => c[It.IsAny<string>()])
            .Returns("chatbot-agent-id");

        _mockAgentsService
            .Setup(s => s.GetAgentDataByIdAsync("chatbot-agent-id", userEmail, It.IsAny<CancellationToken>()))
            .ReturnsAsync(agentData);

        _mockConversationHistoryService
            .Setup(s => s.GetConversationHistoryAsync(userEmail, It.IsAny<CancellationToken>()))
            .ReturnsAsync(conversationData);

        _mockAiServices
            .Setup(s => s.GetChatbotResponseAsync(conversationData, userQuery, agentData.AgentMetaPrompt, It.IsAny<CancellationToken>()))
            .ReturnsAsync(aiResponse);

        // Act
        var result = await _directChatService.GetDirectChatResponseAsync(userQuery, userEmail);

        // Assert
        Assert.Equal(aiResponse, result);
        Assert.Contains(conversationData.ChatHistory, c => c.Content == aiResponse);

        _mockConversationHistoryService
            .Verify(s => s.SaveMessageToConversationHistoryAsync(It.Is<ConversationHistoryDomain>(d => d == conversationData), It.IsAny<CancellationToken>()), Times.Once);
    }

    /// <summary>
    /// Clears the conversation history for user asynchronous null or white space user name throws argument exception.
    /// </summary>
    /// <param name="userName">Name of the user.</param>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task ClearConversationHistoryForUserAsync_NullOrWhiteSpaceUserName_ThrowsArgumentException(string? userName)
    {
        // Act & Assert
        await Assert.ThrowsAnyAsync<ArgumentException>(() =>
            _directChatService.ClearConversationHistoryForUserAsync(userName!)
        );
    }

    /// <summary>
    /// Clears the conversation history for user asynchronous service throws exception throws ai agents business exception.
    /// </summary>
    [Fact]
    public async Task ClearConversationHistoryForUserAsync_ServiceThrowsException_ThrowsAIAgentsBusinessException()
    {
        // Arrange
        _mockConversationHistoryService
            .Setup(s => s.ClearConversationHistoryForUserAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Clear error"));

        // Act & Assert
        var ex = await Assert.ThrowsAsync<AIAgentsBusinessException>(() =>
            _directChatService.ClearConversationHistoryForUserAsync(TestUserEmail)
        );
        Assert.Contains("Clear error", ex.Message);
    }

    /// <summary>
    /// Clears the conversation history for user asynchronous success returns true.
    /// </summary>
    [Fact]
    public async Task ClearConversationHistoryForUserAsync_Success_ReturnsTrue()
    {
        // Arrange
        _mockConversationHistoryService
            .Setup(s => s.ClearConversationHistoryForUserAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _directChatService.ClearConversationHistoryForUserAsync(TestUserEmail);

        // Assert
        Assert.True(result);
        _mockConversationHistoryService
            .Verify(s => s.ClearConversationHistoryForUserAsync(TestUserEmail, It.IsAny<CancellationToken>()), Times.Once);
    }

    /// <summary>
    /// Gets the conversation history data asynchronous null or white space user name throws argument exception.
    /// </summary>
    /// <param name="userName">Name of the user.</param>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task GetConversationHistoryDataAsync_NullOrWhiteSpaceUserName_ThrowsArgumentException(string? userName)
    {
        // Act & Assert
        await Assert.ThrowsAnyAsync<ArgumentException>(() =>
            _directChatService.GetConversationHistoryDataAsync(userName!)
        );
    }

    /// <summary>
    /// Gets the conversation history data asynchronous service throws exception throws ai agents business exception.
    /// </summary>
    [Fact]
    public async Task GetConversationHistoryDataAsync_ServiceThrowsException_ThrowsAIAgentsBusinessException()
    {
        // Arrange
        _mockConversationHistoryService
            .Setup(s => s.GetConversationHistoryAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Get error"));

        // Act & Assert
        var ex = await Assert.ThrowsAsync<AIAgentsBusinessException>(() =>
            _directChatService.GetConversationHistoryDataAsync(TestUserEmail)
        );
        Assert.Contains("Get error", ex.Message);
    }

    /// <summary>
    /// Gets the conversation history data asynchronous service returns null returns new domain.
    /// </summary>
    [Fact]
    public async Task GetConversationHistoryDataAsync_ServiceReturnsNull_ReturnsNewDomain()
    {
        // Arrange
        _mockConversationHistoryService
            .Setup(s => s.GetConversationHistoryAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((ConversationHistoryDomain)null!);

        // Act
        var result = await _directChatService.GetConversationHistoryDataAsync(TestUserEmail);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<ConversationHistoryDomain>(result);
    }

    /// <summary>
    /// Gets the conversation history data asynchronous success returns conversation history.
    /// </summary>
    [Fact]
    public async Task GetConversationHistoryDataAsync_Success_ReturnsConversationHistory()
    {
        // Arrange
        var historyDomain = TestsHelpers.GetConversationHistoryDomain(TestUserEmail);
        _mockConversationHistoryService
            .Setup(s => s.GetConversationHistoryAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(historyDomain);

        // Act
        var result = await _directChatService.GetConversationHistoryDataAsync(TestUserEmail);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(historyDomain, result);
        Assert.Equal(TestUserEmail, result.UserName);
    }
}
