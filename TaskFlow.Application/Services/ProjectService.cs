using AutoMapper;
using TaskFlow.Application.Abstractions;
using TaskFlow.Application.DTOs;
using TaskFlow.Domain.Common;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Services
{
    public class ProjectService(
        IProjectRepository repository,
        IMembershipService membershipService,
        IMapper mapper,
        IUnitOfWork unitOfWork) : EntityService<Project>(repository), IProjectService
    {
        private readonly IProjectRepository _repository = repository;
        private readonly IMembershipService _membershipService = membershipService;
        private readonly IMapper _mapper = mapper;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<IEnumerable<ProjectSummaryDto>>> GetMyProjectsAsync()
        {
            var userMemberships = await _membershipService.GetUserMembershipsAsync();
            var accessibleOrgIds = userMemberships.Select(m => m.OrganizationId);
            var projects = await _repository.GetAllByUserMembershipsAsync(accessibleOrgIds);
            var dtos = _mapper.Map<IEnumerable<ProjectSummaryDto>>(projects);
            return Result.Success(dtos);
        }

        public async Task<Result<ProjectDto>> GetProjectByIdAsync(int id)
        {
            var project = await _repository.GetAsync(id);
            if (project == null)
            {
                return Result.Failure<ProjectDto>("Project not found.");
            }

            if (await _membershipService.GetUserMembershipForOrgAsync(project.OrganizationId) == null)
            {
                return Result.Failure<ProjectDto>("You do not have access to this project.");
            }

            var dto = _mapper.Map<ProjectDto>(project);
            return Result.Success(dto);
        }

        public async Task<Result<IEnumerable<ProjectSummaryDto>>> GetProjectsByOrganizationAsync(int organizationId)
        {
            var membership = await _membershipService.GetUserMembershipForOrgAsync(organizationId);
            if (membership == null)
            {
                return Result.Failure<IEnumerable<ProjectSummaryDto>>("You do not have access to this organization.");
            }

            var projects = await _repository.GetAllByOrganizationIdAsync(organizationId);
            var dtos = _mapper.Map<IEnumerable<ProjectSummaryDto>>(projects);
            return Result.Success(dtos);
        }

        public async Task<Result<ProjectDto>> CreateProjectAsync(int organizationId, string name, string? description)
        {
            if (!await _membershipService.IAmAdminOfOrgAsync(organizationId))
            {
                return Result.Failure<ProjectDto>("You must be an admin to create projects in this organization.");
            }

            var organization = await _unitOfWork.Organizations.GetAsync(organizationId);
            if (organization == null)
            {
                return Result.Failure<ProjectDto>("Organization not found.");
            }

            var project = new Project
            {
                Name = name,
                Description = description,
                Organization = organization,
                OrganizationId = organizationId
            };

            _repository.Add(project);
            await _repository.SaveChangesAsync();

            var dto = _mapper.Map<ProjectDto>(project);
            return Result.Success(dto);
        }

        public async Task<Result<ProjectDto>> UpdateProjectAsync(int id, string name, string? description)
        {
            var project = await _repository.GetAsync(id);
            if (project == null)
            {
                return Result.Failure<ProjectDto>("Project not found.");
            }

            if (!await _membershipService.IAmAdminOfOrgAsync(project.OrganizationId))
            {
                return Result.Failure<ProjectDto>("You must be an admin to update this project.");
            }

            project.Name = name;
            project.Description = description;

            _repository.Attach(project);
            await _repository.SaveChangesAsync();

            var dto = _mapper.Map<ProjectDto>(project);
            return Result.Success(dto);
        }

        public async Task<Result> DeleteProjectAsync(int id)
        {
            var project = await _repository.GetAsync(id);
            if (project == null)
            {
                return Result.Failure("Project not found.");
            }

            if (!await _membershipService.IAmAdminOfOrgAsync(project.OrganizationId))
            {
                return Result.Failure("You must be an admin to delete this project.");
            }

            _repository.Remove(project);
            await _repository.SaveChangesAsync();

            return Result.Success();
        }

        public override async Task<IEnumerable<Project>> GetAllAsync()
        {
            var userMemberships = await _membershipService.GetUserMembershipsAsync();
            var accessibleOrgIds = userMemberships.Select(m => m.OrganizationId);
            return await _repository.GetAllByUserMembershipsAsync(accessibleOrgIds);
        }

        public override async Task<Project?> GetAsync(int id)
        {
            var project = await _repository.GetAsync(id);
            if (project != null && await _membershipService.GetUserMembershipForOrgAsync(project.OrganizationId) == null)
                return null;
            return project;
        }

        public override async Task<bool> AddAsync(Project entity)
        {
            if (!await _membershipService.IAmAdminOfOrgAsync(entity.OrganizationId))
                return false;
            _repository.Add(entity);
            return await _repository.SaveChangesAsync();
        }

        public override async Task<bool> RemoveAsync(int id)
        {
            var project = await _repository.GetAsync(id);
            if (project == null || !await _membershipService.IAmAdminOfOrgAsync(project.OrganizationId))
                return false;
            _repository.Remove(project);
            return await _repository.SaveChangesAsync();
        }
    }
}
