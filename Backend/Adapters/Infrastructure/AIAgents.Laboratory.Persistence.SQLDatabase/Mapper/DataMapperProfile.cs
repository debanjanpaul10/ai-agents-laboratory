using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.Domain.DomainEntities.FeedbackEntities;
using AIAgents.Laboratory.Persistence.SQLDatabase.Models;
using AutoMapper;

namespace AIAgents.Laboratory.Persistence.SQLDatabase.Mapper;

/// <summary>
/// The Data Mapper Profile Class for mapping
/// </summary>
/// <seealso cref="Profile"/>
public sealed class DataMapperProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DataMapperProfile"/> class.
    /// </summary>
    public DataMapperProfile()
    {
        CreateMap<RegisteredApplication, RegisteredApplicationEntity>().ReverseMap();
        CreateMap<BugReportData, BugReportDataEntity>().ReverseMap();
        CreateMap<BaseDomainModel, BaseEntity>().ReverseMap();
        CreateMap<BugItemStatusMappingEntity, BugItemStatusMapping>().ReverseMap();
        CreateMap<BugSeverityMappingEntity, BugSeverityMapping>().ReverseMap();
    }
}
