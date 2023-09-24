using Dapper;
using Microsoft.Extensions.Options;
using NewsManagement.Domain.Interfaces;
using NewsManagement.Domain.Models;
using NewsManagement.Domain.Settings;
using System.Data.SqlClient;

namespace NewsManagement.Persistence
{
    public class NewsQueryRepository : ITodoQueryRepository
    {
        private readonly string _connectionString;

        public NewsQueryRepository(IOptions<ConnectionStrings> connectionStrings)
        {
            _connectionString = connectionStrings.Value.Todo;
        }

        public async Task<Todo> Get(Guid id, CancellationToken cancellationToken)
        {
            await using var connection = new SqlConnection(_connectionString);

            await connection.OpenAsync(cancellationToken);

            var result = await connection.QuerySingleOrDefaultAsync<Todo>(
                @"SELECT * FROM Todos WHERE Id = @id",
                new { id }
            );

            return result;
        }

        public async Task<List<Todo>> GetAll(CancellationToken cancellationToken)
        {
            await using var connection = new SqlConnection(_connectionString);

            await connection.OpenAsync(cancellationToken);

            var results = await connection.QueryAsync<Todo>(
                @"SELECT * FROM Todos"
            );

            return results.ToList();
        }

        public async Task<Guid> Create(Todo todo, CancellationToken cancellationToken)
        {
            await using var connection = new SqlConnection(_connectionString);

            await connection.OpenAsync(cancellationToken);

            await connection.ExecuteAsync(
                @$"INSERT INTO Todos (Id, Name, Status) VALUES (@Id, @Name, 0)",
                new { todo.Id, todo.Name }
            );

            return todo.Id;
        }
    }
}