using System.Globalization;
using cassini.DTOs.Mcp;
using cassini.Services;

namespace cassini.Tools;

/// <summary>
/// MCP tool for querying Cassini observations by time range.
/// </summary>
public class QueryObservationsByTimerangeTool : BaseMcpTool
{
    /// <summary>
    /// Initializes a new instance of the QueryObservationsByTimerangeTool class.
    /// </summary>
    /// <param name="repository">Master plan repository.</param>
    /// <param name="logger">Logger instance.</param>
    public QueryObservationsByTimerangeTool(IMasterPlanRepository repository, ILogger<QueryObservationsByTimerangeTool> logger)
        : base(repository, logger)
    {
    }

    /// <summary>
    /// Gets the tool definition for MCP registration.
    /// </summary>
    public override McpTool GetToolDefinition()
    {
        return new McpTool
        {
            Name = "query_observations_by_timerange",
            Description = "Query Cassini mission observations within a specified time range. Returns observation records filtered by start time.",
            InputSchema = new
            {
                type = "object",
                properties = new
                {
                    start_time = new
                    {
                        type = "string",
                        description = "Start of time range in UTC format (YYYY-DDDTHH:MM:SS or YYYY-MM-DD)"
                    },
                    end_time = new
                    {
                        type = "string",
                        description = "End of time range in UTC format (YYYY-DDDTHH:MM:SS or YYYY-MM-DD)"
                    },
                    limit = new
                    {
                        type = "integer",
                        description = "Maximum number of results to return (default: 100, max: 1000)",
                        minimum = 1,
                        maximum = 1000
                    },
                    offset = new
                    {
                        type = "integer",
                        description = "Number of results to skip for pagination (default: 0)",
                        minimum = 0
                    }
                },
                required = new[] { "start_time", "end_time" }
            }
        };
    }

    /// <summary>
    /// Executes the tool with the provided arguments.
    /// </summary>
    /// <param name="arguments">Tool arguments.</param>
    /// <returns>Tool execution result.</returns>
    public override async Task<McpToolResult> ExecuteAsync(Dictionary<string, object> arguments)
    {
        var startTime = GetStringParameter(arguments, "start_time", required: true)!;
        var endTime = GetStringParameter(arguments, "end_time", required: true)!;
        var limit = GetIntParameter(arguments, "limit", 100);
        var offset = GetIntParameter(arguments, "offset", 0);

        // Ensure limit is within bounds
        limit = Math.Min(Math.Max(limit, 1), 1000);

        _logger.LogInformation(
            "Querying observations by time range: {StartTime} to {EndTime}, limit={Limit}, offset={Offset}",
            startTime, endTime, limit, offset);

        try
        {
            var allObservations = await _repository.GetAllAsync();
            
            var filteredObservations = allObservations
                .Where(o => IsInTimeRange(o.StartTimeUtc, startTime, endTime))
                .Skip(offset)
                .Take(limit)
                .Select(o => new
                {
                    id = o.Id,
                    start_time_utc = o.StartTimeUtc,
                    duration = o.Duration,
                    date = o.Date,
                    team = o.Team,
                    spass_type = o.SpassType,
                    target = o.Target,
                    request_name = o.RequestName,
                    title = o.Title
                })
                .ToList();

            var result = new
            {
                count = filteredObservations.Count,
                offset = offset,
                limit = limit,
                observations = filteredObservations
            };

            _logger.LogInformation("Found {Count} observations in time range", filteredObservations.Count);
            
            return CreateJsonResult(result, $"cassini://observations/timerange?start={startTime}&end={endTime}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error querying observations by time range");
            throw;
        }
    }

    /// <summary>
    /// Checks if an observation's start time falls within the specified range.
    /// </summary>
    /// <param name="startTimeUtc">Observation start time.</param>
    /// <param name="rangeStart">Range start time.</param>
    /// <param name="rangeEnd">Range end time.</param>
    /// <returns>True if the time is in range.</returns>
    private bool IsInTimeRange(string? startTimeUtc, string rangeStart, string rangeEnd)
    {
        if (string.IsNullOrEmpty(startTimeUtc))
            return false;

        try
        {
            var obsTime = ParseTimeString(startTimeUtc);
            var startRange = ParseTimeString(rangeStart);
            var endRange = ParseTimeString(rangeEnd);

            return obsTime >= startRange && obsTime <= endRange;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Parses a time string in various formats.
    /// </summary>
    /// <param name="timeString">Time string to parse.</param>
    /// <returns>Parsed DateTime.</returns>
    private DateTime ParseTimeString(string timeString)
    {
        // Try standard ISO format first
        if (DateTime.TryParse(timeString, out var result))
            return result;

        // Try day-of-year format (YYYY-DDDTHH:MM:SS)
        if (timeString.Length >= 11 && timeString[4] == '-' && timeString[8] == 'T')
        {
            var year = int.Parse(timeString.Substring(0, 4));
            var dayOfYear = int.Parse(timeString.Substring(5, 3));
            var timePart = timeString.Length > 9 ? timeString.Substring(9) : "00:00:00";
            
            var date = new DateTime(year, 1, 1).AddDays(dayOfYear - 1);
            if (TimeSpan.TryParse(timePart, out var time))
                date = date.Add(time);
            
            return date;
        }

        throw new FormatException($"Unable to parse time string: {timeString}");
    }
}
