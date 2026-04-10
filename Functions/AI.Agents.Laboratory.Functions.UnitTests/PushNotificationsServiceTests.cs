using AI.Agents.Laboratory.Functions.Business.Services;
using AI.Agents.Laboratory.Functions.Data.Contracts;
using AI.Agents.Laboratory.Functions.Shared.Exceptions;
using AI.Agents.Laboratory.Functions.Shared.Helpers;
using AI.Agents.Laboratory.Functions.Shared.Models;
using Microsoft.Extensions.Logging;
using Moq;

namespace AI.Agents.Laboratory.Functions.UnitTests;

/// <summary>
/// Unit tests for the PushNotificationsService class, which handles the processing of push notification requests.
/// </summary>
public class PushNotificationsServiceTests
{
    /// <summary>
    /// A static readonly Guid used as a test correlation ID in unit tests.
    /// </summary>
    private readonly static Guid CorrelationGuid = TestsHelper.TestCorrelationIdGuid;

    /// <summary>
    /// Mock logger for testing the PushNotificationsService, allowing us to verify logging behavior without relying on an actual logging implementation.
    /// </summary>
    private readonly Mock<ILogger<PushNotificationsService>> _mockLogger = new();

    /// <summary>
    /// Mock correlation context for testing the PushNotificationsService, enabling us to simulate correlation IDs and verify that they are used correctly in logging and exception handling.
    /// </summary>
    private readonly Mock<ICorrelationContext> _mockCorrelationContext = new();

    /// <summary>
    /// Mock notifications data manager for testing the PushNotificationsService, allowing us to simulate the behavior of the data manager and verify that it is called correctly when processing push notification requests.
    /// </summary>
    private readonly Mock<INotificationsDataManager> _mockNotificationsDataManager = new();

    /// <summary>
    /// Instance of the PushNotificationsService being tested, initialized with the mocked dependencies to allow for isolated unit testing of its functionality.
    /// </summary>
    private readonly PushNotificationsService _pushNotificationsService;

    /// <summary>
    /// Initializes a new instance of the PushNotificationsServiceTests class, setting up the mocked dependencies and creating an instance of the PushNotificationsService to be tested.
    /// </summary>
    public PushNotificationsServiceTests()
    {
        this._pushNotificationsService = new PushNotificationsService(
            this._mockLogger.Object,
            this._mockCorrelationContext.Object,
            this._mockNotificationsDataManager.Object
        );
    }

    /// <summary>
    /// Tests the ReceivePushNotificationAsync method of the PushNotificationsService to ensure that it returns true when the data manager successfully saves the push notification data, and that it interacts with the data manager as expected.
    /// </summary>
    /// <returns>A task representing the asynchronous operation of the test.</returns>
    [Fact]
    public async Task ReceivePushNotificationAsync_ShouldReturnTrue_WhenDataManagerReturnsTrue()
    {
        // Arrange
        var request = new NotificationRequest();
        this._mockCorrelationContext
            .Setup(c => c.CorrelationId)
            .Returns(CorrelationGuid.ToString());
        this._mockNotificationsDataManager
            .Setup(m => m.SavePushNotificationsDataAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await this._pushNotificationsService.ReceivePushNotificationAsync(request);

        // Assert
        Assert.True(result);
        this._mockNotificationsDataManager.Verify(
            m => m.SavePushNotificationsDataAsync(request, It.IsAny<CancellationToken>()), Times.Once);
    }

    /// <summary>
    /// Tests the ReceivePushNotificationAsync method of the PushNotificationsService to ensure that it returns false when the data manager fails to save the push notification data, and that it interacts with the data manager as expected.
    /// </summary>
    /// <returns>A task representing the asynchronous operation of the test.</returns>
    [Fact]
    public async Task ReceivePushNotificationAsync_ShouldReturnFalse_WhenDataManagerReturnsFalse()
    {
        // Arrange
        var request = new NotificationRequest();
        this._mockCorrelationContext
            .Setup(c => c.CorrelationId)
            .Returns(CorrelationGuid.ToString());
        this._mockNotificationsDataManager
            .Setup(m => m.SavePushNotificationsDataAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await this._pushNotificationsService.ReceivePushNotificationAsync(request);

        // Assert
        Assert.False(result);
        this._mockNotificationsDataManager
            .Verify(m => m.SavePushNotificationsDataAsync(request, It.IsAny<CancellationToken>()), Times.Once);
    }

    /// <summary>
    /// Tests the ReceivePushNotificationAsync method of the PushNotificationsService to ensure that it throws an AIAgentsBusinessException when the data manager throws an exception, and that it interacts with the data manager as expected.
    /// </summary>
    /// <returns>A task representing the asynchronous operation of the test.</returns>
    [Fact]
    public async Task ReceivePushNotificationAsync_ShouldThrowAIAgentsBusinessException_WhenDataManagerThrowsException()
    {
        // Arrange
        var request = new NotificationRequest();
        var expectedMessage = "Test exception message";
        this._mockCorrelationContext
            .Setup(c => c.CorrelationId)
            .Returns(CorrelationGuid.ToString());
        this._mockNotificationsDataManager
            .Setup(m => m.SavePushNotificationsDataAsync(request, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception(expectedMessage));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<AIAgentsBusinessException>(() =>
            this._pushNotificationsService.ReceivePushNotificationAsync(request));

        Assert.Equal(expectedMessage, exception.Message);
        this._mockNotificationsDataManager
            .Verify(m => m.SavePushNotificationsDataAsync(request, It.IsAny<CancellationToken>()), Times.Once);
    }
}
