using TaskFlow.Application.Abstractions;
using TaskFlow.Domain.Entities;
using TaskFlow.Infrastructure.Data;

namespace TaskFlow.Infrastructure.Repository
{
    public class ProjectRepository(AppDbContext dbContext) : Repository<Project>(dbContext), IProjectRepository
    {
        private readonly AppDbContext _dbContext = dbContext;
        public void Attach(Project project)
        {
            _dbContext.Attach(project);
            _dbContext.Entry(project).Property(t => t.Name).IsModified = true;
            _dbContext.Entry(project).Property(t => t.Description).IsModified = true;
        }
    }
}
