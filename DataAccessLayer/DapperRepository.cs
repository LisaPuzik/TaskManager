using Dapper;
using DataAccessLayer;
using Kanban.Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer
{
    /// <summary>
    /// Класс, обрабатывающий сценарий работу с Dapperом
    /// </summary>
    public class DapperRepository<T> : IRepository<T> where T : IDomainObject
    {
        private readonly string _connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=KanbanDB_V2;Trusted_Connection=True;";

        private readonly string _tableName = typeof(T).Name + "s";

        /// <summary>
        /// Добавляет новую таску в базу данных.
        /// </summary>
        public void Add(T entity)
        {
            var sql = $"INSERT INTO {_tableName} (Id, Title, Description, DeadLine, Status, Priority) VALUES (@Id, @Title, @Description, @DeadLine, @Status, @Priority);";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(sql, entity);
            }
        }

        /// <summary>
        /// Удаляет таску из базы данных по ее идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор таски для удаления.</param>
        public void Delete(Guid id)
        {
            var sql = $"DELETE FROM {_tableName} WHERE Id = @Id;";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(sql, new { Id = id });
            }
        }

        /// <summary>
        /// Получает все таски из базы данных.
        /// </summary>
        /// <returns>Коллекция всех тасок</returns>
        public IEnumerable<T> GetAll()
        {
            var sql = $"SELECT * FROM {_tableName};";

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<T>(sql).ToList();
            }
        }

        /// <summary>
        /// Получает таску по ее уникальному идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>Найденная таска или null, если она не найдена</returns>
        public T GetById(Guid id)
        {
            var sql = $"SELECT * FROM {_tableName} WHERE Id = @Id;";

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.QueryFirstOrDefault<T>(sql, new { Id = id });
            }
        }

        /// <summary>
        /// Обновляет существующую таску в базе данных.
        /// </summary>
        /// <param name="entity">Таска с обновленными данными.</param>
        public void Update(T entity)
        {
            var sql = $"UPDATE {_tableName} SET Title = @Title, Description = @Description, DeadLine = @DeadLine, Status = @Status WHERE Id = @Id;";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(sql, entity);
            }
        }
    }
}