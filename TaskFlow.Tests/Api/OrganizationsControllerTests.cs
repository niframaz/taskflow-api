using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TaskFlow.Api.Controllers;
using TaskFlow.Application.Abstractions;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.DTOs.Organizations;
using TaskFlow.Domain.Common;

namespace TaskFlow.Tests.Api
{
    public class OrganizationsControllerTests
    {
        private readonly Mock<IOrganizationService> _mockService;
        private readonly OrganizationsController _sut;

        public OrganizationsControllerTests()
        {
            _mockService = new Mock<IOrganizationService>();
            _sut = new OrganizationsController(_mockService.Object);
        }

        [Fact]
        public async Task Get_ShouldReturnOkWithOrganizations()
        {
            // Arrange
            var organizations = new List<OrganizationSummaryDto>
            {
                new() { Id = 1, Name = "Org 1" },
                new() { Id = 2, Name = "Org 2" }
            };
            _mockService.Setup(x => x.GetMyOrganizationsAsync())
                .ReturnsAsync(Result.Success<IEnumerable<OrganizationSummaryDto>>(organizations));

            // Act
            var result = await _sut.Get();

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            okResult!.Value.Should().BeEquivalentTo(organizations);
        }

        [Fact]
        public async Task GetById_WhenOrgExists_ShouldReturnOkWithOrganization()
        {
            // Arrange
            var orgId = 1;
            var organization = new OrganizationDto { Id = orgId, Name = "Test Org" };
            _mockService.Setup(x => x.GetOrganizationByIdAsync(orgId))
                .ReturnsAsync(Result.Success(organization));

            // Act
            var result = await _sut.Get(orgId);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            okResult!.Value.Should().BeEquivalentTo(organization);
        }

        [Fact]
        public async Task GetById_WhenOrgNotFound_ShouldReturnNotFound()
        {
            // Arrange
            var orgId = 999;
            _mockService.Setup(x => x.GetOrganizationByIdAsync(orgId))
                .ReturnsAsync(Result.Failure<OrganizationDto>("Organization not found or access denied."));

            // Act
            var result = await _sut.Get(orgId);

            // Assert
            result.Result.Should().BeOfType<NotFoundObjectResult>();
            var notFoundResult = result.Result as NotFoundObjectResult;
            notFoundResult!.Value.Should().BeEquivalentTo(new { error = "Organization not found or access denied." });
        }

        [Fact]
        public async Task Post_WhenSuccessful_ShouldReturnCreatedAtAction()
        {
            // Arrange
            var request = new OrganizationRequest { Name = "New Org", Description = "Description" };
            var createdOrg = new OrganizationDto { Id = 1, Name = request.Name, Description = request.Description };
            _mockService.Setup(x => x.CreateOrganizationAsync(request.Name, request.Description))
                .ReturnsAsync(Result.Success(createdOrg));

            // Act
            var result = await _sut.Post(request);

            // Assert
            result.Should().BeOfType<CreatedAtActionResult>();
            var createdResult = result as CreatedAtActionResult;
            createdResult!.ActionName.Should().Be(nameof(_sut.Get));
            createdResult.RouteValues!["id"].Should().Be(createdOrg.Id);
            createdResult.Value.Should().BeEquivalentTo(createdOrg);
        }

        [Fact]
        public async Task Post_WhenFailed_ShouldReturnBadRequest()
        {
            // Arrange
            var request = new OrganizationRequest { Name = "New Org", Description = "Description" };
            _mockService.Setup(x => x.CreateOrganizationAsync(request.Name, request.Description))
                .ReturnsAsync(Result.Failure<OrganizationDto>("User not found."));

            // Act
            var result = await _sut.Post(request);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult!.Value.Should().BeEquivalentTo(new { error = "User not found." });
        }

        [Fact]
        public async Task Put_WhenSuccessful_ShouldReturnOk()
        {
            // Arrange
            var orgId = 1;
            var request = new OrganizationRequest { Name = "Updated Org", Description = "Updated Description" };
            var updatedOrg = new OrganizationDto { Id = orgId, Name = request.Name, Description = request.Description };
            _mockService.Setup(x => x.UpdateOrganizationAsync(orgId, request.Name, request.Description))
                .ReturnsAsync(Result.Success(updatedOrg));

            // Act
            var result = await _sut.Put(orgId, request);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult!.Value.Should().BeEquivalentTo(updatedOrg);
        }

        [Fact]
        public async Task Put_WhenNotAdmin_ShouldReturnForbid()
        {
            // Arrange
            var orgId = 1;
            var request = new OrganizationRequest { Name = "Updated Org", Description = "Updated Description" };
            _mockService.Setup(x => x.UpdateOrganizationAsync(orgId, request.Name, request.Description))
                .ReturnsAsync(Result.Failure<OrganizationDto>("You are not an admin of this organization."));

            // Act
            var result = await _sut.Put(orgId, request);

            // Assert
            result.Should().BeOfType<ForbidResult>();
        }

        [Fact]
        public async Task Put_WhenNotFound_ShouldReturnNotFound()
        {
            // Arrange
            var orgId = 999;
            var request = new OrganizationRequest { Name = "Updated Org", Description = "Updated Description" };
            _mockService.Setup(x => x.UpdateOrganizationAsync(orgId, request.Name, request.Description))
                .ReturnsAsync(Result.Failure<OrganizationDto>("Organization not found."));

            // Act
            var result = await _sut.Put(orgId, request);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task Delete_WhenSuccessful_ShouldReturnNoContent()
        {
            // Arrange
            var orgId = 1;
            _mockService.Setup(x => x.DeleteOrganizationAsync(orgId))
                .ReturnsAsync(Result.Success());

            // Act
            var result = await _sut.Delete(orgId);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task Delete_WhenNotAdmin_ShouldReturnForbid()
        {
            // Arrange
            var orgId = 1;
            _mockService.Setup(x => x.DeleteOrganizationAsync(orgId))
                .ReturnsAsync(Result.Failure("You are not an admin of this organization."));

            // Act
            var result = await _sut.Delete(orgId);

            // Assert
            result.Should().BeOfType<ForbidResult>();
        }

        [Fact]
        public async Task Delete_WhenNotFound_ShouldReturnNotFound()
        {
            // Arrange
            var orgId = 999;
            _mockService.Setup(x => x.DeleteOrganizationAsync(orgId))
                .ReturnsAsync(Result.Failure("Organization not found."));

            // Act
            var result = await _sut.Delete(orgId);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }
    }
}
