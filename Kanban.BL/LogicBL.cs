using DataAccessLayer;
using Kanban.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = Kanban.Entities.Task;
using TaskStatus = Kanban.Entities.TaskStatus;

namespace Kanban.BL
{
    public class LogicBL : ILogicBL
    {
        public IRepository<Task> Repository { get; private set; }
        public LogicBL(IRepository<Task> repository)
        {
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository),
                    "Ошибка: В конструктор класса Logic не был передан репозиторий.");
            }
            this.Repository = repository;
        }

        public void ChangeTaskStatus(Guid id, TaskStatus newStatus)
        {
            var task = Repository.GetById(id);
            if (task != null)
            {
                task.Status = newStatus;
                Repository.Update(task);
            }
        }

        public void ChangePriority(Guid id, Priority newPriority)
        {
            var task = Repository.GetById(id);
            if (task != null)
            {
                task.Priority = newPriority;
                Repository.Update(task);
            }
        }
    }
}
