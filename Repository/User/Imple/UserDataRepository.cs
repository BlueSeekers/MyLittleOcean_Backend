using Dapper;
using MySqlConnector;
using System.Data;

public class UserDataRepository : IUserDataRepository {
    private readonly string _connectionString;

    public UserDataRepository(string connectionString) {
        _connectionString = connectionString;
    }
    
    //유저 No로 UserData 조회
    public UserData? GetUserDataByNo(int userNo) {
        using (IDbConnection db = new MySqlConnection(_connectionString)) {
            string sql = @"SELECT coin_amount, token_amount, update_date " +
                "FROM tb_user_data data" +
                "WHERE info.user_no = @userNo";
            return db.QueryFirstOrDefault<UserData>(sql, new { userNo });
        }
    }

    //유저 ID로 UserData 조회
    public UserData? GetUserDataById(string userId) {
        using (IDbConnection db = new MySqlConnection(_connectionString)) {
            string sql = @"SELECT coin_amount, token_amount, update_date " +
                "FROM tb_user_data data" +
                "INNER JOIN tb_user_info info on data.user_no = info.user_no" +
                "WHERE info.user_id = @userId";
            return db.QueryFirstOrDefault<UserData>(sql, new { userId });
        }
    }

}
