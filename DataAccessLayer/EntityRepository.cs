using DataAccessLayer;
using Kanban.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer
{
    /// <summary>
    /// Класс, обрабатывающий сценарий работу с EF
    /// </summary>
    public class EntityRepository<T> : IRepository<T> where T : class, IDomainObject
    {
        private readonly KanbanDbContext _context;

        public EntityRepository()
        {
            _context = new KanbanDbContext();
        }

        /// <summary>
        /// Добавляет новую таску в базу данных.
        /// </summary>
        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
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
                _context.Set<T>().Remove(entity);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Получает все таски из базы данных.
        /// </summary>
        /// <returns>Коллекция всех тасок</returns>
        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        /// <summary>
        /// Получает таску по ее уникальному идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>Найденная таска или null, если она не найдена</returns>
        public T GetById(Guid id)
        {
            return _context.Set<T>().FirstOrDefault(e => e.Id == id);
        }

        /// <summary>
        /// Обновляет существующую таску в базе данных.
        /// </summary>
        /// <param name="entity">Таска с обновленными данными.</param>
        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}