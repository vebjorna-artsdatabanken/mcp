using cassini.EF;

namespace cassini.Services;

/// <summary>
/// Repository interface for accessing Cassini mission activity data
/// </summary>
public interface IMasterPlanRepository
{
    /// <summary>
    /// Retrieves all mission activity entries
    /// </summary>
    /// <returns>Collection of all mission activity entries</returns>
    Task<IEnumerable<MissionActivity>> GetAllAsync();

    /// <summary>
    /// Retrieves a mission activity entry by its ID
    /// </summary>
    /// <param name="id">The entry ID</param>
    /// <returns>Mission activity entry or null if not found</returns>
    Task<MissionActivity?> GetByIdAsync(int id);

    /// <summary>
    /// Retrieves mission activity entries by team
    /// </summary>
    /// <param name="team">Team identifier</param>
    /// <returns>Collection of matching mission activity entries</returns>
    Task<IEnumerable<MissionActivity>> GetByTeamAsync(string team);

    /// <summary>
    /// Retrieves mission activity entries by target
    /// </summary>
    /// <param name="target">Observation target</param>
    /// <returns>Collection of matching mission activity entries</returns>
    Task<IEnumerable<MissionActivity>> GetByTargetAsync(string target);

    /// <summary>
    /// Retrieves the total count of mission activity entries
    /// </summary>
    /// <returns>Total count</returns>
    Task<int> GetCountAsync();
}
