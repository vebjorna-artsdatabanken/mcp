using System.Text.Json;
using cassini.DTOs.Mcp;
using cassini.Services;

namespace cassini.Tools;

/// <summary>
/// Base class for MCP tools providing common functionality.
/// </summary>
public abstract class BaseMcpTool
{
    protected readonly IMasterPlanRepository _repository;
    protected readonly ILogger _logger;

    /// <summary>
    /// Initializes a new instance of the BaseMcpTool class.
    /// </summary>
    /// <param name="repository">Master plan repository.</param>
    /// <param name="logger">Logger instance.</param>
    protected BaseMcpTool(IMasterPlanRepository repository, ILogger logger)
    {
        _repository = repository;
        _logger = logger;
    }

    /// <summary>
    /// Gets the tool definition for MCP registration.
    /// </summary>
    public abstract McpTool GetToolDefinition();

    /// <summary>
    /// Executes the tool with the provided arguments.
    /// </summary>
    /// <param name="arguments">Tool arguments.</param>
    /// <returns>Tool execution result.</returns>
    public abstract Task<McpToolResult> ExecuteAsync(Dictionary<string, object> arguments);

    /// <summary>
    /// Extracts a string parameter from arguments.
    /// </summary>
    /// <param name="arguments">Argument dictionary.</param>
    /// <param name="key">Parameter key.</param>
    /// <param name="required">Whether the parameter is required.</param>
    /// <returns>Parameter value or null.</returns>
    protected string? GetStringParameter(Dictionary<string, object> arguments, string key, bool required = false)
    {
        if (!arguments.TryGetValue(key, out var value))
        {
            if (required)
                throw new ArgumentException($"Required parameter '{key}' is missing");
            return null;
        }

        return value?.ToString();
    }

    /// <summary>
    /// Extracts an integer parameter from arguments.
    /// </summary>
    /// <param name="arguments">Argument dictionary.</param>
    /// <param name="key">Parameter key.</param>
    /// <param name="defaultValue">Default value if not provided.</param>
    /// <returns>Parameter value.</returns>
    protected int GetIntParameter(Dictionary<string, object> arguments, string key, int defaultValue)
    {
        if (!arguments.TryGetValue(key, out var value))
            return defaultValue;

        if (value is JsonElement jsonElement && jsonElement.ValueKind == JsonValueKind.Number)
            return jsonElement.GetInt32();

        if (int.TryParse(value?.ToString(), out var result))
            return result;

        return defaultValue;
    }

    /// <summary>
    /// Creates a text content result.
    /// </summary>
    /// <param name="text">Text content.</param>
    /// <returns>Tool result with text content.</returns>
    protected McpToolResult CreateTextResult(string text)
    {
        return new McpToolResult
        {
            Content = new List<McpContent>
            {
                new McpContent
                {
                    Type = "text",
                    Text = text
                }
            }
        };
    }

    /// <summary>
    /// Creates a JSON resource result.
    /// </summary>
    /// <param name="data">Data to serialize.</param>
    /// <param name="uri">Resource URI.</param>
    /// <returns>Tool result with resource content.</returns>
    protected McpToolResult CreateJsonResult(object data, string uri = "data://result")
    {
        var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
        return new McpToolResult
        {
            Content = new List<McpContent>
            {
                new McpContent
                {
                    Type = "resource",
                    Resource = new McpResource
                    {
                        Uri = uri,
                        MimeType = "application/json",
                        Text = json
                    }
                }
            }
        };
    }
}
