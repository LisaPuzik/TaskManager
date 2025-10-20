using DataAccessLayer;
using Kanban.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Task = Kanban.Entities.Task;
using TaskStatus = Kanban.Entities.TaskStatus;

namespace Kanban.BL
{
    /// <summary>
    /// Перечисление для выбора типа репозитория при инициализации Logic
    /// </summary>
    public enum RepositoryType
    {
        EF,
        Dapper
    }

    /// <summary>
    /// Класс бизнес-логики. Только управляет операциями над сущностями.
    /// </summary>
    public class Logic
    {
        private readonly IRepository<Task> _repository;

        public Logic(RepositoryType type)
        {
            if (type == RepositoryType.EF)
            {
                _repository = new EntityRepository<Task>();
            }
            else
            {
                _repository = new DapperRepository<Task>();
            }
        }


        public List<Task> GetAllTasks()
        {
            return _repository.GetAll().ToList();
        }

        public Task GetTaskById(Guid id)
        {
            return _repository.GetById(id);
        }

        public void AddTask(string title, string description, DateTime dueDate, Priority priority)
        {
            var task = new Task
            {
                Id = Guid.NewGuid(),
                Title = title,
                Description = description,
                DeadLine = dueDate,
                Priority = priority,
                Status = TaskStatus.ToDo 
            };

            _repository.Add(task);
        }

        public void DeleteTask(Guid id)
        {
            _repository.Delete(id);
        }

        /// <summary>
        /// Обновляет ВСЕ поля задачи.
        /// </summary>
        public void UpdateTask(Guid id, string title, string description, DateTime dueDate, Priority priority)
        {
            var task = _repository.GetById(id);

            if (task != null)
            {
                task.Title = title;
                task.Description = description;
                task.DeadLine = dueDate;
                task.Priority = priority;

                _repository.Update(task);
            }
        }

        /// <summary>
        /// Изменяет статус задачи.
        /// </summary>
        public void ChangeTaskStatus(Guid id, TaskStatus newStatus)
        {
            var task = _repository.GetById(id);

            if (task != null)
            {
                task.Status = newStatus;
                _repository.Update(task);
            }
        }
    }
}