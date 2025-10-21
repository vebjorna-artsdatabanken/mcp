# 🔌 Model Context Protocol (MCP) Specification

## 📖 Overview

The Model Context Protocol (MCP) is a JSON-RPC 2.0 based protocol that enables AI assistants to communicate with external data sources and tools. This server implements MCP to expose Cassini mission data.

## 🔄 Protocol Foundation

MCP is built on **JSON-RPC 2.0**, which defines a stateless, lightweight remote procedure call protocol.

### JSON-RPC 2.0 Message Structure

#### Request
```json
{
  "jsonrpc": "2.0",
  "method": "method_name",
  "params": {
    "param1": "value1",
    "param2": "value2"
  },
  "id": 1
}
```

#### Response (Success)
```json
{
  "jsonrpc": "2.0",
  "result": {
    "data": "response_data"
  },
  "id": 1
}
```

#### Response (Error)
```json
{
  "jsonrpc": "2.0",
  "error": {
    "code": -32600,
    "message": "Invalid Request",
    "data": "Additional error information"
  },
  "id": 1
}
```

## 🛠️ MCP Core Methods

### 1. `initialize`
Establishes connection and exchanges capabilities.

**Request**:
```json
{
  "jsonrpc": "2.0",
  "method": "initialize",
  "params": {
    "protocolVersion": "2024-11-05",
    "capabilities": {},
    "clientInfo": {
      "name": "client-name",
      "version": "1.0.0"
    }
  },
  "id": 1
}
```

**Response**:
```json
{
  "jsonrpc": "2.0",
  "result": {
    "protocolVersion": "2024-11-05",
    "capabilities": {
      "tools": {}
    },
    "serverInfo": {
      "name": "cassini-mcp-server",
      "version": "1.0.0"
    }
  },
  "id": 1
}
```

### 2. `tools/list`
Lists all available tools.

**Request**:
```json
{
  "jsonrpc": "2.0",
  "method": "tools/list",
  "id": 2
}
```

**Response**:
```json
{
  "jsonrpc": "2.0",
  "result": {
    "tools": [
      {
        "name": "query_observations_by_team",
        "description": "Query Cassini observations by team/instrument",
        "inputSchema": {
          "type": "object",
          "properties": {
            "team": {
              "type": "string",
              "description": "Team identifier (CAPS, ISS, etc.)"
            }
          },
          "required": ["team"]
        }
      }
    ]
  },
  "id": 2
}
```

### 3. `tools/call`
Executes a specific tool.

**Request**:
```json
{
  "jsonrpc": "2.0",
  "method": "tools/call",
  "params": {
    "name": "query_observations_by_team",
    "arguments": {
      "team": "ISS",
      "limit": 10
    }
  },
  "id": 3
}
```

**Response**:
```json
{
  "jsonrpc": "2.0",
  "result": {
    "content": [
      {
        "type": "text",
        "text": "Query results..."
      }
    ]
  },
  "id": 3
}
```

## ⚠️ Error Codes

MCP uses standard JSON-RPC 2.0 error codes plus custom codes:

| Code | Message | Description |
|------|---------|-------------|
| -32700 | Parse error | Invalid JSON received |
| -32600 | Invalid Request | JSON-RPC request is not valid |
| -32601 | Method not found | Method does not exist |
| -32602 | Invalid params | Invalid method parameters |
| -32603 | Internal error | Internal server error |
| -32000 | Server error | Generic server error |
| -32001 | Tool not found | Requested tool does not exist |
| -32002 | Tool execution error | Tool failed during execution |

## 📝 Message Lifecycle

1. **Client** sends `initialize` request
2. **Server** responds with capabilities
3. **Client** sends `tools/list` to discover tools
4. **Server** returns available tools with schemas
5. **Client** sends `tools/call` to execute tools
6. **Server** executes tool and returns results
7. Process repeats for additional tool calls

## 🔐 Transport

MCP supports multiple transport mechanisms:
- **stdio**: Communication via standard input/output
- **HTTP**: RESTful HTTP endpoints
- **WebSocket**: Persistent bidirectional connection

This server implements **HTTP** transport on ASP.NET Core.

## 📊 Data Types

### Tool Input Schema
Tools use JSON Schema to define input parameters:

```json
{
  "type": "object",
  "properties": {
    "param_name": {
      "type": "string",
      "description": "Parameter description",
      "enum": ["option1", "option2"]
    }
  },
  "required": ["param_name"]
}
```

### Tool Response Content
Tool responses contain content items:

```json
{
  "content": [
    {
      "type": "text",
      "text": "Result text"
    },
    {
      "type": "resource",
      "resource": {
        "uri": "file://path/to/resource",
        "mimeType": "application/json",
        "text": "{\"data\": \"value\"}"
      }
    }
  ]
}
```

## 🎯 Implementation Notes

### Request ID
- Must be a string, number, or null
- Used to match requests with responses
- Notifications have no ID (one-way messages)

### Method Naming
- Use lowercase with underscores or forward slashes
- Examples: `tools/list`, `resources/read`, `prompts/get`

### Versioning
- Protocol version in `initialize` handshake
- Current version: `2024-11-05`
- Servers should support version negotiation

## 📚 References

- [JSON-RPC 2.0 Specification](https://www.jsonrpc.org/specification)
- [Model Context Protocol Documentation](https://modelcontextprotocol.io/)
- [MCP TypeScript SDK](https://github.com/modelcontextprotocol/typescript-sdk)
