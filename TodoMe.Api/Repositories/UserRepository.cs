using System.Data;
using Dapper;
using Microsoft.Data.Sqlite;
using TodoMe.Api.Entities;

namespace TodoMe.Api.Repositories;

internal sealed class UserRepository : IUserRepository
{
    private const string ConnectionString = "Data Source=db.sqlite;Pooling=True;";

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        await using var db = new SqliteConnection(ConnectionString);
        return await db.QueryAsync<User>("SELECT * FROM User");
    }

    public async Task<User?> GetByIdAsync(UserId id)
    {
        await using var db = new SqliteConnection(ConnectionString);
        return (await db.QueryFirstOrDefaultAsync<User>(
            "SELECT * FROM User WHERE id=@Id",
            new { Id = id.Value }));
    }

    public async Task<bool> AddAsync(User user)
    {
        await using var db = new SqliteConnection(ConnectionString);

        if (await UserExistsAsync(user.Id, db)) return false;

        const string sql = @"INSERT INTO User (id, name) 
                             VALUES (@Id, @Name) 
                             ON CONFLICT DO NOTHING;";

        await db.ExecuteAsync(sql, new { user.Id, user.Name }
        );

        return true;
    }

    public async Task Delete(UserId userId)
    {
        await using var db = new SqliteConnection(ConnectionString);
        await db.ExecuteAsync(
            "DELETE FROM User WHERE id=@Id",
            new { Id = userId.Value });
    }

    private static async Task<bool> UserExistsAsync(UserId userId, IDbConnection connection)
    {
        const string sql = @"SELECT CASE WHEN EXISTS 
                             (SELECT 1 FROM User 
                                       WHERE id = @Id ) 
                             THEN 1 ELSE 0 END";

        return await connection.ExecuteScalarAsync<bool>(sql, new
        {
            Id = userId.Value
        });
    }
}

internal interface IUserRepository
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<User?> GetByIdAsync(UserId id);
    Task<bool> AddAsync(User todo);
    Task Delete(UserId id);
}