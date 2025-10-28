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
    /// Класс бизнес-логики. Только управляет операциями над сущностями.
    /// </summary>
    public class Logic
    {
        public IRepository<Task> Repository { get; set; }

        public Logic(IRepository<Task> repository)
        {
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository),
                    "Ошибка: В конструктор класса Logic не был передан репозиторий.");
            }
            this.Repository = repository;
        }


        public List<Task> GetAllTasks()
        {
            return Repository.GetAll().ToList();
        }

        public Task GetTaskById(Guid id)
        {
            return Repository.GetById(id);
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

            Repository.Add(task);
        }

        public void DeleteTask(Guid id)
        {
            Repository.Delete(id);
        }

        /// <summary>
        /// Обновляет ВСЕ поля задачи.
        /// </summary>
        public void UpdateTask(Guid id, string title, string description, DateTime dueDate, Priority priority)
        {
            var task = Repository.GetById(id);

            if (task != null)
            {
                task.Title = title;
                task.Description = description;
                task.DeadLine = dueDate;
                task.Priority = priority;

                Repository.Update(task);
            }
        }

        /// <summary>
        /// Изменяет статус задачи.
        /// </summary>
        public void ChangeTaskStatus(Guid id, TaskStatus newStatus)
        {
            var task = Repository.GetById(id);

            if (task != null)
            {
                task.Status = newStatus;
                Repository.Update(task);
            }
        }
    }
}