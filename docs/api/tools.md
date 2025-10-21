# 🛠️ Cassini MCP Server Tools

## Overview

The Cassini MCP Server provides four tools for querying and exploring Cassini mission observation data through the Model Context Protocol.

## 🔧 Available Tools

### 1. query_observations_by_timerange

Query Cassini mission observations within a specified time range.

**Parameters:**
- `start_time` (string, required): Start of time range in UTC format (YYYY-DDDTHH:MM:SS or YYYY-MM-DD)
- `end_time` (string, required): End of time range in UTC format (YYYY-DDDTHH:MM:SS or YYYY-MM-DD)
- `limit` (integer, optional): Maximum results to return (default: 100, max: 1000)
- `offset` (integer, optional): Number of results to skip for pagination (default: 0)

**Example Request:**
```json
{
  "jsonrpc": "2.0",
  "method": "tools/call",
  "params": {
    "name": "query_observations_by_timerange",
    "arguments": {
      "start_time": "2004-001T00:00:00",
      "end_time": "2004-031T23:59:59",
      "limit": 10
    }
  },
  "id": 1
}
```

**Example Response:**
```json
{
  "jsonrpc": "2.0",
  "result": {
    "content": [
      {
        "type": "resource",
        "resource": {
          "uri": "cassini://observations/timerange?start=2004-001T00:00:00&end=2004-031T23:59:59",
          "mimeType": "application/json",
          "text": "{\n  \"count\": 10,\n  \"offset\": 0,\n  \"limit\": 10,\n  \"observations\": [...]\n}"
        }
      }
    ]
  },
  "id": 1
}
```

---

### 2. query_observations_by_team

Query observations by team/instrument identifier.

**Common Teams:**
- `CAPS` - Cassini Plasma Spectrometer
- `CDA` - Cosmic Dust Analyzer
- `CIRS` - Composite Infrared Spectrometer
- `ISS` - Imaging Science Subsystem
- `INMS` - Ion and Neutral Mass Spectrometer
- `MAG` - Magnetometer
- `MIMI` - Magnetospheric Imaging Instrument
- `RADAR` - Radar
- `RPWS` - Radio and Plasma Wave Science
- `RSS` - Radio Science Subsystem
- `UVIS` - Ultraviolet Imaging Spectrograph
- `VIMS` - Visual and Infrared Mapping Spectrometer

**Parameters:**
- `team` (string, required): Team or instrument identifier
- `limit` (integer, optional): Maximum results to return (default: 100, max: 1000)
- `offset` (integer, optional): Number of results to skip for pagination (default: 0)

**Example Request:**
```json
{
  "jsonrpc": "2.0",
  "method": "tools/call",
  "params": {
    "name": "query_observations_by_team",
    "arguments": {
      "team": "ISS",
      "limit": 20
    }
  },
  "id": 2
}
```

**Example Response:**
```json
{
  "jsonrpc": "2.0",
  "result": {
    "content": [
      {
        "type": "resource",
        "resource": {
          "uri": "cassini://observations/team/ISS",
          "mimeType": "application/json",
          "text": "{\n  \"count\": 20,\n  \"total\": 15432,\n  \"offset\": 0,\n  \"limit\": 20,\n  \"team\": \"ISS\",\n  \"observations\": [...]\n}"
        }
      }
    ]
  },
  "id": 2
}
```

---

### 3. query_observations_by_target

Query observations by observation target.

**Common Targets:**
- `Saturn` - The planet Saturn
- `Titan` - Saturn's largest moon
- `Enceladus` - Icy moon with active geysers
- `Rhea` - Saturn's second-largest moon
- `Iapetus` - Moon with contrasting hemispheres
- `Dione` - Mid-sized icy moon
- `Tethys` - Mid-sized icy moon
- `Mimas` - Small moon known for large crater
- `rings` - Saturn's ring system
- And many other moons and targets

**Parameters:**
- `target` (string, required): Target name
- `limit` (integer, optional): Maximum results to return (default: 100, max: 1000)
- `offset` (integer, optional): Number of results to skip for pagination (default: 0)

**Example Request:**
```json
{
  "jsonrpc": "2.0",
  "method": "tools/call",
  "params": {
    "name": "query_observations_by_target",
    "arguments": {
      "target": "Titan",
      "limit": 50,
      "offset": 100
    }
  },
  "id": 3
}
```

**Example Response:**
```json
{
  "jsonrpc": "2.0",
  "result": {
    "content": [
      {
        "type": "resource",
        "resource": {
          "uri": "cassini://observations/target/Titan",
          "mimeType": "application/json",
          "text": "{\n  \"count\": 50,\n  \"total\": 8234,\n  \"offset\": 100,\n  \"limit\": 50,\n  \"target\": \"Titan\",\n  \"observations\": [...]\n}"
        }
      }
    ]
  },
  "id": 3
}
```

---

### 4. get_observation_details

Retrieve complete details for a specific observation by its ID.

**Parameters:**
- `id` (integer, required): Observation ID (minimum: 1)

**Example Request:**
```json
{
  "jsonrpc": "2.0",
  "method": "tools/call",
  "params": {
    "name": "get_observation_details",
    "arguments": {
      "id": 12345
    }
  },
  "id": 4
}
```

**Example Response:**
```json
{
  "jsonrpc": "2.0",
  "result": {
    "content": [
      {
        "type": "resource",
        "resource": {
          "uri": "cassini://observations/12345",
          "mimeType": "application/json",
          "text": "{\n  \"id\": 12345,\n  \"start_time_utc\": \"2004-182T14:30:00\",\n  \"duration\": \"000T02:15:00\",\n  \"date\": \"30-JUN-04\",\n  \"team\": \"ISS\",\n  \"spass_type\": \"Prime\",\n  \"target\": \"Titan\",\n  \"request_name\": \"TITAN_APPROACH\",\n  \"library_definition\": \"NAC_IMAGE\",\n  \"title\": \"Titan Approach Imaging\",\n  \"description\": \"High-resolution imaging of Titan's atmosphere during approach...\"\n}"
        }
      }
    ]
  },
  "id": 4
}
```

## 📊 Response Fields

All query tools return observations with the following fields:

- `id`: Unique observation identifier
- `start_time_utc`: Observation start time in UTC (YYYY-DDDTHH:MM:SS format)
- `duration`: Duration of observation (DDDTHH:MM:SS format)
- `date`: Date in DD-MMM-YY format
- `team`: Team/instrument identifier
- `spass_type`: SPASS classification (Prime, SPASS Rider, Non-SPASS)
- `target`: Observation target
- `request_name`: Request type identifier
- `title`: Brief observation title

The `get_observation_details` tool additionally includes:
- `library_definition`: Library definition/category
- `description`: Detailed observation description

## 🔄 Pagination

All query tools support pagination through `limit` and `offset` parameters:

- **limit**: Controls how many results to return (1-1000, default: 100)
- **offset**: Skips the specified number of results (default: 0)

**Pagination Example:**
```json
{
  "arguments": {
    "team": "ISS",
    "limit": 50,
    "offset": 100
  }
}
```
This returns observations 101-150 for the ISS team.

## 🌐 URI Scheme

Tool responses use custom URIs to identify resources:

- `cassini://observations/timerange?start={start}&end={end}` - Time range query results
- `cassini://observations/team/{team}` - Team query results
- `cassini://observations/target/{target}` - Target query results
- `cassini://observations/{id}` - Specific observation details

## ⚠️ Error Handling

Tools may return errors for invalid inputs:

```json
{
  "jsonrpc": "2.0",
  "error": {
    "code": -32002,
    "message": "Tool execution error",
    "data": "Required parameter 'team' is missing"
  },
  "id": 1
}
```

Common error scenarios:
- Missing required parameters
- Invalid parameter types
- Invalid time format
- Observation ID not found
- Database connection issues

## 📈 Usage Tips

1. **Start broad, then narrow**: Use `query_observations_by_team` or `query_observations_by_target` first, then use `get_observation_details` for specific items.

2. **Use pagination**: For large result sets, use `limit` and `offset` to retrieve data in manageable chunks.

3. **Time formats**: Time ranges accept both day-of-year format (2004-182T14:30:00) and ISO format (2004-06-30).

4. **Case sensitivity**: Team and target names are case-sensitive. Use exact matches.

5. **Total counts**: Query tools return both `count` (items in response) and `total` (items matching query) to help with pagination planning.
