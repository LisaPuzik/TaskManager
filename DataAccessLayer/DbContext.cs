using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kanban.Entities;
using Microsoft.EntityFrameworkCore;
using Task = Kanban.Entities.Task;

namespace DataAccessLayer
{
    /// <summary>
    /// Представляет контекст базы данных.
    /// </summary>
    public class KanbanDbContext : DbContext
    {
        public DbSet <Task> Tasks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=KanbanDB_V2;Trusted_Connection=True;");
        }
    }
}
