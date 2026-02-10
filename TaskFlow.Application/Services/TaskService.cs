using AutoMapper;
using TaskFlow.Application.Abstractions;
using TaskFlow.Application.DTOs;
using TaskFlow.Domain.Common;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Services
{
    public class TaskService(
        ITaskRepository repository,
        IProjectRepository projectRepository,
        IUserRepository userRepository,
        IMembershipService membershipService,
        IMapper mapper,
        IUnitOfWork unitOfWork) : EntityService<TaskItem>(repository), ITaskService
    {
        private readonly ITaskRepository _repository = repository;
        private readonly IProjectRepository _projectRepository = projectRepository;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IMembershipService _membershipService = membershipService;
        private readonly IMapper _mapper = mapper;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<TaskItemDto>> GetTaskByIdAsync(int id)
        {
            var task = await _repository.GetAsync(id);
            if (task == null)
            {
                return Result.Failure<TaskItemDto>("Task not found.");
            }

            var project = await _projectRepository.GetAsync(task.ProjectId);
            if (project == null || await _membershipService.GetUserMembershipForOrgAsync(project.OrganizationId) == null)
            {
                return Result.Failure<TaskItemDto>("You do not have access to this task.");
            }

            var dto = _mapper.Map<TaskItemDto>(task);
            return Result.Success(dto);
        }

        public async Task<Result<IEnumerable<TaskItemSummaryDto>>> GetTasksByProjectAsync(int projectId)
        {
            var project = await _projectRepository.GetAsync(projectId);
            if (project == null)
            {
                return Result.Failure<IEnumerable<TaskItemSummaryDto>>("Project not found.");
            }

            if (await _membershipService.GetUserMembershipForOrgAsync(project.OrganizationId) == null)
            {
                return Result.Failure<IEnumerable<TaskItemSummaryDto>>("You do not have access to this project.");
            }

            var tasks = await _repository.GetTasksByProjectIdAsync(projectId);
            var dtos = _mapper.Map<IEnumerable<TaskItemSummaryDto>>(tasks);
            return Result.Success(dtos);
        }

        public async Task<Result<TaskItemDto>> CreateTaskAsync(int projectId, string title, string? description)
        {
            var project = await _projectRepository.GetAsync(projectId);
            if (project == null)
            {
                return Result.Failure<TaskItemDto>("Project not found.");
            }

            if (await _membershipService.GetUserMembershipForOrgAsync(project.OrganizationId) == null)
            {
                return Result.Failure<TaskItemDto>("You must be a member of the organization to create tasks.");
            }

            var task = new TaskItem
            {
                Title = title,
                Description = description,
                Project = project,
                ProjectId = projectId
            };

            _repository.Add(task);
            await _repository.SaveChangesAsync();

            var dto = _mapper.Map<TaskItemDto>(task);
            return Result.Success(dto);
        }

        public async Task<Result<TaskItemDto>> UpdateTaskAsync(int id, string title, string? description)
        {
            var task = await _repository.GetAsync(id);
            if (task == null)
            {
                return Result.Failure<TaskItemDto>("Task not found.");
            }

            var project = await _projectRepository.GetAsync(task.ProjectId);
            if (project == null || await _membershipService.GetUserMembershipForOrgAsync(project.OrganizationId) == null)
            {
                return Result.Failure<TaskItemDto>("You do not have access to update this task.");
            }

            task.Title = title;
            task.Description = description;

            _repository.Attach(task);
            await _repository.SaveChangesAsync();

            var dto = _mapper.Map<TaskItemDto>(task);
            return Result.Success(dto);
        }

        public async Task<Result> DeleteTaskAsync(int id)
        {
            var task = await _repository.GetAsync(id);
            if (task == null)
            {
                return Result.Failure("Task not found.");
            }

            var project = await _projectRepository.GetAsync(task.ProjectId);
            if (project == null)
            {
                return Result.Failure("Project not found.");
            }

            if (!await _membershipService.IAmAdminOfOrgAsync(project.OrganizationId))
            {
                return Result.Failure("You must be an admin to delete tasks.");
            }

            _repository.Remove(task);
            await _repository.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result<TaskItemDto>> AssignTaskToUserAsync(int taskId, string userId)
        {
            var task = await _repository.GetAsync(taskId);
            if (task == null)
            {
                return Result.Failure<TaskItemDto>("Task not found.");
            }

            var project = await _projectRepository.GetAsync(task.ProjectId);
            if (project == null || await _membershipService.GetUserMembershipForOrgAsync(project.OrganizationId) == null)
            {
                return Result.Failure<TaskItemDto>("You do not have access to this task.");
            }

            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                return Result.Failure<TaskItemDto>("User not found.");
            }

            if (await _membershipService.GetUserMembershipForOrgAsync(project.OrganizationId, userId) == null)
            {
                return Result.Failure<TaskItemDto>("User is not a member of this organization.");
            }

            task.UserId = userId;
            task.User = user;

            _repository.Attach(task);
            await _repository.SaveChangesAsync();

            var dto = _mapper.Map<TaskItemDto>(task);
            return Result.Success(dto);
        }

        public async Task<Result<TaskItemDto>> UnassignTaskAsync(int taskId)
        {
            var task = await _repository.GetAsync(taskId);
            if (task == null)
            {
                return Result.Failure<TaskItemDto>("Task not found.");
            }

            var project = await _projectRepository.GetAsync(task.ProjectId);
            if (project == null || await _membershipService.GetUserMembershipForOrgAsync(project.OrganizationId) == null)
            {
                return Result.Failure<TaskItemDto>("You do not have access to this task.");
            }

            task.UserId = null;
            task.User = null;

            _repository.Attach(task);
            await _repository.SaveChangesAsync();

            var dto = _mapper.Map<TaskItemDto>(task);
            return Result.Success(dto);
        }
    }
}
