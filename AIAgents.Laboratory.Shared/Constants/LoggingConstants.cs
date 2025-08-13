namespace AIAgents.Laboratory.Shared.Constants;

/// <summary>
/// Logging constants.
/// </summary>
public static class LoggingConstants
{
	/// <summary>
	/// The log helper method start.
	/// </summary>
	public const string LogHelperMethodStart = "{0} started at {1}";

	/// <summary>
	/// The log helper method failed.
	/// </summary>
	public const string LogHelperMethodFailed = "{0} failed at {1} with {2}";

	/// <summary>
	/// The log helper method end.
	/// </summary>
	public const string LogHelperMethodEnd = "{0} ended at {1}";

}

/// <summary>
/// The Exception Constants Class.
/// </summary>
public static class ExceptionConstants
{
	/// <summary>
	/// The story cannot be empty message.
	/// </summary>
	public const string StoryCannotBeEmptyMessage = "The entered story/string is empty!";

	/// <summary>
	/// The ai services down message.
	/// </summary>
	public const string AiServicesDownMessage = "We are facing technical issues with our AI services. Please try again after sometime.";

	/// <summary>
	/// The plugin not found message.
	/// </summary>
	public const string PluginNotFoundMessage = "Plugin not found!";

	/// <summary>
	/// The ai api key missing message.
	/// </summary>
	public const string AiAPIKeyMissingMessage = "The AI Api Key is missing in configuration.";

	/// <summary>
	/// The missing configuration message.
	/// </summary>
	public const string MissingConfigurationMessage = "The Configuration Key is missing";

	/// <summary>
	/// The user unauthorized message constant
	/// </summary>
	public const string UserUnauthorizedMessageConstant = "User Not Authorized";

	/// <summary>
	/// The ai services down message constant.
	/// </summary>
	public const string AiServicesDownMessageConstant = "Our AI Services are down right now. Please try again after sometime.";

	/// <summary>
	/// The authorization missing message.
	/// </summary>
	public const string AuthorizationMissingMessage = "Authorization header is missing or empty.";

	/// <summary>
	/// The token missing message.
	/// </summary>
	public const string TokenMissingMessage = "Token value is missing or empty.";

	/// <summary>
	/// The application id mismatch message.
	/// </summary>
	public const string ApplicationIdMismatchMessage = "Application ID does not match the configured client ID.";

	/// <summary>
	/// The token expiry missing message.
	/// </summary>
	public const string TokenExpiryMissingMessage = "Token expiration time is invalid or missing.";

	/// <summary>
	/// The token expired message constant.
	/// </summary>
	public const string TokenExpiredMessageConstant = "Token has expired.";

	/// <summary>
	/// The plugins directory is missing.
	/// </summary>
	public const string PluginsDirectoryIsMissing = "Oops! The plugins directory has not been properly configured yet!";

	/// <summary>
	/// The user id not present exception constant.
	/// </summary>
	public const string UserIdNotPresentExceptionConstant = "User id is not present in the headers.";

	/// <summary>
	/// The invalid token exception constant.
	/// </summary>
	public const string InvalidTokenExceptionConstant = "Invalid token: Identity is not authenticated.";

	/// <summary>
	/// The model name not found exception constant
	/// </summary>
	public const string ModelNameNotFoundExceptionConstant = "It seems the model name could not be determined";

}