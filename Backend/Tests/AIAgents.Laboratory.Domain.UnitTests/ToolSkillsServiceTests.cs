using AIAgents.Laboratory.Domain.Contracts;
using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.Domain.Helpers;
using AIAgents.Laboratory.Domain.Ports.In;
using AIAgents.Laboratory.Domain.Ports.Out;
using AIAgents.Laboratory.Domain.UseCases;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Client;
using Moq;

namespace AIAgents.Laboratory.Domain.UnitTests;

/// <summary>
/// The Tool Skills Service Tests class.
/// </summary>
public sealed class ToolSkillsServiceTests
{
    /// <summary>
    /// The mock logger.
    /// </summary>
    private readonly Mock<ILogger<ToolSkillsService>> _mockLogger = new();

    /// <summary>
    /// The mock correlation context.
    /// </summary>
    private readonly Mock<ICorrelationContext> _mockCorrelationContext = new();

    /// <summary>
    /// The mock data manager.
    /// </summary>
    private readonly Mock<IToolSkillsDataManager> _mockDataManager = new();

    /// <summary>
    /// The mock MCP client services.
    /// </summary>
    private readonly Mock<IMcpClientServices> _mockMcpClientServices = new();

    /// <summary>
    /// The mock notifications service.
    /// </summary>
    private readonly Mock<INotificationsService> _mockNotificationsService = new();

    /// <summary>
    /// The tools skill service.
    /// </summary>
    private readonly ToolSkillsService _toolSkillsService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ToolSkillsServiceTests"/> class.
    /// </summary>
    public ToolSkillsServiceTests()
    {
        _mockCorrelationContext
            .Setup(c => c.CorrelationId)
            .Returns(Guid.NewGuid().ToString());

        _toolSkillsService = new ToolSkillsService(
            _mockLogger.Object,
            _mockCorrelationContext.Object,
            _mockDataManager.Object,
            _mockMcpClientServices.Object,
            _mockNotificationsService.Object);
    }

    #region AddNewToolSkillAsync

    /// <summary>
    /// Adds the new tool skill asynchronous success assigns guid and returns true.
    /// </summary>
    [Fact]
    public async Task AddNewToolSkillAsync_Success_AssignsGuidAndReturnsTrue()
    {
        // Arrange
        var skill = TestsHelpers.BuildToolSkill(TestsHelpers.ToolSkillGuid);
        skill.ToolSkillGuid = string.Empty;

        _mockDataManager
            .Setup(d => d.AddNewToolSkillAsync(It.IsAny<ToolSkillDomain>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _toolSkillsService.AddNewToolSkillAsync(skill, TestsHelpers.TestUserEmail);

        // Assert
        Assert.True(result);
        Assert.NotEmpty(skill.ToolSkillGuid);
        _mockDataManager.Verify(
            d => d.AddNewToolSkillAsync(skill, TestsHelpers.TestUserEmail, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    /// <summary>
    /// Adds the new tool skill asynchronous data manager returns false returns false.
    /// </summary>
    [Fact]
    public async Task AddNewToolSkillAsync_DataManagerReturnsFalse_ReturnsFalse()
    {
        // Arrange
        var skill = TestsHelpers.BuildToolSkill(TestsHelpers.ToolSkillGuid);
        _mockDataManager
            .Setup(d => d.AddNewToolSkillAsync(It.IsAny<ToolSkillDomain>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _toolSkillsService.AddNewToolSkillAsync(skill, TestsHelpers.TestUserEmail);

        // Assert
        Assert.False(result);
    }

    /// <summary>
    /// Adds the new tool skill asynchronous data manager throws exception throws ai agents business exception.
    /// </summary>
    [Fact]
    public async Task AddNewToolSkillAsync_DataManagerThrowsException_ThrowsAIAgentsBusinessException()
    {
        // Arrange
        var skill = TestsHelpers.BuildToolSkill(TestsHelpers.ToolSkillGuid);
        _mockDataManager
            .Setup(d => d.AddNewToolSkillAsync(It.IsAny<ToolSkillDomain>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("DB insert failed"));

        // Act & Assert
        var ex = await Assert.ThrowsAsync<AIAgentsBusinessException>(() =>
            _toolSkillsService.AddNewToolSkillAsync(skill, TestsHelpers.TestUserEmail));

        Assert.Contains("DB insert failed", ex.Message);
    }

    /// <summary>
    /// Adds the new tool skill asynchronous with cancellation token passes token to data manager.
    /// </summary>
    [Fact]
    public async Task AddNewToolSkillAsync_WithCancellationToken_PassesTokenToDataManager()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        var skill = TestsHelpers.BuildToolSkill(TestsHelpers.ToolSkillGuid);
        _mockDataManager
            .Setup(d => d.AddNewToolSkillAsync(It.IsAny<ToolSkillDomain>(), It.IsAny<string>(), cts.Token))
            .ReturnsAsync(true);

        // Act
        await _toolSkillsService.AddNewToolSkillAsync(skill, TestsHelpers.TestUserEmail, cts.Token);

        // Assert
        _mockDataManager.Verify(
            d => d.AddNewToolSkillAsync(It.IsAny<ToolSkillDomain>(), It.IsAny<string>(), cts.Token),
            Times.Once);
    }

    #endregion

    #region GetAllToolSkillsAsync

    /// <summary>
    /// Gets all tool skills asynchronous success returns list of tool skills.
    /// </summary>
    [Fact]
    public async Task GetAllToolSkillsAsync_Success_ReturnsListOfToolSkills()
    {
        // Arrange
        var skills = new List<ToolSkillDomain> { TestsHelpers.BuildToolSkill("s1"), TestsHelpers.BuildToolSkill("s2") };
        _mockDataManager
            .Setup(d => d.GetAllToolSkillsAsync(TestsHelpers.TestUserEmail, It.IsAny<CancellationToken>()))
            .ReturnsAsync(skills);

        // Act
        var result = await _toolSkillsService.GetAllToolSkillsAsync(TestsHelpers.TestUserEmail);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _mockDataManager.Verify(
            d => d.GetAllToolSkillsAsync(TestsHelpers.TestUserEmail, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    /// <summary>
    /// Gets all tool skills asynchronous data manager returns empty list returns empty.
    /// </summary>
    [Fact]
    public async Task GetAllToolSkillsAsync_DataManagerReturnsEmptyList_ReturnsEmpty()
    {
        // Arrange
        _mockDataManager
            .Setup(d => d.GetAllToolSkillsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        // Act
        var result = await _toolSkillsService.GetAllToolSkillsAsync(TestsHelpers.TestUserEmail);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    /// <summary>
    /// Gets all tool skills asynchronous data manager throws exception throws ai agents business exception.
    /// </summary>
    [Fact]
    public async Task GetAllToolSkillsAsync_DataManagerThrowsException_ThrowsAIAgentsBusinessException()
    {
        // Arrange
        _mockDataManager
            .Setup(d => d.GetAllToolSkillsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("DB read failed"));

        // Act & Assert
        var ex = await Assert.ThrowsAsync<AIAgentsBusinessException>(() =>
            _toolSkillsService.GetAllToolSkillsAsync(TestsHelpers.TestUserEmail));

        Assert.Contains("DB read failed", ex.Message);
    }

    /// <summary>
    /// Gets all tool skills asynchronous with cancellation token passes token to data manager.
    /// </summary>
    [Fact]
    public async Task GetAllToolSkillsAsync_WithCancellationToken_PassesTokenToDataManager()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        _mockDataManager
            .Setup(d => d.GetAllToolSkillsAsync(It.IsAny<string>(), cts.Token))
            .ReturnsAsync([]);

        // Act
        await _toolSkillsService.GetAllToolSkillsAsync(TestsHelpers.TestUserEmail, cts.Token);

        // Assert
        _mockDataManager.Verify(
            d => d.GetAllToolSkillsAsync(It.IsAny<string>(), cts.Token),
            Times.Once);
    }

    #endregion

    #region GetToolSkillBySkillIdAsync

    /// <summary>
    /// Gets the tool skill by skill identifier asynchronous null or white space skill id throws argument exception.
    /// </summary>
    /// <param name="skillId">The skill identifier.</param>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task GetToolSkillBySkillIdAsync_NullOrWhiteSpaceSkillId_ThrowsArgumentException(string? skillId)
    {
        // Act & Assert
        await Assert.ThrowsAnyAsync<ArgumentException>(() =>
            _toolSkillsService.GetToolSkillBySkillIdAsync(skillId!, TestsHelpers.TestUserEmail));
    }

    /// <summary>
    /// Gets the tool skill by skill identifier asynchronous success returns tool skill domain.
    /// </summary>
    [Fact]
    public async Task GetToolSkillBySkillIdAsync_Success_ReturnsToolSkillDomain()
    {
        // Arrange
        var skill = TestsHelpers.BuildToolSkill(TestsHelpers.ToolSkillGuid);
        _mockDataManager
            .Setup(d => d.GetToolSkillBySkillIdAsync(skill.ToolSkillGuid, TestsHelpers.TestUserEmail, It.IsAny<CancellationToken>()))
            .ReturnsAsync(skill);

        // Act
        var result = await _toolSkillsService.GetToolSkillBySkillIdAsync(skill.ToolSkillGuid, TestsHelpers.TestUserEmail);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(skill.ToolSkillGuid, result.ToolSkillGuid);
        _mockDataManager.Verify(
            d => d.GetToolSkillBySkillIdAsync(skill.ToolSkillGuid, TestsHelpers.TestUserEmail, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    /// <summary>
    /// Gets the tool skill by skill identifier asynchronous data manager returns null returns null.
    /// </summary>
    [Fact]
    public async Task GetToolSkillBySkillIdAsync_DataManagerReturnsNull_ReturnsNull()
    {
        // Arrange
        _mockDataManager
            .Setup(d => d.GetToolSkillBySkillIdAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((ToolSkillDomain)null!);

        // Act
        var result = await _toolSkillsService.GetToolSkillBySkillIdAsync("skill-guid-1", TestsHelpers.TestUserEmail);

        // Assert
        Assert.Null(result);
    }

    /// <summary>
    /// Gets the tool skill by skill identifier asynchronous data manager throws exception throws ai agents business exception.
    /// </summary>
    [Fact]
    public async Task GetToolSkillBySkillIdAsync_DataManagerThrowsException_ThrowsAIAgentsBusinessException()
    {
        // Arrange
        _mockDataManager
            .Setup(d => d.GetToolSkillBySkillIdAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Skill not found"));

        // Act & Assert
        var ex = await Assert.ThrowsAsync<AIAgentsBusinessException>(() =>
            _toolSkillsService.GetToolSkillBySkillIdAsync("skill-guid-1", TestsHelpers.TestUserEmail));

        Assert.Contains("Skill not found", ex.Message);
    }

    /// <summary>
    /// Gets the tool skill by skill identifier asynchronous passes correct parameters to data manager.
    /// </summary>
    [Theory]
    [InlineData("skill-a", "user1@example.com")]
    [InlineData("skill-b", "user2@example.com")]
    public async Task GetToolSkillBySkillIdAsync_PassesCorrectParametersToDataManager(string skillId, string userEmail)
    {
        // Arrange
        _mockDataManager
            .Setup(d => d.GetToolSkillBySkillIdAsync(skillId, userEmail, It.IsAny<CancellationToken>()))
            .ReturnsAsync(TestsHelpers.BuildToolSkill(skillId));

        // Act
        await _toolSkillsService.GetToolSkillBySkillIdAsync(skillId, userEmail);

        // Assert
        _mockDataManager.Verify(
            d => d.GetToolSkillBySkillIdAsync(skillId, userEmail, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    #endregion

    #region UpdateExistingToolSkillDataAsync

    /// <summary>
    /// Updates the existing tool skill data asynchronous null tool skill data throws argument null exception.
    /// </summary>
    [Fact]
    public async Task UpdateExistingToolSkillDataAsync_NullToolSkillData_ThrowsArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAnyAsync<ArgumentNullException>(() =>
            _toolSkillsService.UpdateExistingToolSkillDataAsync(null!, TestsHelpers.TestUserEmail));
    }

    /// <summary>
    /// Updates the existing tool skill data asynchronous null or white space user email throws argument exception.
    /// </summary>
    /// <param name="userEmail">The user email.</param>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task UpdateExistingToolSkillDataAsync_NullOrWhiteSpaceUserEmail_ThrowsArgumentException(string? userEmail)
    {
        // Act & Assert
        await Assert.ThrowsAnyAsync<ArgumentException>(() =>
            _toolSkillsService.UpdateExistingToolSkillDataAsync(TestsHelpers.BuildToolSkill(TestsHelpers.ToolSkillGuid), userEmail!));
    }

    /// <summary>
    /// Updates the existing tool skill data asynchronous success returns true and sends notification.
    /// </summary>
    [Fact]
    public async Task UpdateExistingToolSkillDataAsync_Success_ReturnsTrueAndSendsNotification()
    {
        // Arrange
        var skill = TestsHelpers.BuildToolSkill(TestsHelpers.ToolSkillGuid);
        _mockDataManager
            .Setup(d => d.UpdateExistingToolSkillDataAsync(skill, TestsHelpers.TestUserEmail, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _mockNotificationsService
            .Setup(n => n.CreateNewNotificationAsync(It.IsAny<NotificationsDomain>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _toolSkillsService.UpdateExistingToolSkillDataAsync(skill, TestsHelpers.TestUserEmail);

        // Assert
        Assert.True(result);
        _mockNotificationsService.Verify(
            n => n.CreateNewNotificationAsync(It.IsAny<NotificationsDomain>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    /// <summary>
    /// Updates the existing tool skill data asynchronous data manager returns false does not send notification.
    /// </summary>
    [Fact]
    public async Task UpdateExistingToolSkillDataAsync_DataManagerReturnsFalse_DoesNotSendNotification()
    {
        // Arrange
        var skill = TestsHelpers.BuildToolSkill(TestsHelpers.ToolSkillGuid);
        _mockDataManager
            .Setup(d => d.UpdateExistingToolSkillDataAsync(It.IsAny<ToolSkillDomain>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _toolSkillsService.UpdateExistingToolSkillDataAsync(skill, TestsHelpers.TestUserEmail);

        // Assert
        Assert.False(result);
        _mockNotificationsService.Verify(
            n => n.CreateNewNotificationAsync(It.IsAny<NotificationsDomain>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    /// <summary>
    /// Updates the existing tool skill data asynchronous data manager throws exception throws ai agents business exception.
    /// </summary>
    [Fact]
    public async Task UpdateExistingToolSkillDataAsync_DataManagerThrowsException_ThrowsAIAgentsBusinessException()
    {
        // Arrange
        var skill = TestsHelpers.BuildToolSkill(TestsHelpers.ToolSkillGuid);
        _mockDataManager
            .Setup(d => d.UpdateExistingToolSkillDataAsync(It.IsAny<ToolSkillDomain>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Update failed"));

        // Act & Assert
        var ex = await Assert.ThrowsAsync<AIAgentsBusinessException>(() =>
            _toolSkillsService.UpdateExistingToolSkillDataAsync(skill, TestsHelpers.TestUserEmail));

        Assert.Contains("Update failed", ex.Message);
    }

    /// <summary>
    /// Updates the existing tool skill data asynchronous notification sends correct skill name and guid.
    /// </summary>
    [Fact]
    public async Task UpdateExistingToolSkillDataAsync_NotificationSendsCorrectSkillNameAndGuid()
    {
        // Arrange
        var skill = TestsHelpers.BuildToolSkill("my-skill-guid");
        skill.ToolSkillDisplayName = "My Skill";

        _mockDataManager
            .Setup(d => d.UpdateExistingToolSkillDataAsync(It.IsAny<ToolSkillDomain>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        NotificationsDomain? capturedNotification = null;
        _mockNotificationsService
            .Setup(n => n.CreateNewNotificationAsync(It.IsAny<NotificationsDomain>(), It.IsAny<CancellationToken>()))
            .Callback<NotificationsDomain, CancellationToken>((n, _) => capturedNotification = n)
            .ReturnsAsync(true);

        // Act
        await _toolSkillsService.UpdateExistingToolSkillDataAsync(skill, TestsHelpers.TestUserEmail);

        // Assert
        Assert.NotNull(capturedNotification);
        Assert.Contains("My Skill", capturedNotification.Title);
        Assert.Contains("my-skill-guid", capturedNotification.Message);
    }

    #endregion

    #region DeleteExistingToolSkillBySkillIdAsync

    /// <summary>
    /// Deletes the existing tool skill by skill identifier asynchronous null or white space skill id throws argument exception.
    /// </summary>
    /// <param name="skillId">The skill identifier.</param>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task DeleteExistingToolSkillBySkillIdAsync_NullOrWhiteSpaceSkillId_ThrowsArgumentException(string? skillId)
    {
        // Act & Assert
        await Assert.ThrowsAnyAsync<ArgumentException>(() =>
            _toolSkillsService.DeleteExistingToolSkillBySkillIdAsync(skillId!, TestsHelpers.TestUserEmail));
    }

    /// <summary>
    /// Deletes the existing tool skill by skill identifier asynchronous null or white space user email throws argument exception.
    /// </summary>
    /// <param name="userEmail">The user email.</param>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task DeleteExistingToolSkillBySkillIdAsync_NullOrWhiteSpaceUserEmail_ThrowsArgumentException(string? userEmail)
    {
        // Act & Assert
        await Assert.ThrowsAnyAsync<ArgumentException>(() =>
            _toolSkillsService.DeleteExistingToolSkillBySkillIdAsync("skill-guid-1", userEmail!));
    }

    /// <summary>
    /// Deletes the existing tool skill by skill identifier asynchronous success returns true and sends notification.
    /// </summary>
    [Fact]
    public async Task DeleteExistingToolSkillBySkillIdAsync_Success_ReturnsTrueAndSendsNotification()
    {
        // Arrange
        _mockDataManager
            .Setup(d => d.DeleteExistingToolSkillBySkillIdAsync("skill-guid-1", TestsHelpers.TestUserEmail, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _mockNotificationsService
            .Setup(n => n.CreateNewNotificationAsync(It.IsAny<NotificationsDomain>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _toolSkillsService.DeleteExistingToolSkillBySkillIdAsync("skill-guid-1", TestsHelpers.TestUserEmail);

        // Assert
        Assert.True(result);
        _mockNotificationsService.Verify(
            n => n.CreateNewNotificationAsync(It.IsAny<NotificationsDomain>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    /// <summary>
    /// Deletes the existing tool skill by skill identifier asynchronous data manager returns false does not send notification.
    /// </summary>
    [Fact]
    public async Task DeleteExistingToolSkillBySkillIdAsync_DataManagerReturnsFalse_DoesNotSendNotification()
    {
        // Arrange
        _mockDataManager
            .Setup(d => d.DeleteExistingToolSkillBySkillIdAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _toolSkillsService.DeleteExistingToolSkillBySkillIdAsync("skill-guid-1", TestsHelpers.TestUserEmail);

        // Assert
        Assert.False(result);
        _mockNotificationsService.Verify(
            n => n.CreateNewNotificationAsync(It.IsAny<NotificationsDomain>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    /// <summary>
    /// Deletes the existing tool skill by skill identifier asynchronous data manager throws exception throws ai agents business exception.
    /// </summary>
    [Fact]
    public async Task DeleteExistingToolSkillBySkillIdAsync_DataManagerThrowsException_ThrowsAIAgentsBusinessException()
    {
        // Arrange
        _mockDataManager
            .Setup(d => d.DeleteExistingToolSkillBySkillIdAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Delete failed"));

        // Act & Assert
        var ex = await Assert.ThrowsAsync<AIAgentsBusinessException>(() =>
            _toolSkillsService.DeleteExistingToolSkillBySkillIdAsync("skill-guid-1", TestsHelpers.TestUserEmail));

        Assert.Contains("Delete failed", ex.Message);
    }

    /// <summary>
    /// Deletes the existing tool skill by skill identifier asynchronous with cancellation token passes token to data manager.
    /// </summary>
    [Fact]
    public async Task DeleteExistingToolSkillBySkillIdAsync_WithCancellationToken_PassesTokenToDataManager()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        _mockDataManager
            .Setup(d => d.DeleteExistingToolSkillBySkillIdAsync(It.IsAny<string>(), It.IsAny<string>(), cts.Token))
            .ReturnsAsync(true);
        _mockNotificationsService
            .Setup(n => n.CreateNewNotificationAsync(It.IsAny<NotificationsDomain>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        await _toolSkillsService.DeleteExistingToolSkillBySkillIdAsync("skill-guid-1", TestsHelpers.TestUserEmail, cts.Token);

        // Assert
        _mockDataManager.Verify(
            d => d.DeleteExistingToolSkillBySkillIdAsync(It.IsAny<string>(), It.IsAny<string>(), cts.Token),
            Times.Once);
    }

    #endregion

    #region GetAllMcpToolsAvailableAsync

    /// <summary>
    /// Gets all mcp tools available asynchronous null or white space server url throws argument exception.
    /// </summary>
    /// <param name="serverUrl">The server URL.</param>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task GetAllMcpToolsAvailableAsync_NullOrWhiteSpaceServerUrl_ThrowsArgumentException(string? serverUrl)
    {
        // Act & Assert
        await Assert.ThrowsAnyAsync<ArgumentException>(() =>
            _toolSkillsService.GetAllMcpToolsAvailableAsync(serverUrl!, TestsHelpers.TestUserEmail));
    }

    /// <summary>
    /// Gets all mcp tools available asynchronous null or white space user email throws argument exception.
    /// </summary>
    /// <param name="userEmail">The user email.</param>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task GetAllMcpToolsAvailableAsync_NullOrWhiteSpaceUserEmail_ThrowsArgumentException(string? userEmail)
    {
        // Act & Assert
        await Assert.ThrowsAnyAsync<ArgumentException>(() =>
            _toolSkillsService.GetAllMcpToolsAvailableAsync("https://mcp.example.com", userEmail!));
    }

    /// <summary>
    /// Gets all mcp tools available asynchronous success returns mcp tools.
    /// </summary>
    [Fact]
    public async Task GetAllMcpToolsAvailableAsync_Success_ReturnsMcpTools()
    {
        // Arrange
        var serverUrl = "https://mcp.example.com";
        var tools = new List<McpClientTool>();
        _mockMcpClientServices
            .Setup(m => m.GetAllMcpToolsAsync(serverUrl, It.IsAny<CancellationToken>()))
            .ReturnsAsync(tools);

        // Act
        var result = await _toolSkillsService.GetAllMcpToolsAvailableAsync(serverUrl, TestsHelpers.TestUserEmail);

        // Assert
        Assert.NotNull(result);
        _mockMcpClientServices.Verify(
            m => m.GetAllMcpToolsAsync(serverUrl, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    /// <summary>
    /// Gets all mcp tools available asynchronous mcp client throws exception throws ai agents business exception.
    /// </summary>
    [Fact]
    public async Task GetAllMcpToolsAvailableAsync_McpClientThrowsException_ThrowsAIAgentsBusinessException()
    {
        // Arrange
        _mockMcpClientServices
            .Setup(m => m.GetAllMcpToolsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("MCP connection failed"));

        // Act & Assert
        var ex = await Assert.ThrowsAsync<AIAgentsBusinessException>(() =>
            _toolSkillsService.GetAllMcpToolsAvailableAsync("https://mcp.example.com", TestsHelpers.TestUserEmail));

        Assert.Contains("MCP connection failed", ex.Message);
    }

    /// <summary>
    /// Gets all mcp tools available asynchronous passes correct server url to mcp client.
    /// </summary>
    [Theory]
    [InlineData("https://mcp1.example.com")]
    [InlineData("https://mcp2.example.com")]
    public async Task GetAllMcpToolsAvailableAsync_PassesCorrectServerUrlToMcpClient(string serverUrl)
    {
        // Arrange
        _mockMcpClientServices
            .Setup(m => m.GetAllMcpToolsAsync(serverUrl, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        // Act
        await _toolSkillsService.GetAllMcpToolsAvailableAsync(serverUrl, TestsHelpers.TestUserEmail);

        // Assert
        _mockMcpClientServices.Verify(
            m => m.GetAllMcpToolsAsync(serverUrl, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    /// <summary>
    /// Gets all mcp tools available asynchronous with cancellation token passes token to mcp client.
    /// </summary>
    [Fact]
    public async Task GetAllMcpToolsAvailableAsync_WithCancellationToken_PassesTokenToMcpClient()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        _mockMcpClientServices
            .Setup(m => m.GetAllMcpToolsAsync(It.IsAny<string>(), cts.Token))
            .ReturnsAsync([]);

        // Act
        await _toolSkillsService.GetAllMcpToolsAvailableAsync("https://mcp.example.com", TestsHelpers.TestUserEmail, cts.Token);

        // Assert
        _mockMcpClientServices.Verify(
            m => m.GetAllMcpToolsAsync(It.IsAny<string>(), cts.Token),
            Times.Once);
    }

    #endregion

    #region AssociateSkillAndAgentAsync

    /// <summary>
    /// Associates the skill and agent asynchronous null agent data throws argument null exception.
    /// </summary>
    [Fact]
    public async Task AssociateSkillAndAgentAsync_NullAgentData_ThrowsArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAnyAsync<ArgumentNullException>(() =>
            _toolSkillsService.AssociateSkillAndAgentAsync(null!, "skill-guid-1", TestsHelpers.TestUserEmail));
    }

    /// <summary>
    /// Associates the skill and agent asynchronous null or white space tool skill id throws argument exception.
    /// </summary>
    /// <param name="skillId">The skill identifier.</param>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task AssociateSkillAndAgentAsync_NullOrWhiteSpaceToolSkillId_ThrowsArgumentException(string? skillId)
    {
        // Act & Assert
        await Assert.ThrowsAnyAsync<ArgumentException>(() =>
            _toolSkillsService.AssociateSkillAndAgentAsync([], skillId!, TestsHelpers.TestUserEmail));
    }

    /// <summary>
    /// Associates the skill and agent asynchronous null or white space user email throws argument exception.
    /// </summary>
    /// <param name="userEmail">The user email.</param>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task AssociateSkillAndAgentAsync_NullOrWhiteSpaceUserEmail_ThrowsArgumentException(string? userEmail)
    {
        // Act & Assert
        await Assert.ThrowsAnyAsync<ArgumentException>(() =>
            _toolSkillsService.AssociateSkillAndAgentAsync([], "skill-guid-1", userEmail!));
    }

    /// <summary>
    /// Associates the skill and agent asynchronous tool skill not found returns false.
    /// </summary>
    [Fact]
    public async Task AssociateSkillAndAgentAsync_ToolSkillNotFound_ReturnsFalse()
    {
        // Arrange
        _mockDataManager
            .Setup(d => d.GetToolSkillBySkillIdAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((ToolSkillDomain)null!);

        // Act
        var result = await _toolSkillsService.AssociateSkillAndAgentAsync([], "skill-guid-1", TestsHelpers.TestUserEmail);

        // Assert
        Assert.False(result);
        _mockDataManager.Verify(
            d => d.UpdateExistingToolSkillDataAsync(It.IsAny<ToolSkillDomain>(), It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    /// <summary>
    /// Associates the skill and agent asynchronous success updates skill with agent data and returns true.
    /// </summary>
    [Fact]
    public async Task AssociateSkillAndAgentAsync_Success_UpdatesSkillWithAgentDataAndReturnsTrue()
    {
        // Arrange
        var skill = TestsHelpers.BuildToolSkill(TestsHelpers.ToolSkillGuid);
        var agentData = new List<AssociatedAgentsSkillDataDomain>
        {
            new() { AgentGuid = TestsHelpers.TestGuidId, AgentName = "Test Agent" }
        };

        _mockDataManager
            .Setup(d => d.GetToolSkillBySkillIdAsync(skill.ToolSkillGuid, TestsHelpers.TestUserEmail, It.IsAny<CancellationToken>()))
            .ReturnsAsync(skill);
        _mockDataManager
            .Setup(d => d.UpdateExistingToolSkillDataAsync(It.IsAny<ToolSkillDomain>(), TestsHelpers.TestUserEmail, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _mockNotificationsService
            .Setup(n => n.CreateNewNotificationAsync(It.IsAny<NotificationsDomain>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _toolSkillsService.AssociateSkillAndAgentAsync(agentData, skill.ToolSkillGuid, TestsHelpers.TestUserEmail);

        // Assert
        Assert.True(result);
        _mockDataManager.Verify(
            d => d.UpdateExistingToolSkillDataAsync(
                It.Is<ToolSkillDomain>(s => s.AssociatedAgents == agentData),
                TestsHelpers.TestUserEmail,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    /// <summary>
    /// Associates the skill and agent asynchronous data manager throws exception throws ai agents business exception.
    /// </summary>
    [Fact]
    public async Task AssociateSkillAndAgentAsync_DataManagerThrowsException_ThrowsAIAgentsBusinessException()
    {
        // Arrange
        _mockDataManager
            .Setup(d => d.GetToolSkillBySkillIdAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Association failed"));

        // Act & Assert
        var ex = await Assert.ThrowsAsync<AIAgentsBusinessException>(() =>
            _toolSkillsService.AssociateSkillAndAgentAsync([], "skill-guid-1", TestsHelpers.TestUserEmail));

        Assert.Contains("Association failed", ex.Message);
    }

    #endregion
}
