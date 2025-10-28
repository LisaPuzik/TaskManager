using System;
using Kanban.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    /// <summary>
    /// Определяет контракт для чтения сущностей из хранилища.
    /// </summary>
    public interface IReadRepository<T> where T : IDomainObject
    {
        /// <summary>
        /// Получает сущность по ее уникальному идентификатору.
        /// </summary>
        T GetById(Guid id);

        /// <summary>
        /// Получает все сущности данного типа из хранилища.
        /// </summary>
        IEnumerable<T> GetAll();
    }

    /// <summary>
    /// Определяет контракт для записи/изменения сущностей в хранилище.
    /// </summary>
    public interface IWriteRepository<T> where T : IDomainObject
    {
        /// <summary>
        /// Сохраняет новую сущность в хранилище.
        /// </summary>
        void Add(T entity);

        /// <summary>
        /// Обновляет существующую сущность в хранилище.
        /// </summary>
        void Update(T entity);

        /// <summary>
        /// Удаляет сущность из хранилища по ее идентификатору.
        /// </summary>
        void Delete(Guid id);
    }

    /// <summary>
    /// Объединяет контракты чтения и записи для реализации полного CRUD-репозитория.
    /// </summary>
    public interface IRepository<T> : IReadRepository<T>, IWriteRepository<T> where T : IDomainObject
    {
    }
}
