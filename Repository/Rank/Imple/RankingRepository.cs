using MySqlConnector;
using Dapper;
using System.Data;

public class RankingRepository : IRankingRepository
{
    private readonly string _connectionString;

    public RankingRepository(string connectionString)
    {
        _connectionString = connectionString;
    }
    public int GetUserRanking(int userNo)
    {
        using (IDbConnection db = new MySqlConnection(_connectionString))
        {
            var query = @"
                SELECT COUNT(*) + 1 
                FROM tb_rank 
                WHERE rank_value > (
                    SELECT rank_value 
                    FROM tb_rank 
                    WHERE user_no = @UserNo
                )";

            var rank = db.ExecuteScalar<int>(query, new { UserNo = userNo });
            return rank;
        }
    }
    public List<Rank> GetTopRanks(int topN)
    {
        using (IDbConnection db = new MySqlConnection(_connectionString))
        {
            string sql = @"
                SELECT rank_no, user_no, rank_value, create_date 
                FROM tb_rank 
                ORDER BY rank_value DESC 
                LIMIT @TopN";
            return db.Query<Rank>(sql, new { TopN = topN }).ToList();
        }
    }

    public void UpdateRank(Rank rank)
    {
        using (IDbConnection db = new MySqlConnection(_connectionString))
        {
            string sql = @"
                INSERT INTO tb_rank (user_no, rank_value, create_date) 
                VALUES (@UserNo, @RankValue, NOW())
                ON DUPLICATE KEY UPDATE 
                    rank_value = VALUES(rank_value), 
                    create_date = NOW()";
            db.Execute(sql, new { rank.UserNo, rank.RankValue });
        }
    }
}
