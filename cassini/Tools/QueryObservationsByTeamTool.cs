using cassini.DTOs.Mcp;
using cassini.Services;

namespace cassini.Tools;

/// <summary>
/// MCP tool for querying Cassini observations by team/instrument.
/// </summary>
public class QueryObservationsByTeamTool : BaseMcpTool
{
    /// <summary>
    /// Initializes a new instance of the QueryObservationsByTeamTool class.
    /// </summary>
    /// <param name="repository">Master plan repository.</param>
    /// <param name="logger">Logger instance.</param>
    public QueryObservationsByTeamTool(IMasterPlanRepository repository, ILogger<QueryObservationsByTeamTool> logger)
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
            Name = "query_observations_by_team",
            Description = "Query Cassini mission observations by team/instrument identifier. Common teams include: CAPS, CDA, CIRS, ISS, INMS, MAG, MIMI, RADAR, RPWS, RSS, UVIS, VIMS.",
            InputSchema = new
            {
                type = "object",
                properties = new
                {
                    team = new
                    {
                        type = "string",
                        description = "Team or instrument identifier (e.g., ISS, CAPS, RADAR, VIMS)"
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
                required = new[] { "team" }
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
        var team = GetStringParameter(arguments, "team", required: true)!;
        var limit = GetIntParameter(arguments, "limit", 100);
        var offset = GetIntParameter(arguments, "offset", 0);

        // Ensure limit is within bounds
        limit = Math.Min(Math.Max(limit, 1), 1000);

        _logger.LogInformation(
            "Querying observations by team: {Team}, limit={Limit}, offset={Offset}",
            team, limit, offset);

        try
        {
            var observations = (await _repository.GetByTeamAsync(team)).ToList();
            
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
                team = team,
                observations = paginatedObservations
            };

            _logger.LogInformation("Found {Total} observations for team {Team}, returned {Count}", 
                observations.Count, team, paginatedObservations.Count);
            
            return CreateJsonResult(result, $"cassini://observations/team/{team}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error querying observations by team: {Team}", team);
            throw;
        }
    }
}
