namespace cassini.DTOs.Mcp;

/// <summary>
/// Represents a JSON-RPC 2.0 error object.
/// </summary>
public class McpError
{
    /// <summary>
    /// Error code indicating the error type.
    /// </summary>
    public int Code { get; set; }

    /// <summary>
    /// Short description of the error.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Additional information about the error (optional).
    /// </summary>
    public object? Data { get; set; }
}

/// <summary>
/// Standard JSON-RPC 2.0 and MCP error codes.
/// </summary>
public static class McpErrorCodes
{
    /// <summary>
    /// Invalid JSON was received by the server.
    /// </summary>
    public const int ParseError = -32700;

    /// <summary>
    /// The JSON sent is not a valid Request object.
    /// </summary>
    public const int InvalidRequest = -32600;

    /// <summary>
    /// The method does not exist or is not available.
    /// </summary>
    public const int MethodNotFound = -32601;

    /// <summary>
    /// Invalid method parameter(s).
    /// </summary>
    public const int InvalidParams = -32602;

    /// <summary>
    /// Internal JSON-RPC error.
    /// </summary>
    public const int InternalError = -32603;

    /// <summary>
    /// Generic server error.
    /// </summary>
    public const int ServerError = -32000;

    /// <summary>
    /// Requested tool does not exist.
    /// </summary>
    public const int ToolNotFound = -32001;

    /// <summary>
    /// Tool failed during execution.
    /// </summary>
    public const int ToolExecutionError = -32002;
}
