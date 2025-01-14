using Dapper;
using MySqlConnector;

public class UserRepository : IUserRepository
{
    private readonly string _connectionString;

    public UserRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<bool> AddUserAsync(string userId, string userName, string email, string provider, string providerId)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            var query = @"
                INSERT INTO tb_user_info (  
                    user_no,    
                    user_id, 
                    user_name,
                    user_email,
                    account_locked, 
                    last_pwd_date,
                    provider,
                    provider_id,
                    create_date,
                    update_date
                ) VALUES (   
                    NULL,
                    @UserId,
                    @UserName,
                    @UserEmail,
                    false,
                    NOW(),
                    @Provider,
                    @ProviderId,
                    NOW(),
                    NOW()
                )";

            var result = await connection.ExecuteAsync(query, new
            {
                UserId = userId,
                UserName = userName,
                UserEmail = email,
                Provider = provider,
                ProviderId = providerId
            });

            return result > 0;
        }
    }
}
