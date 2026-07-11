using AIAgents.Laboratory.API.Adapters.Models.Request;
using AIAgents.Laboratory.API.Adapters.Models.Response;
using AIAgents.Laboratory.Domain.Models;
using AIAgents.Laboratory.Domain.Models.Agents;
using AIAgents.Laboratory.Domain.Models.Applications;
using AIAgents.Laboratory.Domain.Models.Chats;
using AIAgents.Laboratory.Domain.Models.Feedback;
using AIAgents.Laboratory.Domain.Models.Skills;
using AIAgents.Laboratory.Domain.Models.Workspaces;

namespace AIAgents.Laboratory.API.Adapters.Mapper;

/// <summary>
/// This class provides mapping methods to convert domain entities to response DTOs.
/// </summary>
internal static class DomainToResponseMapper
{
    /// <summary>
    /// Maps to DTO.
    /// </summary>
    /// <param name="domain">The domain entity.</param>
    /// <returns>The response DTO.</returns>
    internal static ChatHistoryDTO MapToDto(
        ChatHistoryDomain domain
    ) =>
        new()
        {
            Role = domain.Role,
            Content = domain.Content
        };

    /// <summary>
    /// Maps to DTO.
    /// </summary>
    /// <param name="domain">The domain entity.</param>
    /// <returns>The response DTO.</returns>
    internal static ConversationHistoryDTO MapToDto(
        ConversationHistoryDomain domain
    ) =>
        new()
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
    internal static AssociatedAgentsSkillDataDTO MapToDto(
        AssociatedAgentsSkillDataDomain domain
    ) =>
        new()
        {
            AgentName = domain.AgentName,
            AgentGuid = domain.AgentGuid
        };

    /// <summary>
    /// Maps to DTO.
    /// </summary>
    /// <param name="domain">The domain entity.</param>
    /// <returns>The response DTO.</returns>
    internal static GroupChatAgentsResponseDto MapToDto(
        GroupChatAgentsResponseDomain domain
    ) =>
        new()
        {
            AgentName = domain.AgentName,
            AgentResponse = domain.AgentResponse
        };

    /// <summary>
    /// Maps to DTO.
    /// </summary>
    /// <param name="domain">The domain entity.</param>
    /// <returns>The response DTO.</returns>
    internal static BugReportDataDto MapToDto(
        BugReportData domain
    ) =>
        new()
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
    internal static NewFeatureRequestDataDto MapToDto(
        NewFeatureRequestData domain
    ) =>
        new()
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
    internal static AgentDataDTO MapToDto(
        AgentDataDomain domain
    ) =>
        new()
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
    internal static AiVisionImagesDataDTO MapToDto(
        AiVisionImagesDomain? domain
    ) =>
        new()
        {
            ImageName = domain?.ImageName ?? string.Empty,
            ImageUrl = domain?.ImageUrl ?? string.Empty
        };

    /// <summary>
    /// Maps to DTO.
    /// </summary>
    /// <param name="domain">The domain entity.</param>
    /// <returns>The response DTO.</returns>
    internal static ChatRequestDTO MapToDto(
        ChatRequestDomain domain
    ) =>
        new()
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
    internal static ToolSkillDTO MapToDto(
        ToolSkillDomain domain
    ) =>
        new()
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
    internal static AgentsWorkspaceDTO MapToDto(
        AgentsWorkspaceDomain domain
    ) =>
        new()
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
    internal static WorkspaceAgentsDataDTO MapToDto(
        WorkspaceAgentsDataDomain domain
    ) =>
        new()
        {
            AgentName = domain.AgentName,
            AgentGuid = domain.AgentGuid
        };

    /// <summary>
    /// Maps to DTO.
    /// </summary>
    /// <param name="domain">The domain entity.</param>
    /// <returns>The response DTO.</returns>
    internal static GroupChatResponseDto MapToDto(
        GroupChatResponseDomain domain
    ) =>
        new()
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
    internal static RegisteredApplicationDto MapToDto(
        RegisteredApplicationDomain domain
    ) =>
        new()
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
    internal static NotificationsResponseDto MapToDto(
        NotificationsDomain domain
    ) =>
        new()
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
}
