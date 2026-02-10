using FluentAssertions;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Specifications;

namespace TaskFlow.Tests.Domain
{
    public class SpecificationTests
    {
        [Fact]
        public void OrganizationWithDetailsSpecification_WithId_ShouldSetCriteriaAndIncludes()
        {
            // Arrange
            var orgId = 1;

            // Act
            var spec = new OrganizationWithDetailsSpecification(orgId);

            // Assert
            spec.Criteria.Should().NotBeNull();
            spec.Includes.Should().HaveCount(2);
            spec.IncludeStrings.Should().HaveCount(2);
            spec.IncludeStrings.Should().Contain("Memberships.User");
            spec.IncludeStrings.Should().Contain("Memberships.OrganizationRoles");
        }

        [Fact]
        public void OrganizationWithDetailsSpecification_WithoutId_ShouldNotSetCriteria()
        {
            // Act
            var spec = new OrganizationWithDetailsSpecification();

            // Assert
            spec.Criteria.Should().BeNull();
            spec.Includes.Should().HaveCount(2);
        }

        [Fact]
        public void OrganizationWithDetailsSpecification_Criteria_ShouldMatchOrganizationId()
        {
            // Arrange
            var orgId = 5;
            var spec = new OrganizationWithDetailsSpecification(orgId);
            var organizations = new List<Organization>
            {
                new() { Id = 1, Name = "Org 1" },
                new() { Id = 5, Name = "Org 5" },
                new() { Id = 10, Name = "Org 10" }
            };

            // Act
            var compiledCriteria = spec.Criteria!.Compile();
            var result = organizations.Where(compiledCriteria).ToList();

            // Assert
            result.Should().HaveCount(1);
            result.First().Id.Should().Be(orgId);
        }

        [Fact]
        public void BaseSpecification_ApplyPaging_ShouldSetPagingProperties()
        {
            // Arrange
            var spec = new TestSpecification();

            // Act
            spec.TestApplyPaging(10, 5);

            // Assert
            spec.IsPagingEnabled.Should().BeTrue();
            spec.Skip.Should().Be(10);
            spec.Take.Should().Be(5);
        }

        [Fact]
        public void BaseSpecification_ApplyOrderBy_ShouldSetOrderByProperty()
        {
            // Arrange
            var spec = new TestSpecification();

            // Act
            spec.TestApplyOrderBy(x => x.Name);

            // Assert
            spec.OrderBy.Should().NotBeNull();
        }

        [Fact]
        public void BaseSpecification_ApplyOrderByDescending_ShouldSetOrderByDescendingProperty()
        {
            // Arrange
            var spec = new TestSpecification();

            // Act
            spec.TestApplyOrderByDescending(x => x.Name);

            // Assert
            spec.OrderByDescending.Should().NotBeNull();
        }

        private class TestSpecification : BaseSpecification<Organization>
        {
            public TestSpecification() : base() { }

            public void TestApplyPaging(int skip, int take)
            {
                ApplyPaging(skip, take);
            }

            public void TestApplyOrderBy(System.Linq.Expressions.Expression<Func<Organization, object>> orderByExpression)
            {
                ApplyOrderBy(orderByExpression);
            }

            public void TestApplyOrderByDescending(System.Linq.Expressions.Expression<Func<Organization, object>> orderByDescExpression)
            {
                ApplyOrderByDescending(orderByDescExpression);
            }
        }
    }
}
