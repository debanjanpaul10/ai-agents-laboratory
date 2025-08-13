namespace AIAgents.Laboratory.API.Adapters.Helpers;

/// <summary>
/// The Constants Class.
/// </summary>
public static class Constants
{
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
	/// The Environment Configuration Constants.
	/// </summary>
	public static class EnvironmentConfigurationConstants
	{
		/// <summary>
		/// The local appsetings file name
		/// </summary>
		public const string LocalAppsetingsFileName = "appsettings.development.json";

		/// <summary>
		/// The application configuration endpoint key constant
		/// </summary>
		public const string AppConfigurationEndpointKeyConstant = "AppConfigurationEndpoint";

		/// <summary>
		/// The managed identity client identifier constant
		/// </summary>
		public const string ManagedIdentityClientIdConstant = "ManagedIdentityClientId";
	}

	/// <summary>
	/// The Exceptions Constants.
	/// </summary>
	public static class ExceptionConstants
	{
		/// <summary>
		/// The missing configuration message.
		/// </summary>
		public const string MissingConfigurationMessage = "The Configuration Key is missing";

		/// <summary>
		/// The invalid token exception constant.
		/// </summary>
		public const string InvalidTokenExceptionConstant = "Invalid token: Identity is not authenticated.";
	}

	/// <summary>
	/// The Azure App Configurations Constants.
	/// </summary>
	public static class AzureAppConfigurationConstants
	{
		/// <summary>
		/// The base configuration application configuration key constant
		/// </summary>
		public const string BaseConfigurationAppConfigKeyConstant = "BaseConfiguration";

		/// <summary>
		/// The azure ad tenant identifier constant
		/// </summary>
		public const string AzureAdTenantIdConstant = "TenantId";

		/// <summary>
		/// The token format url.
		/// </summary>
		public const string TokenFormatUrl = "https://login.microsoftonline.com/{0}/v2.0";

		/// <summary>
		/// The ai agents client identifier constant
		/// </summary>
		public const string AIAgentsClientIdConstant = "AiAgentsClientId";

		/// <summary>
		/// The gemini api key constant.
		/// </summary>
		public const string GeminiAPIKeyConstant = "GeminiAPIKey";

		/// <summary>
		/// The gemini flash model
		/// </summary>
		public const string GeminiFlashModel = "GeminiFlashModel";

		/// <summary>
		/// The gemini pro model
		/// </summary>
		public const string GeminiProModel = "GeminiProModel";

		/// <summary>
		/// The is pro model enabled flag
		/// </summary>
		public const string IsProModelEnabledFlag = "IsProModelEnabled";
	}
}
