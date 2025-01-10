using Dapper;
using MyLittleOcean.Models.Follow;
using MySqlConnector;
using System.Data;

public class FollowRepository : IFollowRepository {

    private readonly string _connectionString;

    public FollowRepository(string connectionString) {
        _connectionString = connectionString;
    }

    /// <summary>
    /// 팔로우 하기
    /// </summary>
    /// <param name="param">targetUserNo : 대상 유저 No, followUserNo : 팔로우 유저 No </param>
    public int CreateFollow(FollowCreateRequestDto param) {
        using (IDbConnection db = new MySqlConnection(_connectionString)) {
            string sql = @"INSERT INTO tb_follow" +
                "(target_user_no, follow_user_no)" +
                "VALUES (@targetUserNo, @followUserNo)";
            int rowsAffected = db.Execute(sql, new { param });
            return rowsAffected;
        }
    }

    /// <summary>
    /// 팔로우 취소
    /// </summary>
    /// <param name="param">targetUserNo : 대상 유저 No, followUserNo : 팔로우 유저 No </param>
    public int DeleteFollow(FollowCreateRequestDto param) {
        using (IDbConnection db = new MySqlConnection(_connectionString)) {
            string sql = @"DELETE FROM tb_follow" +
                "WHERE target_user_no = @targetUserNo AND followUserNo = @followUserNo";
            int rowsAffected = db.Execute(sql, new { param });
            return rowsAffected;
        }
    }

    /// <summary>
    /// 특정 유저가 팔로우 한 목록
    /// </summary>
    /// <param name="userNo">유저 식별 번호</param>
    public List<FollowDetail> GetFollowingList(int userNo) {
        using (IDbConnection db = new MySqlConnection(_connectionString)) {
            string sql = @"SELECT user.user_no, user.user_name" +
                "FROM tb_follow follow" +
                "INNER JOIN tb_user_info user ON follow.target_user_no = user.user_no "
                + "WHERE follow.follow_user_no = @userNo";
            return db.Query<FollowDetail>(sql, new { userNo }).ToList();
        }
    }

    /// <summary>
    /// 특정 유저를 팔로우한 목록
    /// </summary>
    /// <param name="userNo">유저 식별 번호</param>
    public List<FollowDetail> GetFollowersList(int userNo) {
        using (IDbConnection db = new MySqlConnection(_connectionString)) {
            string sql = @"SELECT user.user_no, user.user_name" +
                "FROM tb_follow follow" +
                "INNER JOIN tb_user_info user ON follow.follow_user_no = user.user_no"
                + "WHERE follow.target_user_no = @userNo";
            return db.Query<FollowDetail>(sql, new { userNo }).ToList();
        }
    }
}
