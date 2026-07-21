using AutoMapper;
using FluentAssertions;
using Moq;
using TaskFlow.Application.Abstractions;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.Services;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Tests.Application
{
    public class OrganizationServiceTests
    {
        private readonly Mock<IOrganizationRepository> _mockRepository;
        private readonly Mock<IUserService> _mockUserService;
        private readonly Mock<IMembershipService> _mockMembershipService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly OrganizationService _sut;

        public OrganizationServiceTests()
        {
            _mockRepository = new Mock<IOrganizationRepository>();
            _mockUserService = new Mock<IUserService>();
            _mockMembershipService = new Mock<IMembershipService>();
            _mockMapper = new Mock<IMapper>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();

            _mockUnitOfWork.Setup(x => x.Memberships).Returns(Mock.Of<IMembershipRepository>());

            _sut = new OrganizationService(
                _mockRepository.Object,
                _mockUserService.Object,
                _mockMembershipService.Object,
                _mockMapper.Object,
                _mockUnitOfWork.Object);
        }

        [Fact]
        public async Task GetMyOrganizationsAsync_ShouldReturnOrganizations()
        {
            // Arrange
            var userId = "user123";
            var organizations = new List<Organization>
            {
                new() { Id = 1, Name = "Org 1" },
                new() { Id = 2, Name = "Org 2" }
            };
            var organizationDtos = new List<OrganizationSummaryDto>
            {
                new() { Id = 1, Name = "Org 1" },
                new() { Id = 2, Name = "Org 2" }
            };

            _mockUserService.Setup(x => x.MyId).Returns(userId);
            _mockRepository.Setup(x => x.GetAllAsync(userId))
                .ReturnsAsync(organizations);
            _mockMapper.Setup(x => x.Map<IEnumerable<OrganizationSummaryDto>>(organizations))
                .Returns(organizationDtos);

            // Act
            var result = await _sut.GetMyOrganizationsAsync();

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().HaveCount(2);
            result.Value.Should().BeEquivalentTo(organizationDtos);
        }

        [Fact]
        public async Task GetOrganizationByIdAsync_WhenOrgExists_ShouldReturnOrganization()
        {
            // Arrange
            var orgId = 1;
            var userId = "user123";
            var organization = new Organization { Id = orgId, Name = "Test Org" };
            var organizationDto = new OrganizationDto { Id = orgId, Name = "Test Org" };

            _mockUserService.Setup(x => x.MyId).Returns(userId);
            _mockRepository.Setup(x => x.GetAsync(orgId, userId))
                .ReturnsAsync(organization);
            _mockMapper.Setup(x => x.Map<OrganizationDto>(organization))
                .Returns(organizationDto);

            // Act
            var result = await _sut.GetOrganizationByIdAsync(orgId);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeEquivalentTo(organizationDto);
        }

        [Fact]
        public async Task GetOrganizationByIdAsync_WhenOrgDoesNotExist_ShouldReturnFailure()
        {
            // Arrange
            var orgId = 999;
            var userId = "user123";

            _mockUserService.Setup(x => x.MyId).Returns(userId);
            _mockRepository.Setup(x => x.GetAsync(orgId, userId))
                .ReturnsAsync((Organization?)null);

            // Act
            var result = await _sut.GetOrganizationByIdAsync(orgId);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be("Organization not found or access denied.");
        }

        [Fact]
        public async Task CreateOrganizationAsync_ShouldCreateOrganizationWithAdminMembership()
        {
            // Arrange
            var name = "New Org";
            var description = "Description";
            var user = new ApplicationUser { Id = "user123", Name = "Test User" };
            var organization = new Organization { Id = 1, Name = name, Description = description };
            var organizationDto = new OrganizationDto { Id = 1, Name = name, Description = description };

            _mockUserService.Setup(x => x.GetMeAsync()).ReturnsAsync(user);
            _mockUnitOfWork.Setup(x => x.BeginTransactionAsync()).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(x => x.CommitAsync()).Returns(Task.CompletedTask);
            _mockMapper.Setup(x => x.Map<OrganizationDto>(It.IsAny<Organization>()))
                .Returns(organizationDto);

            // Act
            var result = await _sut.CreateOrganizationAsync(name, description);

            // Assert
            result.IsSuccess.Should().BeTrue();
            _mockUnitOfWork.Verify(x => x.BeginTransactionAsync(), Times.Once);
            _mockUnitOfWork.Verify(x => x.CommitAsync(), Times.Once);
            _mockUserService.Verify(x => x.InvalidateMyCache(), Times.Once);
        }

        [Fact]
        public async Task CreateOrganizationAsync_WhenUserNotFound_ShouldReturnFailure()
        {
            // Arrange
            var name = "New Org";
            var description = "Description";

            _mockUserService.Setup(x => x.GetMeAsync()).ReturnsAsync((ApplicationUser)null!);

            // Act
            var result = await _sut.CreateOrganizationAsync(name, description);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be("User not found.");
        }

        [Fact]
        public async Task UpdateOrganizationAsync_WhenUserIsAdmin_ShouldUpdateOrganization()
        {
            // Arrange
            var orgId = 1;
            var name = "Updated Org";
            var description = "Updated Description";
            var userId = "user123";
            var organization = new Organization { Id = orgId, Name = "Old Name" };
            var organizationDto = new OrganizationDto { Id = orgId, Name = name, Description = description };

            _mockUserService.Setup(x => x.MyId).Returns(userId);
            _mockMembershipService.Setup(x => x.IAmAdminOfOrgAsync(orgId))
                .ReturnsAsync(true);
            _mockRepository.Setup(x => x.GetAsync(orgId, userId))
                .ReturnsAsync(organization);
            _mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(true);
            _mockMapper.Setup(x => x.Map<OrganizationDto>(organization))
                .Returns(organizationDto);

            // Act
            var result = await _sut.UpdateOrganizationAsync(orgId, name, description);

            // Assert
            result.IsSuccess.Should().BeTrue();
            organization.Name.Should().Be(name);
            organization.Description.Should().Be(description);
            _mockRepository.Verify(x => x.Attach(organization), Times.Once);
            _mockUserService.Verify(x => x.InvalidateMyCache(), Times.Once);
        }

        [Fact]
        public async Task UpdateOrganizationAsync_WhenUserIsNotAdmin_ShouldReturnFailure()
        {
            // Arrange
            var orgId = 1;
            var name = "Updated Org";
            var description = "Updated Description";

            _mockMembershipService.Setup(x => x.IAmAdminOfOrgAsync(orgId))
                .ReturnsAsync(false);

            // Act
            var result = await _sut.UpdateOrganizationAsync(orgId, name, description);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be("You are not an admin of this organization.");
        }

        [Fact]
        public async Task DeleteOrganizationAsync_WhenUserIsAdmin_ShouldDeleteOrganization()
        {
            // Arrange
            var orgId = 1;
            var organization = new Organization { Id = orgId, Name = "Test Org" };

            _mockMembershipService.Setup(x => x.IAmAdminOfOrgAsync(orgId))
                .ReturnsAsync(true);
            _mockRepository.Setup(x => x.GetAsync(orgId))
                .ReturnsAsync(organization);
            _mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(true);

            // Act
            var result = await _sut.DeleteOrganizationAsync(orgId);

            // Assert
            result.IsSuccess.Should().BeTrue();
            _mockRepository.Verify(x => x.Remove(organization), Times.Once);
            _mockUserService.Verify(x => x.InvalidateMyCache(), Times.Once);
        }

        [Fact]
        public async Task DeleteOrganizationAsync_WhenUserIsNotAdmin_ShouldReturnFailure()
        {
            // Arrange
            var orgId = 1;

            _mockMembershipService.Setup(x => x.IAmAdminOfOrgAsync(orgId))
                .ReturnsAsync(false);

            // Act
            var result = await _sut.DeleteOrganizationAsync(orgId);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be("You are not an admin of this organization.");
        }

        [Fact]
        public async Task DeleteOrganizationAsync_WhenOrgNotFound_ShouldReturnFailure()
        {
            // Arrange
            var orgId = 999;

            _mockMembershipService.Setup(x => x.IAmAdminOfOrgAsync(orgId))
                .ReturnsAsync(true);
            _mockRepository.Setup(x => x.GetAsync(orgId))
                .ReturnsAsync((Organization?)null);

            // Act
            var result = await _sut.DeleteOrganizationAsync(orgId);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be("Organization not found.");
        }
    }
}
