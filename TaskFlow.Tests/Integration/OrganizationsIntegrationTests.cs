using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.DTOs.Auth;
using TaskFlow.Application.DTOs.Organizations;
using TaskFlow.Infrastructure.Data;

namespace TaskFlow.Tests.Integration
{
    public class OrganizationsIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public OrganizationsIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Testing");
                builder.ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    services.AddDbContext<AppDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("TestDatabase");
                    });
                });
            });
        }

        [Fact]
        public async Task GetOrganizations_WithoutAuth_ShouldReturnUnauthorized()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/organizations");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task CreateOrganization_WithValidData_ShouldReturnCreated()
        {
            // Arrange
            var client = _factory.CreateClient();
            var token = await GetAuthToken(client);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = new OrganizationRequest
            {
                Name = "Test Organization",
                Description = "Test Description"
            };

            // Act
            var response = await client.PostAsJsonAsync("/api/organizations", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var organization = await response.Content.ReadFromJsonAsync<OrganizationDto>();
            organization.Should().NotBeNull();
            organization!.Name.Should().Be(request.Name);
            organization.Description.Should().Be(request.Description);
        }

        [Fact]
        public async Task GetOrganizationById_WhenExists_ShouldReturnOk()
        {
            // Arrange
            var client = _factory.CreateClient();
            var token = await GetAuthToken(client);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var createRequest = new OrganizationRequest
            {
                Name = "Test Organization for Get",
                Description = "Test Description"
            };
            var createResponse = await client.PostAsJsonAsync("/api/organizations", createRequest);
            var createdOrg = await createResponse.Content.ReadFromJsonAsync<OrganizationDto>();

            // Act
            var response = await client.GetAsync($"/api/organizations/{createdOrg!.Id}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var organization = await response.Content.ReadFromJsonAsync<OrganizationDto>();
            organization.Should().NotBeNull();
            organization!.Id.Should().Be(createdOrg.Id);
            organization.Name.Should().Be(createRequest.Name);
        }

        [Fact]
        public async Task UpdateOrganization_WhenUserIsAdmin_ShouldReturnOk()
        {
            // Arrange
            var client = _factory.CreateClient();
            var token = await GetAuthToken(client);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var createRequest = new OrganizationRequest
            {
                Name = "Original Name",
                Description = "Original Description"
            };
            var createResponse = await client.PostAsJsonAsync("/api/organizations", createRequest);
            var createdOrg = await createResponse.Content.ReadFromJsonAsync<OrganizationDto>();

            var updateRequest = new OrganizationRequest
            {
                Name = "Updated Name",
                Description = "Updated Description"
            };

            // Act
            var response = await client.PutAsJsonAsync($"/api/organizations/{createdOrg!.Id}", updateRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var updatedOrg = await response.Content.ReadFromJsonAsync<OrganizationDto>();
            updatedOrg.Should().NotBeNull();
            updatedOrg!.Name.Should().Be(updateRequest.Name);
            updatedOrg.Description.Should().Be(updateRequest.Description);
        }

        [Fact]
        public async Task DeleteOrganization_WhenUserIsAdmin_ShouldReturnNoContent()
        {
            // Arrange
            var client = _factory.CreateClient();
            var token = await GetAuthToken(client);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var createRequest = new OrganizationRequest
            {
                Name = "Organization to Delete",
                Description = "Will be deleted"
            };
            var createResponse = await client.PostAsJsonAsync("/api/organizations", createRequest);
            var createdOrg = await createResponse.Content.ReadFromJsonAsync<OrganizationDto>();

            // Act
            var response = await client.DeleteAsync($"/api/organizations/{createdOrg!.Id}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var getResponse = await client.GetAsync($"/api/organizations/{createdOrg.Id}");
            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        private async Task<string> GetAuthToken(HttpClient client)
        {
            var registerRequest = new RegisterRequest
            {
                Name = "Test User",
                Email = $"testuser{Guid.NewGuid()}@test.com",
                Password = "Test123!"
            };

            var response = await client.PostAsJsonAsync("/api/users/register", registerRequest);
            var authResponse = await response.Content.ReadFromJsonAsync<AuthResponseDto>();
            return authResponse!.Token;
        }
    }
}
