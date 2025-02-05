using Dapper;
using MySqlConnector;
using System.Data;

public class UserDataRepository : IUserDataRepository {
    private readonly string _connectionString;

    public UserDataRepository(string connectionString) {
        _connectionString = connectionString;
    }

    //유저 No로 UserData 조회
    public async Task<UserData?> GetUserDataByNo(int userNo) {
        using (IDbConnection db = new MySqlConnection(_connectionString)) {
            string sql = @"SELECT data.coin_amount, data.token_amount, data.update_date " +
                "FROM tb_user_data data " +
                "INNER JOIN tb_user_info info ON data.user_no = info.user_no " +
                "WHERE info.user_no = @userNo";
            return await db.QueryFirstOrDefaultAsync<UserData>(sql, new { userNo });
        }
    }

    //유저 ID로 UserData 조회
    public async Task<UserData?> GetUserDataById(string userId) {
        using (IDbConnection db = new MySqlConnection(_connectionString)) {
            string sql = @"SELECT data.coin_amount, data.token_amount, data.update_date " +
                "FROM tb_user_data data " +
                "INNER JOIN tb_user_info info ON data.user_no = info.user_no " +
                "WHERE info.user_id = @userId";
            return await db.QueryFirstOrDefaultAsync<UserData>(sql, new { userId });
        }
    }

    //UserNo로 Coin 사용
    public async Task<int> UseCoinByNo(UserDataDto useDataDto) {
        using (IDbConnection db = new MySqlConnection(_connectionString)) {
            string sql = @"UPDATE tb_user_data SET" +
                " coin_amount = coin_amount - @amount" +
                " WHERE data.user_no = @userNo";
            int rowsAffected = await db.ExecuteAsync(sql, useDataDto);
            return rowsAffected > 0 ? 1 : 0;
        }
    }

    //UserID로 Coin 사용
    public async Task<int> UseCoinByID(UserDataDto useDataDto) {
        using (IDbConnection db = new MySqlConnection(_connectionString)) {
            string sql = @" UPDATE tb_user_data SET coin_amount = coin_amount - @amount" +
                        " WHERE user_no = ( SELECT user_no FROM tb_user_info WHERE user_id = @userId)";
            int rowsAffected = await db.ExecuteAsync(sql, useDataDto);
            return rowsAffected > 0 ? 1 : 0;
        }
    }

    //UserNo로 Token 사용
    public async Task<int> UseTokenByNo(UserDataDto useDataDto) {
        using (IDbConnection db = new MySqlConnection(_connectionString)) {
            string sql = @"UPDATE tb_user_data SET" +
                " token_amount = token_amount - @amount" +
                " WHERE data.user_no = @userNo";
            int rowsAffected = await db.ExecuteAsync(sql, useDataDto);
            return rowsAffected > 0 ? 1 : 0;
        }
    }

    //UserID로 Token 사용
    public async Task<int> UseTokenByID(UserDataDto useDataDto) {
        using (IDbConnection db = new MySqlConnection(_connectionString)) {
            string sql = @" UPDATE tb_user_data SET token_amount = token_amount - @amount" +
                        " WHERE user_no = ( SELECT user_no FROM tb_user_info WHERE user_id = @userId)";
            int rowsAffected = await db.ExecuteAsync(sql, useDataDto);
            return rowsAffected > 0 ? 1 : 0;
        }
    }
}
