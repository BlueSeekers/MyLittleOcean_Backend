public interface IRankingRepository
{
    Rank? GetUserRanking(string gameType, int userNo);
    List<Rank> GetTopRanksByPeriod(string gameType, string startDate, string endDate, int topN);
    void UpdateRank(Rank rank);
}
