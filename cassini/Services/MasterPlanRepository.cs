using cassini.EF;
using Microsoft.EntityFrameworkCore;

namespace cassini.Services;

/// <summary>
/// Repository implementation for accessing Cassini master plan data
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
    /// Retrieves all master plan entries
    /// </summary>
    /// <returns>Collection of all master plan entries</returns>
    public async Task<IEnumerable<MasterPlan>> GetAllAsync()
    {
        return await _context.MasterPlans.ToListAsync();
    }

    /// <summary>
    /// Retrieves a master plan entry by its ID
    /// </summary>
    /// <param name="id">The entry ID</param>
    /// <returns>Master plan entry or null if not found</returns>
    public async Task<MasterPlan?> GetByIdAsync(int id)
    {
        return await _context.MasterPlans.FindAsync(id);
    }

    /// <summary>
    /// Retrieves master plan entries by team
    /// </summary>
    /// <param name="team">Team identifier</param>
    /// <returns>Collection of matching master plan entries</returns>
    public async Task<IEnumerable<MasterPlan>> GetByTeamAsync(string team)
    {
        return await _context.MasterPlans
            .Where(mp => mp.Team == team)
            .ToListAsync();
    }

    /// <summary>
    /// Retrieves master plan entries by target
    /// </summary>
    /// <param name="target">Observation target</param>
    /// <returns>Collection of matching master plan entries</returns>
    public async Task<IEnumerable<MasterPlan>> GetByTargetAsync(string target)
    {
        return await _context.MasterPlans
            .Where(mp => mp.Target == target)
            .ToListAsync();
    }

    /// <summary>
    /// Retrieves the total count of master plan entries
    /// </summary>
    /// <returns>Total count</returns>
    public async Task<int> GetCountAsync()
    {
        return await _context.MasterPlans.CountAsync();
    }
}
