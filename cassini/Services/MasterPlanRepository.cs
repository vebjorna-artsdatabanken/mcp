using cassini.EF;
using Microsoft.EntityFrameworkCore;

namespace cassini.Services;

/// <summary>
/// Repository implementation for accessing Cassini mission activity data
/// </summary>
public class MasterPlanRepository : IMasterPlanRepository
{
    private readonly CassiniDbContext _context;

    /// <summary>
    /// Initializes a new instance of the MasterPlanRepository
    /// </summary>
    /// <param name="context">Database context</param>
    public MasterPlanRepository(CassiniDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves all mission activity entries
    /// </summary>
    /// <returns>Collection of all mission activity entries</returns>
    public async Task<IEnumerable<MissionActivity>> GetAllAsync()
    {
        return await _context.MissionActivities.ToListAsync();
    }

    /// <summary>
    /// Retrieves a mission activity entry by its ID
    /// </summary>
    /// <param name="id">The entry ID</param>
    /// <returns>Mission activity entry or null if not found</returns>
    public async Task<MissionActivity?> GetByIdAsync(int id)
    {
        return await _context.MissionActivities.FindAsync(id);
    }

    /// <summary>
    /// Retrieves mission activity entries by team
    /// </summary>
    /// <param name="team">Team identifier</param>
    /// <returns>Collection of matching mission activity entries</returns>
    public async Task<IEnumerable<MissionActivity>> GetByTeamAsync(string team)
    {
        return await _context.MissionActivities
            .Where(mp => mp.Team == team)
            .ToListAsync();
    }

    /// <summary>
    /// Retrieves mission activity entries by target
    /// </summary>
    /// <param name="target">Observation target</param>
    /// <returns>Collection of matching mission activity entries</returns>
    public async Task<IEnumerable<MissionActivity>> GetByTargetAsync(string target)
    {
        return await _context.MissionActivities
            .Where(mp => mp.Target == target)
            .ToListAsync();
    }

    /// <summary>
    /// Retrieves the total count of mission activity entries
    /// </summary>
    /// <returns>Total count</returns>
    public async Task<int> GetCountAsync()
    {
        return await _context.MissionActivities.CountAsync();
    }
}
