using Kanban.Model;

namespace Model
{
    public enum TaskStatus { ToDo, InProgress, Done }
    public class Task
    {
            public Guid Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public DateTime DeadLine { get; set; }
            public TaskStatus Status { get; set; }
            public Priority Priority { get; set; }
    }
}
