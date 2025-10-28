using Dapper;
using DataAccessLayer;
using Kanban.Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using Task = Kanban.Entities.Task;

namespace DataAccessLayer
{
    /// <summary>
    /// Класс, обрабатывающий сценарий работу с Dapperом
    /// </summary>
    public class DapperRepository : IRepository <Task> 
    {
        private readonly string _connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=KanbanDB_V2;Trusted_Connection=True;";

        private readonly string _tableName = typeof(Task).Name + "s";

        public void Add(Task entity)
        {
            var sql = $"INSERT INTO {_tableName} (Id, Title, Description, DeadLine, Status, Priority) VALUES (@Id, @Title, @Description, @DeadLine, @Status, @Priority);";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(sql, entity);
            }
        }

        public void Delete(Guid id)
        {
            var sql = $"DELETE FROM {_tableName} WHERE Id = @Id;";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(sql, new { Id = id });
            }
        }

        public IEnumerable<Task> GetAll()
        {
            var sql = $"SELECT * FROM {_tableName};";

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<Task>(sql).ToList();
            }
        }

        public Task GetById(Guid id)
        {
            var sql = $"SELECT * FROM {_tableName} WHERE Id = @Id;";

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.QueryFirstOrDefault<Task>(sql, new { Id = id });
            }
        }

        public void Update(Task entity)
        {
            var sql = $"UPDATE {_tableName} SET Title = @Title, Description = @Description, DeadLine = @DeadLine, Status = @Status WHERE Id = @Id;";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(sql, entity);
            }
        }
    }
}