using DataAccessLayer;
using Kanban.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Task = Kanban.Entities.Task;
using TaskStatus = Kanban.Entities.TaskStatus;

namespace Kanban.BL
{
    public class LogicCrud : ILogicCrud
    {
        public IRepository<Task> Repository { get; private set; }

        /// <summary>
        /// Инициализирует новый экземпляр класса Logic с предоставленной зависимостью репозитория.
        /// </summary>
        /// <param name="repository">Экземпляр репозитория для работы с данными.</param>
        /// <exception cref="ArgumentNullException">Вызывается, если репозиторий не был предоставлен (null).</exception>
        public LogicCrud(IRepository<Task> repository)
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

        public void UpdateTask(Guid id, string title, string description, DateTime dueDate, Priority priority)
        {
            var task = Repository.GetById(id);

            if (task != null)
            {
                ApplyTaskUpdates(task, title, description, dueDate, priority);

                Repository.Update(task);
            }
        }

        private void ApplyTaskUpdates(Task task, string title, string description, DateTime dueDate, Priority priority)
        {
            task.Title = title;
            task.Description = description;
            task.DeadLine = dueDate;
            task.Priority = priority;
        }
    }
}