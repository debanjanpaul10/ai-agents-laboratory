using AIAgents.Laboratory.API.Adapters.Models.Request;
using AIAgents.Laboratory.API.Adapters.Models.Response;
using AIAgents.Laboratory.Domain.Models;
using AIAgents.Laboratory.Domain.Models.Agents;
using AIAgents.Laboratory.Domain.Models.Applications;
using AIAgents.Laboratory.Domain.Models.Feedback;
using AIAgents.Laboratory.Domain.Models.Skills;
using AIAgents.Laboratory.Domain.Models.Workspaces;
using Microsoft.AspNetCore.Http;

namespace AIAgents.Laboratory.API.Adapters.Mapper;

/// <summary>
/// This class provides mapping methods to convert request DTOs to domain entities.
/// </summary>
public static class RequestToDomainMapper
{
    /// <summary>
    /// Maps to domain.
    /// </summary>
    /// <param name="dto">The request DTO.</param>
    /// <returns>The domain entity.</returns>
    internal static UserRequestDomain MapToDomain(
        UserQueryRequestDTO dto
    ) =>
        new()
        {
            UserQuery = dto.UserQuery
        };

    /// <summary>
    /// Maps to domain.
    /// </summary>
    /// <param name="dto">The request DTO.</param>
    /// <returns>The domain entity.</returns>
    internal static FollowupQuestionsRequestDomain MapToDomain(
        FollowupQuestionsRequestDTO dto
    ) =>
        new()
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
    internal static AgentDataDomain MapToDomain(
        CreateAgentDTO dto
    ) =>
        new()
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
    internal static AgentDataDomain MapToDomain(
        AgentDataDTO dto
    ) =>
        new()
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
    internal static BugReportData MapToDomain(
        AddBugReportDTO dto
    ) =>
        new()
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
    internal static NewFeatureRequestData MapToDomain(
        NewFeatureRequestDTO dto
    ) =>
        new()
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
    internal static WorkspaceAgentChatRequestDomain MapToDomain(
        WorkspaceAgentChatRequestDto dto
    ) =>
        new()
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
    internal static ChatRequestDomain MapToDomain(
        ChatRequestDTO dto
    ) =>
        new()
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
    internal static AgentsWorkspaceDomain MapToDomain(
        AgentsWorkspaceDTO dto
    ) =>
        new()
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
    internal static WorkspaceAgentsDataDomain MapToDomain(
        WorkspaceAgentsDataDTO dto
    ) =>
        new()
        {
            AgentName = dto.AgentName,
            AgentGuid = dto.AgentGuid
        };

    /// <summary>
    /// Maps to domain.
    /// </summary>
    /// <param name="dto">The response DTO.</param>
    /// <returns>The domain entity.</returns>
    internal static RegisteredApplicationDomain MapToDomain(
        RegisteredApplicationDto dto
    ) =>
        new()
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
    internal static NotificationsDomain MapToDomain(
        CreateNotificationRequestDto dto
    ) =>
        new()
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
    internal static NotificationsDomain MapToDomain(
        NotificationsResponseDto dto
    ) =>
        new()
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
    internal static ToolSkillDomain MapToDomain(
        ToolSkillDTO dto
    ) =>
        new()
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
    internal static AssociatedAgentsSkillDataDomain MapToDomain(
        AssociatedAgentsSkillDataDTO dto
    ) =>
        new()
        {
            AgentName = dto.AgentName,
            AgentGuid = dto.AgentGuid
        };

    /// <summary>
    /// Maps to domain.
    /// </summary>
    /// <param name="dto">The response DTO.</param>
    /// <returns>The domain entity.</returns>
    internal static AiVisionImagesDomain MapToDomain(
        AiVisionImagesDataDTO? dto
    ) =>
        new()
        {
            ImageName = dto?.ImageName ?? string.Empty,
            ImageUrl = dto?.ImageUrl ?? string.Empty,
            ImageKeywords = []
        };
}