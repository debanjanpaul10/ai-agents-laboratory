using AIAgents.Laboratory.API.IOC;
using AIAgents.Laboratory.API.Middleware;
using Azure.Identity;
using Microsoft.OpenApi;
using static AIAgents.Laboratory.API.Helpers.Constants;
using SwaggerConstants = AIAgents.Laboratory.API.Helpers.Constants.SwaggerConstants;

var builder = WebApplication.CreateBuilder(args);
if (builder.Environment.IsDevelopment())
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
builder.Services.ConfigureApplicationDependencies(
    configuration: builder.Configuration,
    isDevelopmentMode: builder.Environment.IsDevelopment());

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddCors(
    options => options.AddDefaultPolicy(policy => policy.SetIsOriginAllowed(_ => true).AllowAnyHeader().AllowAnyMethod().AllowCredentials()));
builder.Services.AddApiVersions();

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
builder.Services.AddProblemDetails();
builder.Services.AddHttpContextAccessor();
builder.Services.AddApplicationInsightsTelemetry(options =>
    options.ConnectionString = builder.Configuration[AzureAppConfigurationConstants.ApplicationInsightsConnectionString]
);

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

app.UseCorrelationIdMiddleware();
app.UseRequestLogging();
app.UseExceptionMiddleware();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseCors();
app.MapControllers();


await app.RunAsync();
