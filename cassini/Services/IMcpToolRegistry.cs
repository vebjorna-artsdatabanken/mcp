using cassini.DTOs.Mcp;

namespace cassini.Services;

/// <summary>
/// Interface for MCP tool registration and execution.
/// </summary>
public interface IMcpToolRegistry
{
    /// <summary>
    /// Registers a new tool in the registry.
    /// </summary>
    /// <param name="tool">Tool definition.</param>
    /// <param name="handler">Function to execute when the tool is called. Receives service provider and arguments.</param>
    void RegisterTool(McpTool tool, Func<IServiceProvider, Dictionary<string, object>, Task<McpToolResult>> handler);

    /// <summary>
    /// Gets all registered tools.
    /// </summary>
    /// <returns>List of tool definitions.</returns>
    List<McpTool> GetAllTools();

    /// <summary>
    /// Executes a tool by name with the provided arguments.
    /// </summary>
    /// <param name="serviceProvider">Service provider for resolving dependencies.</param>
    /// <param name="toolName">Name of the tool to execute.</param>
    /// <param name="arguments">Tool arguments.</param>
    /// <returns>Tool execution result.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the tool is not found.</exception>
    Task<McpToolResult> ExecuteToolAsync(IServiceProvider serviceProvider, string toolName, Dictionary<string, object> arguments);

    /// <summary>
    /// Checks if a tool exists in the registry.
    /// </summary>
    /// <param name="toolName">Name of the tool.</param>
    /// <returns>True if the tool exists, false otherwise.</returns>
    bool ToolExists(string toolName);
}
