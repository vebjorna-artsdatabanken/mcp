using cassini.DTOs.Mcp;

namespace cassini.Services;

/// <summary>
/// Registry for MCP tools that manages tool registration and execution.
/// </summary>
public class McpToolRegistry : IMcpToolRegistry
{
    private readonly Dictionary<string, McpTool> _tools = new();
    private readonly Dictionary<string, Func<IServiceProvider, Dictionary<string, object>, Task<McpToolResult>>> _handlers = new();
    private readonly ILogger<McpToolRegistry> _logger;

    /// <summary>
    /// Initializes a new instance of the McpToolRegistry class.
    /// </summary>
    /// <param name="logger">Logger for registry operations.</param>
    public McpToolRegistry(ILogger<McpToolRegistry> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Registers a new tool in the registry.
    /// </summary>
    /// <param name="tool">Tool definition.</param>
    /// <param name="handler">Function to execute when the tool is called. Receives service provider and arguments.</param>
    public void RegisterTool(McpTool tool, Func<IServiceProvider, Dictionary<string, object>, Task<McpToolResult>> handler)
    {
        if (string.IsNullOrEmpty(tool.Name))
        {
            throw new ArgumentException("Tool name cannot be empty", nameof(tool));
        }

        if (_tools.ContainsKey(tool.Name))
        {
            _logger.LogWarning("Tool {ToolName} is already registered. It will be replaced.", tool.Name);
        }

        _tools[tool.Name] = tool;
        _handlers[tool.Name] = handler;
        _logger.LogInformation("Registered tool: {ToolName}", tool.Name);
    }

    /// <summary>
    /// Gets all registered tools.
    /// </summary>
    /// <returns>List of tool definitions.</returns>
    public List<McpTool> GetAllTools()
    {
        return _tools.Values.ToList();
    }

    /// <summary>
    /// Executes a tool by name with the provided arguments.
    /// </summary>
    /// <param name="serviceProvider">Service provider for resolving dependencies.</param>
    /// <param name="toolName">Name of the tool to execute.</param>
    /// <param name="arguments">Tool arguments.</param>
    /// <returns>Tool execution result.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the tool is not found.</exception>
    public async Task<McpToolResult> ExecuteToolAsync(IServiceProvider serviceProvider, string toolName, Dictionary<string, object> arguments)
    {
        if (!_handlers.TryGetValue(toolName, out var handler))
        {
            _logger.LogError("Tool not found: {ToolName}", toolName);
            throw new KeyNotFoundException($"Tool not found: {toolName}");
        }

        _logger.LogInformation("Executing tool: {ToolName} with arguments: {@Arguments}", toolName, arguments);

        try
        {
            var result = await handler(serviceProvider, arguments);
            _logger.LogInformation("Tool {ToolName} executed successfully", toolName);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Tool execution failed for {ToolName}", toolName);
            throw;
        }
    }

    /// <summary>
    /// Checks if a tool exists in the registry.
    /// </summary>
    /// <param name="toolName">Name of the tool.</param>
    /// <returns>True if the tool exists, false otherwise.</returns>
    public bool ToolExists(string toolName)
    {
        return _tools.ContainsKey(toolName);
    }
}
