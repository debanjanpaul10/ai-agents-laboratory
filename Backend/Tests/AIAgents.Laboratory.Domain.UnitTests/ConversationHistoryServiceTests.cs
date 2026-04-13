using AIAgents.Laboratory.Domain.Contracts;
using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.Domain.Helpers;
using AIAgents.Laboratory.Domain.Ports.Out;
using AIAgents.Laboratory.Domain.UseCases;
using Microsoft.Extensions.Logging;
using Moq;

namespace AIAgents.Laboratory.Domain.UnitTests;

/// <summary>
/// The Conversation History Service Tests class.
/// </summary>
public sealed class ConversationHistoryServiceTests
{
    /// <summary>
    /// The mock logger.
    /// </summary>
    private readonly Mock<ILogger<ConversationHistoryService>> _mockLogger = new();

    /// <summary>
    /// The mock correlation context.
    /// </summary>
    private readonly Mock<ICorrelationContext> _mockCorrelationContext = new();

    /// <summary>
    /// The mock data manager.
    /// </summary>
    private readonly Mock<IConversationHistoryDataManager> _mockDataManager = new();

    /// <summary>
    /// The service.
    /// </summary>
    private readonly ConversationHistoryService _service;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConversationHistoryServiceTests"/> class.
    /// </summary>
    public ConversationHistoryServiceTests()
    {
        _mockCorrelationContext
            .Setup(c => c.CorrelationId)
            .Returns(Guid.NewGuid().ToString());

        _service = new ConversationHistoryService(
            _mockLogger.Object,
            _mockCorrelationContext.Object,
            _mockDataManager.Object);
    }

    #region GetConversationHistoryAsync

    /// <summary>
    /// Gets the conversation history asynchronous null or white space user name throws argument exception.
    /// </summary>
    /// <param name="userName">Name of the user.</param>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task GetConversationHistoryAsync_NullOrWhiteSpaceUserName_ThrowsArgumentException(string? userName)
    {
        // Act & Assert
        await Assert.ThrowsAnyAsync<ArgumentException>(() =>
            _service.GetConversationHistoryAsync(userName!));
    }

    /// <summary>
    /// Gets the conversation history asynchronous success returns conversation history domain.
    /// </summary>
    [Fact]
    public async Task GetConversationHistoryAsync_Success_ReturnsConversationHistoryDomain()
    {
        // Arrange
        var expected = TestsHelpers.GetConversationHistoryDomain(TestsHelpers.TestUserEmail);
        _mockDataManager
            .Setup(d => d.GetConversationHistoryAsync(TestsHelpers.TestUserEmail, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        // Act
        var result = await _service.GetConversationHistoryAsync(TestsHelpers.TestUserEmail);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expected, result);
        Assert.Equal(TestsHelpers.TestUserEmail, result.UserName);
    }

    /// <summary>
    /// Gets the conversation history asynchronous data manager returns null returns null.
    /// </summary>
    [Fact]
    public async Task GetConversationHistoryAsync_DataManagerReturnsNull_ReturnsNull()
    {
        // Arrange
        _mockDataManager
            .Setup(d => d.GetConversationHistoryAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((ConversationHistoryDomain)null!);

        // Act
        var result = await _service.GetConversationHistoryAsync(TestsHelpers.TestUserEmail);

        // Assert
        Assert.Null(result);
    }

    /// <summary>
    /// Gets the conversation history asynchronous data manager throws exception throws ai agents business exception.
    /// </summary>
    [Fact]
    public async Task GetConversationHistoryAsync_DataManagerThrowsException_ThrowsAIAgentsBusinessException()
    {
        // Arrange
        _mockDataManager
            .Setup(d => d.GetConversationHistoryAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("DB error"));

        // Act & Assert
        var ex = await Assert.ThrowsAsync<AIAgentsBusinessException>(() =>
            _service.GetConversationHistoryAsync(TestsHelpers.TestUserEmail));

        Assert.Contains("DB error", ex.Message);
    }

    /// <summary>
    /// Gets the conversation history asynchronous passes correct user name to data manager.
    /// </summary>
    [Theory]
    [InlineData("user1@example.com")]
    [InlineData("user2@example.com")]
    [InlineData("admin@example.com")]
    public async Task GetConversationHistoryAsync_PassesCorrectUserNameToDataManager(string userName)
    {
        // Arrange
        _mockDataManager
            .Setup(d => d.GetConversationHistoryAsync(userName, It.IsAny<CancellationToken>()))
            .ReturnsAsync(TestsHelpers.GetConversationHistoryDomain(userName));

        // Act
        await _service.GetConversationHistoryAsync(userName);

        // Assert
        _mockDataManager.Verify(
            d => d.GetConversationHistoryAsync(userName, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    /// <summary>
    /// Gets the conversation history asynchronous with cancellation token passes token to data manager.
    /// </summary>
    [Fact]
    public async Task GetConversationHistoryAsync_WithCancellationToken_PassesTokenToDataManager()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        _mockDataManager
            .Setup(d => d.GetConversationHistoryAsync(It.IsAny<string>(), cts.Token))
            .ReturnsAsync(TestsHelpers.GetConversationHistoryDomain(TestsHelpers.TestUserEmail));

        // Act
        await _service.GetConversationHistoryAsync(TestsHelpers.TestUserEmail, cts.Token);

        // Assert
        _mockDataManager.Verify(
            d => d.GetConversationHistoryAsync(It.IsAny<string>(), cts.Token),
            Times.Once);
    }

    #endregion

    #region SaveMessageToConversationHistoryAsync

    /// <summary>
    /// Saves the message to conversation history asynchronous null conversation history throws argument null exception.
    /// </summary>
    [Fact]
    public async Task SaveMessageToConversationHistoryAsync_NullConversationHistory_ThrowsArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAnyAsync<ArgumentNullException>(() =>
            _service.SaveMessageToConversationHistoryAsync(null!));
    }

    /// <summary>
    /// Saves the message to conversation history asynchronous success returns true.
    /// </summary>
    [Fact]
    public async Task SaveMessageToConversationHistoryAsync_Success_ReturnsTrue()
    {
        // Arrange
        var history = TestsHelpers.GetConversationHistoryDomain(TestsHelpers.TestUserEmail);
        _mockDataManager
            .Setup(d => d.SaveMessageToConversationHistoryAsync(history, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _service.SaveMessageToConversationHistoryAsync(history);

        // Assert
        Assert.True(result);
        _mockDataManager.Verify(
            d => d.SaveMessageToConversationHistoryAsync(history, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    /// <summary>
    /// Saves the message to conversation history asynchronous data manager returns false returns false.
    /// </summary>
    [Fact]
    public async Task SaveMessageToConversationHistoryAsync_DataManagerReturnsFalse_ReturnsFalse()
    {
        // Arrange
        var history = TestsHelpers.GetConversationHistoryDomain(TestsHelpers.TestUserEmail);
        _mockDataManager
            .Setup(d => d.SaveMessageToConversationHistoryAsync(It.IsAny<ConversationHistoryDomain>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _service.SaveMessageToConversationHistoryAsync(history);

        // Assert
        Assert.False(result);
    }

    /// <summary>
    /// Saves the message to conversation history asynchronous data manager throws exception throws ai agents business exception.
    /// </summary>
    [Fact]
    public async Task SaveMessageToConversationHistoryAsync_DataManagerThrowsException_ThrowsAIAgentsBusinessException()
    {
        // Arrange
        var history = TestsHelpers.GetConversationHistoryDomain(TestsHelpers.TestUserEmail);
        _mockDataManager
            .Setup(d => d.SaveMessageToConversationHistoryAsync(It.IsAny<ConversationHistoryDomain>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Save failed"));

        // Act & Assert
        var ex = await Assert.ThrowsAsync<AIAgentsBusinessException>(() =>
            _service.SaveMessageToConversationHistoryAsync(history));

        Assert.Contains("Save failed", ex.Message);
    }

    /// <summary>
    /// Saves the message to conversation history asynchronous passes correct domain to data manager.
    /// </summary>
    [Fact]
    public async Task SaveMessageToConversationHistoryAsync_PassesCorrectDomainToDataManager()
    {
        // Arrange
        var history = TestsHelpers.GetConversationHistoryDomain(TestsHelpers.TestUserEmail);
        _mockDataManager
            .Setup(d => d.SaveMessageToConversationHistoryAsync(history, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        await _service.SaveMessageToConversationHistoryAsync(history);

        // Assert
        _mockDataManager.Verify(
            d => d.SaveMessageToConversationHistoryAsync(
                It.Is<ConversationHistoryDomain>(h => h.ConversationId == history.ConversationId),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    /// <summary>
    /// Saves the message to conversation history asynchronous with cancellation token passes token to data manager.
    /// </summary>
    [Fact]
    public async Task SaveMessageToConversationHistoryAsync_WithCancellationToken_PassesTokenToDataManager()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        var history = TestsHelpers.GetConversationHistoryDomain(TestsHelpers.TestUserEmail);
        _mockDataManager
            .Setup(d => d.SaveMessageToConversationHistoryAsync(It.IsAny<ConversationHistoryDomain>(), cts.Token))
            .ReturnsAsync(true);

        // Act
        await _service.SaveMessageToConversationHistoryAsync(history, cts.Token);

        // Assert
        _mockDataManager.Verify(
            d => d.SaveMessageToConversationHistoryAsync(It.IsAny<ConversationHistoryDomain>(), cts.Token),
            Times.Once);
    }

    #endregion

    #region ClearConversationHistoryForUserAsync

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
            _service.ClearConversationHistoryForUserAsync(userName!));
    }

    /// <summary>
    /// Clears the conversation history for user asynchronous success returns true.
    /// </summary>
    [Fact]
    public async Task ClearConversationHistoryForUserAsync_Success_ReturnsTrue()
    {
        // Arrange
        _mockDataManager
            .Setup(d => d.ClearConversationHistoryForUserAsync(TestsHelpers.TestUserEmail, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _service.ClearConversationHistoryForUserAsync(TestsHelpers.TestUserEmail);

        // Assert
        Assert.True(result);
        _mockDataManager.Verify(
            d => d.ClearConversationHistoryForUserAsync(TestsHelpers.TestUserEmail, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    /// <summary>
    /// Clears the conversation history for user asynchronous data manager returns false returns false.
    /// </summary>
    [Fact]
    public async Task ClearConversationHistoryForUserAsync_DataManagerReturnsFalse_ReturnsFalse()
    {
        // Arrange
        _mockDataManager
            .Setup(d => d.ClearConversationHistoryForUserAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _service.ClearConversationHistoryForUserAsync(TestsHelpers.TestUserEmail);

        // Assert
        Assert.False(result);
    }

    /// <summary>
    /// Clears the conversation history for user asynchronous data manager throws exception throws ai agents business exception.
    /// </summary>
    [Fact]
    public async Task ClearConversationHistoryForUserAsync_DataManagerThrowsException_ThrowsAIAgentsBusinessException()
    {
        // Arrange
        _mockDataManager
            .Setup(d => d.ClearConversationHistoryForUserAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Clear failed"));

        // Act & Assert
        var ex = await Assert.ThrowsAsync<AIAgentsBusinessException>(() =>
            _service.ClearConversationHistoryForUserAsync(TestsHelpers.TestUserEmail));

        Assert.Contains("Clear failed", ex.Message);
    }

    /// <summary>
    /// Clears the conversation history for user asynchronous passes correct user name to data manager.
    /// </summary>
    [Theory]
    [InlineData("user1@example.com")]
    [InlineData("user2@example.com")]
    [InlineData("admin@example.com")]
    public async Task ClearConversationHistoryForUserAsync_PassesCorrectUserNameToDataManager(string userName)
    {
        // Arrange
        _mockDataManager
            .Setup(d => d.ClearConversationHistoryForUserAsync(userName, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        await _service.ClearConversationHistoryForUserAsync(userName);

        // Assert
        _mockDataManager.Verify(
            d => d.ClearConversationHistoryForUserAsync(userName, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    /// <summary>
    /// Clears the conversation history for user asynchronous with cancellation token passes token to data manager.
    /// </summary>
    [Fact]
    public async Task ClearConversationHistoryForUserAsync_WithCancellationToken_PassesTokenToDataManager()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        _mockDataManager
            .Setup(d => d.ClearConversationHistoryForUserAsync(It.IsAny<string>(), cts.Token))
            .ReturnsAsync(true);

        // Act
        await _service.ClearConversationHistoryForUserAsync(TestsHelpers.TestUserEmail, cts.Token);

        // Assert
        _mockDataManager.Verify(
            d => d.ClearConversationHistoryForUserAsync(It.IsAny<string>(), cts.Token),
            Times.Once);
    }

    #endregion
}
