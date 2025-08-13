// *********************************************************************************
//	<copyright file="Program.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>Program class from where the execution starts</summary>
// *********************************************************************************

using AIAgents.Laboratory.API.IOC;
using AIAgents.Laboratory.API.Middleware;
using Azure.Identity;
using Microsoft.OpenApi.Models;
using static AIAgents.Laboratory.API.Adapters.Helpers.Constants;

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
		services.AddOpenApi();
		services.AddCors(options =>
		{
			options.AddDefaultPolicy(policy =>
			{
				policy.AllowAnyOrigin()
				.AllowAnyHeader()
				.AllowAnyMethod();
			});
		});

		services.AddSwaggerGen(c =>
		{
			c.SwaggerDoc("v1", new OpenApiInfo
			{
				Title = "AIAgents.Laboratory",
				Version = "v1",
				Description = "API Documentation for AIAgents.Laboratory",
				Contact = new OpenApiContact
				{
					Name = "Debanjan Paul",
					Email = "debanjanpaul10@gmail.com"
				}
			});
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
		app.Run();
	}
}