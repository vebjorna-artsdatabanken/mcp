using cassini.EF;
using cassini.Middleware;
using cassini.Services;
using cassini.Tools;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Configure database context
builder.Services.AddDbContext<CassiniDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("CassiniDatabase")));

// Register repository
builder.Services.AddScoped<IMasterPlanRepository, MasterPlanRepository>();

// Register MCP services
builder.Services.AddSingleton<IMcpToolRegistry, McpToolRegistry>();

var app = builder.Build();

// Register MCP tools
var toolRegistry = app.Services.GetRequiredService<IMcpToolRegistry>();

// Register timerange query tool
toolRegistry.RegisterTool(
    new QueryObservationsByTimerangeTool(null!, null!).GetToolDefinition(),
    async (serviceProvider, arguments) =>
    {
        var repository = serviceProvider.GetRequiredService<IMasterPlanRepository>();
        var logger = serviceProvider.GetRequiredService<ILogger<QueryObservationsByTimerangeTool>>();
        var tool = new QueryObservationsByTimerangeTool(repository, logger);
        return await tool.ExecuteAsync(arguments);
    });

// Register team query tool
toolRegistry.RegisterTool(
    new QueryObservationsByTeamTool(null!, null!).GetToolDefinition(),
    async (serviceProvider, arguments) =>
    {
        var repository = serviceProvider.GetRequiredService<IMasterPlanRepository>();
        var logger = serviceProvider.GetRequiredService<ILogger<QueryObservationsByTeamTool>>();
        var tool = new QueryObservationsByTeamTool(repository, logger);
        return await tool.ExecuteAsync(arguments);
    });

// Register target query tool
toolRegistry.RegisterTool(
    new QueryObservationsByTargetTool(null!, null!).GetToolDefinition(),
    async (serviceProvider, arguments) =>
    {
        var repository = serviceProvider.GetRequiredService<IMasterPlanRepository>();
        var logger = serviceProvider.GetRequiredService<ILogger<QueryObservationsByTargetTool>>();
        var tool = new QueryObservationsByTargetTool(repository, logger);
        return await tool.ExecuteAsync(arguments);
    });

// Register observation details tool
toolRegistry.RegisterTool(
    new GetObservationDetailsTool(null!, null!).GetToolDefinition(),
    async (serviceProvider, arguments) =>
    {
        var repository = serviceProvider.GetRequiredService<IMasterPlanRepository>();
        var logger = serviceProvider.GetRequiredService<ILogger<GetObservationDetailsTool>>();
        var tool = new GetObservationDetailsTool(repository, logger);
        return await tool.ExecuteAsync(arguments);
    });

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Add MCP middleware
app.UseMiddleware<McpMiddleware>();

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

// Test endpoint to verify database connectivity
app.MapGet("/api/test/db", async (IMasterPlanRepository repository) =>
{
    try
    {
        var count = await repository.GetCountAsync();
        return Results.Ok(new
        {
            Status = "Connected",
            TotalRecords = count,
            Message = "Database connection successful"
        });
    }
    catch (Exception ex)
    {
        return Results.Problem(
            detail: ex.Message,
            title: "Database connection failed"
        );
    }
})
.WithName("TestDatabaseConnection");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
