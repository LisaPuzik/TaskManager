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

        public void Add(Task entity)
        {
            _context.Tasks.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var entity = GetById(id);
            if (entity != null)
            {
                _context.Tasks.Remove(entity);
                _context.SaveChanges();
            }
        }

        public IEnumerable<Task> GetAll()
        {
            return _context.Tasks.ToList();
        }

        public Task GetById(Guid id)
        {
            return _context.Tasks.FirstOrDefault(e => e.Id == id);
        }

        public void Update(Task entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}