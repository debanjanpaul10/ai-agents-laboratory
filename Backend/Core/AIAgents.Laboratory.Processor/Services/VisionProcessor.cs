using AIAgents.Laboratory.Processor.Contracts;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using static AIAgents.Laboratory.Processor.Helpers.ProcessorConstants;

namespace AIAgents.Laboratory.Processor.Services;

/// <summary>
/// Provides functionality to extract text data from images using a computer vision service.
/// </summary>
/// <remarks>This class implements the IVisionProcessor interface and is intended for use in scenarios where automated extraction of textual information from images is required. The class relies on external configuration for
/// service credentials and is designed to be used as part of a larger image processing or document analysis workflow.</remarks>
/// <param name="configuration">The application configuration used to retrieve computer vision service credentials and endpoints.</param>
/// <param name="logger">The logger instance used to record diagnostic and operational information for the vision processing operations.</param>
/// <seealso cref="IVisionProcessor"/>
public sealed class VisionProcessor(IConfiguration configuration, ILogger<VisionProcessor> logger) : IVisionProcessor
{
    /// <summary>
    /// Asynchronously extracts text data from an image located at the specified URL using a computer vision service.
    /// </summary>
    /// <remarks>This method uses an external computer vision service to perform optical character recognition
    /// (OCR) on the image. Network connectivity and appropriate service credentials are required. The operation may take several seconds to complete depending on image size and service response time.</remarks>
    /// <param name="imageUrl">The URL of the image to analyze. Must be a valid, accessible image URL.</param>
    /// <returns>A collection of strings containing the lines of text recognized in the image. The collection is empty if no text is found.</returns>
    public async Task<IEnumerable<string>> ReadDataFromImageWithComputerVisionAsync(string imageUrl)
    {
        try
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(ReadDataFromImageWithComputerVisionAsync), DateTime.UtcNow, imageUrl);

            IList<string> imageTextData = [];

            // Step 1: Prepare the computer vision client.
            var computerVisionClient = this.PrepareComputerVisionClient();

            // Step 2: Read text from URL and get operation location.
            var textHeaders = await computerVisionClient.ReadAsync(imageUrl).ConfigureAwait(false);
            var operationLocation = textHeaders.OperationLocation;

            // Step 3: Retrieve the URI where the extracted text will be stored from the Operation-Location header.
            var numberOfCharsInOperationId = AiVisionConstants.NUMBER_OF_CHARACTERS_IN_OPERATION_ID;
            var operationId = operationLocation[^numberOfCharsInOperationId..];

            // Step 4: Extract text
            ReadOperationResult results;
            do
            {
                results = await computerVisionClient.GetReadResultAsync(Guid.Parse(operationId)).ConfigureAwait(false);
            }
            while (results.Status == OperationStatusCodes.Running || results.Status == OperationStatusCodes.NotStarted);

            // Step 5: Read and return the found text.
            var textUrlFileResults = results.AnalyzeResult.ReadResults;
            foreach (var page in textUrlFileResults)
                foreach (var line in page.Lines)
                    imageTextData.Add(line.Text);

            return imageTextData;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, LoggingConstants.LogHelperMethodFailed, nameof(ReadDataFromImageWithComputerVisionAsync), DateTime.UtcNow, ex.Message);
            return [];
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(ReadDataFromImageWithComputerVisionAsync), DateTime.UtcNow, imageUrl);
        }
    }

    #region PRIVATE METHODS

    /// <summary>
    /// Creates and configures a new instance of the ComputerVisionClient using application configuration settings.
    /// </summary>
    /// <remarks>The client is configured using the values of the "AiVisionKey" and "Endpoint" entries from the application's configuration. Ensure these configuration values are set before calling this method.</remarks>
    /// <returns>A ComputerVisionClient instance initialized with the API key and endpoint specified in the configuration.</returns>
    private ComputerVisionClient PrepareComputerVisionClient()
    {
        var aiVisionKey = configuration[AzureAppConfigurationConstants.AzureAiVisionKey];
        var aiVisionEndpoint = configuration[AzureAppConfigurationConstants.AzureAiVisionEndpoint];

        var aiVisionClient = new ComputerVisionClient(new ApiKeyServiceClientCredentials(aiVisionKey)) { Endpoint = aiVisionEndpoint };
        return aiVisionClient;
    }

    #endregion
}
