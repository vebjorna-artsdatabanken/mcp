using cassini.DTOs.Mcp;
using cassini.Services;

namespace cassini.Tools;

/// <summary>
/// MCP tool for retrieving detailed information about a specific observation.
/// </summary>
public class GetObservationDetailsTool : BaseMcpTool
{
    /// <summary>
    /// Initializes a new instance of the GetObservationDetailsTool class.
    /// </summary>
    /// <param name="repository">Master plan repository.</param>
    /// <param name="logger">Logger instance.</param>
    public GetObservationDetailsTool(IMasterPlanRepository repository, ILogger<GetObservationDetailsTool> logger)
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
            Name = "get_observation_details",
            Description = "Retrieve complete details for a specific Cassini observation by its ID. Returns all available fields including description and library definition.",
            InputSchema = new
            {
                type = "object",
                properties = new
                {
                    id = new
                    {
                        type = "integer",
                        description = "Observation ID",
                        minimum = 1
                    }
                },
                required = new[] { "id" }
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
        var id = GetIntParameter(arguments, "id", 0);

        if (id <= 0)
            throw new ArgumentException("ID must be a positive integer");

        _logger.LogInformation("Retrieving observation details for ID: {Id}", id);

        try
        {
            var observation = await _repository.GetByIdAsync(id);

            if (observation == null)
            {
                return CreateTextResult($"No observation found with ID: {id}");
            }

            var result = new
            {
                id = observation.Id,
                start_time_utc = observation.StartTimeUtc,
                duration = observation.Duration,
                date = observation.Date,
                team = observation.Team,
                spass_type = observation.SpassType,
                target = observation.Target,
                request_name = observation.RequestName,
                library_definition = observation.LibraryDefinition,
                title = observation.Title,
                description = observation.Description
            };

            _logger.LogInformation("Retrieved observation {Id}: {Title}", id, observation.Title);
            
            return CreateJsonResult(result, $"cassini://observations/{id}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving observation details for ID: {Id}", id);
            throw;
        }
    }
}
