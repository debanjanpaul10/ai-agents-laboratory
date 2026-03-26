using AIAgents.Laboratory.Domain.DomainEntities;

namespace AIAgents.Laboratory.Domain.Ports.Out;

/// <summary>
/// Provides an interface for managing tool skills data in the data source. This includes operations for adding, updating, retrieving, and deleting tool skill data.
/// </summary>
/// <remarks>
/// The <c>IToolSkillsDataManager</c> interface abstracts the underlying data storage mechanism, allowing for flexibility in how tool skills data is persisted and accessed. 
/// Implementations of this interface can interact with various types of databases or storage systems without affecting the domain logic.
/// </remarks>
public interface IToolSkillsDataManager
{
    /// <summary>
    /// Adds a new tool skill asynchronously.
    /// </summary>
    /// <param name="toolSkillData">The tool skill data domain model.</param>
    /// <param name="userEmail">The user email.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The boolean for <c>success/failure</c></returns>
    Task<bool> AddNewToolSkillAsync(ToolSkillDomain toolSkillData, string userEmail, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing tool skill data asynchronously.
    /// </summary>
    /// <param name="updateToolSkillData">The tool skill data domain model.</param>
    /// <param name="currentUserEmail">The user email.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The boolean for <c>success/failure</c></returns>
    Task<bool> UpdateExistingToolSkillDataAsync(ToolSkillDomain updateToolSkillData, string currentUserEmail, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all the tool skill data asynchronously.
    /// </summary>
    /// <param name="userEmail">The current logged in user email.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The list of <see cref="ToolSkillDomain"/></returns>
    Task<IEnumerable<ToolSkillDomain>> GetAllToolSkillsAsync(string userEmail, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a single tool skill data by its skill id asynchronously.
    /// </summary>
    /// <param name="toolSkillId">The tool skill id to be fetched.</param>
    /// <param name="currentUserEmail">The current logged in user email.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The tool skill domain model.</returns>
    Task<ToolSkillDomain> GetToolSkillBySkillIdAsync(string toolSkillId, string currentUserEmail, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an existing tool by tool skill id asynchronously.
    /// </summary>
    /// <param name="toolSkillId">The tool skill id to delete.</param>
    /// <param name="currentUserEmail">The current logged in user email.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A boolean for success/failure.</returns>
    Task<bool> DeleteExistingToolSkillBySkillIdAsync(string toolSkillId, string currentUserEmail, CancellationToken cancellationToken = default);
}
