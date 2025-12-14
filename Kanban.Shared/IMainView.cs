using Kanban.Entities;
using System;
using System.Collections.Generic;
using Task = Kanban.Entities.Task;
using TaskStatus = Kanban.Entities.TaskStatus;

namespace Kanban.Shared
{
    public class TaskEventArgs : EventArgs
    {
        public Guid? Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DeadLine { get; set; }
        public Priority Priority { get; set; }
    }

    public class TaskStatusEventArgs : EventArgs
    {
        public Guid Id { get; set; }
        public TaskStatus NewStatus { get; set; }
    }

    public class TaskIdEventArgs : EventArgs
    {
        public Guid Id { get; set; }
    }
    public class TaskPriorityEventArgs : EventArgs
    {
        public Guid Id { get; set; }
        public Priority NewPriority { get; set; }
    }

    public interface IMainView
    {
        /// <summary>
        /// Вызывается Презентером, когда нужно обновить список задач.
        /// </summary>
        /// <param name="tasks">Актуальный список задач из базы данных.</param>
        void SetTaskList(IEnumerable<Task> tasks);

        /// <summary>
        /// Вызывается Презентером, чтобы сообщить пользователю об ошибке или успехе операции.
        /// </summary>
        /// <param name="message">Текст сообщения.</param>
        void ShowMessage(string message);

        /// <summary>
        /// Срабатывает, когда вьюха полностью загрузилось и готова к работе.Используется Презентером для начальной загрузки и отображения списка задач.
        /// </summary>
        event EventHandler ViewReady;

        /// <summary>
        /// Срабатывает, когда пользователь ввел данные новой задачи и подтвердил создание.Передает данные полученные презентеру.
        /// </summary>
        event EventHandler<TaskEventArgs> CreateRequest;

        /// <summary>
        /// Срабатывает, когда пользователь отредактировал существующую задачу и сохранил изменения. Передает обновленные данные и ID задачи презентеру.
        /// </summary>
        event EventHandler<TaskEventArgs> UpdateRequest;

        /// <summary>
        /// Срабатывает, когда пользователь подствердил удаление задачи. Передает ID удаляемой задачи.
        /// </summary>
        event EventHandler<TaskIdEventArgs> DeleteRequest;

        /// <summary>
        /// Сообщает презентеру, что задачу нужно перевести в новый статус. Передает ID задачи и новый статус.
        /// </summary>
        event EventHandler<TaskStatusEventArgs> ChangeStatusRequest;

        /// <summary>
        /// Сообщает презентеру, что задачу нужно перевести в новый статус.Передает ID задачи и новый уровень приоритета.
        /// </summary>
        event EventHandler<TaskPriorityEventArgs> ChangePriorityRequest;
    }
}