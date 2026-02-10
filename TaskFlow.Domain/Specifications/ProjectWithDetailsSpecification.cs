using TaskFlow.Domain.Entities;

namespace TaskFlow.Domain.Specifications
{
    public class ProjectWithDetailsSpecification : BaseSpecification<Project>
    {
        public ProjectWithDetailsSpecification(int projectId)
            : base(p => p.Id == projectId)
        {
            AddInclude(p => p.Organization);
            AddInclude(p => p.TaskItems);
            AddInclude("TaskItems.User");
        }

        public ProjectWithDetailsSpecification()
            : base()
        {
            AddInclude(p => p.Organization);
            AddInclude(p => p.TaskItems);
        }
    }
}
