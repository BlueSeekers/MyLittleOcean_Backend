public interface IRankingRepository
{
    long GetUserRanking(long userNo);
    List<Rank> GetTopRanks(int topN);
    void UpdateRank(Rank rank);
}
