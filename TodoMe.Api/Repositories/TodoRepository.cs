using System.Data;
using Dapper;
using Microsoft.Data.Sqlite;
using TodoMe.Api.Entities;

namespace TodoMe.Api.Repositories;

// TODO: Get todos by user id
internal sealed class TodoRepository : ITodoRepository
{
    private const string ConnectionString = "Data Source=db.sqlite;Pooling=True;";

    public async Task<IEnumerable<Todo>> GetAllAsync(UserId userId)
    {
        await using var db = new SqliteConnection(ConnectionString);

        const string sql = @"SELECT 
                            t.id, t.title, t.is_complete 
                            FROM Todo t 
                            WHERE t.user_id=@User_Id;";

        return await db.QueryAsync<Todo>(sql, new
        {
            User_Id = userId.Value
        });
    }

    public async Task<Todo?> GetByIdAsync(int id, UserId userId)
    {
        await using var db = new SqliteConnection(ConnectionString);

        const string sql = @"SELECT 
                            t.id, t.title, t.is_complete 
                            FROM Todo t 
                            WHERE t.id=@Id
                            AND t.user_id=@User_Id;";

        return await db.QueryFirstOrDefaultAsync<Todo>(sql, new
        {
            Id = id,
            User_Id = userId.Value
        });
    }

    public async Task AddAsync(Todo todo, UserId userId)
    {
        await using var db = new SqliteConnection(ConnectionString);

        const string sql = @"INSERT INTO Todo (title, is_complete, user_id) 
                             VALUES (@Title, @Is_Complete, @User_Id);";
        await db.ExecuteAsync(sql, new
        {
            todo.Title,
            Is_Complete = todo.IsComplete ? 1 : 0,
            User_Id = userId.Value
        });
    }

    public async Task DeleteAsync(int id, UserId userId)
    {
        await using var db = new SqliteConnection(ConnectionString);

        const string sql = @"DELETE FROM Todo 
                            WHERE id=@Id AND user_id=@User_Id;";

        await db.ExecuteAsync(sql, new
        {
            Id = id,
            User_Id = userId.Value
        });
    }
}

internal interface ITodoRepository
{
    Task<IEnumerable<Todo>> GetAllAsync(UserId userId);
    Task<Todo?> GetByIdAsync(int id, UserId userId);
    Task AddAsync(Todo todo,  UserId userId);
    Task DeleteAsync(int id, UserId userId);
}