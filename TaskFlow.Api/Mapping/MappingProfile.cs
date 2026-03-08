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
            CreateMap<Organization, OrganizationResponse>();

            CreateMap<Membership, MembershipResponse>()
           .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.OrganizationRoles));

            CreateMap<OrganizationRole, OrganizationRoleDto>()
                .ForMember(dest => dest.OrganizationRoleId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.MembershipId, opt => opt.MapFrom(src => src.MembershipId))
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role));
        }
    }
}
