using MySqlConnector;
using Dapper;
using System.Data;
using System.Security.AccessControl;

public class RankingRepository : IRankingRepository
{
    private readonly string _connectionString;

    public RankingRepository(string connectionString)
    {
        _connectionString = connectionString;
    }
    // 특정 유저의 랭킹 순위 조회
    public Rank? GetUserRanking(string gameType, int userNo)
    {
        using (IDbConnection db = new MySqlConnection(_connectionString))
        {
            var query = @"   
            SELECT * 
            FROM tb_rank               
            WHERE game_type = @GameType 
            AND user_no = @UserNo";

            var rank = db.QuerySingleOrDefault<Rank>(query, new { GameType = gameType, UserNo = userNo });
            return rank;
        }
    }
    // 기간별 상위 랭킹 조회
    public List<Rank> GetTopRanksByPeriod(string gameType, string startDate, string endDate, int topN)
    {
        using (IDbConnection db = new MySqlConnection(_connectionString))
        {
            string sql = @"
                SELECT rank_no, game_type, user_no, rank_value, create_date 
                FROM tb_rank 
                WHERE game_type = @GameType
                AND create_date BETWEEN @StartDate AND @EndDate
                ORDER BY rank_value DESC 
                LIMIT @TopN";

            return db.Query<Rank>(sql, new
            {
                GameType = gameType,
                StartDate = startDate,
                EndDate = endDate,
                TopN = topN
            }).ToList();
        }
    }
    // 랭킹 데이터 저장/수정
    public void UpdateRank(Rank rank)
    {
        using (IDbConnection db = new MySqlConnection(_connectionString))
        {
            string sql = @"
                INSERT INTO tb_rank (game_type, user_no, rank_value, create_date) 
                VALUES (@GameType, @UserNo, @RankValue, NOW())
                ON DUPLICATE KEY UPDATE 
                    rank_value = VALUES(rank_value), 
                    create_date = NOW()";

            db.Execute(sql, new { rank.GameType, rank.UserNo, rank.RankValue });
        }
    }
}
