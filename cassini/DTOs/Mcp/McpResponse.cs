namespace cassini.DTOs.Mcp;

/// <summary>
/// Represents a JSON-RPC 2.0 response message for MCP protocol.
/// </summary>
public class McpResponse
{
    /// <summary>
    /// JSON-RPC protocol version (must be "2.0").
    /// </summary>
    public string Jsonrpc { get; set; } = "2.0";

    /// <summary>
    /// The result of the method invocation (null if error occurred).
    /// </summary>
    public object? Result { get; set; }

    /// <summary>
    /// Error object if the method invocation failed (null if successful).
    /// </summary>
    public McpError? Error { get; set; }

    /// <summary>
    /// Request identifier matching the original request.
    /// </summary>
    public object? Id { get; set; }
}
