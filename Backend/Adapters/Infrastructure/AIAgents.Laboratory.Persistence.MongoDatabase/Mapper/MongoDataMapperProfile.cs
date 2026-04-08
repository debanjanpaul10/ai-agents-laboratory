using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.Domain.DomainEntities.AgentsEntities;
using AIAgents.Laboratory.Domain.DomainEntities.Workspaces;
using AIAgents.Laboratory.Persistence.MongoDatabase.Models;
using AutoMapper;

namespace AIAgents.Laboratory.Persistence.MongoDatabase.Mapper;

/// <summary>
/// AutoMapper profile for mapping between MongoDB data models and domain models.
/// </summary>
/// <seealso cref="Profile"/>
public sealed class MongoDataMapperProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MongoDataMapperProfile"/> class and configures the mappings.
    /// </summary>
    public MongoDataMapperProfile()
    {
        CreateMap<BaseDomainModel, BaseDataModel>().ReverseMap();
        CreateMap<AgentDataDomain, AgentDataModel>().ReverseMap();
        CreateMap<ChatHistoryDomain, ChatHistoryModel>().ReverseMap();
        CreateMap<ConversationHistoryDomain, ConversationHistoryModel>().ReverseMap();
        CreateMap<AgentsWorkspaceDomain, AgentsWorkspaceDataModel>().ReverseMap();
        CreateMap<WorkspaceAgentsDataDomain, WorkspaceAgentsDataModel>().ReverseMap();
        CreateMap<RegisteredApplicationDomain, RegisteredApplicationDataModel>().ReverseMap();
        CreateMap<ToolSkillDomain, ToolSkillModel>().ReverseMap();
        CreateMap<AssociatedAgentsSkillDataDomain, AssociatedAgentsSkillDataModel>().ReverseMap();
        CreateMap<NotificationsDomain, NotificationsModel>().ReverseMap();
    }
}
