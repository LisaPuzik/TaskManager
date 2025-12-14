using Kanban.Entities;
using Task = Kanban.Entities.Task;
using TaskStatus = Kanban.Entities.TaskStatus;

namespace Kanban.BL
{
    public class LogicAll : ILogicAll
    {
        private readonly ILogicCrud _crudLogic;
        private readonly ILogicBL _businessLogic;

        public event EventHandler ModelUpdated;

        public LogicAll(ILogicCrud crudLogic, ILogicBL businessLogic)
        {
            _crudLogic = crudLogic;
            _businessLogic = businessLogic;
        }

        private void Notify()
        {
            ModelUpdated?.Invoke(this, EventArgs.Empty);
        }

        public void AddTask(string title, string description, DateTime dueDate, Priority priority)
        {
            _crudLogic.AddTask(title, description, dueDate, priority);
            Notify();
        }

        public void DeleteTask(Guid id)
        {
            _crudLogic.DeleteTask(id);
            Notify();
        }

        public void UpdateTask(Guid id, string title, string description, DateTime dueDate, Priority priority)
        {
            _crudLogic.UpdateTask(id, title, description, dueDate, priority);
            Notify();
        }

        public void ChangePriority(Guid id, Priority newPriority)
        {
            _businessLogic.ChangePriority(id, newPriority);
            Notify();
        }

        public void ChangeTaskStatus(Guid id, TaskStatus newStatus)
        {
            _businessLogic.ChangeTaskStatus(id, newStatus);
            Notify();
        }

        public List<Task> GetAllTasks() => _crudLogic.GetAllTasks();
        public Task GetTaskById(Guid id) => _crudLogic.GetTaskById(id);
    }
}