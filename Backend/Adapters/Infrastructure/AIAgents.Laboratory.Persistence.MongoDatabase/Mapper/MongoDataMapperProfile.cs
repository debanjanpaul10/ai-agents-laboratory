using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.Domain.DomainEntities.AgentsEntities;
using AIAgents.Laboratory.Domain.DomainEntities.Workspaces;
using AIAgents.Laboratory.Persistence.MongoDatabase.Models;

namespace AIAgents.Laboratory.Persistence.MongoDatabase.Mapper;

/// <summary>
/// The Mongo Data Mapper Profile class for mapping between MongoDB data models and domain models.
/// </summary>
internal static class MongoDataMapperProfile
{
    #region Domain to Model Mappings

    /// <summary>
    /// Maps to model.
    /// </summary>
    /// <param name="domain">The domain input.</param>
    /// <returns>The data model.</returns>
    internal static AgentDataModel MapToModel(AgentDataDomain domain) => new()
    {
        Id = domain.Id,
        AgentId = domain.AgentId,
        AgentName = domain.AgentName,
        AgentDescription = domain.AgentDescription,
        AgentMetaPrompt = domain.AgentMetaPrompt,
        ApplicationId = domain.ApplicationId,
        KnowledgeBaseDocument = domain.KnowledgeBaseDocument,
        RemovedKnowledgeBaseDocuments = domain.RemovedKnowledgeBaseDocuments,
        StoredKnowledgeBase = domain.StoredKnowledgeBase,
        IsPrivate = domain.IsPrivate,
        IsDefaultChatbot = domain.IsDefaultChatbot,
        VisionImages = domain.VisionImages,
        RemovedAiVisionImages = domain.RemovedAiVisionImages,
        AiVisionImagesData = domain.AiVisionImagesData,
        AssociatedSkillGuids = domain.AssociatedSkillGuids,
        IsActive = domain.IsActive,
        DateCreated = domain.DateCreated,
        CreatedBy = domain.CreatedBy,
        DateModified = domain.DateModified,
        ModifiedBy = domain.ModifiedBy
    };

    /// <summary>
    /// Maps to model.
    /// </summary>
    /// <param name="domain">The domain input.</param>
    /// <returns>The data model.</returns>
    internal static ChatHistoryModel MapToModel(ChatHistoryDomain domain) => new()
    {
        Role = domain.Role,
        Content = domain.Content
    };

    /// <summary>
    /// Maps to model.
    /// </summary>
    /// <param name="domain">The domain input.</param>
    /// <returns>The data model.</returns>
    internal static ConversationHistoryModel MapToModel(ConversationHistoryDomain domain) => new()
    {
        Id = domain.Id,
        ConversationId = domain.ConversationId,
        UserName = domain.UserName,
        ChatHistory = [.. domain.ChatHistory.Select(MapToModel)],
        IsActive = domain.IsActive,
        LastModifiedOn = domain.LastModifiedOn
    };

    /// <summary>
    /// Maps to model.
    /// </summary>
    /// <param name="domain">The domain input.</param>
    /// <returns>The data model.</returns>
    internal static AgentsWorkspaceDataModel MapToModel(AgentsWorkspaceDomain domain) => new()
    {
        Id = domain.Id,
        AgentWorkspaceGuid = domain.AgentWorkspaceGuid,
        AgentWorkspaceName = domain.AgentWorkspaceName,
        ActiveAgentsListInWorkspace = domain.ActiveAgentsListInWorkspace.Select(MapToModel),
        WorkspaceUsers = domain.WorkspaceUsers,
        IsGroupChatEnabled = domain.IsGroupChatEnabled,
        IsActive = domain.IsActive,
        DateCreated = domain.DateCreated,
        CreatedBy = domain.CreatedBy,
        DateModified = domain.DateModified,
        ModifiedBy = domain.ModifiedBy
    };

    /// <summary>
    /// Maps to model.
    /// </summary>
    /// <param name="domain">The domain input.</param>
    /// <returns>The data model.</returns>
    internal static WorkspaceAgentsDataModel MapToModel(WorkspaceAgentsDataDomain domain) => new()
    {
        AgentName = domain.AgentName,
        AgentGuid = domain.AgentGuid
    };

    /// <summary>
    /// Maps to model.
    /// </summary>
    /// <param name="domain">The domain input.</param>
    /// <returns>The data model.</returns>
    internal static RegisteredApplicationDataModel MapToModel(RegisteredApplicationDomain domain) => new()
    {
        _id = domain._id,
        Id = domain.Id,
        ApplicationName = domain.ApplicationName,
        Description = domain.Description,
        ApplicationRegistrationGuid = domain.ApplicationRegistrationGuid,
        IsAzureRegistered = domain.IsAzureRegistered,
        IsActive = domain.IsActive,
        DateCreated = domain.DateCreated,
        CreatedBy = domain.CreatedBy,
        DateModified = domain.DateModified,
        ModifiedBy = domain.ModifiedBy
    };

    /// <summary>
    /// Maps to model.
    /// </summary>
    /// <param name="domain">The domain input.</param>
    /// <returns>The data model.</returns>
    internal static ToolSkillModel MapToModel(ToolSkillDomain domain) => new()
    {
        Id = domain.Id,
        ToolSkillGuid = domain.ToolSkillGuid,
        ToolSkillDisplayName = domain.ToolSkillDisplayName,
        ToolSkillTechnicalName = domain.ToolSkillTechnicalName,
        ToolSkillMcpServerUrl = domain.ToolSkillMcpServerUrl,
        AssociatedAgents = [.. domain.AssociatedAgents.Select(MapToModel)],
        IsActive = domain.IsActive,
        DateCreated = domain.DateCreated,
        CreatedBy = domain.CreatedBy,
        DateModified = domain.DateModified,
        ModifiedBy = domain.ModifiedBy
    };

    /// <summary>
    /// Maps to model.
    /// </summary>
    /// <param name="domain">The domain input.</param>
    /// <returns>The data model.</returns>
    internal static AssociatedAgentsSkillDataModel MapToModel(AssociatedAgentsSkillDataDomain domain) => new()
    {
        AgentName = domain.AgentName,
        AgentGuid = domain.AgentGuid
    };

    #endregion

    #region Model to Domain Mappings

    /// <summary>
    /// Maps to domain.
    /// </summary>
    /// <param name="model">The data model.</param>
    /// <returns>The domain entity.</returns>
    internal static AgentDataDomain MapToDomain(AgentDataModel model) => new()
    {
        Id = model.Id,
        AgentId = model.AgentId,
        AgentName = model.AgentName,
        AgentDescription = model.AgentDescription,
        AgentMetaPrompt = model.AgentMetaPrompt,
        ApplicationId = model.ApplicationId,
        KnowledgeBaseDocument = model.KnowledgeBaseDocument,
        RemovedKnowledgeBaseDocuments = model.RemovedKnowledgeBaseDocuments,
        StoredKnowledgeBase = model.StoredKnowledgeBase,
        IsPrivate = model.IsPrivate,
        IsDefaultChatbot = model.IsDefaultChatbot,
        VisionImages = model.VisionImages,
        RemovedAiVisionImages = model.RemovedAiVisionImages,
        AiVisionImagesData = model.AiVisionImagesData,
        AssociatedSkillGuids = model.AssociatedSkillGuids,
        IsActive = model.IsActive,
        DateCreated = model.DateCreated,
        CreatedBy = model.CreatedBy,
        DateModified = model.DateModified,
        ModifiedBy = model.ModifiedBy
    };

    /// <summary>
    /// Maps to domain.
    /// </summary>
    /// <param name="model">The data model.</param>
    /// <returns>The domain entity.</returns>
    internal static ChatHistoryDomain MapToDomain(ChatHistoryModel model) => new()
    {
        Role = model.Role,
        Content = model.Content
    };

    /// <summary>
    /// Maps to domain.
    /// </summary>
    /// <param name="model">The data model.</param>
    /// <returns>The domain entity.</returns>
    internal static ConversationHistoryDomain MapToDomain(ConversationHistoryModel model) => new()
    {
        Id = model.Id,
        ConversationId = model.ConversationId,
        UserName = model.UserName,
        ChatHistory = [.. model.ChatHistory.Select(MapToDomain)],
        IsActive = model.IsActive,
        LastModifiedOn = model.LastModifiedOn
    };

    /// <summary>
    /// Maps to domain.
    /// </summary>
    /// <param name="model">The data model.</param>
    /// <returns>The domain entity.</returns>
    internal static AgentsWorkspaceDomain MapToDomain(AgentsWorkspaceDataModel model) => new()
    {
        Id = model.Id,
        AgentWorkspaceGuid = model.AgentWorkspaceGuid,
        AgentWorkspaceName = model.AgentWorkspaceName,
        ActiveAgentsListInWorkspace = model.ActiveAgentsListInWorkspace.Select(MapToDomain),
        WorkspaceUsers = model.WorkspaceUsers,
        IsGroupChatEnabled = model.IsGroupChatEnabled,
        IsActive = model.IsActive,
        DateCreated = model.DateCreated,
        CreatedBy = model.CreatedBy,
        DateModified = model.DateModified,
        ModifiedBy = model.ModifiedBy
    };

    /// <summary>
    /// Maps to domain.
    /// </summary>
    /// <param name="model">The data model.</param>
    /// <returns>The domain entity.</returns>
    internal static WorkspaceAgentsDataDomain MapToDomain(WorkspaceAgentsDataModel model) => new()
    {
        AgentName = model.AgentName,
        AgentGuid = model.AgentGuid
    };

    /// <summary>
    /// Maps to domain.
    /// </summary>
    /// <param name="model">The data model.</param>
    /// <returns>The domain entity.</returns>
    internal static RegisteredApplicationDomain MapToDomain(RegisteredApplicationDataModel model) => new()
    {
        _id = model._id,
        Id = model.Id,
        ApplicationName = model.ApplicationName,
        Description = model.Description,
        ApplicationRegistrationGuid = model.ApplicationRegistrationGuid,
        IsAzureRegistered = model.IsAzureRegistered,
        IsActive = model.IsActive,
        DateCreated = model.DateCreated,
        CreatedBy = model.CreatedBy,
        DateModified = model.DateModified,
        ModifiedBy = model.ModifiedBy
    };

    /// <summary>
    /// Maps to domain.
    /// </summary>
    /// <param name="model">The data model.</param>
    /// <returns>The domain entity.</returns>
    internal static ToolSkillDomain MapToDomain(ToolSkillModel model) => new()
    {
        Id = model.Id,
        ToolSkillGuid = model.ToolSkillGuid,
        ToolSkillDisplayName = model.ToolSkillDisplayName,
        ToolSkillTechnicalName = model.ToolSkillTechnicalName,
        ToolSkillMcpServerUrl = model.ToolSkillMcpServerUrl,
        AssociatedAgents = [.. model.AssociatedAgents.Select(MapToDomain)],
        IsActive = model.IsActive,
        DateCreated = model.DateCreated,
        CreatedBy = model.CreatedBy,
        DateModified = model.DateModified,
        ModifiedBy = model.ModifiedBy
    };

    /// <summary>
    /// Maps to domain.
    /// </summary>
    /// <param name="model">The data model.</param>
    /// <returns>The domain entity.</returns>
    internal static AssociatedAgentsSkillDataDomain MapToDomain(AssociatedAgentsSkillDataModel model) => new()
    {
        AgentName = model.AgentName,
        AgentGuid = model.AgentGuid
    };

    /// <summary>
    /// Maps to domain.
    /// </summary>
    /// <param name="model">The data model.</param>
    /// <returns>The domain entity.</returns>
    internal static NotificationsDomain MapToDomain(NotificationsModel model) => new()
    {
        Id = model.Id,
        Title = model.Title,
        Message = model.Message,
        RecipientUserName = model.RecipientUserName,
        NotificationType = model.NotificationType,
        CreatedBy = model.CreatedBy,
        IsGlobal = model.IsGlobal,
        IsRead = model.IsRead,
        IsActive = model.IsActive,
        DateCreated = model.DateCreated
    };

    #endregion
}
