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
        void Add(T entity);
        void Update(T entity);
        void Delete(Guid id);
        T GetById(Guid id);
        IEnumerable<T> GetAll();
    }
}
