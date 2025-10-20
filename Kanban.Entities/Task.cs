

namespace Kanban.Entities
{
    public enum Priority
    {
        Low,
        Medium,
        High
    }
    public enum TaskStatus { ToDo, InProgress, Done }
    public class Task : IDomainObject
    {
            public Guid Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public DateTime DeadLine { get; set; }
            public Priority Priority { get; set; }
            public TaskStatus Status { get; set; }
           
    }
}
