using AIAgents.Laboratory.API.Adapters.Models.Base;
using AIAgents.Laboratory.API.Adapters.Models.Request;
using AIAgents.Laboratory.API.Adapters.Models.Response;
using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.Domain.DomainEntities.AgentsEntities;
using AIAgents.Laboratory.Domain.DomainEntities.FeedbackEntities;
using AIAgents.Laboratory.Domain.DomainEntities.SkillsEntities;
using AIAgents.Laboratory.Domain.DomainEntities.Workspaces;
using AutoMapper;

namespace AIAgents.Laboratory.API.Adapters.Mapper;

/// <summary>
/// The Domain Mapper Profile Class.
/// </summary>
/// <seealso cref="Profile" />
public class DomainMapperProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DomainMapperProfile"/> class.
    /// </summary>
    public DomainMapperProfile()
    {
        CreateMap<SkillsInputDTO, SkillsInputDomain>();
        CreateMap<NltosqlInputDTO, NltosqlInputDomain>();
        CreateMap<UserQueryRequestDTO, UserRequestDomain>();
        CreateMap<FollowupQuestionsRequestDTO, FollowupQuestionsRequestDomain>();
        CreateMap<CreateAgentDTO, AgentDataDomain>();
        CreateMap<AddBugReportDTO, BugReportData>()
            .ForMember(destination => destination.Id, options => options.Ignore())
            .ForMember(destination => destination.BugStatusId, options => options.Ignore())
            .ForMember(destination => destination.BugSeverityId, option => option.MapFrom(source => source.BugSeverity))
            .ForMember(destination => destination.Title, option => option.MapFrom(source => source.BugTitle))
            .ForMember(destination => destination.Description, option => option.MapFrom(source => source.BugDescription))
            .ForMember(destination => destination.AgentDetails, option => option.MapFrom(source => source.AgentDetails));
        CreateMap<NewFeatureRequestDTO, NewFeatureRequestData>()
            .ForMember(destination => destination.Id, options => options.Ignore())
            .ForMember(destination => destination.Title, options => options.MapFrom(source => source.Title))
            .ForMember(destination => destination.Description, options => options.MapFrom(source => source.Description))
            .ForMember(destination => destination.CreatedBy, options => options.MapFrom(source => source.CreatedBy));
        CreateMap<WorkspaceAgentChatRequestDTO, WorkspaceAgentChatRequestDomain>();

        CreateMap<AgentStatus, AgentStatusDTO>();
        CreateMap<AIAgentResponseDomain, AIAgentResponseDTO>();
        CreateMap<ChatHistoryDomain, ChatHistoryDTO>();
        CreateMap<ConversationHistoryDomain, ConversationHistoryDTO>();
        CreateMap<AssociatedAgentsSkillDataDomain, AssociatedAgentsSkillDataDTO>();

        CreateMap<AgentDataDomain, AgentDataDTO>().ReverseMap()
            .ForMember(destination => destination.AgentId, opt => opt.MapFrom(source => source.AgentId));
        CreateMap<ChatRequestDTO, ChatRequestDomain>().ReverseMap();
        CreateMap<AiVisionImagesDomain, AiVisionImagesDataDTO>().ReverseMap()
            .ForMember(destination => destination.ImageKeywords, options => options.Ignore());
        CreateMap<ToolSkillDomain, ToolSkillDTO>().ReverseMap()
            .ForMember(destination => destination.IsActive, options => options.Ignore());
        CreateMap<AgentsWorkspaceDomain, AgentsWorkspaceDTO>().ReverseMap();
        CreateMap<WorkspaceAgentsDataDomain, WorkspaceAgentsDataDTO>().ReverseMap();
        CreateMap<GroupChatResponseDomain, GroupChatResponseDTO>().ReverseMap();
    }
}
