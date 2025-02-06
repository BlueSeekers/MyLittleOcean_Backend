using Dapper;
using MySqlConnector;

public class UserRepository : IUserRepository {
    private readonly string _connectionString;
    private readonly QueryLogger _queryLogger;

    public UserRepository(string connectionString, QueryLogger queryLogger) {
        _connectionString = connectionString;
        _queryLogger = queryLogger;
    }

    public async Task<bool> AddUserAsync(string userId, string userEmail, string provider) {
        using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();

        using var transaction = await connection.BeginTransactionAsync();  // 트랜잭션 시작
        try {
            // 1. 유저가 이미 존재하는지 확인
            var checkQuery = "SELECT user_no FROM tb_user_info WHERE user_id = @UserId";
            var existingUserNo = await connection.QueryFirstOrDefaultAsync<int?>(checkQuery, new { UserId = userId }, transaction);

            if (existingUserNo.HasValue) {
                await transaction.CommitAsync();
                return true;
            }

            // 2. tb_user_info에 insert
            var userQuery = @"
            INSERT INTO tb_user_info (  
                user_no, user_id, user_email,
                account_locked, provider, create_date, update_date
            ) VALUES (   
                NULL, @UserId, @UserEmail,
                false, @Provider, NOW(), NOW()
            );";

            await connection.ExecuteAsync(userQuery, new { UserId = userId, UserEmail = userEmail, Provider = provider }, transaction);

            var userNo = await connection.ExecuteScalarAsync<int>("SELECT LAST_INSERT_ID();", transaction: transaction);

            // 3. tb_user_data에 insert
            var dataQuery = @"
            INSERT INTO tb_user_data (
                user_no, coin_amount, token_amount, update_date
            ) VALUES (
                @UserNo, 0, 5, NOW()
            )";

            await connection.ExecuteAsync(dataQuery, new { UserNo = userNo }, transaction);

            await transaction.CommitAsync();
            return true;
        }
        catch (Exception ex) {
            await transaction.RollbackAsync();
            await _queryLogger.ExecuteAsync("INSERT INTO tb_error_logs (error_message, create_date) VALUES (@Message, NOW());",
                new { Message = ex.Message });
            throw;
        }
    }

    public async Task<bool> ValidateUserCredentialsAsync(string userId) {
        using var connection = new MySqlConnection(_connectionString);
        var query = "SELECT COUNT(1) FROM tb_user_info WHERE user_id = @UserID";

        // SQL 실행 전 로그 기록
        await _queryLogger.ExecuteAsync(query, new { UserID = userId });

        var count = await connection.ExecuteScalarAsync<int>(query, new { UserID = userId });
        return count > 0;
    }

    public async Task<UserDto> GetUserByUsernameAsync(string username) {
        using var connection = new MySqlConnection(_connectionString);
        var query = "SELECT user_name as Username, provider as Provider FROM tb_user_info WHERE user_id = @Username";
        await _queryLogger.ExecuteAsync(query, new { Username = username });
        return await connection.QueryFirstOrDefaultAsync<UserDto>(query, new { Username = username });
    }

    public async Task<int> CreateUserAsync(AuthCreateDto userCreateDto) {
        using var connection = new MySqlConnection(_connectionString);
        var sql = @"INSERT INTO tb_user_info
            (user_id, user_name, user_email, account_locked, provider, create_date, update_date)
            VALUES (@userId, @userName, @userEmail, false, @provider, NOW(), NOW());
            SELECT LAST_INSERT_ID();";

        await _queryLogger.ExecuteAsync(sql, new { userCreateDto });
        return await connection.ExecuteScalarAsync<int>(sql, userCreateDto);
    }

    public async Task<int> CreateDefaultUserDataAsync(int userNo) {
        using var connection = new MySqlConnection(_connectionString);
        var sql = @"INSERT INTO tb_user_data
            (user_no, coin_amount, token_amount, update_date)
            VALUES (@userNo, 0, 5, NOW())";

        await _queryLogger.ExecuteAsync(sql, new { userNo });
        return await connection.ExecuteAsync(sql, new { userNo });
    }

}
