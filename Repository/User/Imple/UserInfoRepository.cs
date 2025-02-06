using Dapper;
using MySqlConnector;
using System.Data;

public class UserInfoRepository : IUserInfoRepository {
    private readonly string _connectionString;
    private readonly QueryLogger _queryLogger;

    public UserInfoRepository(string connectionString, QueryLogger queryLogger) {
        _connectionString = connectionString;
        _queryLogger = queryLogger;
    }

    // 유저 No 로 UserInfo 조회
    public async Task<UserInfo?> GetUserInfoByNo(int no) {
        using (IDbConnection db = new MySqlConnection(_connectionString)) {
            string sql = @"SELECT user_no, user_id, user_name, user_email, account_locked, lock_date, provider, create_date, update_date " +
                "FROM tb_user_info info " +
                "WHERE info.user_no = @no";
            await _queryLogger.ExecuteAsync(sql, new { no });
            return await db.QueryFirstOrDefaultAsync<UserInfo>(sql, new { no });
        }
    }

    // 유저 ID 로 UserInfo 조회
    public async Task<UserInfo?> GetUserInfoById(string id) {
        using (IDbConnection db = new MySqlConnection(_connectionString)) {
            string sql = @"SELECT user_no, user_id, user_name, user_email, account_locked, lock_date, provider, create_date, update_date " +
                "FROM tb_user_info info " +
                "WHERE info.user_id = @id";
            await _queryLogger.ExecuteAsync(sql, new { id });
            return await db.QueryFirstOrDefaultAsync<UserInfo>(sql, new { id });
        }
    }

    //유저 이름 업데이트
    public async Task<int> UpdateUserNameAsync(string userId, string newUserName) {
        using var connection = new MySqlConnection(_connectionString);
        var sql = @"
        UPDATE tb_user_info 
        SET user_name = @newUserName, 
            update_date = NOW() 
        WHERE user_id = @userId 
        AND NOT EXISTS (
            SELECT 1 
            FROM tb_user_info 
            WHERE user_name = @newUserName 
            AND user_id != @userId
        );
        SELECT ROW_COUNT() as updated_rows;";

        await _queryLogger.ExecuteAsync(sql, new { userId, newUserName });
        return await connection.QueryFirstAsync<int>(sql, new { userId, newUserName });
    }

}
