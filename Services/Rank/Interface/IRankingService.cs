public interface IRankingService
{
    List<Rank> GetRanks();
    List<Rank> GetTopRanks(int count);
    long GetUserRanking(long userNo);
    void UpdateRank(Rank rank);
    RankingInfo GetRankingInfo(long userNo, int topCount);
}
