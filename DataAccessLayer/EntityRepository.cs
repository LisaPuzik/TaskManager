using DataAccessLayer;
using Kanban.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Task = Kanban.Entities.Task;

namespace DataAccessLayer
{
    /// <summary>
    /// Класс, обрабатывающий сценарий работу с EF
    /// </summary>
    public class EntityRepository : IRepository<Task>
    {
        private readonly KanbanDbContext _context;

        public EntityRepository()
        {
            _context = new KanbanDbContext();
        }

        /// <summary>
        /// Добавляет новую таску в базу данных.
        /// </summary>
        public void Add(Task entity)
        {
            _context.Tasks.Add(entity);
            _context.SaveChanges();
        }

        /// <summary>
        /// Удаляет таску из базы данных по ее идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор таски для удаления.</param>
        public void Delete(Guid id)
        {
            var entity = GetById(id);
            if (entity != null)
            {
                _context.Tasks.Remove(entity);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Получает все таски из базы данных.
        /// </summary>
        /// <returns>Коллекция всех тасок</returns>
        public IEnumerable<Task> GetAll()
        {
            return _context.Tasks.ToList();
        }

        /// <summary>
        /// Получает таску по ее уникальному идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>Найденная таска или null, если она не найдена</returns>
        public Task GetById(Guid id)
        {
            return _context.Tasks.FirstOrDefault(e => e.Id == id);
        }

        /// <summary>
        /// Обновляет существующую таску в базе данных.
        /// </summary>
        /// <param name="entity">Таска с обновленными данными.</param>
        public void Update(Task entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}