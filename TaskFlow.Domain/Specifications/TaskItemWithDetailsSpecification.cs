using TaskFlow.Domain.Entities;

namespace TaskFlow.Domain.Specifications
{
    public class TaskItemWithDetailsSpecification : BaseSpecification<TaskItem>
    {
        public TaskItemWithDetailsSpecification(int taskId)
            : base(t => t.Id == taskId)
        {
            AddInclude(t => t.Project);
            AddInclude(t => t.User);
            AddInclude(t => t.Comments);
            AddInclude("Project.Organization");
            AddInclude("Comments.User");
        }

        public TaskItemWithDetailsSpecification()
            : base()
        {
            AddInclude(t => t.Project);
            AddInclude(t => t.User);
        }
    }
}
