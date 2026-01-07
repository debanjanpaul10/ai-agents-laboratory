namespace AIAgents.Laboratory.Processor.Contracts;

/// <summary>
/// Defines a contract for processing images using computer vision to extract data asynchronously.
/// </summary>
public interface IVisionProcessor
{
    /// <summary>
    /// Asynchronously extracts text data from an image located at the specified URL using a computer vision service.
    /// </summary>
    /// <remarks>This method uses an external computer vision service to perform optical character recognition
    /// (OCR) on the image. Network connectivity and appropriate service credentials are required. The operation may take several seconds to complete depending on image size and service response time.</remarks>
    /// <param name="imageUrl">The URL of the image to analyze. Must be a valid, accessible image URL.</param>
    /// <returns>A collection of strings containing the lines of text recognized in the image. The collection is empty if no text is found.</returns>
    Task<IEnumerable<string>> ReadDataFromImageWithComputerVisionAsync(string imageUrl);
}
