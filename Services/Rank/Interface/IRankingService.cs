public interface IRankingService
{
    List<Rank> GetRanks();
    List<Rank> GetTopRanks(int count);
    int GetUserRanking(int userNo);
    void UpdateRank(Rank rank);
    RankingInfoDto GetRankingInfo(int userNo, int topCount);
}
