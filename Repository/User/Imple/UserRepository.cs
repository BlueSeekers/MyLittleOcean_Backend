using Dapper;
using MySqlConnector;

public class UserRepository : IUserRepository
{
    private readonly string _connectionString;

    public UserRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<bool> AddUserAsync(string userId, string userName, string provider)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            var query = @"
                INSERT INTO tb_user_info (  
                    user_no,    
                    user_id, 
                    user_name,
                    account_locked, 
                    provider,
                    create_date
                ) VALUES (   
                    NULL,
                    @UserId,
                    @UserName,
                    false,
                    @Provider,
                    NOW()
                )";

            var result = await connection.ExecuteAsync(query, new
            {
                UserId = userId,
                UserName = userName,
                Provider = provider,
            });

            return result > 0;
        }
    }
}
