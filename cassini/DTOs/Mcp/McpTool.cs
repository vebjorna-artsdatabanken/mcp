namespace cassini.DTOs.Mcp;

/// <summary>
/// Represents an MCP tool definition.
/// </summary>
public class McpTool
{
    /// <summary>
    /// Unique tool identifier.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Human-readable tool description.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// JSON Schema defining the tool's input parameters.
    /// </summary>
    public object InputSchema { get; set; } = new { };
}

/// <summary>
/// Represents the result of a tool invocation.
/// </summary>
public class McpToolResult
{
    /// <summary>
    /// Content items returned by the tool.
    /// </summary>
    public List<McpContent> Content { get; set; } = new();
}

/// <summary>
/// Represents a content item in a tool result.
/// </summary>
public class McpContent
{
    /// <summary>
    /// Content type (e.g., "text", "resource").
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Text content (when type is "text").
    /// </summary>
    public string? Text { get; set; }

    /// <summary>
    /// Resource information (when type is "resource").
    /// </summary>
    public McpResource? Resource { get; set; }
}

/// <summary>
/// Represents a resource in MCP content.
/// </summary>
public class McpResource
{
    /// <summary>
    /// Resource URI.
    /// </summary>
    public string Uri { get; set; } = string.Empty;

    /// <summary>
    /// Resource MIME type.
    /// </summary>
    public string? MimeType { get; set; }

    /// <summary>
    /// Resource content as text.
    /// </summary>
    public string? Text { get; set; }
}
