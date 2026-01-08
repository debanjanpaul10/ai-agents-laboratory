
namespace AIAgents.Laboratory.API.Adapters.Models.Response;

/// <summary>
/// The AI Vision Images DTO model.
/// </summary>
/// <remarks>Will be used for AI vision images to be read by the AI Vision service and parse the words present in the image.</remarks>
public sealed record AiVisionImagesDataDTO
{
    /// <summary>
    /// Gets or sets the image name.
    /// </summary>
    /// <value>
    ///     The image name value.
    /// </value>
    public string ImageName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the image url.
    /// </summary>
    /// <value>
    ///     The image url value.
    /// </value>
    public string ImageUrl { get; set; } = string.Empty;
}
