using AutoMapper;
using TaskFlow.Application.Abstractions;
using TaskFlow.Application.DTOs;
using TaskFlow.Domain.Common;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Application.Services
{
    public class OrganizationService(
        IOrganizationRepository repository,
        IUserService userService,
        IMembershipService membershipService,
        IMapper mapper,
        IUnitOfWork unitOfWork) :
        EntityService<Organization>(repository), IOrganizationService
    {
        private readonly IOrganizationRepository _repository = repository;
        private readonly IUserService _userService = userService;
        private readonly IMembershipService _membershipService = membershipService;
        private readonly IMapper _mapper = mapper;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async override Task<IEnumerable<Organization>> GetAllAsync()
        {
            return await _repository.GetAllAsync(_userService.MyId!);
        }

        public async override Task<Organization?> GetAsync(int id)
        {
            return await _repository.GetAsync(id, _userService.MyId!);
        }

        public async Task<Result<IEnumerable<OrganizationSummaryDto>>> GetMyOrganizationsAsync()
        {
            var organizations = await _repository.GetAllAsync(_userService.MyId!);
            var dtos = _mapper.Map<IEnumerable<OrganizationSummaryDto>>(organizations);
            return Result.Success(dtos);
        }

        public async Task<Result<OrganizationDto>> GetOrganizationByIdAsync(int id)
        {
            var organization = await _repository.GetAsync(id, _userService.MyId!);
            if (organization == null)
            {
                return Result.Failure<OrganizationDto>("Organization not found or access denied.");
            }

            var dto = _mapper.Map<OrganizationDto>(organization);
            return Result.Success(dto);
        }

        public async Task<Result<OrganizationDto>> CreateOrganizationAsync(string name, string? description)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var user = await _userService.GetMeAsync();
                if (user == null)
                {
                    return Result.Failure<OrganizationDto>("User not found.");
                }

                var organization = new Organization { Name = name, Description = description };

                var membership = new Membership
                {
                    Organization = organization,
                    User = user,
                    OrganizationRoles = [new() { Role = OrgRole.Admin }]
                };

                _unitOfWork.Memberships.Add(membership);
                await _unitOfWork.CommitAsync();

                _userService.InvalidateMyCache();

                var dto = _mapper.Map<OrganizationDto>(organization);
                return Result.Success(dto);
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<Result<OrganizationDto>> UpdateOrganizationAsync(int id, string name, string? description)
        {
            if (!await _membershipService.IAmAdminOfOrgAsync(id))
            {
                return Result.Failure<OrganizationDto>("You are not an admin of this organization.");
            }

            var organization = await _repository.GetAsync(id, _userService.MyId!);
            if (organization == null)
            {
                return Result.Failure<OrganizationDto>("Organization not found.");
            }

            organization.Name = name;
            organization.Description = description;

            _repository.Attach(organization);
            await _repository.SaveChangesAsync();

            _userService.InvalidateMyCache();

            var dto = _mapper.Map<OrganizationDto>(organization);
            return Result.Success(dto);
        }

        public async Task<Result> DeleteOrganizationAsync(int id)
        {
            if (!await _membershipService.IAmAdminOfOrgAsync(id))
            {
                return Result.Failure("You are not an admin of this organization.");
            }

            var organization = await _repository.GetAsync(id);
            if (organization == null)
            {
                return Result.Failure("Organization not found.");
            }

            _repository.Remove(organization);
            await _repository.SaveChangesAsync();

            _userService.InvalidateMyCache();

            return Result.Success();
        }

        public async override Task<bool> AddAsync(Organization organization)
        {
            var user = await _userService.GetMeAsync();
            var success = await _membershipService.AddAsync(new Membership
            {
                Organization = organization,
                User = user!,
                OrganizationRoles =
                [
                    new() {
                        Role = OrgRole.Admin
                    }
                ]
            });
            _userService.InvalidateMyCache();
            return success;
        }

        public async Task<bool> UpdateAsync(int id, Organization organization)
        {
            if (await _membershipService.IAmAdminOfOrgAsync(id))
            {
                organization.Id = id;
                _repository.Attach(organization);
                _userService.InvalidateMyCache();
                return await _repository.SaveChangesAsync();
            }
            return false;
        }

        public async override Task<bool> RemoveAsync(int id)
        {
            if (await _membershipService.IAmAdminOfOrgAsync(id))
            {
                _userService.InvalidateMyCache();
                return await base.RemoveAsync(id);
            }
            return false;
        }
    }
}
