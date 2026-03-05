using AutoMapper;
using TaskFlow.Api.Contracts;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Api.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<TaskItemRequest, TaskItem>();
            CreateMap<UserRegistrationRequest, ApplicationUser>();
            CreateMap<ProjectRequest, Project>();
            CreateMap<OrganizationRequest, Organization>();
        }
    }
}
