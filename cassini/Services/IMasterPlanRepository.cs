using cassini.EF;

namespace cassini.Services;

/// <summary>
/// Repository interface for accessing Cassini master plan data
/// </summary>
public interface IMasterPlanRepository
{
    /// <summary>
    /// Retrieves all master plan entries
    /// </summary>
    /// <returns>Collection of all master plan entries</returns>
    Task<IEnumerable<MasterPlan>> GetAllAsync();

    /// <summary>
    /// Retrieves a master plan entry by its ID
    /// </summary>
    /// <param name="id">The entry ID</param>
    /// <returns>Master plan entry or null if not found</returns>
    Task<MasterPlan?> GetByIdAsync(int id);

    /// <summary>
    /// Retrieves master plan entries by team
    /// </summary>
    /// <param name="team">Team identifier</param>
    /// <returns>Collection of matching master plan entries</returns>
    Task<IEnumerable<MasterPlan>> GetByTeamAsync(string team);

    /// <summary>
    /// Retrieves master plan entries by target
    /// </summary>
    /// <param name="target">Observation target</param>
    /// <returns>Collection of matching master plan entries</returns>
    Task<IEnumerable<MasterPlan>> GetByTargetAsync(string target);

    /// <summary>
    /// Retrieves the total count of master plan entries
    /// </summary>
    /// <returns>Total count</returns>
    Task<int> GetCountAsync();
}
