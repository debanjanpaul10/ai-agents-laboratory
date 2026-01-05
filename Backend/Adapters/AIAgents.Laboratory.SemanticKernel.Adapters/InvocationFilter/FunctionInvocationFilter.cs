using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;

namespace AIAgents.Laboratory.SemanticKernel.Adapters.InvocationFilter;

/// <summary>
/// The Function Invocation Filter for Semantic Kernel.
/// </summary>
/// <param name="logger">The logger instance.</param>
/// <seealso cref="IFunctionInvocationFilter"/>
internal sealed class FunctionInvocationFilter(ILogger<FunctionInvocationFilter> logger) : IFunctionInvocationFilter
{
    /// <summary>
    /// The logger instance.
    /// </summary>
    private readonly ILogger<FunctionInvocationFilter> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    /// <summary>
    /// Handles the function invocations. Called during function calls from semantic kernel.
    /// </summary>
    /// <param name="context">The function invocation context.</param>
    /// <param name="next">The next function.</param>
    /// <returns>A task to wait on.</returns>
    public async Task OnFunctionInvocationAsync(FunctionInvocationContext context, Func<FunctionInvocationContext, Task> next)
    {
        await next(context);
        var pluginName = context.Function.PluginName;
        var functionName = context.Function.Name;
        try
        {
            if (!string.IsNullOrEmpty(pluginName) && !string.IsNullOrEmpty(functionName) && context is not null)
            {
                context.Kernel.Data.TryGetValue("ChatId_InteractionId", out var chatId);
                var key = $"plugin.{pluginName}.function.{functionName}.{chatId}";
                _logger.LogInformation("FunctionInvocationFilter: Plugin '{PluginName}', Function '{FunctionName}', Key '{Key}'", pluginName, functionName, key);

                context.Kernel.Data[key] = chatId;
                var paramsKey = $"plugin.{pluginName}.function.{functionName}.params.{chatId}";
                var parameters = context.Arguments.Select(arg => new { Name = arg.Key, arg.Value }).ToList();
                _logger.LogInformation("FunctionInvocationFilter: Plugin '{PluginName}', Function '{FunctionName}', Key '{Key}', Parameters: {@Parameters}", pluginName, functionName, key, parameters);

                context.Kernel.Data[paramsKey] = JsonSerializer.Serialize(parameters);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during invocation of function {FunctionName} from plugin {PluginName}", functionName, pluginName);
            throw;
        }
    }
}
