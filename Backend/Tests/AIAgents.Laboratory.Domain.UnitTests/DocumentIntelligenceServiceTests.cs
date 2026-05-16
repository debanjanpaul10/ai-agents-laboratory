using AIAgents.Laboratory.Domain.Contracts;
using AIAgents.Laboratory.Domain.DomainEntities.AgentsEntities;
using AIAgents.Laboratory.Domain.Helpers;
using AIAgents.Laboratory.Domain.Ports.Out;
using AIAgents.Laboratory.Domain.UseCases;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using static AIAgents.Laboratory.Domain.Helpers.Constants;

namespace AIAgents.Laboratory.Domain.UnitTests;

/// <summary>
/// The Document Intelligence Service Tests class.
/// </summary>
public sealed class DocumentIntelligenceServiceTests
{
    /// <summary>
    /// The mock logger.
    /// </summary>
    private readonly Mock<ILogger<DocumentIntelligenceService>> _mockLogger = new();

    /// <summary>
    /// The mock configuration.
    /// </summary>
    private readonly Mock<IConfiguration> _mockConfiguration = new();

    /// <summary>
    /// The mock correlation context.
    /// </summary>
    private readonly Mock<ICorrelationContext> _mockCorrelationContext = new();

    /// <summary>
    /// The mock knowledge base processor.
    /// </summary>
    private readonly Mock<IKnowledgeBaseProcessor> _mockKnowledgeBaseProcessor = new();

    /// <summary>
    /// The mock BLOB storage manager.
    /// </summary>
    private readonly Mock<IBlobStorageManager> _mockBlobStorageManager = new();

    /// <summary>
    /// The mock vision processor.
    /// </summary>
    private readonly Mock<IVisionProcessor> _mockVisionProcessor = new();

    /// <summary>
    /// The service.
    /// </summary>
    private readonly DocumentIntelligenceService _service;

    /// <summary>
    /// Initializes a new instance of the <see cref="DocumentIntelligenceServiceTests"/> class.
    /// </summary>
    public DocumentIntelligenceServiceTests()
    {
        _mockConfiguration
            .Setup(c => c[AzureAppConfigurationConstants.AllowedKbFileFormatsConstant])
            .Returns(".pdf,.txt,.docx");
        _mockConfiguration
            .Setup(c => c[AzureAppConfigurationConstants.AllowedVisionImageFileFormatsConstant])
            .Returns(".jpg,.png,.jpeg");
        _mockCorrelationContext
            .Setup(c => c.CorrelationId)
            .Returns(Guid.NewGuid().ToString());

        _service = new DocumentIntelligenceService(
            _mockLogger.Object,
            _mockConfiguration.Object,
            _mockCorrelationContext.Object,
            _mockKnowledgeBaseProcessor.Object,
            _mockBlobStorageManager.Object,
            _mockVisionProcessor.Object
        );
    }

    #region DeleteKnowledgebaseAndImagesDataAsync

    /// <summary>
    /// Deletes the knowledgebase and images data asynchronous success calls blob storage delete.
    /// </summary>
    [Fact]
    public async Task DeleteKnowledgebaseAndImagesDataAsync_Success_CallsBlobStorageDelete()
    {
        // Arrange
        var agentId = TestsHelpers.TestGuidId;
        _mockBlobStorageManager
            .Setup(b => b.DeleteDocumentsFolderAndDataAsync(agentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        await _service.DeleteKnowledgebaseAndImagesDataAsync(agentId);

        // Assert
        _mockBlobStorageManager.Verify(
            b => b.DeleteDocumentsFolderAndDataAsync(agentId, It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    /// <summary>
    /// Deletes the knowledgebase and images data asynchronous blob storage throws exception throws ai agents business exception.
    /// </summary>
    [Fact]
    public async Task DeleteKnowledgebaseAndImagesDataAsync_BlobStorageThrowsException_ThrowsAIAgentsBusinessException()
    {
        // Arrange
        _mockBlobStorageManager
            .Setup(b => b.DeleteDocumentsFolderAndDataAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Blob delete failed"));

        // Act & Assert
        var ex = await Assert.ThrowsAsync<AIAgentsBusinessException>(() =>
            _service.DeleteKnowledgebaseAndImagesDataAsync(TestsHelpers.TestGuidId));

        Assert.Contains("Blob delete failed", ex.Message);
    }

    /// <summary>
    /// Deletes the knowledgebase and images data asynchronous with cancellation token passes token to blob storage.
    /// </summary>
    [Fact]
    public async Task DeleteKnowledgebaseAndImagesDataAsync_WithCancellationToken_PassesTokenToBlobStorage()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        _mockBlobStorageManager
            .Setup(b => b.DeleteDocumentsFolderAndDataAsync(It.IsAny<string>(), cts.Token))
            .ReturnsAsync(true);

        // Act
        await _service.DeleteKnowledgebaseAndImagesDataAsync(TestsHelpers.TestGuidId, cts.Token);

        // Assert
        _mockBlobStorageManager.Verify(
            b => b.DeleteDocumentsFolderAndDataAsync(It.IsAny<string>(), cts.Token),
            Times.Once
        );

        // Cleanup
        cts.Dispose();
    }

    #endregion

    #region CreateAndProcessKnowledgeBaseDocumentAsync

    /// <summary>
    /// Creates and processes knowledge base document asynchronous null knowledge base document returns without processing.
    /// </summary>
    [Fact]
    public async Task CreateAndProcessKnowledgeBaseDocumentAsync_NullKnowledgeBaseDocument_ReturnsWithoutProcessing()
    {
        // Arrange
        var agentData = TestsHelpers.GetAgentDataDomain();
        agentData.KnowledgeBaseDocument = null;

        // Act
        await _service.CreateAndProcessKnowledgeBaseDocumentAsync(agentData);

        // Assert
        _mockBlobStorageManager.Verify(
            b => b.UploadDocumentsToStorageAsync(It.IsAny<IFormFile>(), It.IsAny<string>(), It.IsAny<UploadedFileType>(), It.IsAny<CancellationToken>()),
            Times.Never
        );
    }

    /// <summary>
    /// Creates and processes knowledge base document asynchronous empty knowledge base document returns without processing.
    /// </summary>
    [Fact]
    public async Task CreateAndProcessKnowledgeBaseDocumentAsync_EmptyKnowledgeBaseDocument_ReturnsWithoutProcessing()
    {
        // Arrange
        var agentData = TestsHelpers.GetAgentDataDomain();
        agentData.KnowledgeBaseDocument = [];

        // Act
        await _service.CreateAndProcessKnowledgeBaseDocumentAsync(agentData);

        // Assert
        _mockBlobStorageManager.Verify(
            b => b.UploadDocumentsToStorageAsync(It.IsAny<IFormFile>(), It.IsAny<string>(), It.IsAny<UploadedFileType>(), It.IsAny<CancellationToken>()),
            Times.Never
        );
    }

    /// <summary>
    /// Creates and processes knowledge base document asynchronous invalid file format throws ai agents business exception.
    /// </summary>
    [Fact]
    public async Task CreateAndProcessKnowledgeBaseDocumentAsync_InvalidFileFormat_ThrowsAIAgentsBusinessException()
    {
        // Arrange
        var agentData = TestsHelpers.GetAgentDataDomain();
        agentData.KnowledgeBaseDocument = [TestsHelpers.CreateMockFormFile("document.exe")];

        // Act & Assert
        await Assert.ThrowsAsync<AIAgentsBusinessException>(() =>
            _service.CreateAndProcessKnowledgeBaseDocumentAsync(agentData));
    }

    /// <summary>
    /// Creates and processes knowledge base document asynchronous valid file uploads and processes document.
    /// </summary>
    [Fact]
    public async Task CreateAndProcessKnowledgeBaseDocumentAsync_ValidFile_UploadsAndProcessesDocument()
    {
        // Arrange
        var agentData = TestsHelpers.GetAgentDataDomain();
        agentData.KnowledgeBaseDocument = [TestsHelpers.CreateMockFormFile("document.pdf", "application/pdf")];

        _mockBlobStorageManager
            .Setup(b => b.UploadDocumentsToStorageAsync(It.IsAny<IFormFile>(), agentData.AgentId, UploadedFileType.KnowledgeBaseDocument, It.IsAny<CancellationToken>()))
            .ReturnsAsync("https://storage.example.com/document.pdf");
        _mockKnowledgeBaseProcessor
            .Setup(k => k.DetectAndReadFileContent(It.IsAny<KnowledgeBaseDocumentDomain>()))
            .Returns("file content");
        _mockKnowledgeBaseProcessor
            .Setup(k => k.ProcessKnowledgeBaseDocumentAsync(It.IsAny<string>(), agentData.AgentId, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _service.CreateAndProcessKnowledgeBaseDocumentAsync(agentData);

        // Assert
        _mockBlobStorageManager.Verify(
            b => b.UploadDocumentsToStorageAsync(It.IsAny<IFormFile>(), agentData.AgentId, UploadedFileType.KnowledgeBaseDocument, It.IsAny<CancellationToken>()),
            Times.Once
        );
        _mockKnowledgeBaseProcessor.Verify(
            k => k.ProcessKnowledgeBaseDocumentAsync(It.IsAny<string>(), agentData.AgentId, It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    /// <summary>
    /// Creates and processes knowledge base document asynchronous multiple valid files uploads and processes all documents.
    /// </summary>
    [Fact]
    public async Task CreateAndProcessKnowledgeBaseDocumentAsync_MultipleValidFiles_UploadsAndProcessesAllDocuments()
    {
        // Arrange
        var agentData = TestsHelpers.GetAgentDataDomain();
        agentData.KnowledgeBaseDocument =
        [
            TestsHelpers.CreateMockFormFile("doc1.pdf", "application/pdf"),
            TestsHelpers.CreateMockFormFile("doc2.txt", "text/plain")
        ];

        _mockBlobStorageManager
            .Setup(b => b.UploadDocumentsToStorageAsync(It.IsAny<IFormFile>(), It.IsAny<string>(), UploadedFileType.KnowledgeBaseDocument, It.IsAny<CancellationToken>()))
            .ReturnsAsync("https://storage.example.com/file");
        _mockKnowledgeBaseProcessor
            .Setup(k => k.DetectAndReadFileContent(It.IsAny<KnowledgeBaseDocumentDomain>()))
            .Returns("file content");
        _mockKnowledgeBaseProcessor
            .Setup(k => k.ProcessKnowledgeBaseDocumentAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _service.CreateAndProcessKnowledgeBaseDocumentAsync(agentData);

        // Assert
        _mockBlobStorageManager.Verify(
            b => b.UploadDocumentsToStorageAsync(It.IsAny<IFormFile>(), It.IsAny<string>(), UploadedFileType.KnowledgeBaseDocument, It.IsAny<CancellationToken>()),
            Times.Exactly(2)
        );
        _mockKnowledgeBaseProcessor.Verify(
            k => k.ProcessKnowledgeBaseDocumentAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Exactly(2)
        );
    }

    /// <summary>
    /// Creates and processes knowledge base document asynchronous blob storage throws exception throws ai agents business exception.
    /// </summary>
    [Fact]
    public async Task CreateAndProcessKnowledgeBaseDocumentAsync_BlobStorageThrowsException_ThrowsAIAgentsBusinessException()
    {
        // Arrange
        var agentData = TestsHelpers.GetAgentDataDomain();
        agentData.KnowledgeBaseDocument = [TestsHelpers.CreateMockFormFile("document.pdf", "application/pdf")];

        _mockBlobStorageManager
            .Setup(b => b.UploadDocumentsToStorageAsync(It.IsAny<IFormFile>(), It.IsAny<string>(), It.IsAny<UploadedFileType>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Upload failed"));

        // Act & Assert
        var ex = await Assert.ThrowsAsync<AIAgentsBusinessException>(() =>
            _service.CreateAndProcessKnowledgeBaseDocumentAsync(agentData));

        Assert.Contains("Upload failed", ex.Message);
    }

    /// <summary>
    /// Creates and processes knowledge base document asynchronous knowledge base processor throws exception throws ai agents business exception.
    /// </summary>
    [Fact]
    public async Task CreateAndProcessKnowledgeBaseDocumentAsync_KnowledgeBaseProcessorThrowsException_ThrowsAIAgentsBusinessException()
    {
        // Arrange
        var agentData = TestsHelpers.GetAgentDataDomain();
        agentData.KnowledgeBaseDocument = [TestsHelpers.CreateMockFormFile("document.pdf", "application/pdf")];

        _mockBlobStorageManager
            .Setup(b => b.UploadDocumentsToStorageAsync(It.IsAny<IFormFile>(), It.IsAny<string>(), It.IsAny<UploadedFileType>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync("https://storage.example.com/document.pdf");
        _mockKnowledgeBaseProcessor
            .Setup(k => k.DetectAndReadFileContent(It.IsAny<KnowledgeBaseDocumentDomain>()))
            .Returns("file content");
        _mockKnowledgeBaseProcessor
            .Setup(k => k.ProcessKnowledgeBaseDocumentAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Processing failed"));

        // Act & Assert
        var ex = await Assert.ThrowsAsync<AIAgentsBusinessException>(() =>
            _service.CreateAndProcessKnowledgeBaseDocumentAsync(agentData));

        Assert.Contains("Processing failed", ex.Message);
    }

    #endregion

    #region HandleKnowledgeBaseDataUpdateAsync

    /// <summary>
    /// Handles the knowledge base data update asynchronous no changes does not update stored knowledge base.
    /// </summary>
    [Fact]
    public async Task HandleKnowledgeBaseDataUpdateAsync_NoChanges_DoesNotUpdateStoredKnowledgeBase()
    {
        // Arrange
        var existingKb = new KnowledgeBaseDocumentDomain { FileName = "existing.pdf" };
        var existingAgent = TestsHelpers.GetAgentDataDomain();
        existingAgent.StoredKnowledgeBase = [existingKb];

        var updateDomain = TestsHelpers.GetAgentDataDomain();
        updateDomain.KnowledgeBaseDocument = [];
        updateDomain.RemovedKnowledgeBaseDocuments = [];

        // Act
        await _service.HandleKnowledgeBaseDataUpdateAsync(updateDomain, existingAgent);

        // Assert
        _mockBlobStorageManager.Verify(
            b => b.UploadDocumentsToStorageAsync(It.IsAny<IFormFile>(), It.IsAny<string>(), It.IsAny<UploadedFileType>(), It.IsAny<CancellationToken>()),
            Times.Never
        );
    }

    /// <summary>
    /// Handles the knowledge base data update asynchronous with removed documents removes them from stored knowledge base.
    /// </summary>
    [Fact]
    public async Task HandleKnowledgeBaseDataUpdateAsync_WithRemovedDocuments_RemovesThemFromStoredKnowledgeBase()
    {
        // Arrange
        var existingAgent = TestsHelpers.GetAgentDataDomain();
        existingAgent.StoredKnowledgeBase =
        [
            new KnowledgeBaseDocumentDomain { FileName = "keep.pdf" },
            new KnowledgeBaseDocumentDomain { FileName = "remove.pdf" }
        ];

        var updateDomain = TestsHelpers.GetAgentDataDomain();
        updateDomain.RemovedKnowledgeBaseDocuments = ["remove.pdf"];
        updateDomain.KnowledgeBaseDocument = [];

        // Act
        await _service.HandleKnowledgeBaseDataUpdateAsync(updateDomain, existingAgent);

        // Assert
        Assert.Single(updateDomain.StoredKnowledgeBase);
        Assert.Equal("keep.pdf", updateDomain.StoredKnowledgeBase[0].FileName);
    }

    /// <summary>
    /// Handles the knowledge base data update asynchronous remove all documents sets stored knowledge base to empty list.
    /// </summary>
    [Fact]
    public async Task HandleKnowledgeBaseDataUpdateAsync_RemoveAllDocuments_SetsStoredKnowledgeBaseToEmptyList()
    {
        // Arrange
        var existingAgent = TestsHelpers.GetAgentDataDomain();
        existingAgent.StoredKnowledgeBase = [new KnowledgeBaseDocumentDomain { FileName = "only.pdf" }];

        var updateDomain = TestsHelpers.GetAgentDataDomain();
        updateDomain.RemovedKnowledgeBaseDocuments = ["only.pdf"];
        updateDomain.KnowledgeBaseDocument = [];

        // Act
        await _service.HandleKnowledgeBaseDataUpdateAsync(updateDomain, existingAgent);

        // Assert
        Assert.NotNull(updateDomain.StoredKnowledgeBase);
        Assert.Empty(updateDomain.StoredKnowledgeBase);
    }

    /// <summary>
    /// Handles the knowledge base data update asynchronous with new documents uploads and processes them.
    /// </summary>
    [Fact]
    public async Task HandleKnowledgeBaseDataUpdateAsync_WithNewDocuments_UploadsAndProcessesThem()
    {
        // Arrange
        var existingAgent = TestsHelpers.GetAgentDataDomain();
        existingAgent.StoredKnowledgeBase = [new KnowledgeBaseDocumentDomain { FileName = "existing.pdf" }];

        var updateDomain = TestsHelpers.GetAgentDataDomain();
        updateDomain.KnowledgeBaseDocument = [TestsHelpers.CreateMockFormFile("new.pdf", "application/pdf")];
        updateDomain.RemovedKnowledgeBaseDocuments = [];

        _mockBlobStorageManager
            .Setup(b => b.UploadDocumentsToStorageAsync(It.IsAny<IFormFile>(), It.IsAny<string>(), UploadedFileType.KnowledgeBaseDocument, It.IsAny<CancellationToken>()))
            .ReturnsAsync("https://storage.example.com/new.pdf");
        _mockKnowledgeBaseProcessor
            .Setup(k => k.DetectAndReadFileContent(It.IsAny<KnowledgeBaseDocumentDomain>()))
            .Returns("new content");
        _mockKnowledgeBaseProcessor
            .Setup(k => k.ProcessKnowledgeBaseDocumentAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _service.HandleKnowledgeBaseDataUpdateAsync(updateDomain, existingAgent);

        // Assert
        _mockBlobStorageManager.Verify(
            b => b.UploadDocumentsToStorageAsync(It.IsAny<IFormFile>(), It.IsAny<string>(), UploadedFileType.KnowledgeBaseDocument, It.IsAny<CancellationToken>()),
            Times.Once
        );
        _mockKnowledgeBaseProcessor.Verify(
            k => k.ProcessKnowledgeBaseDocumentAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Once
        );
        Assert.Equal(2, updateDomain.StoredKnowledgeBase.Count);
    }

    /// <summary>
    /// Handles the knowledge base data update asynchronous remove and add documents updates stored knowledge base correctly.
    /// </summary>
    [Fact]
    public async Task HandleKnowledgeBaseDataUpdateAsync_RemoveAndAddDocuments_UpdatesStoredKnowledgeBaseCorrectly()
    {
        // Arrange
        var existingAgent = TestsHelpers.GetAgentDataDomain();
        existingAgent.StoredKnowledgeBase =
        [
            new KnowledgeBaseDocumentDomain { FileName = "keep.pdf" },
            new KnowledgeBaseDocumentDomain { FileName = "remove.pdf" }
        ];

        var updateDomain = TestsHelpers.GetAgentDataDomain();
        updateDomain.RemovedKnowledgeBaseDocuments = ["remove.pdf"];
        updateDomain.KnowledgeBaseDocument = [TestsHelpers.CreateMockFormFile("new.pdf", "application/pdf")];

        _mockBlobStorageManager
            .Setup(b => b.UploadDocumentsToStorageAsync(It.IsAny<IFormFile>(), It.IsAny<string>(), UploadedFileType.KnowledgeBaseDocument, It.IsAny<CancellationToken>()))
            .ReturnsAsync("https://storage.example.com/new.pdf");
        _mockKnowledgeBaseProcessor
            .Setup(k => k.DetectAndReadFileContent(It.IsAny<KnowledgeBaseDocumentDomain>()))
            .Returns("content");
        _mockKnowledgeBaseProcessor
            .Setup(k => k.ProcessKnowledgeBaseDocumentAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _service.HandleKnowledgeBaseDataUpdateAsync(updateDomain, existingAgent);

        // Assert
        Assert.Equal(2, updateDomain.StoredKnowledgeBase.Count);
        Assert.Contains(updateDomain.StoredKnowledgeBase, d => d.FileName == "keep.pdf");
        Assert.DoesNotContain(updateDomain.StoredKnowledgeBase, d => d.FileName == "remove.pdf");
    }

    /// <summary>
    /// Handles the knowledge base data update asynchronous invalid file format throws ai agents business exception.
    /// </summary>
    [Fact]
    public async Task HandleKnowledgeBaseDataUpdateAsync_InvalidFileFormat_ThrowsAIAgentsBusinessException()
    {
        // Arrange
        var existingAgent = TestsHelpers.GetAgentDataDomain();
        var updateDomain = TestsHelpers.GetAgentDataDomain();
        updateDomain.KnowledgeBaseDocument = [TestsHelpers.CreateMockFormFile("malware.exe")];

        // Act & Assert
        await Assert.ThrowsAsync<AIAgentsBusinessException>(() =>
            _service.HandleKnowledgeBaseDataUpdateAsync(updateDomain, existingAgent));
    }

    /// <summary>
    /// Handles the knowledge base data update asynchronous existing agent null stored knowledge base starts from empty list.
    /// </summary>
    [Fact]
    public async Task HandleKnowledgeBaseDataUpdateAsync_ExistingAgentNullStoredKnowledgeBase_StartsFromEmptyList()
    {
        // Arrange
        var existingAgent = TestsHelpers.GetAgentDataDomain();
        existingAgent.StoredKnowledgeBase = null!;

        var updateDomain = TestsHelpers.GetAgentDataDomain();
        updateDomain.KnowledgeBaseDocument = [TestsHelpers.CreateMockFormFile("new.pdf", "application/pdf")];
        updateDomain.RemovedKnowledgeBaseDocuments = [];

        _mockBlobStorageManager
            .Setup(b => b.UploadDocumentsToStorageAsync(It.IsAny<IFormFile>(), It.IsAny<string>(), UploadedFileType.KnowledgeBaseDocument, It.IsAny<CancellationToken>()))
            .ReturnsAsync("https://storage.example.com/new.pdf");
        _mockKnowledgeBaseProcessor
            .Setup(k => k.DetectAndReadFileContent(It.IsAny<KnowledgeBaseDocumentDomain>()))
            .Returns("content");
        _mockKnowledgeBaseProcessor
            .Setup(k => k.ProcessKnowledgeBaseDocumentAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _service.HandleKnowledgeBaseDataUpdateAsync(updateDomain, existingAgent);

        // Assert
        Assert.NotNull(updateDomain.StoredKnowledgeBase);
        Assert.Single(updateDomain.StoredKnowledgeBase);
    }

    /// <summary>
    /// Handles the knowledge base data update asynchronous blob storage throws exception throws ai agents business exception.
    /// </summary>
    [Fact]
    public async Task HandleKnowledgeBaseDataUpdateAsync_BlobStorageThrowsException_ThrowsAIAgentsBusinessException()
    {
        // Arrange
        var existingAgent = TestsHelpers.GetAgentDataDomain();
        var updateDomain = TestsHelpers.GetAgentDataDomain();
        updateDomain.KnowledgeBaseDocument = [TestsHelpers.CreateMockFormFile("doc.pdf", "application/pdf")];

        _mockBlobStorageManager
            .Setup(b => b.UploadDocumentsToStorageAsync(It.IsAny<IFormFile>(), It.IsAny<string>(), It.IsAny<UploadedFileType>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Storage error"));

        // Act & Assert
        var ex = await Assert.ThrowsAsync<AIAgentsBusinessException>(() =>
            _service.HandleKnowledgeBaseDataUpdateAsync(updateDomain, existingAgent));

        Assert.Contains("Storage error", ex.Message);
    }

    /// <summary>
    /// Handles the knowledge base data update asynchronous removed documents case insensitive removes correctly.
    /// </summary>
    [Fact]
    public async Task HandleKnowledgeBaseDataUpdateAsync_RemovedDocumentsCaseInsensitive_RemovesCorrectly()
    {
        // Arrange
        var existingAgent = TestsHelpers.GetAgentDataDomain();
        existingAgent.StoredKnowledgeBase =
        [
            new KnowledgeBaseDocumentDomain { FileName = "Document.PDF" },
            new KnowledgeBaseDocumentDomain { FileName = "keep.pdf" }
        ];

        var updateDomain = TestsHelpers.GetAgentDataDomain();
        updateDomain.RemovedKnowledgeBaseDocuments = ["document.pdf"];
        updateDomain.KnowledgeBaseDocument = [];

        // Act
        await _service.HandleKnowledgeBaseDataUpdateAsync(updateDomain, existingAgent);

        // Assert
        Assert.Single(updateDomain.StoredKnowledgeBase);
        Assert.Equal("keep.pdf", updateDomain.StoredKnowledgeBase[0].FileName);
    }

    #endregion

    #region DownloadKnowledgebaseFileAsync

    /// <summary>
    /// Downloads the knowledgebase file asynchronous success returns download url.
    /// </summary>
    [Fact]
    public async Task DownloadKnowledgebaseFileAsync_Success_ReturnsDownloadUrl()
    {
        // Arrange
        var agentGuid = TestsHelpers.TestGuidId;
        var fileName = "document.pdf";
        var expectedUrl = "https://storage.example.com/download/document.pdf";

        _mockBlobStorageManager
            .Setup(b => b.DownloadFileFromBlobStorageAsync(agentGuid, fileName, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedUrl);

        // Act
        var result = await _service.DownloadKnowledgebaseFileAsync(agentGuid, fileName);

        // Assert
        Assert.Equal(expectedUrl, result);
        _mockBlobStorageManager.Verify(
            b => b.DownloadFileFromBlobStorageAsync(agentGuid, fileName, It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    /// <summary>
    /// Downloads the knowledgebase file asynchronous blob storage throws exception throws ai agents business exception.
    /// </summary>
    [Fact]
    public async Task DownloadKnowledgebaseFileAsync_BlobStorageThrowsException_ThrowsAIAgentsBusinessException()
    {
        // Arrange
        _mockBlobStorageManager
            .Setup(b => b.DownloadFileFromBlobStorageAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Download failed"));

        // Act & Assert
        var ex = await Assert.ThrowsAsync<AIAgentsBusinessException>(() =>
            _service.DownloadKnowledgebaseFileAsync(TestsHelpers.TestGuidId, "file.pdf"));

        Assert.Contains("Download failed", ex.Message);
    }

    /// <summary>
    /// Downloads the knowledgebase file asynchronous passes correct parameters to blob storage.
    /// </summary>
    [Theory]
    [InlineData("agent-123", "report.pdf")]
    [InlineData("agent-456", "manual.docx")]
    [InlineData("agent-789", "notes.txt")]
    public async Task DownloadKnowledgebaseFileAsync_PassesCorrectParametersToBlobStorage(string agentGuid, string fileName)
    {
        // Arrange
        _mockBlobStorageManager
            .Setup(b => b.DownloadFileFromBlobStorageAsync(agentGuid, fileName, It.IsAny<CancellationToken>()))
            .ReturnsAsync("https://storage.example.com/file");

        // Act
        await _service.DownloadKnowledgebaseFileAsync(agentGuid, fileName);

        // Assert
        _mockBlobStorageManager.Verify(
            b => b.DownloadFileFromBlobStorageAsync(agentGuid, fileName, It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    #endregion

    #region CreateAndProcessAiVisionImagesKeywordsAsync

    /// <summary>
    /// Creates and processes ai vision images keywords asynchronous null vision images returns without processing.
    /// </summary>
    [Fact]
    public async Task CreateAndProcessAiVisionImagesKeywordsAsync_NullVisionImages_ReturnsWithoutProcessing()
    {
        // Arrange
        var agentData = TestsHelpers.GetAgentDataDomain();
        agentData.VisionImages = null;

        // Act
        await _service.CreateAndProcessAiVisionImagesKeywordsAsync(agentData);

        // Assert
        _mockBlobStorageManager.Verify(
            b => b.UploadDocumentsToStorageAsync(It.IsAny<IFormFile>(), It.IsAny<string>(), It.IsAny<UploadedFileType>(), It.IsAny<CancellationToken>()),
            Times.Never
        );
    }

    /// <summary>
    /// Creates and processes ai vision images keywords asynchronous empty vision images returns without processing.
    /// </summary>
    [Fact]
    public async Task CreateAndProcessAiVisionImagesKeywordsAsync_EmptyVisionImages_ReturnsWithoutProcessing()
    {
        // Arrange
        var agentData = TestsHelpers.GetAgentDataDomain();
        agentData.VisionImages = [];

        // Act
        await _service.CreateAndProcessAiVisionImagesKeywordsAsync(agentData);

        // Assert
        _mockBlobStorageManager.Verify(
            b => b.UploadDocumentsToStorageAsync(It.IsAny<IFormFile>(), It.IsAny<string>(), It.IsAny<UploadedFileType>(), It.IsAny<CancellationToken>()),
            Times.Never
        );
    }

    /// <summary>
    /// Creates and processes ai vision images keywords asynchronous invalid image format throws ai agents business exception.
    /// </summary>
    [Fact]
    public async Task CreateAndProcessAiVisionImagesKeywordsAsync_InvalidImageFormat_ThrowsAIAgentsBusinessException()
    {
        // Arrange
        var agentData = TestsHelpers.GetAgentDataDomain();
        agentData.VisionImages = [TestsHelpers.CreateMockFormFile("image.bmp")];

        // Act & Assert
        await Assert.ThrowsAsync<AIAgentsBusinessException>(() =>
            _service.CreateAndProcessAiVisionImagesKeywordsAsync(agentData));
    }

    /// <summary>
    /// Creates and processes ai vision images keywords asynchronous valid image uploads and extracts keywords.
    /// </summary>
    [Fact]
    public async Task CreateAndProcessAiVisionImagesKeywordsAsync_ValidImage_UploadsAndExtractsKeywords()
    {
        // Arrange
        var agentData = TestsHelpers.GetAgentDataDomain();
        agentData.VisionImages = [TestsHelpers.CreateMockFormFile("photo.jpg", "image/jpeg")];
        var imageUrl = "https://storage.example.com/photo.jpg";
        var keywords = new List<string> { "cat", "animal", "pet" };

        _mockBlobStorageManager
            .Setup(b => b.UploadDocumentsToStorageAsync(It.IsAny<IFormFile>(), agentData.AgentId, UploadedFileType.AiVisionImageDocument, It.IsAny<CancellationToken>()))
            .ReturnsAsync(imageUrl);
        _mockVisionProcessor
            .Setup(v => v.ReadDataFromImageWithComputerVisionAsync(imageUrl, It.IsAny<CancellationToken>()))
            .ReturnsAsync(keywords);

        // Act
        await _service.CreateAndProcessAiVisionImagesKeywordsAsync(agentData);

        // Assert
        _mockBlobStorageManager.Verify(
            b => b.UploadDocumentsToStorageAsync(It.IsAny<IFormFile>(), agentData.AgentId, UploadedFileType.AiVisionImageDocument, It.IsAny<CancellationToken>()),
            Times.Once
        );
        _mockVisionProcessor.Verify(
            v => v.ReadDataFromImageWithComputerVisionAsync(imageUrl, It.IsAny<CancellationToken>()),
            Times.Once
        );
        Assert.Single(agentData.AiVisionImagesData);
        Assert.Equal("photo.jpg", agentData.AiVisionImagesData[0]!.ImageName);
        Assert.Equal(imageUrl, agentData.AiVisionImagesData[0]!.ImageUrl);
        Assert.Equal(keywords, agentData.AiVisionImagesData[0]!.ImageKeywords);
    }

    /// <summary>
    /// Creates and processes ai vision images keywords asynchronous multiple valid images uploads and processes all.
    /// </summary>
    [Fact]
    public async Task CreateAndProcessAiVisionImagesKeywordsAsync_MultipleValidImages_UploadsAndProcessesAll()
    {
        // Arrange
        var agentData = TestsHelpers.GetAgentDataDomain();
        agentData.VisionImages =
        [
            TestsHelpers.CreateMockFormFile("photo1.jpg", "image/jpeg"),
            TestsHelpers.CreateMockFormFile("photo2.png", "image/png")
        ];

        _mockBlobStorageManager
            .Setup(b => b.UploadDocumentsToStorageAsync(It.IsAny<IFormFile>(), It.IsAny<string>(), UploadedFileType.AiVisionImageDocument, It.IsAny<CancellationToken>()))
            .ReturnsAsync("https://storage.example.com/image");
        _mockVisionProcessor
            .Setup(v => v.ReadDataFromImageWithComputerVisionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(["keyword"]);

        // Act
        await _service.CreateAndProcessAiVisionImagesKeywordsAsync(agentData);

        // Assert
        _mockBlobStorageManager.Verify(
            b => b.UploadDocumentsToStorageAsync(It.IsAny<IFormFile>(), It.IsAny<string>(), UploadedFileType.AiVisionImageDocument, It.IsAny<CancellationToken>()),
            Times.Exactly(2)
        );
        Assert.Equal(2, agentData.AiVisionImagesData.Count);
    }

    /// <summary>
    /// Creates and processes ai vision images keywords asynchronous empty image url skips vision processing.
    /// </summary>
    [Fact]
    public async Task CreateAndProcessAiVisionImagesKeywordsAsync_EmptyImageUrl_SkipsVisionProcessing()
    {
        // Arrange
        var agentData = TestsHelpers.GetAgentDataDomain();
        agentData.VisionImages = [TestsHelpers.CreateMockFormFile("photo.jpg", "image/jpeg")];

        _mockBlobStorageManager
            .Setup(b => b.UploadDocumentsToStorageAsync(It.IsAny<IFormFile>(), It.IsAny<string>(), It.IsAny<UploadedFileType>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(string.Empty);

        // Act
        await _service.CreateAndProcessAiVisionImagesKeywordsAsync(agentData);

        // Assert
        _mockVisionProcessor.Verify(
            v => v.ReadDataFromImageWithComputerVisionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Never
        );
        Assert.Empty(agentData.AiVisionImagesData);
    }

    /// <summary>
    /// Creates and processes ai vision images keywords asynchronous blob storage throws exception throws ai agents business exception.
    /// </summary>
    [Fact]
    public async Task CreateAndProcessAiVisionImagesKeywordsAsync_BlobStorageThrowsException_ThrowsAIAgentsBusinessException()
    {
        // Arrange
        var agentData = TestsHelpers.GetAgentDataDomain();
        agentData.VisionImages = [TestsHelpers.CreateMockFormFile("photo.jpg", "image/jpeg")];

        _mockBlobStorageManager
            .Setup(b => b.UploadDocumentsToStorageAsync(It.IsAny<IFormFile>(), It.IsAny<string>(), It.IsAny<UploadedFileType>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Upload failed"));

        // Act & Assert
        var ex = await Assert.ThrowsAsync<AIAgentsBusinessException>(() =>
            _service.CreateAndProcessAiVisionImagesKeywordsAsync(agentData));

        Assert.Contains("Upload failed", ex.Message);
    }

    /// <summary>
    /// Creates and processes ai vision images keywords asynchronous vision processor throws exception throws ai agents business exception.
    /// </summary>
    [Fact]
    public async Task CreateAndProcessAiVisionImagesKeywordsAsync_VisionProcessorThrowsException_ThrowsAIAgentsBusinessException()
    {
        // Arrange
        var agentData = TestsHelpers.GetAgentDataDomain();
        agentData.VisionImages = [TestsHelpers.CreateMockFormFile("photo.jpg", "image/jpeg")];

        _mockBlobStorageManager
            .Setup(b => b.UploadDocumentsToStorageAsync(It.IsAny<IFormFile>(), It.IsAny<string>(), It.IsAny<UploadedFileType>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync("https://storage.example.com/photo.jpg");
        _mockVisionProcessor
            .Setup(v => v.ReadDataFromImageWithComputerVisionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Vision processing failed"));

        // Act & Assert
        var ex = await Assert.ThrowsAsync<AIAgentsBusinessException>(() =>
            _service.CreateAndProcessAiVisionImagesKeywordsAsync(agentData));

        Assert.Contains("Vision processing failed", ex.Message);
    }

    #endregion

    #region HandleAiVisionImagesDataUpdateAsync

    /// <summary>
    /// Handles the ai vision images data update asynchronous no changes does not update stored images.
    /// </summary>
    [Fact]
    public async Task HandleAiVisionImagesDataUpdateAsync_NoChanges_DoesNotUpdateStoredImages()
    {
        // Arrange
        var existingAgent = TestsHelpers.GetAgentDataDomain();
        existingAgent.AiVisionImagesData = [new AiVisionImagesDomain { ImageName = "existing.jpg" }];

        var updateDomain = TestsHelpers.GetAgentDataDomain();
        updateDomain.VisionImages = [];
        updateDomain.RemovedAiVisionImages = [];

        // Act
        await _service.HandleAiVisionImagesDataUpdateAsync(updateDomain, existingAgent);

        // Assert
        _mockBlobStorageManager.Verify(
            b => b.UploadDocumentsToStorageAsync(It.IsAny<IFormFile>(), It.IsAny<string>(), It.IsAny<UploadedFileType>(), It.IsAny<CancellationToken>()),
            Times.Never
        );
    }

    /// <summary>
    /// Handles the ai vision images data update asynchronous with removed images removes them from stored images.
    /// </summary>
    [Fact]
    public async Task HandleAiVisionImagesDataUpdateAsync_WithRemovedImages_RemovesThemFromStoredImages()
    {
        // Arrange
        var existingAgent = TestsHelpers.GetAgentDataDomain();
        existingAgent.AiVisionImagesData =
        [
            new AiVisionImagesDomain { ImageName = "keep.jpg" },
            new AiVisionImagesDomain { ImageName = "remove.jpg" }
        ];

        var updateDomain = TestsHelpers.GetAgentDataDomain();
        updateDomain.RemovedAiVisionImages = ["remove.jpg"];
        updateDomain.VisionImages = [];

        // Act
        await _service.HandleAiVisionImagesDataUpdateAsync(updateDomain, existingAgent);

        // Assert
        Assert.Single(updateDomain.AiVisionImagesData);
        Assert.Equal("keep.jpg", updateDomain.AiVisionImagesData[0]!.ImageName);
    }

    /// <summary>
    /// Handles the ai vision images data update asynchronous remove all images sets ai vision images data to empty list.
    /// </summary>
    [Fact]
    public async Task HandleAiVisionImagesDataUpdateAsync_RemoveAllImages_SetsAiVisionImagesDataToEmptyList()
    {
        // Arrange
        var existingAgent = TestsHelpers.GetAgentDataDomain();
        existingAgent.AiVisionImagesData = [new AiVisionImagesDomain { ImageName = "only.jpg" }];

        var updateDomain = TestsHelpers.GetAgentDataDomain();
        updateDomain.RemovedAiVisionImages = ["only.jpg"];
        updateDomain.VisionImages = [];

        // Act
        await _service.HandleAiVisionImagesDataUpdateAsync(updateDomain, existingAgent);

        // Assert
        Assert.NotNull(updateDomain.AiVisionImagesData);
        Assert.Empty(updateDomain.AiVisionImagesData);
    }

    /// <summary>
    /// Handles the ai vision images data update asynchronous with new images uploads and processes them.
    /// </summary>
    [Fact]
    public async Task HandleAiVisionImagesDataUpdateAsync_WithNewImages_UploadsAndProcessesThem()
    {
        // Arrange
        var existingAgent = TestsHelpers.GetAgentDataDomain();
        existingAgent.AiVisionImagesData = [new AiVisionImagesDomain { ImageName = "existing.jpg" }];

        var updateDomain = TestsHelpers.GetAgentDataDomain();
        updateDomain.VisionImages = [TestsHelpers.CreateMockFormFile("new.jpg", "image/jpeg")];
        updateDomain.RemovedAiVisionImages = [];

        var imageUrl = "https://storage.example.com/new.jpg";
        var keywords = new List<string> { "new", "image" };

        _mockBlobStorageManager
            .Setup(b => b.UploadDocumentsToStorageAsync(It.IsAny<IFormFile>(), It.IsAny<string>(), UploadedFileType.AiVisionImageDocument, It.IsAny<CancellationToken>()))
            .ReturnsAsync(imageUrl);
        _mockVisionProcessor
            .Setup(v => v.ReadDataFromImageWithComputerVisionAsync(imageUrl, It.IsAny<CancellationToken>()))
            .ReturnsAsync(keywords);

        // Act
        await _service.HandleAiVisionImagesDataUpdateAsync(updateDomain, existingAgent);

        // Assert
        _mockBlobStorageManager.Verify(
            b => b.UploadDocumentsToStorageAsync(It.IsAny<IFormFile>(), It.IsAny<string>(), UploadedFileType.AiVisionImageDocument, It.IsAny<CancellationToken>()),
            Times.Once
        );
        _mockVisionProcessor.Verify(
            v => v.ReadDataFromImageWithComputerVisionAsync(imageUrl, It.IsAny<CancellationToken>()),
            Times.Once
        );
        Assert.Equal(2, updateDomain.AiVisionImagesData.Count);
    }

    /// <summary>
    /// Handles the ai vision images data update asynchronous remove and add images updates stored images correctly.
    /// </summary>
    [Fact]
    public async Task HandleAiVisionImagesDataUpdateAsync_RemoveAndAddImages_UpdatesStoredImagesCorrectly()
    {
        // Arrange
        var existingAgent = TestsHelpers.GetAgentDataDomain();
        existingAgent.AiVisionImagesData =
        [
            new AiVisionImagesDomain { ImageName = "keep.jpg" },
            new AiVisionImagesDomain { ImageName = "remove.jpg" }
        ];

        var updateDomain = TestsHelpers.GetAgentDataDomain();
        updateDomain.RemovedAiVisionImages = ["remove.jpg"];
        updateDomain.VisionImages = [TestsHelpers.CreateMockFormFile("new.png", "image/png")];

        _mockBlobStorageManager
            .Setup(b => b.UploadDocumentsToStorageAsync(It.IsAny<IFormFile>(), It.IsAny<string>(), UploadedFileType.AiVisionImageDocument, It.IsAny<CancellationToken>()))
            .ReturnsAsync("https://storage.example.com/new.png");
        _mockVisionProcessor
            .Setup(v => v.ReadDataFromImageWithComputerVisionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(["keyword"]);

        // Act
        await _service.HandleAiVisionImagesDataUpdateAsync(updateDomain, existingAgent);

        // Assert
        Assert.Equal(2, updateDomain.AiVisionImagesData.Count);
        Assert.Contains(updateDomain.AiVisionImagesData, i => i!.ImageName == "keep.jpg");
        Assert.DoesNotContain(updateDomain.AiVisionImagesData, i => i!.ImageName == "remove.jpg");
    }

    /// <summary>
    /// Handles the ai vision images data update asynchronous invalid image format throws ai agents business exception.
    /// </summary>
    [Fact]
    public async Task HandleAiVisionImagesDataUpdateAsync_InvalidImageFormat_ThrowsAIAgentsBusinessException()
    {
        // Arrange
        var existingAgent = TestsHelpers.GetAgentDataDomain();
        var updateDomain = TestsHelpers.GetAgentDataDomain();
        updateDomain.VisionImages = [TestsHelpers.CreateMockFormFile("image.bmp")];

        // Act & Assert
        await Assert.ThrowsAsync<AIAgentsBusinessException>(() =>
            _service.HandleAiVisionImagesDataUpdateAsync(updateDomain, existingAgent));
    }

    /// <summary>
    /// Handles the ai vision images data update asynchronous existing agent null ai vision images data starts from empty list.
    /// </summary>
    [Fact]
    public async Task HandleAiVisionImagesDataUpdateAsync_ExistingAgentNullAiVisionImagesData_StartsFromEmptyList()
    {
        // Arrange
        var existingAgent = TestsHelpers.GetAgentDataDomain();
        existingAgent.AiVisionImagesData = null!;

        var updateDomain = TestsHelpers.GetAgentDataDomain();
        updateDomain.VisionImages = [TestsHelpers.CreateMockFormFile("new.jpg", "image/jpeg")];
        updateDomain.RemovedAiVisionImages = [];

        _mockBlobStorageManager
            .Setup(b => b.UploadDocumentsToStorageAsync(It.IsAny<IFormFile>(), It.IsAny<string>(), UploadedFileType.AiVisionImageDocument, It.IsAny<CancellationToken>()))
            .ReturnsAsync("https://storage.example.com/new.jpg");
        _mockVisionProcessor
            .Setup(v => v.ReadDataFromImageWithComputerVisionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(["keyword"]);

        // Act
        await _service.HandleAiVisionImagesDataUpdateAsync(updateDomain, existingAgent);

        // Assert
        Assert.NotNull(updateDomain.AiVisionImagesData);
        Assert.Single(updateDomain.AiVisionImagesData);
    }

    /// <summary>
    /// Handles the ai vision images data update asynchronous blob storage throws exception throws ai agents business exception.
    /// </summary>
    [Fact]
    public async Task HandleAiVisionImagesDataUpdateAsync_BlobStorageThrowsException_ThrowsAIAgentsBusinessException()
    {
        // Arrange
        var existingAgent = TestsHelpers.GetAgentDataDomain();
        var updateDomain = TestsHelpers.GetAgentDataDomain();
        updateDomain.VisionImages = [TestsHelpers.CreateMockFormFile("photo.jpg", "image/jpeg")];

        _mockBlobStorageManager
            .Setup(b => b.UploadDocumentsToStorageAsync(It.IsAny<IFormFile>(), It.IsAny<string>(), It.IsAny<UploadedFileType>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Storage error"));

        // Act & Assert
        var ex = await Assert.ThrowsAsync<AIAgentsBusinessException>(() =>
            _service.HandleAiVisionImagesDataUpdateAsync(updateDomain, existingAgent));

        Assert.Contains("Storage error", ex.Message);
    }

    /// <summary>
    /// Handles the ai vision images data update asynchronous vision processor throws exception throws ai agents business exception.
    /// </summary>
    [Fact]
    public async Task HandleAiVisionImagesDataUpdateAsync_VisionProcessorThrowsException_ThrowsAIAgentsBusinessException()
    {
        // Arrange
        var existingAgent = TestsHelpers.GetAgentDataDomain();
        var updateDomain = TestsHelpers.GetAgentDataDomain();
        updateDomain.VisionImages = [TestsHelpers.CreateMockFormFile("photo.jpg", "image/jpeg")];

        _mockBlobStorageManager
            .Setup(b => b.UploadDocumentsToStorageAsync(It.IsAny<IFormFile>(), It.IsAny<string>(), It.IsAny<UploadedFileType>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync("https://storage.example.com/photo.jpg");
        _mockVisionProcessor
            .Setup(v => v.ReadDataFromImageWithComputerVisionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Vision error"));

        // Act & Assert
        var ex = await Assert.ThrowsAsync<AIAgentsBusinessException>(() =>
            _service.HandleAiVisionImagesDataUpdateAsync(updateDomain, existingAgent));

        Assert.Contains("Vision error", ex.Message);
    }

    /// <summary>
    /// Handles the ai vision images data update asynchronous removed images case insensitive removes correctly.
    /// </summary>
    [Fact]
    public async Task HandleAiVisionImagesDataUpdateAsync_RemovedImagesCaseInsensitive_RemovesCorrectly()
    {
        // Arrange
        var existingAgent = TestsHelpers.GetAgentDataDomain();
        existingAgent.AiVisionImagesData =
        [
            new AiVisionImagesDomain { ImageName = "Photo.JPG" },
            new AiVisionImagesDomain { ImageName = "keep.jpg" }
        ];

        var updateDomain = TestsHelpers.GetAgentDataDomain();
        updateDomain.RemovedAiVisionImages = ["photo.jpg"];
        updateDomain.VisionImages = [];

        // Act
        await _service.HandleAiVisionImagesDataUpdateAsync(updateDomain, existingAgent);

        // Assert
        Assert.Single(updateDomain.AiVisionImagesData);
        Assert.Equal("keep.jpg", updateDomain.AiVisionImagesData[0]!.ImageName);
    }

    /// <summary>
    /// Handles the ai vision images data update asynchronous new image stores correct image name and url.
    /// </summary>
    [Fact]
    public async Task HandleAiVisionImagesDataUpdateAsync_NewImage_StoresCorrectImageNameAndUrl()
    {
        // Arrange
        var existingAgent = TestsHelpers.GetAgentDataDomain();
        var updateDomain = TestsHelpers.GetAgentDataDomain();
        updateDomain.VisionImages = [TestsHelpers.CreateMockFormFile("landscape.png", "image/png")];
        updateDomain.RemovedAiVisionImages = [];

        var expectedUrl = "https://storage.example.com/landscape.png";
        var expectedKeywords = new List<string> { "landscape", "nature" };

        _mockBlobStorageManager
            .Setup(b => b.UploadDocumentsToStorageAsync(It.IsAny<IFormFile>(), It.IsAny<string>(), UploadedFileType.AiVisionImageDocument, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedUrl);
        _mockVisionProcessor
            .Setup(v => v.ReadDataFromImageWithComputerVisionAsync(expectedUrl, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedKeywords);

        // Act
        await _service.HandleAiVisionImagesDataUpdateAsync(updateDomain, existingAgent);

        // Assert
        Assert.Single(updateDomain.AiVisionImagesData);
        Assert.Equal("landscape.png", updateDomain.AiVisionImagesData[0]!.ImageName);
        Assert.Equal(expectedUrl, updateDomain.AiVisionImagesData[0]!.ImageUrl);
        Assert.Equal(expectedKeywords, updateDomain.AiVisionImagesData[0]!.ImageKeywords);
    }

    #endregion
}
