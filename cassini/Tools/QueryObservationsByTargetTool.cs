using cassini.DTOs.Mcp;
using cassini.Services;

namespace cassini.Tools;

/// <summary>
/// MCP tool for querying Cassini observations by target.
/// </summary>
public class QueryObservationsByTargetTool : BaseMcpTool
{
    /// <summary>
    /// Initializes a new instance of the QueryObservationsByTargetTool class.
    /// </summary>
    /// <param name="repository">Master plan repository.</param>
    /// <param name="logger">Logger instance.</param>
    public QueryObservationsByTargetTool(IMasterPlanRepository repository, ILogger<QueryObservationsByTargetTool> logger)
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
            Name = "query_observations_by_target",
            Description = "Query Cassini mission observations by observation target. Common targets include: Saturn, Titan, Enceladus, Rhea, Iapetus, Dione, Tethys, rings, and other moons.",
            InputSchema = new
            {
                type = "object",
                properties = new
                {
                    target = new
                    {
                        type = "string",
                        description = "Target name (e.g., Saturn, Titan, Enceladus, rings)"
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
                required = new[] { "target" }
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
        var target = GetStringParameter(arguments, "target", required: true)!;
        var limit = GetIntParameter(arguments, "limit", 100);
        var offset = GetIntParameter(arguments, "offset", 0);

        // Ensure limit is within bounds
        limit = Math.Min(Math.Max(limit, 1), 1000);

        _logger.LogInformation(
            "Querying observations by target: {Target}, limit={Limit}, offset={Offset}",
            target, limit, offset);

        try
        {
            var observations = (await _repository.GetByTargetAsync(target)).ToList();
            
            var paginatedObservations = observations
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
                count = paginatedObservations.Count,
                total = observations.Count,
                offset = offset,
                limit = limit,
                target = target,
                observations = paginatedObservations
            };

            _logger.LogInformation("Found {Total} observations for target {Target}, returned {Count}", 
                observations.Count, target, paginatedObservations.Count);
            
            return CreateJsonResult(result, $"cassini://observations/target/{target}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error querying observations by target: {Target}", target);
            throw;
        }
    }
}
