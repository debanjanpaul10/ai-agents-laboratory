// *********************************************************************************
//	<copyright file="DomainMapperProfile.cs" company="Personal">
//		Copyright (c) 2025 <Debanjan's Lab>
//	</copyright>
// <summary>The Domain Mapper Profile Class.</summary>
// *********************************************************************************

using AIAgents.Laboratory.API.Adapters.Models.Request;
using AIAgents.Laboratory.API.Adapters.Models.Response;
using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.Domain.DomainEntities.SkillsEntities;
using AutoMapper;

namespace AIAgents.Laboratory.API.Adapters.Mapper;

/// <summary>
/// The Domain Mapper Profile Class.
/// </summary>
/// <seealso cref="AutoMapper.Profile" />
public class DomainMapperProfile : Profile
{
	/// <summary>
	/// Initializes a new instance of the <see cref="DomainMapperProfile"/> class.
	/// </summary>
	public DomainMapperProfile()
	{
		CreateMap<SkillsInputDTO, SkillsInputDomain>();
		CreateMap<NltosqlInputDTO, NltosqlInputDomain>();
		CreateMap<BugSeverityInputDTO, BugSeverityInput>();
		CreateMap<UserQueryRequestDTO, UserRequestDomain>();
		CreateMap<FollowupQuestionsRequestDTO, FollowupQuestionsRequestDomain>();

		CreateMap<BaseResponse, BaseResponseDTO>().ReverseMap();
		CreateMap<TagResponse, TagResponseDTO>();
		CreateMap<ModerationContentResponse, ModerationContentResponseDTO>();
		CreateMap<RewriteResponse, RewriteResponseDTO>();
		CreateMap<AgentStatus, AgentStatusDTO>();
		CreateMap<BugSeverityResponse, BugSeverityResponseDTO>();
		CreateMap<AIAgentResponseDomain, AIAgentResponseDTO>();
	}
}
