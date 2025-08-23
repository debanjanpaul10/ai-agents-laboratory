// *********************************************************************************
//	<copyright file="Program.cs" company="Personal">
//		Copyright (options) 2025 Personal
//	</copyright>
// <summary>Program class from where the execution starts</summary>
// *********************************************************************************

using AIAgents.Laboratory.API.Helpers;
using AIAgents.Laboratory.API.IOC;
using AIAgents.Laboratory.API.Middleware;
using AIAgents.Laboratory.Messaging.Adapters.Services;
using Azure.Identity;
using Microsoft.OpenApi.Models;
using static AIAgents.Laboratory.API.Helpers.Constants;
using SwaggerConstants = AIAgents.Laboratory.API.Helpers.Constants.SwaggerConstants;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(EnvironmentConfigurationConstants.LocalAppsetingsFileName, true).AddEnvironmentVariables();

var miCredentials = builder.Configuration[EnvironmentConfigurationConstants.ManagedIdentityClientIdConstant];
var credentials = builder.Environment.IsDevelopment()
	? new DefaultAzureCredential()
	: new DefaultAzureCredential(new DefaultAzureCredentialOptions
	{
		ManagedIdentityClientId = miCredentials,
	});

builder.ConfigureAzureAppConfiguration(credentials);
builder.Services.ConfigureAiDependencies(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddOpenApi();
builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.SetIsOriginAllowed(_ => true).AllowAnyHeader().AllowAnyMethod().AllowCredentials()));

builder.Services.AddSwaggerGen(options =>
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
builder.Services.AddExceptionHandler<GlobalExceptionMiddleware>();
builder.Services.AddProblemDetails();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
	app.UseDeveloperExceptionPage();
	app.UseSwagger();
	app.UseSwaggerUI(c =>
	{
		c.SwaggerEndpoint(SwaggerConstants.SwaggerEndpointUrl, $"{SwaggerConstants.ApplicationAPIName}.{SwaggerConstants.ApiVersion}");
		c.RoutePrefix = SwaggerConstants.SwaggerUiPrefix;
	});
}

app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseCors();
app.MapControllers();
app.MapHub<AgentStatusHub>(RouteConstants.AgentStatusHub_Route);

app.Run();
