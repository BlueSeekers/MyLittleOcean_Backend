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
    public RankDetail? GetUserRanking(string gameType, int userNo) {
        using (IDbConnection db = new MySqlConnection(_connectionString)) {
            var query = @"
            SELECT r.*, u.user_name
            FROM tb_rank r
            LEFT JOIN mylio.tb_user_info u ON r.user_no = u.user_no
            WHERE r.game_type = @GameType
            AND r.user_no = @UserNo";

            Task<int> task = _queryLogger.ExecuteAsync(query, new { GameType = gameType, UserNo = userNo });
            var rank = db.QuerySingleOrDefault<RankDetail>(query, new { GameType = gameType, UserNo = userNo });
            return rank;
        }
    }
    // 기간별 상위 랭킹 조회
    public List<RankDetail> GetTopRanksByPeriod(string gameType, string startDate, string endDate, int topN) {
        using (IDbConnection db = new MySqlConnection(_connectionString)) {
            string sql = @"
                SELECT r.rank_no, r.game_type, r.user_no, r.rank_value, r.create_date, u.user_name
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
    public void InsertRank(RankDetail rank) {
        using (IDbConnection db = new MySqlConnection(_connectionString)) {
            string sql = @"
                INSERT INTO tb_rank (game_type, user_no, rank_value, create_date)
                SELECT @GameType, @UserNo, @RankValue, NOW()
                WHERE NOT EXISTS (
                    SELECT 1
                    FROM tb_rank
                    WHERE game_type = @GameType
                        AND user_no = @UserNo
                        AND DATE(create_date) = CURDATE()
                );";
            Task<int> task = _queryLogger.ExecuteAsync(sql, new { rank.GameType, rank.UserNo, rank.RankValue });
            db.Execute(sql, new { rank.GameType, rank.UserNo, rank.RankValue });
        }
    }

    // 랭킹 데이터 수정(rank value가 크면 업데이트)
    public void UpdateRank(RankDetail rank) {
        using (IDbConnection db = new MySqlConnection(_connectionString)) {
            string sql = @"
            UPDATE tb_rank
            SET rank_value = @RankValue, create_date = NOW()
            WHERE game_type = @GameType
                AND user_no = @UserNo
                AND DATE(create_date) = CURDATE()
                AND rank_value < @RankValue;";
            db.Execute(sql, new { rank.GameType, rank.UserNo, rank.RankValue });
        }
    }
}
