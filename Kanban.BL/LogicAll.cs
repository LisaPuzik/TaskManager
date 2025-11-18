using Kanban.Entities;
using System;
using System.Collections.Generic;
using Task = Kanban.Entities.Task;
using TaskStatus = Kanban.Entities.TaskStatus;

namespace Kanban.BL
{
    /// <summary>
    /// Реализация Фасада.
    /// Реализует объединенный интерфейс ILogicAll, делегируя вызовы специализированным классам LogicCrud и LogicBL.
    /// </summary>
    public class LogicAll : ILogicAll
    {
        private readonly ILogicCrud _crudLogic;
        private readonly ILogicBL _businessLogic;

        /// <summary>
        /// Конструктор для внедрения зависимостей.
        /// </summary>
        /// <param name="crudLogic">Экземпляр логики для CRUD-операций.</param>
        /// <param name="businessLogic">Экземпляр бизнес-логики.</param>
        public LogicAll(ILogicCrud crudLogic, ILogicBL businessLogic)
        {
            _crudLogic = crudLogic;
            _businessLogic = businessLogic;
        }

        public void AddTask(string title, string description, DateTime dueDate, Priority priority)
        {
            _crudLogic.AddTask(title, description, dueDate, priority);
        }

        public void DeleteTask(Guid id)
        {
            _crudLogic.DeleteTask(id);
        }

        public List<Task> GetAllTasks()
        {
            return _crudLogic.GetAllTasks();
        }

        public Task GetTaskById(Guid id)
        {
            return _crudLogic.GetTaskById(id);
        }

        public void UpdateTask(Guid id, string title, string description, DateTime dueDate, Priority priority)
        {
            _crudLogic.UpdateTask(id, title, description, dueDate, priority);
        }

        public void ChangePriority(Guid id, Priority newPriority)
        {
            _businessLogic.ChangePriority(id, newPriority);
        }

        public void ChangeTaskStatus(Guid id, TaskStatus newStatus)
        {
            _businessLogic.ChangeTaskStatus(id, newStatus);
        }

    }
}