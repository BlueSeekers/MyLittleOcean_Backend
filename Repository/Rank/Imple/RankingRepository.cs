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
    // 랭킹 데이터 저장/수정
    public void UpdateRank(RankDetail rank) {
        using (IDbConnection db = new MySqlConnection(_connectionString)) {
            string sql = @"
                INSERT INTO tb_rank (game_type, user_no, rank_value, create_date) 
                VALUES (@GameType, @UserNo, @RankValue, NOW())
                ON DUPLICATE KEY UPDATE 
                    rank_value = VALUES(rank_value), 
                    create_date = NOW()";

            Task<int> task = _queryLogger.ExecuteAsync(sql, new { rank.GameType, rank.UserNo, rank.RankValue });
            db.Execute(sql, new { rank.GameType, rank.UserNo, rank.RankValue });
        }
    }
}
