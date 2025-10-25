using System;
using Kanban.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    /// <summary>
    /// Интерфейс, обеспечивающий основные CRUD операции
    /// </summary>
    public interface IRepository<T> where T : IDomainObject
    {
        /// <summary>
        /// Добавляет новую таску в базу данных.
        /// </summary>
        void Add(T entity);

        /// <summary>
        /// Обновляет существующую таску в базе данных.
        /// </summary>
        /// <param name="entity">Таска с обновленными данными.</param>
        void Update(T entity);

        /// <summary>
        /// Удаляет таску из базы данных по ее идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор таски для удаления.</param>
        void Delete(Guid id);


        /// <summary>
        /// Получает таску по ее уникальному идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>Найденная таска или null, если она не найдена</returns>
        T GetById(Guid id);


        /// <summary>
        /// Получает все таски из базы данных.
        /// </summary>
        /// <returns>Коллекция всех тасок</returns>
        IEnumerable<T> GetAll();
    }
}
