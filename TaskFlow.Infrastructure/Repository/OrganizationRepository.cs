using TaskFlow.Application.Abstractions;
using TaskFlow.Domain.Entities;
using TaskFlow.Infrastructure.Data;

namespace TaskFlow.Infrastructure.Repository
{
    public class OrganizationRepository(AppDbContext dbContext) : Repository<Organization>(dbContext), IOrganizationRepository
    {
        private readonly AppDbContext _dbContext = dbContext;
        public void Attach(Organization organization)
        {
            _dbContext.Attach(organization);
            _dbContext.Entry(organization).Property(t => t.Name).IsModified = true;
            _dbContext.Entry(organization).Property(t => t.Description).IsModified = true;
        }
    }
}
