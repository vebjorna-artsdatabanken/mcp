using System.Text.Json;
using cassini.DTOs.Mcp;
using cassini.Services;

namespace cassini.Middleware;

/// <summary>
/// Middleware for handling MCP (Model Context Protocol) JSON-RPC 2.0 requests.
/// </summary>
public class McpMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<McpMiddleware> _logger;

    /// <summary>
    /// Initializes a new instance of the McpMiddleware class.
    /// </summary>
    /// <param name="next">The next middleware in the pipeline.</param>
    /// <param name="logger">Logger for middleware operations.</param>
    public McpMiddleware(RequestDelegate next, ILogger<McpMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Processes MCP requests at the /mcp endpoint.
    /// </summary>
    /// <param name="context">HTTP context.</param>
    /// <param name="toolRegistry">MCP tool registry service.</param>
    public async Task InvokeAsync(HttpContext context, IMcpToolRegistry toolRegistry)
    {
        if (context.Request.Path != "/mcp" || context.Request.Method != "POST")
        {
            await _next(context);
            return;
        }

        _logger.LogInformation("Received MCP request at {Path}", context.Request.Path);

        McpRequest? request = null;
        McpResponse response;

        try
        {
            request = await JsonSerializer.DeserializeAsync<McpRequest>(
                context.Request.Body,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            if (request == null)
            {
                response = CreateErrorResponse(null, McpErrorCodes.InvalidRequest, "Request body is empty");
            }
            else if (request.Jsonrpc != "2.0")
            {
                response = CreateErrorResponse(request.Id, McpErrorCodes.InvalidRequest, "Invalid jsonrpc version");
            }
            else
            {
                _logger.LogInformation("Processing method: {Method}", request.Method);
                response = await ProcessRequestAsync(request, toolRegistry);
            }
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to parse JSON request");
            response = CreateErrorResponse(null, McpErrorCodes.ParseError, "Invalid JSON");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error processing request");
            response = CreateErrorResponse(request?.Id, McpErrorCodes.InternalError, "Internal server error");
        }

        context.Response.ContentType = "application/json";
        await JsonSerializer.SerializeAsync(context.Response.Body, response);
    }

    /// <summary>
    /// Routes the request to the appropriate handler based on the method name.
    /// </summary>
    /// <param name="request">MCP request.</param>
    /// <param name="toolRegistry">Tool registry service.</param>
    /// <returns>MCP response.</returns>
    private async Task<McpResponse> ProcessRequestAsync(McpRequest request, IMcpToolRegistry toolRegistry)
    {
        return request.Method switch
        {
            "initialize" => await HandleInitializeAsync(request),
            "tools/list" => await HandleToolsListAsync(request, toolRegistry),
            "tools/call" => await HandleToolsCallAsync(request, toolRegistry),
            _ => CreateErrorResponse(request.Id, McpErrorCodes.MethodNotFound, $"Method not found: {request.Method}")
        };
    }

    /// <summary>
    /// Handles the initialize method.
    /// </summary>
    /// <param name="request">MCP request.</param>
    /// <returns>MCP response with server information.</returns>
    private Task<McpResponse> HandleInitializeAsync(McpRequest request)
    {
        var result = new InitializeResult
        {
            ProtocolVersion = "2024-11-05",
            Capabilities = new ServerCapabilities
            {
                Tools = new { }
            },
            ServerInfo = new ServerInfo
            {
                Name = "cassini-mcp-server",
                Version = "1.0.0"
            }
        };

        return Task.FromResult(new McpResponse
        {
            Jsonrpc = "2.0",
            Result = result,
            Id = request.Id
        });
    }

    /// <summary>
    /// Handles the tools/list method.
    /// </summary>
    /// <param name="request">MCP request.</param>
    /// <param name="toolRegistry">Tool registry service.</param>
    /// <returns>MCP response with list of available tools.</returns>
    private Task<McpResponse> HandleToolsListAsync(McpRequest request, IMcpToolRegistry toolRegistry)
    {
        var tools = toolRegistry.GetAllTools();
        var result = new ToolsListResult { Tools = tools };

        return Task.FromResult(new McpResponse
        {
            Jsonrpc = "2.0",
            Result = result,
            Id = request.Id
        });
    }

    /// <summary>
    /// Handles the tools/call method.
    /// </summary>
    /// <param name="request">MCP request.</param>
    /// <param name="toolRegistry">Tool registry service.</param>
    /// <returns>MCP response with tool execution results.</returns>
    private async Task<McpResponse> HandleToolsCallAsync(McpRequest request, IMcpToolRegistry toolRegistry)
    {
        try
        {
            var paramsJson = JsonSerializer.Serialize(request.Params);
            var toolCallParams = JsonSerializer.Deserialize<ToolCallParams>(paramsJson, 
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (toolCallParams == null || string.IsNullOrEmpty(toolCallParams.Name))
            {
                return CreateErrorResponse(request.Id, McpErrorCodes.InvalidParams, "Tool name is required");
            }

            var result = await toolRegistry.ExecuteToolAsync(toolCallParams.Name, toolCallParams.Arguments);

            return new McpResponse
            {
                Jsonrpc = "2.0",
                Result = result,
                Id = request.Id
            };
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Tool not found");
            return CreateErrorResponse(request.Id, McpErrorCodes.ToolNotFound, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Tool execution failed");
            return CreateErrorResponse(request.Id, McpErrorCodes.ToolExecutionError, ex.Message);
        }
    }

    /// <summary>
    /// Creates an error response.
    /// </summary>
    /// <param name="id">Request ID.</param>
    /// <param name="code">Error code.</param>
    /// <param name="message">Error message.</param>
    /// <returns>MCP response with error.</returns>
    private static McpResponse CreateErrorResponse(object? id, int code, string message)
    {
        return new McpResponse
        {
            Jsonrpc = "2.0",
            Error = new McpError
            {
                Code = code,
                Message = message
            },
            Id = id
        };
    }
}
