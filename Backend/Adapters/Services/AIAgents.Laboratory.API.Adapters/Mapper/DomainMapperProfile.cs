using AIAgents.Laboratory.API.Adapters.Models.Request;
using AIAgents.Laboratory.API.Adapters.Models.Response;
using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.Domain.DomainEntities.AgentsEntities;
using AIAgents.Laboratory.Domain.DomainEntities.FeedbackEntities;
using AIAgents.Laboratory.Domain.DomainEntities.SkillsEntities;
using AIAgents.Laboratory.Domain.DomainEntities.Workspaces;
using Microsoft.AspNetCore.Http;

namespace AIAgents.Laboratory.API.Adapters.Mapper;

/// <summary>
/// The Domain Mapper Profile class for mapping between request/response DTOs and domain models.
/// </summary>
internal static class DomainMapperProfile
{
    #region DTO to Domain Mappings

    /// <summary>
    /// Maps to domain.
    /// </summary>
    /// <param name="dto">The request DTO.</param>
    /// <returns>The domain entity.</returns>
    internal static UserRequestDomain MapToDomain(UserQueryRequestDTO dto) => new()
    {
        UserQuery = dto.UserQuery
    };

    /// <summary>
    /// Maps to domain.
    /// </summary>
    /// <param name="dto">The request DTO.</param>
    /// <returns>The domain entity.</returns>
    internal static FollowupQuestionsRequestDomain MapToDomain(FollowupQuestionsRequestDTO dto) => new()
    {
        UserQuery = dto.UserQuery,
        UserIntent = dto.UserIntent,
        AiResponseData = dto.AiResponseData
    };

    /// <summary>
    /// Maps to domain.
    /// </summary>
    /// <param name="dto">The request DTO.</param>
    /// <returns>The domain entity.</returns>
    internal static AgentDataDomain MapToDomain(CreateAgentDTO dto) => new()
    {
        AgentName = dto.AgentName,
        AgentDescription = dto.AgentDescription,
        AgentMetaPrompt = dto.AgentMetaPrompt,
        ApplicationId = dto.ApplicationId,
        KnowledgeBaseDocument = dto.KnowledgeBaseDocument?.ToList(),
        RemovedKnowledgeBaseDocuments = dto.RemovedKnowledgeBaseDocuments,
        IsPrivate = dto.IsPrivate,
        VisionImages = dto.VisionImages?.OfType<IFormFile>().ToList(),
        RemovedAiVisionImages = dto.RemovedAiVisionImages,
        AssociatedSkillGuids = dto.AssociatedSkillGuids
    };

    /// <summary>
    /// Maps to domain.
    /// </summary>
    /// <param name="dto">The request DTO.</param>
    /// <returns>The domain entity.</returns>
    internal static AgentDataDomain MapToDomain(AgentDataDTO dto) => new()
    {
        AgentId = dto.AgentId,
        AgentName = dto.AgentName,
        AgentDescription = dto.AgentDescription,
        AgentMetaPrompt = dto.AgentMetaPrompt,
        ApplicationId = dto.ApplicationId,
        KnowledgeBaseDocument = dto.KnowledgeBaseDocument?.ToList(),
        RemovedKnowledgeBaseDocuments = dto.RemovedKnowledgeBaseDocuments,
        IsPrivate = dto.IsPrivate,
        IsDefaultChatbot = dto.IsDefaultChatbot,
        VisionImages = dto.VisionImages?.OfType<IFormFile>().ToList(),
        RemovedAiVisionImages = dto.RemovedAiVisionImages,
        AiVisionImagesData = [.. dto.AiVisionImagesData.Select(MapToDomain)],
        AssociatedSkillGuids = dto.AssociatedSkillGuids,
        CreatedBy = dto.CreatedBy,
        DateCreated = dto.DateCreated,
        DateModified = dto.DateModified
    };

    /// <summary>
    /// Maps to domain.
    /// </summary>
    /// <param name="dto">The request DTO.</param>
    /// <returns>The domain entity.</returns>
    internal static BugReportData MapToDomain(AddBugReportDTO dto) => new()
    {
        BugSeverityId = dto.BugSeverity,
        Title = dto.BugTitle,
        Description = dto.BugDescription,
        AgentDetails = dto.AgentDetails,
        CreatedBy = dto.CreatedBy
    };

    /// <summary>
    /// Maps to domain.
    /// </summary>
    /// <param name="dto">The request DTO.</param>
    /// <returns>The domain entity.</returns>
    internal static NewFeatureRequestData MapToDomain(NewFeatureRequestDTO dto) => new()
    {
        Title = dto.Title,
        Description = dto.Description,
        CreatedBy = dto.CreatedBy
    };

    /// <summary>
    /// Maps to domain.
    /// </summary>
    /// <param name="dto">The request DTO.</param>
    /// <returns>The domain entity.</returns>
    internal static WorkspaceAgentChatRequestDomain MapToDomain(WorkspaceAgentChatRequestDto dto) => new()
    {
        ConversationId = dto.ConversationId,
        WorkspaceId = dto.WorkspaceId,
        AgentId = dto.AgentId,
        UserMessage = dto.UserMessage,
        ApplicationName = dto.ApplicationName
    };

    /// <summary>
    /// Maps to domain.
    /// </summary>
    /// <param name="dto">The request DTO.</param>
    /// <returns>The domain entity.</returns>
    internal static ChatRequestDomain MapToDomain(ChatRequestDTO dto) => new()
    {
        AgentName = dto.AgentName,
        AgentId = dto.AgentId,
        ConversationId = dto.ConversationId,
        UserMessage = dto.UserMessage
    };

    /// <summary>
    /// Maps to domain.
    /// </summary>
    /// <param name="dto">The response DTO.</param>
    /// <returns>The domain entity.</returns>
    internal static AgentsWorkspaceDomain MapToDomain(AgentsWorkspaceDTO dto) => new()
    {
        AgentWorkspaceGuid = dto.AgentWorkspaceGuid,
        AgentWorkspaceName = dto.AgentWorkspaceName,
        ActiveAgentsListInWorkspace = dto.ActiveAgentsListInWorkspace.Select(MapToDomain),
        WorkspaceUsers = dto.WorkspaceUsers,
        IsGroupChatEnabled = dto.IsGroupChatEnabled,
        DateCreated = dto.DateCreated,
        CreatedBy = dto.CreatedBy,
        DateModified = dto.DateModified,
        ModifiedBy = dto.ModifiedBy
    };

    /// <summary>
    /// Maps to domain.
    /// </summary>
    /// <param name="dto">The request DTO.</param>
    /// <returns>The domain entity.</returns>
    internal static WorkspaceAgentsDataDomain MapToDomain(WorkspaceAgentsDataDTO dto) => new()
    {
        AgentName = dto.AgentName,
        AgentGuid = dto.AgentGuid
    };

    /// <summary>
    /// Maps to domain.
    /// </summary>
    /// <param name="dto">The response DTO.</param>
    /// <returns>The domain entity.</returns>
    internal static RegisteredApplicationDomain MapToDomain(RegisteredApplicationDto dto) => new()
    {
        Id = dto.Id,
        ApplicationName = dto.ApplicationName,
        Description = dto.Description,
        ApplicationRegistrationGuid = dto.ApplicationRegistrationGuid,
        IsAzureRegistered = dto.IsAzureRegistered,
        DateCreated = dto.DateCreated,
        CreatedBy = dto.CreatedBy,
        DateModified = dto.DateModified,
        ModifiedBy = dto.ModifiedBy
    };

    /// <summary>
    /// Maps to domain.
    /// </summary>
    /// <param name="dto">The request DTO.</param>
    /// <returns>The domain entity.</returns>
    internal static NotificationsDomain MapToDomain(CreateNotificationRequestDto dto) => new()
    {
        Id = dto.Id,
        Title = dto.Title,
        Message = dto.Message,
        RecipientUserName = dto.RecipientUserName,
        NotificationType = dto.NotificationType,
        CreatedBy = dto.CreatedBy,
        IsGlobal = dto.IsGlobal
    };

    /// <summary>
    /// Maps to domain.
    /// </summary>
    /// <param name="dto">The response DTO.</param>
    /// <returns>The domain entity.</returns>
    internal static NotificationsDomain MapToDomain(NotificationsResponseDto dto) => new()
    {
        Id = dto.Id,
        Title = dto.Title,
        Message = dto.Message,
        RecipientUserName = dto.RecipientUserName,
        NotificationType = dto.NotificationType,
        CreatedBy = dto.CreatedBy,
        IsGlobal = dto.IsGlobal,
        IsRead = dto.IsRead,
        IsActive = dto.IsActive,
        DateCreated = dto.DateCreated
    };

    /// <summary>
    /// Maps to domain.
    /// </summary>
    /// <param name="dto">The response DTO.</param>
    /// <returns>The domain entity.</returns>
    internal static ToolSkillDomain MapToDomain(ToolSkillDTO dto) => new()
    {
        ToolSkillGuid = dto.ToolSkillGuid,
        ToolSkillDisplayName = dto.ToolSkillDisplayName,
        ToolSkillTechnicalName = dto.ToolSkillTechnicalName,
        ToolSkillMcpServerUrl = dto.ToolSkillMcpServerUrl,
        AssociatedAgents = [.. dto.AssociatedAgents.Select(MapToDomain)],
        DateCreated = dto.DateCreated,
        CreatedBy = dto.CreatedBy,
        DateModified = dto.DateModified,
        ModifiedBy = dto.ModifiedBy
    };

    /// <summary>
    /// Maps to domain.
    /// </summary>
    /// <param name="dto">The response DTO.</param>
    /// <returns>The domain entity.</returns>
    internal static AssociatedAgentsSkillDataDomain MapToDomain(AssociatedAgentsSkillDataDTO dto) => new()
    {
        AgentName = dto.AgentName,
        AgentGuid = dto.AgentGuid
    };

    /// <summary>
    /// Maps to domain.
    /// </summary>
    /// <param name="dto">The response DTO.</param>
    /// <returns>The domain entity.</returns>
    internal static AiVisionImagesDomain MapToDomain(AiVisionImagesDataDTO? dto) => new()
    {
        ImageName = dto?.ImageName ?? string.Empty,
        ImageUrl = dto?.ImageUrl ?? string.Empty,
        ImageKeywords = []
    };

    #endregion

    #region Domain to DTO Mappings

    /// <summary>
    /// Maps to DTO.
    /// </summary>
    /// <param name="domain">The domain entity.</param>
    /// <returns>The response DTO.</returns>
    internal static ChatHistoryDTO MapToDto(ChatHistoryDomain domain) => new()
    {
        Role = domain.Role,
        Content = domain.Content
    };

    /// <summary>
    /// Maps to DTO.
    /// </summary>
    /// <param name="domain">The domain entity.</param>
    /// <returns>The response DTO.</returns>
    internal static ConversationHistoryDTO MapToDto(ConversationHistoryDomain domain) => new()
    {
        Id = domain.Id,
        ConversationId = domain.ConversationId,
        UserName = domain.UserName,
        ChatHistory = [.. domain.ChatHistory.Select(MapToDto)],
        IsActive = domain.IsActive,
        LastModifiedOn = domain.LastModifiedOn
    };

    /// <summary>
    /// Maps to DTO.
    /// </summary>
    /// <param name="domain">The domain entity.</param>
    /// <returns>The response DTO.</returns>
    internal static AssociatedAgentsSkillDataDTO MapToDto(AssociatedAgentsSkillDataDomain domain) => new()
    {
        AgentName = domain.AgentName,
        AgentGuid = domain.AgentGuid
    };

    /// <summary>
    /// Maps to DTO.
    /// </summary>
    /// <param name="domain">The domain entity.</param>
    /// <returns>The response DTO.</returns>
    internal static GroupChatAgentsResponseDto MapToDto(GroupChatAgentsResponseDomain domain) => new()
    {
        AgentName = domain.AgentName,
        AgentResponse = domain.AgentResponse
    };

    /// <summary>
    /// Maps to DTO.
    /// </summary>
    /// <param name="domain">The domain entity.</param>
    /// <returns>The response DTO.</returns>
    internal static BugReportDataDto MapToDto(BugReportData domain) => new()
    {
        Id = domain.Id,
        Title = domain.Title,
        Description = domain.Description,
        BugSeverityId = domain.BugSeverityId,
        BugStatusId = domain.BugStatusId,
        AgentDetails = domain.AgentDetails,
        DateCreated = domain.DateCreated,
        CreatedBy = domain.CreatedBy,
        DateModified = domain.DateModified,
        ModifiedBy = domain.ModifiedBy
    };

    /// <summary>
    /// Maps to DTO.
    /// </summary>
    /// <param name="domain">The domain entity.</param>
    /// <returns>The response DTO.</returns>
    internal static NewFeatureRequestDataDto MapToDto(NewFeatureRequestData domain) => new()
    {
        Id = domain.Id,
        Title = domain.Title,
        Description = domain.Description,
        DateCreated = domain.DateCreated,
        CreatedBy = domain.CreatedBy,
        DateModified = domain.DateModified,
        ModifiedBy = domain.ModifiedBy
    };

    /// <summary>
    /// Maps to DTO.
    /// </summary>
    /// <param name="domain">The domain entity.</param>
    /// <returns>The response DTO.</returns>
    internal static AgentDataDTO MapToDto(AgentDataDomain domain) => new()
    {
        AgentId = domain.AgentId,
        AgentName = domain.AgentName,
        AgentDescription = domain.AgentDescription,
        AgentMetaPrompt = domain.AgentMetaPrompt,
        ApplicationId = domain.ApplicationId,
        KnowledgeBaseDocument = domain.KnowledgeBaseDocument,
        RemovedKnowledgeBaseDocuments = domain.RemovedKnowledgeBaseDocuments,
        IsPrivate = domain.IsPrivate,
        IsDefaultChatbot = domain.IsDefaultChatbot,
        VisionImages = domain.VisionImages ?? [],
        RemovedAiVisionImages = domain.RemovedAiVisionImages,
        AiVisionImagesData = [.. domain.AiVisionImagesData.Select(MapToDto)],
        AssociatedSkillGuids = domain.AssociatedSkillGuids,
        CreatedBy = domain.CreatedBy,
        DateCreated = domain.DateCreated,
        DateModified = domain.DateModified
    };

    /// <summary>
    /// Maps to DTO.
    /// </summary>
    /// <param name="domain">The domain entity.</param>
    /// <returns>The response DTO.</returns>
    internal static AiVisionImagesDataDTO MapToDto(AiVisionImagesDomain? domain) => new()
    {
        ImageName = domain?.ImageName ?? string.Empty,
        ImageUrl = domain?.ImageUrl ?? string.Empty
    };

    /// <summary>
    /// Maps to DTO.
    /// </summary>
    /// <param name="domain">The domain entity.</param>
    /// <returns>The response DTO.</returns>
    internal static ChatRequestDTO MapToDto(ChatRequestDomain domain) => new()
    {
        AgentName = domain.AgentName,
        AgentId = domain.AgentId,
        ConversationId = domain.ConversationId,
        UserMessage = domain.UserMessage
    };

    /// <summary>
    /// Maps to DTO.
    /// </summary>
    /// <param name="domain">The domain entity.</param>
    /// <returns>The response DTO.</returns>
    internal static ToolSkillDTO MapToDto(ToolSkillDomain domain) => new()
    {
        ToolSkillGuid = domain.ToolSkillGuid,
        ToolSkillDisplayName = domain.ToolSkillDisplayName,
        ToolSkillTechnicalName = domain.ToolSkillTechnicalName,
        ToolSkillMcpServerUrl = domain.ToolSkillMcpServerUrl,
        AssociatedAgents = [.. domain.AssociatedAgents.Select(MapToDto)],
        DateCreated = domain.DateCreated,
        CreatedBy = domain.CreatedBy,
        DateModified = domain.DateModified,
        ModifiedBy = domain.ModifiedBy
    };

    /// <summary>
    /// Maps to DTO.
    /// </summary>
    /// <param name="domain">The domain entity.</param>
    /// <returns>The response DTO.</returns>
    internal static AgentsWorkspaceDTO MapToDto(AgentsWorkspaceDomain domain) => new()
    {
        AgentWorkspaceGuid = domain.AgentWorkspaceGuid,
        AgentWorkspaceName = domain.AgentWorkspaceName,
        ActiveAgentsListInWorkspace = domain.ActiveAgentsListInWorkspace.Select(MapToDto),
        WorkspaceUsers = domain.WorkspaceUsers,
        IsGroupChatEnabled = domain.IsGroupChatEnabled,
        DateCreated = domain.DateCreated,
        CreatedBy = domain.CreatedBy,
        DateModified = domain.DateModified,
        ModifiedBy = domain.ModifiedBy
    };

    /// <summary>
    /// Maps to DTO.
    /// </summary>
    /// <param name="domain">The domain entity.</param>
    /// <returns>The response DTO.</returns>
    internal static WorkspaceAgentsDataDTO MapToDto(WorkspaceAgentsDataDomain domain) => new()
    {
        AgentName = domain.AgentName,
        AgentGuid = domain.AgentGuid
    };

    /// <summary>
    /// Maps to DTO.
    /// </summary>
    /// <param name="domain">The domain entity.</param>
    /// <returns>The response DTO.</returns>
    internal static GroupChatResponseDto MapToDto(GroupChatResponseDomain domain) => new()
    {
        AgentResponse = domain.AgentResponse,
        AgentsInvoked = domain.AgentsInvoked,
        ConversationId = domain.ConversationId
    };

    /// <summary>
    /// Maps to DTO.
    /// </summary>
    /// <param name="domain">The domain entity.</param>
    /// <returns>The response DTO.</returns>
    internal static RegisteredApplicationDto MapToDto(RegisteredApplicationDomain domain) => new()
    {
        Id = domain.Id,
        ApplicationName = domain.ApplicationName,
        Description = domain.Description,
        ApplicationRegistrationGuid = domain.ApplicationRegistrationGuid,
        IsAzureRegistered = domain.IsAzureRegistered,
        DateCreated = domain.DateCreated,
        CreatedBy = domain.CreatedBy,
        DateModified = domain.DateModified,
        ModifiedBy = domain.ModifiedBy
    };

    /// <summary>
    /// Maps to DTO.
    /// </summary>
    /// <param name="domain">The domain entity.</param>
    /// <returns>The response DTO.</returns>
    internal static NotificationsResponseDto MapToDto(NotificationsDomain domain) => new()
    {
        Id = domain.Id,
        Title = domain.Title,
        Message = domain.Message,
        RecipientUserName = domain.RecipientUserName,
        NotificationType = domain.NotificationType,
        CreatedBy = domain.CreatedBy,
        IsGlobal = domain.IsGlobal,
        IsRead = domain.IsRead,
        IsActive = domain.IsActive,
        DateCreated = domain.DateCreated
    };

    #endregion
}
