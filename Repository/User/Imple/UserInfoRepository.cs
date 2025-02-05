using Dapper;
using MySqlConnector;
using System.Data;

public class UserInfoRepository : IUserInfoRepository {
    private readonly string _connectionString;

    public UserInfoRepository(string connectionString) {
        _connectionString = connectionString;
    }
    // 유저 No 로 UserInfo 조회
    public async Task<UserInfo?> GetUserInfoByNo(int no) {
        using (IDbConnection db = new MySqlConnection(_connectionString)) {
            string sql = @"SELECT user_no, user_id, user_name, user_email, account_locked, lock_date, provider, create_date, update_date " +
                "FROM tb_user_info info " +
                "WHERE info.user_no = @no";
            return await db.QueryFirstOrDefaultAsync<UserInfo>(sql, new { no });
        }
    }

    // 유저 ID 로 UserInfo 조회
    public async Task<UserInfo?> GetUserInfoById(string id) {
        using (IDbConnection db = new MySqlConnection(_connectionString)) {
            string sql = @"SELECT user_no, user_id, user_name, user_email, account_locked, lock_date, provider, create_date, update_date " +
                "FROM tb_user_info info " +
                "WHERE info.user_id = @id";
            return await db.QueryFirstOrDefaultAsync<UserInfo>(sql, new { id });
        }
    }

    // 유저 닉네임 중복 확인
    public async Task<bool> IsNameDuplicate(string name) {
        using (IDbConnection db = new MySqlConnection(_connectionString)) {
            string sql = @"SELECT COUNT(1) FROM tb_user_info info WHERE info.user_name = @name";
            int count = await db.QueryFirstOrDefaultAsync<int>(sql, new { name });
            return count > 0;
        }
    }

    //유저 닉네임 수정
    public async Task<bool> UpdateUserName(string userId, string name) {
        using (IDbConnection db = new MySqlConnection(_connectionString)) {
            string sql = @"UPDATE tb_user_info SET user_name = @name WHERE user_id = @userId";
            int count = await db.ExecuteAsync(sql, new { userId, name });
            return count > 0;
        }
    }

}
