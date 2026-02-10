using AutoMapper;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.DTOs.Auth;
using TaskFlow.Application.DTOs.Memberships;
using TaskFlow.Application.DTOs.Organizations;
using TaskFlow.Application.DTOs.Projects;
using TaskFlow.Application.DTOs.Tasks;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Api.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Request DTOs to Domain Entities
            CreateMap<TaskItemRequest, TaskItem>();
            CreateMap<RegisterRequest, ApplicationUser>();
            CreateMap<ProjectRequest, Project>();
            CreateMap<OrganizationRequest, Organization>();

            // Domain Entities to Response DTOs
            CreateMap<Organization, OrganizationResponse>();
            CreateMap<ApplicationUser, UserResponse>();

            CreateMap<Membership, MembershipResponse>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.OrganizationRoles))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email));

            CreateMap<OrganizationRole, OrganizationRoleDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Role));

            // Entity to DTO mappings
            CreateMap<Organization, OrganizationDto>();
            CreateMap<Organization, OrganizationSummaryDto>();
            CreateMap<Project, ProjectDto>();
            CreateMap<Project, ProjectSummaryDto>();
            CreateMap<TaskItem, TaskItemDto>();
            CreateMap<TaskItem, TaskItemSummaryDto>();
            CreateMap<ApplicationUser, UserDto>()
                .ForMember(dest => dest.Roles, opt => opt.Ignore());
            CreateMap<Comment, CommentDto>();
            CreateMap<Comment, CommentSummaryDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.Name : null));
            CreateMap<CommentReaction, CommentReactionDto>()
                .ForMember(dest => dest.Reaction, opt => opt.MapFrom(src => src.Reaction.ToString()))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.Name : null));
            CreateMap<TaskReaction, TaskReactionDto>()
                .ForMember(dest => dest.Reaction, opt => opt.MapFrom(src => src.Liked ? "Like" : ""))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.Name : null));
        }
    }
}
