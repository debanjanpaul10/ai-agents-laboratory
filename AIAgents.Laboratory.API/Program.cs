// *********************************************************************************
//	<copyright file="Program.cs" company="Personal">
//		Copyright (options) 2025 Personal
//	</copyright>
// <summary>Program class from where the execution starts</summary>
// *********************************************************************************

using AIAgents.Laboratory.API.IOC;
using AIAgents.Laboratory.API.Middleware;
using AIAgents.Laboratory.Messaging.Adapters.Services;
using Azure.Identity;
using Microsoft.OpenApi.Models;
using static AIAgents.Laboratory.API.Helpers.Constants;

namespace AIAgents.Laboratory.API;

/// <summary>
/// The Program Class.
/// </summary>
public static class Program
{
	/// <summary>
	/// Defines the entry point of the application.
	/// </summary>
	/// <param name="args">The arguments.</param>
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);
		builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile(EnvironmentConfigurationConstants.LocalAppsetingsFileName, true).AddEnvironmentVariables();

		var miCredentials = builder.Configuration[EnvironmentConfigurationConstants.ManagedIdentityClientIdConstant];
		var credentials = builder.Environment.IsDevelopment()
			? new DefaultAzureCredential()
			: new DefaultAzureCredential(new DefaultAzureCredentialOptions
			{
				ManagedIdentityClientId = miCredentials,
			});

		builder.ConfigureAzureAppConfiguration(credentials);
		builder.Services.ConfigureAiDependencies(builder.Configuration);
		builder.Services.ConfigureServices();

		var app = builder.Build();
		app.ConfigureApplication();
	}

	/// <summary>
	/// Configures the services.
	/// </summary>
	/// <param name="services">The services.</param>
	internal static void ConfigureServices(this IServiceCollection services)
	{
		services.AddControllers();
		services.AddSignalR();
		services.AddOpenApi();
		services.AddCors(options =>
		{
			options.AddDefaultPolicy(policy =>
			{
				policy.SetIsOriginAllowed(_ => true).AllowAnyHeader().AllowAnyMethod().AllowCredentials();
			});
		});

		services.AddSwaggerGen(options =>
		{
			options.SwaggerDoc(SwaggerConstants.ApiVersion, new OpenApiInfo
			{
				Title = SwaggerConstants.ApplicationAPIName,
				Version = SwaggerConstants.ApiVersion,
				Description = SwaggerConstants.SwaggerDescription,
				Contact = new OpenApiContact
				{
					Name = SwaggerConstants.AuthorDetails.Name,
					Email = SwaggerConstants.AuthorDetails.Email
				}

			});
			options.EnableAnnotations();
		});
		services.AddExceptionHandler<GlobalExceptionMiddleware>();
		services.AddProblemDetails();
	}

	/// <summary>
	/// Configures the application.
	/// </summary>
	/// <param name="app">The application.</param>
	internal static void ConfigureApplication(this WebApplication app)
	{
		if (app.Environment.IsDevelopment())
		{
			app.MapOpenApi();
			app.UseDeveloperExceptionPage();
			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "AIAgents.Laboratory API v1");
				c.RoutePrefix = "swaggerui";
			});
		}

		app.UseExceptionHandler();
		app.UseHttpsRedirection();
		app.UseAuthentication();
		app.UseAuthorization();

		app.UseCors();
		app.MapControllers();
		app.MapHub<AgentStatusHub>("/hubs/agent-status");

		app.Run();
	}
}