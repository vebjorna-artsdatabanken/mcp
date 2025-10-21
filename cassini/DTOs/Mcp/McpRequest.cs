namespace cassini.DTOs.Mcp;

/// <summary>
/// Represents a JSON-RPC 2.0 request message for MCP protocol.
/// </summary>
public class McpRequest
{
    /// <summary>
    /// JSON-RPC protocol version (must be "2.0").
    /// </summary>
    public string Jsonrpc { get; set; } = "2.0";

    /// <summary>
    /// The method name to be invoked.
    /// </summary>
    public string Method { get; set; } = string.Empty;

    /// <summary>
    /// Method parameters (optional).
    /// </summary>
    public object? Params { get; set; }

    /// <summary>
    /// Request identifier for matching response (null for notifications).
    /// </summary>
    public object? Id { get; set; }
}
