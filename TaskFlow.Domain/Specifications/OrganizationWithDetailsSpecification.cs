using TaskFlow.Domain.Entities;

namespace TaskFlow.Domain.Specifications
{
    public class OrganizationWithDetailsSpecification : BaseSpecification<Organization>
    {
        public OrganizationWithDetailsSpecification(int organizationId)
            : base(o => o.Id == organizationId)
        {
            AddInclude(o => o.Projects);
            AddInclude(o => o.Memberships);
            AddInclude("Memberships.User");
            AddInclude("Memberships.OrganizationRoles");
        }

        public OrganizationWithDetailsSpecification()
            : base()
        {
            AddInclude(o => o.Projects);
            AddInclude(o => o.Memberships);
        }
    }
}
