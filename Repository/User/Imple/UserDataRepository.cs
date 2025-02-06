using Dapper;
using MySqlConnector;
using System.Data;

public class UserDataRepository : IUserDataRepository {
    private readonly string _connectionString;
    private readonly QueryLogger _queryLogger;

    public UserDataRepository(string connectionString, QueryLogger queryLogger) {
        _connectionString = connectionString;
        _queryLogger = queryLogger;
    }

    //유저 No로 UserData 조회
    public async Task<UserData?> GetUserDataByNo(int userNo) {
        using (IDbConnection db = new MySqlConnection(_connectionString)) {
            string sql = @"SELECT data.coin_amount, data.token_amount, data.update_date " +
                "FROM tb_user_data data " +
                "INNER JOIN tb_user_info info ON data.user_no = info.user_no " +
                "WHERE info.user_no = @userNo";

            await _queryLogger.ExecuteAsync(sql, new { userNo });
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

            await _queryLogger.ExecuteAsync(sql, new { userId });
            return await db.QueryFirstOrDefaultAsync<UserData>(sql, new { userId });
        }
    }

    //UserNo로 Coin 사용
    public async Task<int> UseCoinByNo(UserUseDto useDataDto) {
        using (IDbConnection db = new MySqlConnection(_connectionString)) {
            string sql = @"UPDATE tb_user_data SET" +
                " coin_amount = coin_amount - @amount, update_date = NOW()" +
                " WHERE data.user_no = @userNo AND coin_amount >= @amount";

            var parameters = new { amount = useDataDto.Amount, userId = useDataDto.UserId };
            await _queryLogger.ExecuteAsync(sql, new { parameters });
            int rowsAffected = await db.ExecuteAsync(sql, parameters);
            return rowsAffected > 0 ? 1 : 0;
        }
    }

    //UserID로 Coin 사용
    public async Task<int> UseCoinByID(UserUseDto useDataDto) {
        using (IDbConnection db = new MySqlConnection(_connectionString)) {
            string sql = @"
                UPDATE tb_user_data 
                SET coin_amount = coin_amount - @amount, update_date = NOW()
                WHERE user_no = ( SELECT user_no FROM tb_user_info WHERE user_id = @userId) 
                AND coin_amount >= @amount";

            var parameters = new { amount = useDataDto.Amount, userId = useDataDto.UserId };
            await _queryLogger.ExecuteAsync(sql, new { parameters });
            int rowsAffected = await db.ExecuteAsync(sql, parameters);
            return rowsAffected > 0 ? 1 : 0;
        }
    }

    //UserNo로 Token 사용
    public async Task<int> UseTokenByNo(UserUseDto useDataDto) {
        using (IDbConnection db = new MySqlConnection(_connectionString)) {
            string sql = @"UPDATE tb_user_data SET" +
                " token_amount = token_amount - @amount, update_date = NOW()" +
                " WHERE data.user_no = @userNo AND token_amount >= @amount";

            var parameters = new { amount = useDataDto.Amount, userId = useDataDto.UserId };
            await _queryLogger.ExecuteAsync(sql, new { parameters });
            int rowsAffected = await db.ExecuteAsync(sql, parameters);
            return rowsAffected > 0 ? 1 : 0;
        }
    }

    //UserID로 Token 사용
    public async Task<int> UseTokenByID(UserUseDto useDataDto) {
        using (IDbConnection db = new MySqlConnection(_connectionString)) {
            string sql = @" UPDATE tb_user_data SET token_amount = token_amount - @amount, update_date = NOW()" +
                        " WHERE user_no = ( SELECT user_no FROM tb_user_info WHERE user_id = @userId)" +
                        " AND token_amount >= @amount";
            var parameters = new { amount = useDataDto.Amount, userId = useDataDto.UserId };
            await _queryLogger.ExecuteAsync(sql, new { parameters });
            int rowsAffected = await db.ExecuteAsync(sql, parameters);
            return rowsAffected > 0 ? 1 : 0;
        }
    }

    public async Task<bool> UserDataUpdate(UserUpdateDataDto updateData) {
        using (IDbConnection db = new MySqlConnection(_connectionString)) {
            string sql = @"UPDATE tb_user_data SET token_amount = @tokenAmount, coin_amount = @coinAmount, update_date = NOW()" +
                        " WHERE user_no = ( SELECT user_no FROM tb_user_info WHERE user_id = @userId)";

            var parameters = new { tokenAmont = updateData.tokenAmount, coinAmount = updateData.coinAmount, userId = updateData.userId };
            await _queryLogger.ExecuteAsync(sql, new { parameters });
            int rowsAffected = await db.ExecuteAsync(sql, parameters);
            return rowsAffected > 0 ? true : false;
        }
    }
}
