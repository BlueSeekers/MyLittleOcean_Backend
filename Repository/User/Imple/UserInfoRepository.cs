using Dapper;
using MySqlConnector;
using System.Data;

public class UserInfoRepository : IUserInfoRepository {
    private readonly string _connectionString;

    public UserInfoRepository(string connectionString) {
        _connectionString = connectionString;
    }


    public UserInfo? getUserInfo(string id) {
        using (IDbConnection db = new MySqlConnection(_connectionString)) {
            string sql = @"SELECT user_no, user_id, user_name, user_email, account_locked, lock_date, last_pwd_date, provider, create_date, update_date " +
                "FROM tb_user_info info" +
                "WHERE info.user_id = @id";
            return db.QueryFirstOrDefault<UserInfo>(sql, new { id });
        }
    }

    public UserInfo? getUserInfo(int no) {
        using (IDbConnection db = new MySqlConnection(_connectionString)) {
            string sql = @"SELECT user_no, user_id, user_name, user_email, account_locked, lock_date, last_pwd_date, provider, create_date, update_date " +
                "FROM tb_user_info info" +
                "WHERE info.user_no = @no";
            return db.QueryFirstOrDefault<UserInfo>(sql, new { no });
        }
    }
}
