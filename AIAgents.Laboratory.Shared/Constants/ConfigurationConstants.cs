// *********************************************************************************
//	<copyright file="ConfigurationConstants.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>The Configuration Constants Class.</summary>
// *********************************************************************************

namespace AIAgents.Laboratory.Shared.Constants;

/// <summary>
/// The Configuration Constants Class.
/// </summary>
public static class ConfigurationConstants
{
	/// <summary>
	/// The Azure App Configuration Constants.
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
}
