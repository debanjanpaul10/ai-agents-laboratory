using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.Domain.DomainEntities.FeedbackEntities;
using AIAgents.Laboratory.Persistence.SQLDatabase.Models;

namespace AIAgents.Laboratory.Persistence.SQLDatabase.Mapper;

/// <summary>
/// The Data Mapper Profile Class for mapping
/// </summary>
internal static class DataMapperProfile
{
    #region Domain to Entity Mappings

    /// <summary>
    /// Maps to entity.
    /// </summary>
    /// <param name="domainInput">The domain input.</param>
    /// <returns>The entity model.</returns>
    internal static BugReportDataEntity MapToEntity(
        BugReportData domainInput
    ) => new()
    {
        AgentDetails = domainInput.AgentDetails,
        BugSeverityId = domainInput.BugSeverityId,
        BugStatusId = domainInput.BugStatusId,
        CreatedBy = domainInput.CreatedBy,
        DateCreated = domainInput.DateCreated,
        DateModified = domainInput.DateModified,
        Description = domainInput.Description,
        Id = domainInput.Id,
        IsActive = domainInput.IsActive,
        ModifiedBy = domainInput.ModifiedBy,
        Title = domainInput.Title
    };

    /// <summary>
    /// Maps to entity.
    /// </summary>
    /// <param name="domainInput">The domain input.</param>
    /// <returns>The entity model.</returns>
    internal static NewFeatureRequestDataEntity MapToEntity(
        NewFeatureRequestData domainInput
    ) => new()
    {
        CreatedBy = domainInput.CreatedBy,
        DateCreated = domainInput.DateCreated,
        DateModified = domainInput.DateModified,
        Description = domainInput.Description,
        Id = domainInput.Id,
        IsActive = domainInput.IsActive,
        ModifiedBy = domainInput.ModifiedBy,
        Title = domainInput.Title
    };

    #endregion

    #region Entity to Domain Mappings

    /// <summary>
    /// Maps to domain.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns></returns>
    internal static BugItemStatusMapping MapToDomain(
        BugItemStatusMappingEntity entity
    ) => new()
    {
        Id = entity.Id,
        StatusName = entity.StatusName,
        CreatedBy = entity.CreatedBy,
        DateCreated = entity.DateCreated,
        DateModified = entity.DateModified,
        IsActive = entity.IsActive,
        ModifiedBy = entity.ModifiedBy
    };

    /// <summary>
    /// Maps to domain.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns></returns>
    internal static BugSeverityMapping MapToDomain(
        BugSeverityMappingEntity entity
    ) => new()
    {
        Id = entity.Id,
        SeverityName = entity.SeverityName,
        CreatedBy = entity.CreatedBy,
        DateCreated = entity.DateCreated,
        DateModified = entity.DateModified,
        IsActive = entity.IsActive,
        ModifiedBy = entity.ModifiedBy
    };

    /// <summary>
    /// Maps to domain.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns></returns>
    internal static BugReportData MapToDomain(
        BugReportDataEntity entity
    ) => new()
    {
        Id = entity.Id,
        Title = entity.Title,
        Description = entity.Description,
        AgentDetails = entity.AgentDetails,
        BugSeverityId = entity.BugSeverityId,
        BugStatusId = entity.BugStatusId,
        CreatedBy = entity.CreatedBy,
        DateCreated = entity.DateCreated,
        DateModified = entity.DateModified,
        IsActive = entity.IsActive,
        ModifiedBy = entity.ModifiedBy
    };

    /// <summary>
    /// Maps to domain.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns></returns>
    internal static NewFeatureRequestData MapToDomain(
        NewFeatureRequestDataEntity entity
    ) => new()
    {
        Id = entity.Id,
        Title = entity.Title,
        Description = entity.Description,
        CreatedBy = entity.CreatedBy,
        DateCreated = entity.DateCreated,
        DateModified = entity.DateModified,
        IsActive = entity.IsActive,
        ModifiedBy = entity.ModifiedBy
    };

    /// <summary>
    /// Maps to domain.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns></returns>
    internal static BaseDomainModel MapToDomain(
        BaseEntity entity
    ) => new()
    {
        CreatedBy = entity.CreatedBy,
        DateCreated = entity.DateCreated,
        DateModified = entity.DateModified,
        IsActive = entity.IsActive,
        ModifiedBy = entity.ModifiedBy
    };

    #endregion
}
