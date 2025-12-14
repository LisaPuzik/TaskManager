using Kanban.Entities;
using System;
using System.Collections.Generic;
using Task = Kanban.Entities.Task;
using TaskStatus = Kanban.Entities.TaskStatus;
namespace Kanban.BL
{
    /// <summary>
    /// Определяет контракт для слоя бизнес-логики (Фасад),
    /// который инкапсулирует и оркестрирует все операции, связанные с задачами.
    /// </summary>
    public interface ILogicCrud
    {
        /// <summary>
        /// Получает полный список всех задач.
        /// </summary>
        /// <returns>Коллекция объектов Task.</returns>
        List<Task> GetAllTasks();


        /// <summary>
        /// Осуществляет поиск и возвращает задачу по ее уникальному идентификатору.
        /// </summary>
        /// <param name="id">Уникальный идентификатор задачи.</param>
        /// <returns>Найденный объект Task или null, если задача не найдена.</returns>
        Task GetTaskById(Guid id);

        /// <summary>
        /// Создает новую задачу на основе переданных данных, присваивает ей
        /// начальный статус и сохраняет в хранилище.
        /// </summary>
        /// <param name="title">Название задачи.</param>
        /// <param name="description">Подробное описание задачи.</param>
        /// <param name="dueDate">Срок выполнения задачи.</param>
        /// <param name="priority">Приоритет задачи.</param>
        void AddTask(string title, string description, DateTime dueDate, Priority priority);

        /// <summary>
        /// Удаляет задачу из хранилища по ее уникальному идентификатору.
        /// </summary>
        /// <param name="id">Уникальный идентификатор задачи для удаления.</param>
        void DeleteTask(Guid id);

        /// <summary>
        /// Находит задачу по идентификатору и полностью обновляет ее данные
        /// на основе переданных параметров.
        /// </summary>
        /// <param name="id">Уникальный идентификатор обновляемой задачи.</param>
        /// <param name="title">Новое название задачи.</param>
        /// <param name="description">Новое описание задачи.</param>
        /// <param name="dueDate">Новый срок выполнения.</param>
        /// <param name="priority">Новый приоритет.</param>
        void UpdateTask(Guid id, string title, string description, DateTime dueDate, Priority priority);
    }
    public interface ILogicBL
    {
        /// <summary>
        /// Находит задачу по идентификатору и изменяет ее статус.
        /// </summary>
        /// <param name="id">Уникальный идентификатор задачи.</param>
        /// <param name="newStatus">Новый статус для задачи.</param>
        void ChangeTaskStatus(Guid id, TaskStatus newStatus);

        /// <summary>
        /// Изменяет приоритет задачи.
        /// </summary>
        /// <param name="id">Уникальный идентификатор.</param>
        /// <param name="newPriority">Новый приоритет задачи.</param>
        void ChangePriority(Guid id, Priority newPriority);
    }
    public interface ILogicAll : ILogicBL, ILogicCrud
    {
        /// <summary>
        /// Событие, которое срабатывает при любом изменении данных (добавление, удаление, редактирование).
        /// </summary>
        event EventHandler ModelUpdated;
    }
}