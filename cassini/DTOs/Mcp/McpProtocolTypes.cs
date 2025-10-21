namespace cassini.DTOs.Mcp;

/// <summary>
/// Parameters for the initialize method.
/// </summary>
public class InitializeParams
{
    /// <summary>
    /// MCP protocol version.
    /// </summary>
    public string ProtocolVersion { get; set; } = string.Empty;

    /// <summary>
    /// Client capabilities.
    /// </summary>
    public object Capabilities { get; set; } = new { };

    /// <summary>
    /// Information about the client.
    /// </summary>
    public ClientInfo ClientInfo { get; set; } = new();
}

/// <summary>
/// Result of the initialize method.
/// </summary>
public class InitializeResult
{
    /// <summary>
    /// MCP protocol version.
    /// </summary>
    public string ProtocolVersion { get; set; } = string.Empty;

    /// <summary>
    /// Server capabilities.
    /// </summary>
    public ServerCapabilities Capabilities { get; set; } = new();

    /// <summary>
    /// Information about the server.
    /// </summary>
    public ServerInfo ServerInfo { get; set; } = new();
}

/// <summary>
/// Client information.
/// </summary>
public class ClientInfo
{
    /// <summary>
    /// Client name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Client version.
    /// </summary>
    public string Version { get; set; } = string.Empty;
}

/// <summary>
/// Server information.
/// </summary>
public class ServerInfo
{
    /// <summary>
    /// Server name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Server version.
    /// </summary>
    public string Version { get; set; } = string.Empty;
}

/// <summary>
/// Server capabilities.
/// </summary>
public class ServerCapabilities
{
    /// <summary>
    /// Tools capability (empty object means tools are supported).
    /// </summary>
    public object Tools { get; set; } = new { };
}

/// <summary>
/// Parameters for tools/call method.
/// </summary>
public class ToolCallParams
{
    /// <summary>
    /// Tool name to invoke.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Tool arguments as key-value pairs.
    /// </summary>
    public Dictionary<string, object> Arguments { get; set; } = new();
}

/// <summary>
/// Result of tools/list method.
/// </summary>
public class ToolsListResult
{
    /// <summary>
    /// List of available tools.
    /// </summary>
    public List<McpTool> Tools { get; set; } = new();
}
