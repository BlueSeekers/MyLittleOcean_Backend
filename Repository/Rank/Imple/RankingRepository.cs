using MySqlConnector;
using Dapper;
using System.Data;
using System.Security.AccessControl;

public class RankingRepository : IRankingRepository {
    private readonly string _connectionString;
    private readonly QueryLogger _queryLogger;

    public RankingRepository(string connectionString, QueryLogger queryLogger) {
        _connectionString = connectionString;
        _queryLogger = queryLogger;
    }

    // 특정 유저의 랭킹 순위 조회
    public async Task<RankDetail?> GetUserRanking(RankParamsDto rankParams) {
        using (IDbConnection db = new MySqlConnection(_connectionString)) {
            var query = @" 
                    WITH RankedUsers AS (
                        SELECT 
                            u.user_no,
                            RANK() OVER (ORDER BY r.rank_value DESC) AS ranking,
                            r.rank_value,
                            DATE_FORMAT(r.create_date, '%y/%m/%d'),
                            u.user_name
                        FROM tb_rank r
                        LEFT JOIN tb_user_info u ON r.user_no = u.user_no
                        WHERE r.game_type = @gameType
                        AND r.create_date BETWEEN CONCAT(CURDATE(), ' 00:00:00') AND CONCAT(CURDATE(), ' 23:59:59')
                    )
                    SELECT 
                        ru.ranking, 
                        ru.user_name, 
                        ru.rank_value, 
                        ru.create_date
                    FROM RankedUsers ru
                    INNER JOIN tb_user_info info ON info.user_no = ru.user_no
                    WHERE info.user_id = @userId;";

            await _queryLogger.ExecuteAsync(query, new {
                gameType = rankParams.gameType.ToString(),
                userId = rankParams.userId
            });

            var userRank = await db.QuerySingleOrDefaultAsync<RankDetail>(query, new {
                gameType = rankParams.gameType.ToString(),
                userId = rankParams.userId
            });

            return userRank;
        }
    }

    // 기간별 상위 랭킹 조회 (일간)
    public async Task<List<RankDetail>> GetDailyRankList(RankParamsDto rankParams) {
        using (IDbConnection db = new MySqlConnection(_connectionString)) {
            string sql = @"
                SELECT 
                    RANK() OVER (ORDER BY r.rank_value DESC) AS ranking,
                    r.rank_value, DATE_FORMAT(r.create_date, '%y/%m/%d'), u.user_name
                FROM 
                    tb_rank r
                INNER JOIN 
                    tb_user_info u ON r.user_no = u.user_no
                WHERE
                    r.game_type = @gameType
                    AND r.create_date BETWEEN CONCAT(CURDATE(), ' 00:00:00') AND CONCAT(CURDATE(), ' 23:59:59')
                ORDER BY r.rank_value DESC
                LIMIT @rankCount"
            ;

            Task<int> task = _queryLogger.ExecuteAsync(sql, new {
                gameType = rankParams.gameType.ToString(),
                userId = rankParams.userId,
                rankCount = rankParams.rankCount
            });

            var rankList = await db.QueryAsync<RankDetail>(sql, new {
                gameType = rankParams.gameType.ToString(),
                userId = rankParams.userId,
                rankCount = rankParams.rankCount
            });

            return rankList.ToList();
        }
    }

    // 기간별 상위 랭킹 조회 (월간)
    public async Task<List<RankDetail>> GetMonthRankList(RankParamsDto rankParams) {
        using (IDbConnection db = new MySqlConnection(_connectionString)) {
            string sql = @"
                SELECT 
                    RANK() OVER (ORDER BY r.rank_value DESC) AS ranking,
                    r.game_type, r.rank_value, DATE_FORMAT(r.create_date, '%y/%m/%d'), u.user_name
                FROM 
                    tb_rank r
                INNER JOIN 
                    tb_user_info u ON r.user_no = u.user_no
                WHERE
                    r.game_type = @gameType
                    AND r.create_date 
                        BETWEEN DATE_FORMAT(CURDATE(), '%Y-%m-01') AND LAST_DAY(CURDATE())
                ORDER BY r.rank_value DESC
                LIMIT @rankCount"
            ;

            Task<int> task = _queryLogger.ExecuteAsync(sql, new {
                gameType = rankParams.gameType.ToString(),
                userId = rankParams.userId,
                rankCount = rankParams.rankCount
            });

            var rankList = await db.QueryAsync<RankDetail>(sql, new {
                gameType = rankParams.gameType.ToString(),
                userId = rankParams.userId,
                rankCount = rankParams.rankCount
            });

            return rankList.ToList();
        }
    }

    // 랭킹 데이터 저장
    public async Task<bool> InsertRank(RankInsertDto rankDto) {
        using (IDbConnection db = new MySqlConnection(_connectionString)) {
            string sql = @"
                INSERT INTO tb_rank (game_type, user_no, rank_value, create_date)
                SELECT @gameType, u.user_no, @rankValue, NOW()
                FROM tb_user_info u
                WHERE u.user_id = @userId;";

            await _queryLogger.ExecuteAsync(sql, new { gameType = rankDto.gameType.ToString(), rankDto.userId, rankDto.rankValue });
            int rowsAffected = await db.ExecuteAsync(sql, new { gameType = rankDto.gameType.ToString(), rankDto.userId, rankDto.rankValue });
            return rowsAffected > 0;
        }
    }

    // 랭킹 데이터 수정(rank value가 크면 업데이트)
    public async Task<bool> UpdateRank(RankInsertDto rankDto) {
        using (IDbConnection db = new MySqlConnection(_connectionString)) {
            string sql = @"
                UPDATE tb_rank r
                JOIN tb_user_info u ON r.user_no = u.user_no
                SET r.rank_value = @rankValue, r.create_date = NOW()
                WHERE r.game_type = @gameType
                    AND u.user_id = @userId
                    AND DATE(r.create_date) = CURDATE()
                    AND r.rank_value < @rankValue;";

            await _queryLogger.ExecuteAsync(sql, new { gameType = rankDto.gameType.ToString(), rankDto.userId, rankDto.rankValue });
            int rowsAffected = await db.ExecuteAsync(sql, new { gameType = rankDto.gameType.ToString(), rankDto.userId, rankDto.rankValue });
            return rowsAffected > 0;
        }
    }

    //현재 날짜에 데이터 있는지 여부
    public async Task<bool> CheckRankExists(RankInsertDto rankDto) {
        using (IDbConnection db = new MySqlConnection(_connectionString)) {
            string sql = @"SELECT rank_value 
                    FROM tb_rank rank
                    INNER JOIN tb_user_info info ON info.user_no = rank.user_no
                    WHERE 
                        rank.game_type = @gameType
                        AND info.user_id = @userId
                        AND DATE(rank.create_date) = CURDATE()";
            await _queryLogger.ExecuteAsync(sql, new { gameType = rankDto.gameType.ToString(), rankDto.userId });
            bool exist = await db.QueryFirstOrDefaultAsync(sql, new { gameType = rankDto.gameType.ToString(), rankDto.userId }) != null;
            return exist;
        }
    }

    public async Task<bool> CheckRankLow(RankInsertDto rankDto) {
        using (IDbConnection db = new MySqlConnection(_connectionString)) {
            string sql = @"SELECT rank_value 
                    FROM tb_rank rank
                    INNER JOIN tb_user_info info ON info.user_no = rank.user_no
                    WHERE 
                        rank.game_type = @gameType
                        AND info.user_id = @userId
                        AND DATE(rank.create_date) = CURDATE()
                        AND rank.rank_value < @rankValue";
            await _queryLogger.ExecuteAsync(sql, new { gameType = rankDto.gameType.ToString(), rankDto.userId , rankDto.rankValue });
            bool check = await db.QueryFirstOrDefaultAsync(sql, new { gameType = rankDto.gameType.ToString(), rankDto.userId, rankDto.rankValue }) != null;
            return check;
        }
    }
}
