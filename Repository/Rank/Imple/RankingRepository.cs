using MySqlConnector;
using Dapper;
using System.Data;

public class RankingRepository : IRankingRepository {
    private readonly string _connectionString;
    private readonly QueryLogger _queryLogger;

    public RankingRepository(string connectionString, QueryLogger queryLogger) {
        _connectionString = connectionString;
        _queryLogger = queryLogger;
    }

    // 특정 유저의 랭킹 순위 조회
    public RankDetail? GetUserRanking(string gameType, string startDate, string endDate, int userNo) {
        using (IDbConnection db = new MySqlConnection(_connectionString)) {
            var query = @" 
                    WITH RankedUsers AS (
                        SELECT 
                            RANK() OVER (ORDER BY r.rank_value DESC) AS ranking,
                            r.user_no,
                            r.rank_value,
                            r.create_date,
                            u.user_name
                        FROM tb_rank r
                        LEFT JOIN tb_user_info u ON r.user_no = u.user_no
                        WHERE r.game_type = @GameType
                        AND r.create_date BETWEEN @StartDate AND @EndDate
                    )
                    SELECT 
                        ranking, 
                        user_name, 
                        rank_value, 
                        create_date
                    FROM RankedUsers
                    WHERE user_no = @UserNo;";

            Task<int> task = _queryLogger.ExecuteAsync(query, new {
                GameType = gameType,
                StartDate = startDate,
                EndDate = endDate,
                UserNo = userNo
            });
            var userRank = db.QuerySingleOrDefault<RankDetail>(query, new {
                GameType = gameType,
                StartDate = startDate,
                EndDate = endDate,
                UserNo = userNo
            });
            return userRank;
        }
    }

    // 기간별 상위 랭킹 조회
    public List<RankDetail> GetTopRanksByPeriod(string gameType, string startDate, string endDate, int topN) {
        using (IDbConnection db = new MySqlConnection(_connectionString)) {
            string sql = @"
                SELECT 
                    RANK() OVER (ORDER BY r.rank_value DESC) AS ranking,
                    r.game_type, r.user_no, r.rank_value, r.create_date, u.user_name
                FROM tb_rank r
                LEFT JOIN mylio.tb_user_info u ON r.user_no = u.user_no
                WHERE r.game_type = @GameType
                AND r.create_date BETWEEN @StartDate AND @EndDate
                ORDER BY r.rank_value DESC
                LIMIT @TopN"
            ;

            Task<int> task = _queryLogger.ExecuteAsync(sql, new {
                GameType = gameType,
                StartDate = startDate,
                EndDate = endDate,
                TopN = topN
            });

            return db.Query<RankDetail>(sql, new {
                GameType = gameType,
                StartDate = startDate,
                EndDate = endDate,
                TopN = topN
            }).ToList();
        }
    }

    // 랭킹 데이터 저장
    public async Task<bool> InsertRank(RankInsertDto rankDto) {
        using (IDbConnection db = new MySqlConnection(_connectionString)) {
            string sql = @"
                INSERT INTO tb_rank (game_type, user_no, rank_value, create_date)
                VALUES (@gameType, @userNo, @rankValue, NOW())";

            await _queryLogger.ExecuteAsync(sql, new { rankDto.gameType, rankDto.userNo, rankDto.rankValue });
            int rowsAffected = await db.ExecuteAsync(sql, new { rankDto.gameType, rankDto.userNo, rankDto.rankValue });
            return rowsAffected > 0;
        }
    }

    // 랭킹 데이터 수정(rank value가 크면 업데이트)
    public async Task<bool> UpdateRank(RankInsertDto rankDto) {
        using (IDbConnection db = new MySqlConnection(_connectionString)) {
            string sql = @"
                UPDATE tb_rank
                SET rank_value = @rankValue, create_date = NOW()
                WHERE game_type = @gameType
                    AND user_no = @userNo
                    AND DATE(create_date) = CURDATE()
                    AND rank_value < @rankValue;";

            await _queryLogger.ExecuteAsync(sql, new { rankDto.gameType, rankDto.userNo, rankDto.rankValue });
            int rowsAffected = await db.ExecuteAsync(sql, new { rankDto.gameType, rankDto.userNo, rankDto.rankValue });
            return rowsAffected > 0;
        }
    }

    public async Task<bool> CheckRankExists(RankInsertDto rankDto) {
        using (IDbConnection db = new MySqlConnection(_connectionString)) {
            string sql = @"SELECT rank_value FROM tb_rank
                    WHERE game_type = @gameType
                        AND user_no = @userNo
                        AND DATE(create_date) = CURDATE()
                    LIMIT 1;";
            await _queryLogger.ExecuteAsync(sql, new { rankDto.gameType, rankDto.userNo });
            bool exist = await db.QueryFirstOrDefaultAsync(sql, new { rankDto.gameType, rankDto.userNo });
            return exist;
        }
    }
}
