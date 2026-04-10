namespace AI.Agents.Laboratory.Functions.Shared.Constants;

/// <summary>
/// The exception constants class, providing a centralized location for defining constant values related to exceptions and error messages used throughout the application, ensuring consistency and maintainability in handling exceptions and providing user-friendly error responses.
/// </summary>
public static class ExceptionConstants
{
    /// <summary>
    /// The default message for unhandled exceptions, providing a user-friendly response when an unexpected error occurs during request processing.
    /// </summary>
    public const string SomethingWentWrongDefaultMessage = "Oops! Something went wrong while processing the request. Please try again after sometime!";

    /// <summary>
    /// The missing configuration message.
    /// </summary>
    public const string MissingConfigurationMessage = "The Configuration Key is missing";

    /// <summary>
    /// The invalid token exception constant.
    /// </summary>
    public const string InvalidTokenExceptionConstant = "Invalid token: Identity is not authenticated.";

    /// <summary>
    /// The unauthorized access message exception constant.
    /// </summary>
    public const string UnauthorizedAccessMessageConstant = "Unauthorized access. Please log in to continue.";

    /// <summary>
    /// The file not found exception message constant.
    /// </summary>
    public const string FileNotFoundExceptionMessageConstant = "Oops! The file could not be downloaded at this moment!";

    /// <summary>
    /// The conversation history cannot be fetched exception message constant.
    /// </summary>
    public const string ConversationHistoryCannotBeFetchedMessageConstant = "Oops! It seems the conversation history could not be fetched!";

    /// <summary>
    /// The conversation history cannot be fetched exception message constant.
    /// </summary>
    public const string ConversationHistoryCannotBeClearedMessageConstant = "Oops! It seems the conversation history cannot be cleared or there does not exists any!";

    /// <summary>
    /// The invalid bug report data message.
    /// </summary>
    public const string InvalidBugReportDataMessage = "The bug report data provided is invalid.";

    /// <summary>
    /// The invalid feature request data message.
    /// </summary>
    public const string InvalidFeatureRequestDataMessage = "The feature request data provided is invalid.";

    /// <summary>
    /// The requested data not found exception message constant.
    /// </summary>
    public const string DataCannotBeFoundExceptionMessage = "Oops! The requested data not exist!";
}
