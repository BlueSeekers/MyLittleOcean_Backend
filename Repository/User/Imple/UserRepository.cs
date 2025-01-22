using Dapper;
using MySqlConnector;

public class UserRepository : IUserRepository
{
    private readonly string _connectionString;

    public UserRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<bool> AddUserAsync(string userId, string userEmail, string provider)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            var query = @"
            INSERT INTO tb_user_info (  
                user_no, user_id, user_email,
                account_locked, provider, create_date
            ) VALUES (   
                NULL, @UserId, @UserEmail,
                false, @Provider, NOW()
            )";


            var result = await connection.ExecuteAsync(query, new
            {
                UserId = userId,
                UserEmail = userEmail,
                Provider = provider,
            });

            return result > 0;
        }
    }

    public async Task<bool> ValidateUserCredentialsAsync(string username, string password) {
        using var connection = new MySqlConnection(_connectionString);
        var query = "SELECT COUNT(1) FROM tb_user_info WHERE user_id = @Username AND user_pwd = @Password";
        var count = await connection.ExecuteScalarAsync<int>(query, new { Username = username, Password = password });
        return count > 0;
    }

    public async Task<UserDto> GetUserByUsernameAsync(string username) {
        using var connection = new MySqlConnection(_connectionString);
        var query = "SELECT user_name as Username, provider as Provider FROM tb_user_info WHERE user_id = @Username";
        return await connection.QueryFirstOrDefaultAsync<UserDto>(query, new { Username = username });
    }

    public async Task<int> CreateUserAsync(AuthCreateDto userCreateDto) {
        using var connection = new MySqlConnection(_connectionString);
        var sql = @"INSERT INTO tb_user_info
            (user_id, user_name, user_email, account_locked, create_date, provider)
            VALUES (@userId, @userName, @userEmail, false, NOW(), @provider);
            SELECT LAST_INSERT_ID();";

        return await connection.ExecuteScalarAsync<int>(sql, userCreateDto);
    }

    public async Task<int> CreateDefaultUserDataAsync(int userNo) {
        using var connection = new MySqlConnection(_connectionString);
        var sql = @"INSERT INTO tb_user_data
            (user_no, coin_amount, token_amount, update_date)
            VALUES (@userNo, 0, 5, NOW())";

        return await connection.ExecuteAsync(sql, new { userNo });
    }
}
