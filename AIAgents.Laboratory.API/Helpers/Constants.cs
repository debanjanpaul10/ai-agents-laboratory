﻿// *********************************************************************************
//	<copyright file="Constants.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>The Constants Class.</summary>
// *********************************************************************************

namespace AIAgents.Laboratory.API.Helpers;

/// <summary>
/// The Constants Class.
/// </summary>
internal static class Constants
{
	/// <summary>
	/// The Swagger Constants class.
	/// </summary>
	internal static class SwaggerConstants
	{
		/// <summary>
		/// The api version for Swagger documentation.
		/// </summary>
		public const string ApiVersion = "v1";

		/// <summary>
		/// The swagger endpoint for the API documentation.
		/// </summary>
		public const string SwaggerEndpointUrl = "/swagger/v1/swagger.json";

		/// <summary>
		/// The swagger ui endpoint prefix.
		/// </summary>
		public const string SwaggerUiPrefix = "swaggerui";

		/// <summary>
		/// The description for the Swagger documentation.
		/// </summary>
		public const string SwaggerDescription = "API documentation for AI.Agents.Laboratory";

		/// <summary>
		/// The Author Details class contains information about the author of the API.
		/// </summary>
		public static class AuthorDetails
		{
			/// <summary>
			/// The author's name.
			/// </summary>
			public static readonly string Name = "Debanjan Paul";

			/// <summary>
			/// The author's email address.
			/// </summary>
			public static readonly string Email = "debanjanpaul10@gmail.com";
		}

		/// <summary>
		/// The API name for Swagger documentation.
		/// </summary>
		public const string ApplicationAPIName = "AI.Agents.Laboratory.API";
	}

	/// <summary>
	/// Logging constants.
	/// </summary>
	internal static class LoggingConstants
	{
		/// <summary>
		/// The log helper method start.
		/// </summary>
		internal const string LogHelperMethodStart = "{0} started at {1}";

		/// <summary>
		/// The log helper method failed.
		/// </summary>
		internal const string LogHelperMethodFailed = "{0} failed at {1} with {2}";

		/// <summary>
		/// The log helper method end.
		/// </summary>
		internal const string LogHelperMethodEnd = "{0} ended at {1}";
	}

	/// <summary>
	/// The Environment Configuration Constants.
	/// </summary>
	internal static class EnvironmentConfigurationConstants
	{
		/// <summary>
		/// The local appsetings file name
		/// </summary>
		internal const string LocalAppsetingsFileName = "appsettings.development.json";

		/// <summary>
		/// The application configuration endpoint key constant
		/// </summary>
		internal const string AppConfigurationEndpointKeyConstant = "AppConfigurationEndpoint";

		/// <summary>
		/// The managed identity client identifier constant
		/// </summary>
		internal const string ManagedIdentityClientIdConstant = "ManagedIdentityClientId";

		/// <summary>
		/// The application json constant
		/// </summary>
		internal const string ApplicationJsonConstant = "application/json";
	}

	/// <summary>
	/// The Exception Constants class.
	/// </summary>
	internal static class ExceptionConstants
	{
		/// <summary>
		/// The ai services down message constant.
		/// </summary>
		internal const string AiServicesDownMessage = "Our AI Services are down right now. Please try again after sometime.";

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
	internal static class AzureAppConfigurationConstants
	{
		/// <summary>
		/// The base configuration application configuration key constant
		/// </summary>
		internal const string BaseConfigurationAppConfigKeyConstant = "BaseConfiguration";

		/// <summary>
		/// The azure ad tenant identifier constant
		/// </summary>
		internal const string AzureAdTenantIdConstant = "TenantId";

		/// <summary>
		/// The token format url.
		/// </summary>
		internal const string TokenFormatUrl = "https://login.microsoftonline.com/{0}/v2.0";

		/// <summary>
		/// The ai agents client identifier constant
		/// </summary>
		internal const string AIAgentsClientIdConstant = "AiAgentsClientId";

		/// <summary>
		/// The gemini api key constant.
		/// </summary>
		internal const string GeminiAPIKeyConstant = "GeminiAPIKey";

		/// <summary>
		/// The gemini flash model
		/// </summary>
		internal const string GeminiFlashModel = "GeminiFlashModel";

		/// <summary>
		/// The gemini pro model
		/// </summary>
		internal const string GeminiProModel = "GeminiProModel";

		/// <summary>
		/// The is pro model enabled flag
		/// </summary>
		internal const string IsProModelEnabledFlag = "IsProModelEnabled";

		/// <summary>
		/// The azure application configuration constant
		/// </summary>
		internal const string AzureAppConfigurationConstant = "AzureAppConfiguration";

		/// <summary>
		/// The azure signal r connection
		/// </summary>
		internal const string AzureSignalRConnection = "AzureSignalRConnection";
	}
}
